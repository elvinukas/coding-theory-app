import React, { useState } from "react";
import "./SecondScenarioText.css";

export function SecondScenarioText() {
    const [originalText, setOriginalText] = useState("");
    const [originalTextBinary, setOriginalTextBinary] = useState("");
    const [originalTextChanneled, setOriginalTextChanneled] = useState("");
    const [encodedTextBinary, setEncodedTextBinary] = useState("");
    const [decodedTextBinary, setDecodedTextBinary] = useState("");
    const [decodedText, setDecodedText] = useState("");
    const [decodedTextChanneled, setDecodedTextChanneled] = useState("");

    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction
    const [allowManualEdit, setAllowManualEdit] = useState(false);

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

                <div className="text-field">
                    <h4>Text to be encoded:</h4>
                    <textarea placeholder="Enter text here..."/>
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