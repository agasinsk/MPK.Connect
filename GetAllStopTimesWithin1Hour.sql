declare @now time
set @now = CAST(GETDATE() as time)

declare @later time
set @later = DATEADD(minute, 30, @now)


select st.StopId, s.Name, s.Code, st.TripId, st.DepartureTime, st.StopSequence , r.ShortName RouteId
from 
dbo.StopTimes st
inner join dbo.Trips tr on tr.Id = st.TripId 
inner join dbo.Stops s on s.Id = st.StopId 
inner join dbo.Routes r on r.Id = tr.RouteId
where st.DepartureTime BETWEEN @now AND @LATER

