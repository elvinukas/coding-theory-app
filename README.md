# Coding Theory Application

This is a web-based application designed specifically for demonstrating encoding and decoding proceses of the linear encoding and step-by-step decoding algorithms.
The project is written in C# (.NET) for the backend and React for the frontend.

Three scenarios are present to showcase the encoding/decoding features, ranked by difficulty.

Binary vector coding represents the simpliest demonstration of both algorithms, text coding scenario merges many binary vector codings into one, 
where as image coding is the final and most programmically challenging part of this project.

## Features

+ Encoding and decoding of binary vectors, text and images
+ Simulation of real-life channels where passing data can introduce corruption
+ Tools to specify generator matrices for encoding
+ Ability to manually input errors into binary vectors
+ Side-by-side comparisons for text and image encoding efficiency with raw (non-encoded) data

## Showcase

<img width="1452" alt="image" src="https://github.com/user-attachments/assets/f8e4c132-5004-41d0-b0a5-6a5f934ddad9" />
<img width="626" alt="image" src="https://github.com/user-attachments/assets/75ed7caf-afbd-4c2a-a2cf-0f46a324253d" />
<img width="644" alt="image" src="https://github.com/user-attachments/assets/cd2efa99-91de-4104-89f1-13eae4017c75" />


## Installation

This application is currently not hosted, thus it must be run locally in a dev environment. Application built using Microsoft.DotNet.Web.Spa.ProjectTemplates.

1. Install .NET SDK 7.0, Next.js.
2. Open the terminal in `\app\`, then do:
   ```
   export ASPNETCORE_ENVIRONMENT=Development && \
   export ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=Microsoft.AspNetCore.SpaProxy && \
   export ASPNETCORE_URLS="https://localhost:7117;http://localhost:5109" && \
   dotnet run --launch-profile app
   ```
3. Open application in `https://localhost:7117`.



