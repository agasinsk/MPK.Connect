/****** Script for SelectTopNRows command from SSMS  ******/
SELECT s.Name Przystanek, r.ShortName as Linia, st.*
  FROM dbo.StopTimes st,
   dbo.Stops s ,
   dbo.Trips t,
  dbo.Routes r
    where st.StopId = s.Id
	and t.Id = st.TripId
	and r.Id = t.RouteId
	and t.RouteId = '1'
	 order by st.TripId, st.StopSequence