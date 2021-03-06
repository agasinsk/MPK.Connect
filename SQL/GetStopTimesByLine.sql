use [MPK.Fast.Db]
go

SELECT st.Id, st.StopId, st.DepartureTime, st.TripId, s.[Name] as Przystanek, r.ShortName as Linia
  FROM dbo.StopTimes st,
   dbo.Stops s,
   dbo.Trips t,
   dbo.Routes r
    where st.StopId = s.Id
	and t.Id = st.TripId
	and r.Id = t.RouteId
	and t.RouteId = '106'
	 order by st.TripId, st.StopSequence