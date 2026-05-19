# Coffee2Live Project

## Overview
Coffee2Live is a full-stack application that provides a web interface for managing coffee-related data. It consists of a backend built with ASP.NET Core and two frontend options: Angular and Blazor WebAssembly. Each group uses one frontend based on their familiarity — no switching between them is required.

## Technical Requirements

### Development Environment
- **VS Code**: Recommended IDE for development
  - **C# Dev Kit Extension**: Provides rich language support for C# and helps you manage your code with a solution explorer and test your code with integrated unit test discovery and execution

### Backend (ASP.NET Core)
- **.NET SDK**: 8.0 or higher
- **ASP.NET Core Runtime**: 8.0
- **Database**: None (uses JSON files for data storage)

### Frontend (Angular)
- **Node.js**: 18 or higher
- **npm**: 9 or higher

### Frontend (Blazor WebAssembly)
- **.NET SDK**: 8.0 or higher (same as backend)

## Project Structure
- **Backend**: Located in the `dotnet/` directory, contains the ASP.NET Core Web API.
- **Frontend (Angular)**: Located in the `angular/` directory, contains the Angular application.
- **Frontend (Blazor)**: Located in the `blazor/` directory, contains the Blazor WebAssembly application.
- **Data**: JSON files located in `dotnet/src/Coffee2Live.App/Data/`.

## Quick Start

### Backend
1. Navigate to the backend directory:
   ```bash
   cd dotnet/src/Coffee2Live.App
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5000`.

### Frontend (Angular)
1. Navigate to the frontend directory:
   ```bash
   cd angular
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the Angular application:
   ```bash
   npm start
   ```
   The application will be available at `http://localhost:4200`.

### Frontend (Blazor WebAssembly)
1. Navigate to the Blazor directory:
   ```bash
   cd blazor
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
   The application will be available at the URL shown in the terminal output.

> **Note**: The backend must be running before starting either frontend. Run only one frontend at a time.

4. If all goes well, you should see this in your browser:

   ![Coffee2Live Screenshot](./coffee2live-screenshot.png)