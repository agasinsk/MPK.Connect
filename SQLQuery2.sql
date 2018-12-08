declare @now time
set @now = CAST(GETDATE() as time)

declare @later time
set @later = DATEADD(hour, 1, @now)


select st.StopId, st.TripId, st.DepartureTime, st.StopSequence , r.ShortName Route
from dbo.StopTimes st, dbo.Trips tr
inner join dbo.Routes r on r.Id = tr.RouteId
where tr.Id = st.Tripid and st.DepartureTime BETWEEN @now AND @LATER
and StopId = '1418'
order by DepartureTime, StopSequence

