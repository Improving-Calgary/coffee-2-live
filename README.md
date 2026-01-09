# Coffee2Live Project

## Overview
Coffee2Live is a full-stack application that provides a web interface for managing coffee-related data. It consists of a backend built with ASP.NET Core and a frontend developed using Angular.

## Technical Requirements

### Backend (ASP.NET Core)
- **.NET SDK**: 8.0 or higher
- **ASP.NET Core Runtime**: 8.0
- **Database**: None (uses JSON files for data storage)

### Frontend (Angular)
- **Node.js**: 18 or higher
- **npm**: 9 or higher
- **Angular CLI**: 20.3.8 or higher

## Project Structure
- **Backend**: Located in the `dotnet/` directory, contains the ASP.NET Core Web API.
- **Frontend**: Located in the `angular/` directory, contains the Angular application.
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

### Frontend
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