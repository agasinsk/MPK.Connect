use [MPK.Connect.Db];
go

delete from dbo.FeedInfos;
go

delete from dbo.StopTimes;
delete from dbo.Trips;
go

delete from dbo.Shapes;
delete from dbo.ShapeBases;
delete from dbo.Stops;
go

delete from dbo.CalendarDates;
delete from dbo.Calendars;
go

delete from dbo.FareRules;
delete from dbo.FareAttributes;
go

delete from dbo.Routes;
delete from dbo.Agencies;
go