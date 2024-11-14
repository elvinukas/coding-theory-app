import React, { useState } from "react";
import "./FirstScenarioBinary.css";

export function FirstScenarioBinary() {
    const [binaryVector, setBinaryVector] = useState("");
    const [encodedVector, setEncodedVector] = useState("");
    const [channelVector, setChannelVector] = useState("");
    const [decodedVector, setDecodedVector] = useState("");
    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction

    const [useCustomGeneratorMatrix, setUseCustomGeneratorMatrix] = useState(false);
    const [matrixRows, setMatrixRows] = useState(3);
    const [matrixCols, setMatrixCols] = useState(5);
    const [generatorMatrix, setGeneratorMatrix] = useState([]);
    
    
    const handleUseCustomMatrixToggle = (event) => {
        setUseCustomGeneratorMatrix(event.target.checked);
    }

    const handleMatrixRowsChange = (event) => {
        setMatrixRows(parseInt(event.target.value, 10));
    };

    const handleMatrixColsChange = (event) => {
        setMatrixCols(parseInt(event.target.value, 10));
    };
    
    
    // handler to update the binary vector input
    const handleInputChange = (event) => {
        const input = event.target.value;
        if (/^[01]*$/.test(input)) { // Only allow binary input
            setBinaryVector(input);
        }
    };

    // handler to set error probability
    const handleErrorProbabilityChange = (event) => {
        setErrorProbability(parseFloat(event.target.value));
    };

    // unction for encoding
    const handleEncode = () => {
        const encodedVector = `Encoded(${binaryVector})`; // Replace with encoding logic
        setEncodedVector(encodedVector);
    };

    // function for channel error introduction 
    const handleIntroduceErrors = () => {
        const channelVector = `Channel(${encodedVector}, P=${errorProbability})`; // Replace with channel logic
        setChannelVector(channelVector);
    };

    // function for decoding
    const handleDecode = () => {
        const decodedVector = `Decoded(${channelVector})`; // Replace with decoding logic
        setDecodedVector(decodedVector);
    };

    return (
        <div className="container">
        {/* Left Section for Main Content */}
        <div className="left-section">
            <h1>First Scenario - <strong>Binary Vector</strong></h1>
            <p>In this scenario, the user can write a binary vector according to the size of a provided generator matrix.</p>

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
                    step="0.01"
                />
            </div>

            <div className="buttons">
                <button onClick={handleEncode}>Encode Vector</button>
                <button onClick={handleIntroduceErrors} disabled={!encodedVector}>Introduce Errors</button>
                <button onClick={handleDecode} disabled={!channelVector}>Decode Vector</button>
            </div>

            <div>
                <h3>Encoded Vector</h3>
                <p>{encodedVector}</p>
            </div>

            <div>
                <h3>Channel Vector (with Errors)</h3>
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
                <div className="matrix-settings">
                    <h3>Generator Matrix Dimensions</h3>
                    <label>
                        Rows:
                        <input
                            type="number"
                            value={matrixRows}
                            onChange={handleMatrixRowsChange}
                            min="1"
                        />
                    </label>
                    <label>
                        Columns:
                        <input
                            type="number"
                            value={matrixCols}
                            onChange={handleMatrixColsChange}
                            min="1"
                        />
                    </label>
                </div>
            )}
        </div>
    </div>
);
}