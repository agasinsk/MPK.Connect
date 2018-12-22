using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business;

namespace MPK.Connect.Test.Service.Business
{
    [TestClass]
    public class PathComparerTests
    {
        private PathComparer _pathComparer;

        [TestMethod]
        public void TestIfEqualsReturnsTrueWhenBothPathsAreEmpty()
        {
            // Arrange
            var x = new Path<StopTimeInfo>();
            var y = new Path<StopTimeInfo>();
            // Act
            var result = _pathComparer.Equals(x, y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestIfEqualsReturnsTrueWhenPathsAreEqual()
        {
            // Arrange
            var x = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };
            var y = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };

            // Act
            var result = _pathComparer.Equals(x, y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestIfEqualsReturnsFalseWhenPathsAreNotEqual()
        {
            // Arrange
            var x = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };
            var y = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1.5),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };

            // Act
            var result = _pathComparer.Equals(x, y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestIfEqualsReturnsFalseWhenRoutesAreNotEqual()
        {
            // Arrange
            var x = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };
            var y = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "B"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "D"
                }
            };

            // Act
            var result = _pathComparer.Equals(x, y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestInitialize]
        public void SetUp()
        {
            _pathComparer = new PathComparer();
        }
    }
}