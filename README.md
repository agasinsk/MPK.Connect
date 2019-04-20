# MPK.Connect
Web app for searching routes in urban network.

# Description

Developed with:
- ASP.NET Core 2 WebApi,
- Entity Framework Core,
- React,
- Material UI + OpenStreetMap,
- General Transit Feed Specification (GTFS) data,
- SQL Server.

The main route search algorithm is A* (using the airline distance heuristic).

# How to run 

1. You need to import the data from .csv files (included in repo) to the database by running MPK.Console.DataImporter app.
2. Run the web API by starting the MPK.Connect.WebApp project.
3. Start the front-end using npm start command.
4. Try to search for routes in the urban area of Wrocław, Poland.

