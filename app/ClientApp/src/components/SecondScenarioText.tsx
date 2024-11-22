import React, {useRef, useState} from "react";
import "./SecondScenarioText.css";

export function SecondScenarioText() {
    const [originalText, setOriginalText] = useState("");
    // const [originalTextBinary, setOriginalTextBinary] = useState("");
    const [originalTextChanneled, setOriginalTextChanneled] = useState("");
    const [encodedTextBinary, setEncodedTextBinary] = useState("");
    const [encodedTextChanneled, setEncodedTextChanneled] = useState("");
    const [decodedTextBinary, setDecodedTextBinary] = useState("");
    const [decodedText, setDecodedText] = useState("");
    const [decodedTextChanneled, setDecodedTextChanneled] = useState("");

    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction
    const [allowManualEdit, setAllowManualEdit] = useState(false);
    const [isEncodedTextVisible, setIsEncodedTextVisible] = useState(false);

    const [useCustomGeneratorMatrix, setUseCustomGeneratorMatrix] = useState(false);
    const [matrixRows, setMatrixRows] = useState(4);
    const [matrixCols, setMatrixCols] = useState(7);
    const preinputtedMatrixData = [
        ["1", "0", "0", "0", "1", "1", "0"],
        ["0", "1", "0", "0", "1", "0", "1"],
        ["0", "0", "1", "0", "1", "1", "1"],
        ["0", "0", "0", "1", "0", "1", "1"]
    ];
    const [generatorMatrix, setGeneratorMatrix] = useState(
        Array.from({ length: 4 }, (_, rowIndex) =>
            Array.from({ length: 7 }, (_, colIndex) => preinputtedMatrixData[rowIndex][colIndex] || "")
        )
    );

    const handleMatrixDimensionsChange = () => {
        setGeneratorMatrix(Array(matrixRows).fill(Array(matrixCols).fill("")));
    };

    const handleMatrixInputChange = (row, col, value) => {
        const newMatrix = generatorMatrix.map((r, rowIndex) => {
            if (rowIndex === row) {
                return r.map((cell, colIndex) => {
                    if (colIndex === col) {
                        return value;
                    } else {
                        return cell;
                    }
                });
            } else {
                return r;
            }
        });
    };

    const handleUseCustomMatrixToggle = (event) => {
        setUseCustomGeneratorMatrix(event.target.checked);
    }

    const handleErrorProbabilityChange = (event) => {
        const value = parseFloat(event.target.value);
        if (value >= 0 && value <= 100) {
            setErrorProbability(value);
        }
    };
    
    const fetchGeneratorMatrix = async () => {
        let generatorMatrixArray;
        if (useCustomGeneratorMatrix) {
            generatorMatrixArray = generatorMatrix.map(row =>
                row.map(column => parseInt(column, 10))
            );
            return generatorMatrixArray;
        } else {
            // fetching a randomly generated matrix
            try {
                const response = await fetch("/api/Matrix/", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        rows: parseInt(matrixRows, 10),
                        cols: parseInt(matrixCols, 10)
                    })
                });

                if (response.ok) {
                    const data = await response.json();
                    generatorMatrixArray = data.matrix.map(row =>
                        row.map(column => parseInt(column, 10))
                    );
                    return generatorMatrixArray;
                } else {
                    alert("Error: Failed to fetch generator matrix.");
                    return null;
                }
            } catch (error) {
                alert("Failed to fetch generator matrix: " + error.message);
                return null;
            }
        }
    }

    const textareaRef = useRef(null); // reference to the text area
    
    
    const handleEncode = async () => {
        
        const text = textareaRef.current?.value || "";
        setOriginalText(text);
        

        // -- generator matrix portion
        let generatorMatrixArray = await fetchGeneratorMatrix();
        
        // - encoding fetching

        const requestData = {
            Type: "text",
            Text: text,
            GeneratorMatrix: generatorMatrixArray
        }
        
        try {
            console.log(requestData);
            const response = await fetch("/api/Encoding/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(requestData)
            });

            if (response.ok) {
                const data = await response.json();
                const dataString = convertListToString(data.encodedMessage);
                console.log(dataString);
                setEncodedTextBinary(dataString);
            } else {
                alert("Error: Failed to encode the text.");
            }
        } catch (error) {
            alert("Failed to encode the text: " + error.message);
        }
    }

    const convertListToString = (list) => {
        return list.map(row => row.join("")).join("");
    }
    
    const handlePassThroughChannel = () => {
        
    }
    
    const handleDecode = () => {
        
        
    }
    
    return (
        <div className="container">
            {/* Left Section */}
            <div className="left-section">
                <h1>Second Scenario - <strong>Text Encoding</strong></h1>
                <p>In this scenario, the user can provide a text, encoded in UTF-8, which will be encoded, channeled and
                    decoded accordingly. The length of the text is not limited.
                    <br/><br/>
                    Two situations are displayed - message sent through the channel without and with encoding - this is
                    for observing how the decoding algorithm works.<br/>
                    If no generator matrix is provided, a random one will be generated.</p>
                <p><b>WARNING!</b> The user takes the
                    responsibility if the encoded message cannot be properly decoded if no generator matrix is provided.
                    The randomly generated matrix may
                    not have
                    the best properties for decoding (its Hamming distance may be very small). Be advised.
                </p>

                <div>
                    <h3>Error Probability</h3>
                    <input
                        type="number"
                        value={errorProbability}
                        onChange={handleErrorProbabilityChange}
                        min="0"
                        max="100"
                        step="0.000001"
                    />
                </div>


                <div className="buttons">
                    <button
                        onClick={handleEncode}
                    >
                        Encode Text
                    </button>
                    <button onClick={handlePassThroughChannel} disabled={!encodedTextBinary}>Pass through channel
                    </button>
                    <button onClick={handleDecode} disabled={!encodedTextChanneled}>Decode Text</button>
                </div>

                <div className="text-field">
                    <h4>Text to be encoded:</h4>
                    <textarea placeholder="Enter text here..."
                              ref={textareaRef}
                    />
                </div>

                <div className="output-area">
                    <h4>Text successfully encoded!</h4>
                    <button onClick={() => setIsEncodedTextVisible(!isEncodedTextVisible)}>
                        {isEncodedTextVisible ? "Hide Encoded Text" : "Show Encoded Text"}
                    </button>
                    {isEncodedTextVisible && (
                        <div className="encoded-text-box">
                            <p>{encodedTextBinary}</p>
                        </div>
                    )}
                </div>


            </div>

            <div className="right-section">
                <div>
                    <input
                        type="checkbox"
                        checked={useCustomGeneratorMatrix}
                        onChange={handleUseCustomMatrixToggle}
                    />
                    <label>Use custom generator matrix?</label>
                </div>

                <div>
                    <div className="matrix-dimensions">
                        <label> Parameters: </label>
                        <br/>
                        <span>Dimension: </span>
                        <input
                            type="number"
                            value={matrixRows}
                            onChange={(e) => setMatrixRows(parseInt(e.target.value))}
                            onBlur={handleMatrixDimensionsChange}
                            min="1"
                        />
                        <br/>
                        <span>Code length: </span>
                        <input
                            type="number"
                            value={matrixCols}
                            onChange={(e) => setMatrixCols(parseInt(e.target.value))}
                            onBlur={handleMatrixDimensionsChange}
                            min="1"
                        />
                    </div>


                    {useCustomGeneratorMatrix && (
                        <div>
                            <div className="matrix-input">
                                {generatorMatrix.map((row, rowIndex) => (
                                    <div key={rowIndex} className="matrix-row">
                                        {row.map((value, colIndex) => (
                                            <input
                                                key={`${rowIndex}-${colIndex}`}
                                                type="text"
                                                value={value.replace(/[^01]/g, "")}
                                                onChange={(e) =>
                                                    handleMatrixInputChange(rowIndex, colIndex, e.target.value)
                                                }
                                                className="matrix-cell"
                                                maxLength="1"
                                            />
                                        ))}
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}
                </div>

            </div>
        </div>
    );

}