## Project Overview
Group 9 **iCARE** implementation for CS 4320/7320

## Table of Contents
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Installation](#installation)

## Technologies Used
- **Programming Language**: C# .net 8
- **Database Management**: MS SQL Server
- **Development Environment**: Microsoft Visual Studio
- **Framework**: ASP.NET MVC

## Features
* **Patient Management:** 
    * Nurses and doctors can create, view, edit, and delete patient records (CRUD operations).
    * Patients can be assigned to specific healthcare workers.
* **Document Management:**
    * Supports uploading and searching documents related to patients.
    * Sorting and retrieval capabilities to easily access relevant documents.
    * Uses CKEditor and Aspose for integration of pdf format.
* **Drug Integration:**
    * Integrates with a drug database to provide medication options when prescribing.
    * Users can select drugs from the database and assign them to patient records.
* **Secure Access:**
    * Users can login and manage their passwords to ensure patient information security.
* **Treatment Management:**
    * Allows for adding and managing treatment plans for each patient.
    * Allows assigning of drugs to patients.
 
* **Logging in:**
The admin login is admin@admin.com and the password is Password1!. More users can be created from there.

## Installation
1. Clone the repo
2. Install .net 8 and Microsoft SQL Server 2022
3. Set up Microsoft SQL Server and restore the database using the .bak file
4. Change the connection string in appsettings.json and ICAREDBContext.cs in models to connect to your database.
5. Compile to executable.
6. Run executable.
