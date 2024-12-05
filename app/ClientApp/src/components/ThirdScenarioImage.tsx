import React, {useRef, useState} from "react";
import "./ThirdScenarioImage.css";
// @ts-ignore
import { fetchGeneratorMatrix, encode, channel, decode, converter} from "./ApiCallHandlers.ts";


export function ThirdScenarioImage() {
    const [uploadedImage, setUploadedImage] = useState(null);
    const [encodedImageUrl, setEncodedImageUrl] = useState("");
    const [inProgress, setInProgress] = useState(false);
    //const [encodedImage, setEncodedImage] = useState(null);
    //const [encodedChanneledImage, setEncodedChanelledImage] = useState(null);
    const [decodedImage, setDecodedImage] = useState(null);

    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction
    const [isEncodingSuccessful, setIsEncodingSuccessful] = useState(false);
    const [isEncChannelingSuccessful, setIsEncChannelingSuccessful] = useState(false);

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
        Array.from({length: 4}, (_, rowIndex) =>
            Array.from({length: 7}, (_, colIndex) => preinputtedMatrixData[rowIndex][colIndex] || "")
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
    
    
    const handleImageUpload = (event) => {
        const file = event.target.files[0];
        if (file) {
            setUploadedImage(file);
        }
    }

    
    const handleEncode = async () => {

        setInProgress(true);
        
        try {
            // -- generator matrix portion
            let generatorMatrixArray = await fetchGeneratorMatrix(useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols);

            // - encoding fetching
            
            const FormData = require('form-data');
            const formData = new FormData();
            formData.append("Type", "image");
            formData.append("Image", uploadedImage);
            formData.append("Matrix", JSON.stringify(generatorMatrixArray));
            
            for (let [key, value] of formData) {
                console.log(`${key} = ${value}`);
            }
            

            const data = await encode(formData, true);
            if (data) {
                console.log("Encoding was successful.");
                setIsEncodingSuccessful(true);
            } else {
                console.error("Encoding failed - no data received.")
            } 
        } catch (error) {
            console.error("Error while encoding.", error.message);
        } finally {
            setInProgress(false);
        }
        

    }

    const convertListToString = (list) => {
        return list.map(row => row.join("")).join("");
    }

    const handlePassThroughChannel = async () => {
        
        

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
        

    }

    return (
        <div className="container">
            {/* Left Section */}
            <div className="left-section">
                <h1>Third scenario - <strong>Image Coding</strong></h1>
                <p>In this scenario, the user can upload an image file, which will be encoded, channeled and decoded to
                    a .bmp image, which
                    is displayed on the screen.
                    <br/><br/>
                    Two situations are displayed - image sent through the channel without and with encoding - this is
                    for observing how the decoding algorithm works.<br/>
                    If no generator matrix is provided, a random one will be generated.</p>
                <p><b>WARNING!</b> The user takes the
                    responsibility if the encoded message cannot be properly decoded if no generator matrix is provided.
                    The randomly generated matrix may
                    not have
                    the best properties for decoding (its Hamming distance may be very small). Be advised.
                    <br/><br/>
                    <b>The image encoding and decoding processes can take a while, depending on hardware. In personal
                        practice runs,
                        the entire process can take up to 2 minutes, depending on file size. </b> This is because the
                    decoder uses bitmap
                    .bmp files, which notoriously take up a lot of storage and thus a lot of matrix calculations are
                    required,
                    however these files are perfect for demonstrating encoding and decoding
                    images for this example.
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
                        Encode Image
                    </button>
                    <button onClick={handlePassThroughChannel} disabled={!isEncodingSuccessful}>Pass through channel
                    </button>
                    <button onClick={handleDecode} disabled={!isEncChannelingSuccessful}>Decode Image</button>
                </div>

                <div className="text-field">
                    <h4>Upload Image to be encoded:</h4>
                    <input
                        type="file"
                        accept="image/*"
                        onChange={handleImageUpload}
                    />
                </div>


            <div className="comparison-section">
                <div className="left-comparison">

                    {isEncodingSuccessful && (
                        <div className="output-area">
                            <h4>Image successfully encoded!</h4>
                        </div>
                    )}
                    
                    
                    </div>


                    <div className="right-comparison">
                        
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