
declare @serviceId int
set @serviceId = 6

declare @now time
set @now = CAST(GETDATE() as time)

declare @later time
set @later = DATEADD(minute, 45, @now)


select st.StopId, s.Name, s.Code, st.TripId, st.DepartureTime, st.StopSequence , r.ShortName RouteId,  tr.[HeadSign] AS [Direction], tr.[DirectionId], r.[ShortName] AS [Route]
from 
dbo.StopTimes st
inner join dbo.Trips tr on tr.Id = st.TripId 
inner join dbo.Stops s on s.Id = st.StopId 
inner join dbo.Routes r on r.Id = tr.RouteId
where st.DepartureTime BETWEEN @now AND @LATER AND ([st.Trip].[ServiceId] = @serviceId)

