
export const fetchGeneratorMatrix = async (useCustomGeneratorMatrix, generatorMatrix, matrixRows, matrixCols) => {
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

export const encode = async (requestData) => {
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
            return await response.json();
        } else {
            alert("Error: Failed to encode the text.");
        }
    } catch (error) {
        alert("Failed to encode the text: " + error.message);
    }
}

export const channel = async (requestData) => {
    try {
        console.log(requestData);
        const response = await fetch ("api/Channel/", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            return await response.json();
        } else {
            alert("Error: Failed to channel the text.");
        }

    } catch (error) {
        alert("Failed to channel the text: " + error.message);
    }
}


export const decode = async (requestData) => {
    try {
        console.log(requestData);
        const response = await fetch('/api/Decoding/', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            return await response.json();
        } else {
            alert("Failed to decode the vector. Please check if vector length is correct after manual editing.");
        }
    } catch (error) {
        alert("Failed to decode the vector: " + error.message);
    }
}

export const converter = async (requestData) => {
    try {
        console.log(requestData);
        const response = await fetch('/api/Binary/toString/', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            return await response.json();
        } else {
            alert("Failed to convert to string");
        }
    } catch (error) {
        alert("Failed to convert to string." + error.message);
    }
}
