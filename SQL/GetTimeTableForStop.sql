/****** Script for SelectTopNRows command from SSMS  ******/
SELECT s.Name Przystanek, r.ShortName as Linia, st.*
  FROM dbo.StopTimes st,
   dbo.Stops s ,
   dbo.Trips t,
  dbo.Routes r
    where st.StopId = s.Id
	and t.Id = st.TripId
	and r.Id = t.RouteId
	and s.Code = '24016'
	order by st.ArrivalTime