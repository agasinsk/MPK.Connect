using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Test.Service.Business
{
    [TestClass]
    public class TravelPlanServiceTests
    {
        private Mock<IGraphBuilder> _graphBuilderMock;
        private Mock<IMapper> _mapper;
        private Mock<IPathProvider> _pathProviderMock;
        private TravelPlanService _travelPlanService;

        [TestInitialize]
        public void SetUp()
        {
            _graphBuilderMock = new Mock<IGraphBuilder>();
            _pathProviderMock = new Mock<IPathProvider>();
            var logger = new Logger<TravelPlanService>(new LoggerFactory());
            _mapper = new Mock<IMapper>();
            _travelPlanService = new TravelPlanService(_graphBuilderMock.Object, _pathProviderMock.Object, logger, _mapper.Object);
        }

        [TestMethod]
        public void TestIfGetTravelPlansCorrectlySelectsNamesForLocations()
        {
            // Arrange
            var source = new Location(12.1, 11.2);
            var destination = new Location(24.2, 42.4);
            var travelOptions = new TravelOptions
            {
                Source = source,
                Destination = destination
            };

            var stopTimes = new List<StopTimeInfo>
            {
                new StopTimeInfo()
                {
                    Id = 1,
                    StopId = 1,
                    DepartureTime = TimeSpan.FromHours(12),
                    StopDto = new StopDto()
                    {
                        Id = 1,
                        Latitude = 10.1,
                        Longitude = 11.0,
                        Name = "First"
                    }
                },
                new StopTimeInfo()
                {
                    Id = 2,
                    StopId = 2,
                    DepartureTime = TimeSpan.FromHours(12),
                    StopDto = new StopDto()
                    {
                        Id = 2,
                        Latitude = 20.2,
                        Longitude = 21.2,
                        Name = "Second"
                    }
                },
                new StopTimeInfo()
                {
                    Id = 3,
                    StopId = 3,
                    DepartureTime = TimeSpan.FromHours(12),
                    StopDto = new StopDto()
                    {
                        Id = 3,
                        Latitude = 12.1,
                        Longitude = 11.2,
                        Name = "Third"
                    }
                },
                new StopTimeInfo()
                {
                    Id = 4,
                    StopId = 4,
                    DepartureTime = TimeSpan.FromHours(12),
                    StopDto = new StopDto()
                    {
                        Id = 4,
                        Latitude = 24.2,
                        Longitude = 42.4,
                        Name = "Fourth"
                    }
                }
            };

            var graph = new Graph<int, StopTimeInfo>();
            graph.AddNodes(stopTimes);

            _graphBuilderMock.Setup(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()))
                .Returns(graph);

            _pathProviderMock.Setup(p => p.GetAvailablePaths(graph, It.Is<Location>(l => l.Name == "Third"), It.Is<Location>(l => l.Name == "Fourth")))
                .Returns(new List<Path<StopTimeInfo>>());

            _mapper.Setup(p => p.Map<List<TravelPlan>>(It.IsAny<List<Path<StopTimeInfo>>>()))
                .Returns(new List<TravelPlan>());

            // Act
            var result = _travelPlanService.GetTravelPlans(travelOptions);

            // Assert
            _graphBuilderMock.Verify(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()), Times.Once);
            _graphBuilderMock.Verify(p => p.GetGraph(It.Is<DateTime>(d => d < DateTime.Now), null), Times.Once);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(It.IsAny<Graph<int, StopTimeInfo>>(), It.IsAny<Location>(), It.IsAny<Location>()), Times.Once);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(graph, It.Is<Location>(l => l.Name == "Third"), It.Is<Location>(l => l.Name == "Fourth")), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Empty latitude in location was allowed.")]
        public void TestIfGetTravelPlansThrowsExceptionWhenLocationLatitudeIsEmpty()
        {
            // Arrange
            var source = new Location("A");
            var destination = new Location
            {
                Longitude = 12.121
            };
            var travelOptions = new TravelOptions
            {
                Source = source,
                Destination = destination
            };

            // Act
            _travelPlanService.GetTravelPlans(travelOptions);

            // Assert
            _graphBuilderMock.Verify(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()), Times.Never);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(It.IsAny<Graph<int, StopTimeInfo>>(), It.IsAny<Location>(), It.IsAny<Location>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Empty longitude in location was allowed.")]
        public void TestIfGetTravelPlansThrowsExceptionWhenLocationLongitudeIsEmpty()
        {
            // Arrange
            var source = new Location("A");
            var destination = new Location
            {
                Latitude = 12.121
            };
            var travelOptions = new TravelOptions
            {
                Source = source,
                Destination = destination
            };

            // Act
            _travelPlanService.GetTravelPlans(travelOptions);

            // Assert
            _graphBuilderMock.Verify(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()), Times.Never);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(It.IsAny<Graph<int, StopTimeInfo>>(), It.IsAny<Location>(), It.IsAny<Location>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Empty name in location was allowed.")]
        public void TestIfGetTravelPlansThrowsExceptionWhenLocationNameIsEmpty()
        {
            // Arrange
            var source = new Location("A");
            var destination = new Location();
            var travelOptions = new TravelOptions
            {
                Source = source,
                Destination = destination
            };

            // Act
            _travelPlanService.GetTravelPlans(travelOptions);

            // Assert
            _graphBuilderMock.Verify(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()), Times.Never);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(It.IsAny<Graph<int, StopTimeInfo>>(), It.IsAny<Location>(), It.IsAny<Location>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Empty name in location was allowed.")]
        public void TestIfGetTravelPlansThrowsExceptionWhenSourceLocationNameIsEmpty()
        {
            // Arrange
            var source = new Location();
            var destination = new Location("B");
            var travelOptions = new TravelOptions
            {
                Source = source,
                Destination = destination
            };

            // Act
            _travelPlanService.GetTravelPlans(travelOptions);

            // Assert
            _graphBuilderMock.Verify(p => p.GetGraph(It.IsAny<DateTime>(), It.IsAny<CoordinateLimits>()), Times.Never);
            _pathProviderMock.Verify(p => p.GetAvailablePaths(It.IsAny<Graph<int, StopTimeInfo>>(), It.IsAny<Location>(), It.IsAny<Location>()), Times.Never);
        }
    }
}