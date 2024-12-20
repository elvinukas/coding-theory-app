import React, {useRef, useState, useEffect} from "react";
import * as signalR from "@microsoft/signalr";
import "./ThirdScenarioImage.css";
// @ts-ignore
import { fetchGeneratorMatrix, encode, channel, decode, converter} from "./ApiCallHandlers.ts";


export function ThirdScenarioImage() {
    const [uploadedImage, setUploadedImage] = useState(null);
    const [inProgress, setInProgress] = useState(false);
    const [encodingInProgress, setEncodingInProgress] = useState(false);
    const [channelingInProgress, setChannelingInProgress] = useState(false);
    const [decodingInProgress, setDecodingInProgress] = useState(false);
    const [decodedImage, setDecodedImage] = useState(null);
    
    const [originalImageUrl, setOriginalImageUrl] = useState("");
    const [decodedImageUrl, setDecodedImageUrl] = useState("");
    const [originalChanneledImageUrl, setOriginalChanneledImageUrl] = useState("");
    const [hubConnection, setHubConnection] = useState(null);
    const [encodingProgress, setEncodingProgress] = useState(0);
    const [decodingProgress, setDecodingProgress] = useState(0);

    const [errorProbability, setErrorProbability] = useState(0.1); // default probability for error introduction
    const [isEncodingSuccessful, setIsEncodingSuccessful] = useState(false);
    const [isEncChannelingSuccessful, setIsEncChannelingSuccessful] = useState(false);
    const [isDecodingSuccessful, setIsDecodingSuccessful] = useState(false);
    const [isOgChannelingSuccessful, setIsOgChannelingSuccessful] = useState(false);

    const [displayedRandomGenMatrix, setDisplayedRandomGenMatrix] = useState([]);
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

        setEncodingInProgress(true);
        
        try {
            // -- generator matrix portion
            let generatorMatrixArray = await fetchGeneratorMatrix(useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols);

            // - encoding fetching
            setDisplayedRandomGenMatrix(generatorMatrixArray);
            
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
                setEncodingInProgress(false);
            } else {
                console.error("Encoding failed - no data received.")
            } 
        } catch (error) {
            console.error("Error while encoding.", error.message);
        } finally {
            //setOriginalImageUrl(await fetchImage(uploadedImage.name));
            console.log(originalImageUrl);
        }
        

    }

    const convertListToString = (list) => {
        return list.map(row => row.join("")).join("");
    }

    const handlePassThroughChannel = async () => {
        setChannelingInProgress(true);
        
        try {
            let generatorMatrixArray = await fetchGeneratorMatrix(useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols);

            const fileName = uploadedImage.name.split('.')[0] + "._encoded.bin";

            const requestData = {
                Type: "image",
                FileName: fileName,
                GeneratorMatrix: generatorMatrixArray,
                ErrorPercentage: errorProbability / 100
            };

            const data = await channel(requestData);
            if (data) {
                console.log("Channeling was successful.");
                setIsEncChannelingSuccessful(true);
            }

            // channeling non-encoded image for comparison
            
            const ogBinFileName = uploadedImage.name.split('.')[0] + ".bin";
            
            const originalData = {
                Type: "image",
                FileName: ogBinFileName,
                GeneratorMatrix: generatorMatrixArray,
                ErrorPercentage: errorProbability / 100
            }
            
            
            const otherData = await channel(originalData); // channel changes ogbinfile 
            if (otherData) {
                console.log("Channeling original .bin was successful.");
                setIsOgChannelingSuccessful(true);
                //setOriginalChanneledImageUrl(await fetchImage(ogBinFileName));
            }
            
            setChannelingInProgress(false);
            
            
        } catch (error) {
            console.error("Error while channeling. ", error.message);
            setChannelingInProgress(false);
        }
        
        
        
    }
    

    const handleDecode = async () => {
        
        setDecodingInProgress(true);
        
        
        try {
            let generatorMatrixArray = await fetchGeneratorMatrix(useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols);
            
            const fileName = uploadedImage.name.split('.')[0];
            
            const requestData = {
                Type: "image",
                FileName: fileName,
                GeneratorMatrix: generatorMatrixArray
            }
            
            const data = await decode(requestData);
            if (data) {
                console.log("Decoding was successful.");
                setIsDecodingSuccessful(true);
            }
        } catch (error) {
            console.error("Error while decoding: ", error.message);
        } finally {
            //setDecodedImageUrl(await fetchImage(uploadedImage.name.split('.')[0] + "_decoded.bmp"));
            setDecodingInProgress(false);
            await handleImages();
        }

    }

    useEffect(() => {
        if (originalImageUrl) {
            console.log("Original image url: " + originalImageUrl);
        } else {
            console.error("Original image url could not be retrieved.");
        }
    }, [originalImageUrl]);
    
    useEffect(() => {
        if (decodedImageUrl) {
            console.log("Decoded image url: " + decodedImageUrl);
        } else {
            console.error("Decoded image url could not be retrieved.");
        }
    }, [decodedImageUrl]);

    useEffect(() => {
        if (originalChanneledImageUrl) {
            console.log("Original channeled image url: " + originalChanneledImageUrl);
        } else {
            console.error("Original channeled image url could not be retrieved.");
        }
    }, [originalChanneledImageUrl]);
    
    
    const fetchImage = async (fileName: string) : Promise<string | null> => {
        try {
            const response = await fetch(`/api/image/${fileName}`);
            if (response.ok) {
                const imageBlob = await response.blob(); // required for image files, js binary files for images
                const imageUrl = URL.createObjectURL(imageBlob);
                return imageUrl;
            } else {
                console.error("Failed to retrieve image from the server.");
                return null;
            }
        } catch (error) {
            console.error("Failed to retrieve image from the server.");
            return null;
        }
        
    }
    
    const handleImages = async () => {
        setOriginalImageUrl(await fetchImage(uploadedImage.name))
        setOriginalChanneledImageUrl(await fetchImage(uploadedImage.name.split('.')[0] + ".bin"));
        setDecodedImageUrl(await fetchImage(uploadedImage.name.split('.')[0] + "_decoded.bmp"));
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
                        onClick={() => {
                            setIsEncodingSuccessful(false);
                            setIsEncChannelingSuccessful(false);
                            setIsDecodingSuccessful(false);
                            handleEncode();
                        }}
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


                {encodingInProgress && (
                    <div className="encoding-text">
                        Encoding<span className="dot-1">.</span><span className="dot-2">.</span>
                        <span className="dot-3">.</span>
                    </div>
                )}


                <div className="comparison-section">
                    <div className="left-comparison">

                        {isEncodingSuccessful && (
                            <div className="output-area">
                                <h4>Image successfully encoded!</h4>
                            </div>
                        )}

                        {isEncChannelingSuccessful && (
                            <div className="output-area">
                                <h4>Image successfully channeled!</h4>
                            </div>
                        )}

                        {decodingInProgress && (
                            <div className="decoding-text">
                                Decoding<span className="dot-1">.</span><span className="dot-2">.</span>
                                <span className="dot-3">.</span>
                            </div>
                        )}

                        {isDecodingSuccessful && (
                            <div>
                                <h4>Image sucessfully decoded!</h4>
                            </div>
                        )}

                    </div>


                    <div className="right-comparison">
                        {isDecodingSuccessful && (
                            <div style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
                                <div>
                                    <h4><b>Original Image</b></h4>
                                    {originalImageUrl && (
                                        <a href={originalImageUrl} target="_blank" rel="noopener noreferrer">
                                            <img src={originalImageUrl} style={{maxWidth: '100%', maxHeight: '500px'}}/>
                                        </a>
                                    )}
                                </div>
                                <div>
                                    <h4><b>Decoded Image</b></h4>
                                    {decodedImageUrl && (
                                        <a href={decodedImageUrl} target="_blank" rel="noopener noreferrer">
                                            <img src={decodedImageUrl} style={{maxWidth: '100%', maxHeight: '500px'}}/>
                                        </a>
                                    )}
                                </div>
                                {isOgChannelingSuccessful && (
                                    <div>
                                        <h4><b>Original Channeled Image</b></h4>
                                        {originalChanneledImageUrl && (
                                            <a href={originalChanneledImageUrl} target="_blank"
                                               rel="noopener noreferrer">
                                                <img src={originalChanneledImageUrl}
                                                     style={{maxWidth: '100%', maxHeight: '500px'}}/>
                                            </a>
                                        )}
                                    </div>
                                )}
                            </div>)}
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

                    {!useCustomGeneratorMatrix && displayedRandomGenMatrix.length > 0 && (
                        <div>
                            <h3>Random Generator Matrix:</h3>
                            {displayedRandomGenMatrix.map((row, rowIndex) => (
                                <div key={rowIndex} className="matrix-row">
                                    {row.map((value, colIndex) => (
                                        <span key={colIndex} className="matrix-cell">
                                        {value}
                                        </span>
                                    ))}
                                </div>
                            ))}
                        </div>
                    )}
                    
                </div>

            </div>
        </div>
    );
}