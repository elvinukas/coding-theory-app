import React, {useRef, useState} from "react";
import "./SecondScenarioText.css";
// @ts-ignore
import { fetchGeneratorMatrix, encode, channel, decode} from "./ApiCallHandlers.ts";

export function SecondScenarioText() {
    const [originalText, setOriginalText] = useState("");
    const [originalTextBinary, setOriginalTextBinary] = useState("");
    const [originalTextBinaryLength, setOriginalTextBinaryLength] = useState("");
    const [originalTextChanneled, setOriginalTextChanneled] = useState("");
    const [encodedTextBinary, setEncodedTextBinary] = useState("");
    const [encodedTextChanneled, setEncodedTextChanneled] = useState("");
    const [decodedText, setDecodedText] = useState("");
    const [decodedTextChanneled, setDecodedTextChanneled] = useState("");

    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction
    const [allowManualEdit, setAllowManualEdit] = useState(false);
    const [isOriginalBinaryTextVisisble, setIsOriginalBinaryTextVisible] = useState(false);
    const [isOriginalChanneledTextVisible, setIsOriginalChanneledTextVisible] = useState(false);
    const [isEncodedTextVisible, setIsEncodedTextVisible] = useState(false);
    const [isEncodedChanneledTextVisible, setIsEncodedChannelTextVisible] = useState(false);

    const [useCustomGeneratorMatrix, setUseCustomGeneratorMatrix] = useState(true);
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
    const [generatorMatrixArray, setGeneratorMatrixArray] = useState(null);

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

    const binaryVectorConverter = (vector) => {
        const messageMatrix = [
            vector.split('').map(bit => parseInt(bit, 10))
        ];
        return messageMatrix;
    }

    const handleUseCustomMatrixToggle = (event) => {
        setUseCustomGeneratorMatrix(event.target.checked);
    }

    const handleErrorProbabilityChange = (event) => {
        const value = parseFloat(event.target.value);
        if (value >= 0 && value <= 100) {
            setErrorProbability(value);
        }
    };
    

    const textareaRef = useRef(null); // reference to the text area
    
    
    const handleEncode = async () => {
        
        const text = textareaRef.current?.value || "";
        setOriginalText(text);
        

        // -- generator matrix portion
        let generatorMatrixArray = await fetchGeneratorMatrix(useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols);
        
        // - encoding fetching

        const requestData = {
            Type: "text",
            Text: text,
            GeneratorMatrix: generatorMatrixArray
        }
        
        const data = await encode(requestData);
        if (data) {
            const dataString = convertListToString(data.encodedMessage);
            setEncodedTextBinary(dataString);
            setOriginalTextBinaryLength(data.originalMessageBinaryLength);
            const binaryString = convertListToString(data.originalMessageBinary);
            setOriginalTextBinary(binaryString);
        }
        
    }

    const convertListToString = (list) => {
        return list.map(row => row.join("")).join("");
    }
    
    const handlePassThroughChannel = async () => {
        const encodedText = binaryVectorConverter(encodedTextBinary);
        
        const requestData = {
            Type: "vector",
            Matrix: encodedText,
            errorPercentage: errorProbability/100
        };
        
        const data = await channel(requestData);
        if (data) {
            const dataString = convertListToString(data.matrix);
            console.log(dataString);
            setEncodedTextChanneled(dataString);
        }

        const binaryText = binaryVectorConverter(originalTextBinary);
        
        const nonEncodedRequestData = {
            Type: "vector",
            Matrix: binaryText,
            errorPercentage: errorProbability/100
        }
        
        const nonEncodedData = await channel(nonEncodedRequestData);
        if (nonEncodedData) {
            const dataString = convertListToString(nonEncodedData.matrix);
            console.log(dataString);
            setOriginalTextChanneled(dataString);
        }
        
    }

    const markErrors = (textChanneled, textBinary) => {
        return textChanneled.split("").map((bit, index) => {
            if (textBinary[index] !== bit) {
                return <span key={index} className="error-bit">{bit}</span>
            }
            return bit;

        });
    };
    
    const handleDecode = async () => {
        const encodedText = binaryVectorConverter(encodedTextChanneled);
        const generatorMatrixArray = generatorMatrix.map(row =>
            row.map(column => parseInt(column, 10))
        );
        
        const requestData = {
            Type: "text",
            MessageMatrix: encodedText,
            GeneratorMatrix: generatorMatrixArray,
            Length: originalTextBinaryLength
        }
        
        const data = await decode(requestData);
        if (data) {
            console.log(data.message);
            setDecodedText(data.message);
        }
        
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

                <div className="comparison-section">
                    <div className="left-comparison">
                        {encodedTextBinary && (
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
                        )}

                        {encodedTextChanneled && (
                            <div className="output-area">
                                <h4>Text successfully channeled!</h4>

                                <button onClick={() => setIsEncodedChannelTextVisible(!isEncodedChanneledTextVisible)}>
                                    {isEncodedChanneledTextVisible ? "Hide Channeled Text" : "Show Channeled Text"}
                                </button>
                                {isEncodedChanneledTextVisible && (
                                    <div className="encoded-text-box">
                                        <div>{markErrors(encodedTextChanneled, encodedTextBinary)}</div>
                                    </div>
                                )}
                            </div>
                        )}

                        {decodedText && (
                            <div className="output-area">
                                <h4>Text successfully decoded!</h4>

                                <div className="text-field">
                            <textarea
                                value={decodedText}
                                readOnly
                            />
                                </div>
                            </div>
                        )}
                    </div>


                    <div className="right-comparison">
                        {originalText && (
                            <div className="output-area">
                                <h4>Original Text in Binary:</h4>
                                <button onClick={() => setIsOriginalBinaryTextVisible(!isOriginalBinaryTextVisisble)}>
                                    {isOriginalBinaryTextVisisble ? "Hide Original Text Binary" : "Show Original Text Binary"}
                                </button>
                                {isOriginalBinaryTextVisisble&& (
                                    <div className="encoded-text-box">
                                        <p>{originalTextBinary}</p>
                                    </div>
                                )}
                            </div>
                        )}

                        {originalTextChanneled && (
                            <div className="output-area">
                                <h4>Original Text Channeled:</h4>
                                <button onClick={() => setIsOriginalChanneledTextVisible(!isOriginalChanneledTextVisible)}>
                                    {isOriginalChanneledTextVisible ? "Hide Original Text Channeled" : "Show Original Text Channeled"}
                                </button>
                                {isOriginalChanneledTextVisible && (
                                    <div className="encoded-text-box">
                                        <p>{markErrors(originalTextChanneled, originalTextBinary)}</p>
                                    </div>
                                )}
                            </div>
                        )}
                    </div>
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