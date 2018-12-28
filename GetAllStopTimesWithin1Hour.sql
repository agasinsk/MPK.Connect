use [MPK.Fast.Db]
go
set arithabort on;
go

declare @serviceId nvarchar(450)
set @serviceId = N'8'

declare @now time(7)
set @now = CAST(GETDATE() as time)

declare @later time(7)
set @later = DATEADD(minute, 90, @now)

SELECT [st].[TripId], [st].[StopId], [st].[StopSequence], [st].[ArrivalTime], [st].[DepartureTime], 
[st].[DropOffTypes], [st].[HeadSign], [st].[PickupType], [st].[ShapeDistTraveled], [st].[TimePoint],
  [st.Trip].[HeadSign] AS [Direction], [st.Trip].[DirectionId], [st.Trip.Route].[ShortName] AS [Route], [st.Trip.Route].[Type] AS [RouteType]
FROM [StopTimes] AS [st]
INNER JOIN [Trips] AS [st.Trip] ON [st].[TripId] = [st.Trip].[Id]
INNER JOIN [Routes] AS [st.Trip.Route] ON [st.Trip].[RouteId] = [st.Trip.Route].[Id]
WHERE ((@now < [st].[DepartureTime]) AND ([st].[DepartureTime] < @later)) AND ([st.Trip].[ServiceId] = @serviceId)
