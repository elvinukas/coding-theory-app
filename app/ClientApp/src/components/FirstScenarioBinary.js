import React, { useState } from "react";
import "./FirstScenarioBinary.css";

export function FirstScenarioBinary() {
    const [binaryVector, setBinaryVector] = useState("");
    const [encodedVector, setEncodedVector] = useState("");
    const [channelVector, setChannelVector] = useState("");
    const [decodedVector, setDecodedVector] = useState("");
    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction

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
    
    
    const handleUseCustomMatrixToggle = (event) => {
        setUseCustomGeneratorMatrix(event.target.checked);
    }

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

        setGeneratorMatrix(newMatrix);
    };
    
    
    // binary vector conversion into a int[,] matrix
    
    const binaryVectorConverter = (vector) => {
        const messageMatrix = [
            vector.split('').map(bit => parseInt(bit, 10))   
        ];
        return messageMatrix;
    }
    
    
    
    // handler to update the binary vector input
    const handleInputChange = (event) => {
        const input = event.target.value;
        const maxLength = useCustomGeneratorMatrix ? matrixRows : Infinity;
        if (/^[01]*$/.test(input) && input.length <= maxLength) { // Only allow binary input
            setBinaryVector(input);
        }
    };

    // handler to set error probability
    const handleErrorProbabilityChange = (event) => {
        setErrorProbability(parseFloat(event.target.value));
    };

    // function for encoding
    const handleEncode = async () => {
        const messageMatrix = binaryVectorConverter(binaryVector);
        const generatorMatrixArray = generatorMatrix.map(row =>
            row.map(column => parseInt(column, 10))
        );
        
        const requestData = {
            Type: "vector",
            MessageMatrix: messageMatrix,
            GeneratorMatrix: generatorMatrixArray,
            Dimension: matrixRows,
            N: matrixCols
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
                setEncodedVector(dataString);
            } else {
                const errorData = await response.json();
                alert("Error: " + errorData.Message);
            }
        } catch (error) {
            alert("Failed to encode the vector: " + error.message);
        }
        
    };
    
    const convertListToString = (list) => {
        return list.map(row => row.join("")).join("");
    }

    // function for channel error introduction 
    const handleIntroduceErrors = async () => {
        const messageBeforeErrors = binaryVectorConverter(encodedVector);
        
        const requestData = {
            Matrix: messageBeforeErrors,
            errorPercentage: errorProbability/100
        }
        
        try {
            console.log(requestData);
            const response = await fetch("/api/Channel", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(requestData)
            });
            
            if (response.ok) {
                const data = await response.json();
                const dataString = convertListToString(data.matrix);
                console.log(dataString);
                setChannelVector(dataString);
                
            } else {
                const errorData = await response.json();
                alert("Error: " + errorData.Message);
            }
            
            
        } catch (error) {
            alert("Failed to pass the vector through the channel: " + error.message);
        }
        
        
    };

    // function for decoding
    const handleDecode = () => {
        const decodedVector = `Decoded(${channelVector})`;
        setDecodedVector(decodedVector);
    };

    return (
        <div className="container">
        {/* Left Section for Main Content */}
        <div className="left-section">
            <h1>First Scenario - <strong>Binary Vector</strong></h1>
            <p>In this scenario, the user can write a binary vector according to the size of a generator matrix if provided.
                If no generator matrix is provided, a random one will be generated.</p>
            <p> <b>WARNING!</b> The user takes the
                responsibility if the encoded message cannot be properly decoded. The randomly generated matrix may not have
                the best properties for decoding (its Hamming distance may be very small). Be advised.
            </p>

            <div>
                <h3>Input Binary Vector</h3>
                <input
                    type="text"
                    value={binaryVector}
                    onChange={handleInputChange}
                    placeholder="Enter binary vector"
                />
            </div>

            <div>
                <h3>Error Probability</h3>
                <input
                    type="number"
                    value={errorProbability}
                    onChange={handleErrorProbabilityChange}
                    min="0"
                    max="1"
                    step="0.000001"
                />
            </div>

            <div className="buttons">
                <button 
                    onClick={handleEncode}
                    disabled={useCustomGeneratorMatrix && binaryVector.length !== matrixRows}
                >
                    Encode Vector
                </button>
                <button onClick={handleIntroduceErrors} disabled={!encodedVector}>Introduce Errors</button>
                <button onClick={handleDecode} disabled={!channelVector}>Decode Vector</button>
            </div>

            <div>
                <h3>Encoded Vector</h3>
                <p>{encodedVector}</p>
            </div>

            <div>
                <h3>Channel Vector (with possible errors)</h3>
                <p>{channelVector}</p>
            </div>

            <div>
                <h3>Decoded Vector</h3>
                <p>{decodedVector}</p>
            </div>
        </div>

        {/* Right Section for Custom Matrix Options */}
        <div className="right-section">
            <div>
                <input
                    type="checkbox"
                    checked={useCustomGeneratorMatrix}
                    onChange={handleUseCustomMatrixToggle}
                />
                <label>Use custom generator matrix?</label>
            </div>

            {useCustomGeneratorMatrix && (
                <div>
                    <div className="matrix-dimensions">
                        <label>
                            Rows:
                            <input
                                type="number"
                                value={matrixRows}
                                onChange={(e) => setMatrixRows(parseInt(e.target.value))}
                                onBlur={handleMatrixDimensionsChange}
                                min="1"
                            />
                        </label>
                        <label>
                            Columns:
                            <input
                                type="number"
                                value={matrixCols}
                                onChange={(e) => setMatrixCols(parseInt(e.target.value))}
                                onBlur={handleMatrixDimensionsChange}
                                min="1"
                            />
                        </label>
                    </div>

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
);
}