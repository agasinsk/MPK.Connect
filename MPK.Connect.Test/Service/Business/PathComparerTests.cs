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

        [TestMethod]
        public void TestIfGetHashCodeIsTheSameWhenRoutesAreEqual()
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
                    DepartureTime = TimeSpan.FromHours(3),
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
                    DepartureTime = TimeSpan.FromHours(3),
                    Route = "D"
                }
            };

            // Act
            var resultX = _pathComparer.GetHashCode(x);
            var resultY = _pathComparer.GetHashCode(y);

            // Assert
            Assert.AreEqual(resultX, resultY);
        }

        [TestMethod]
        public void TestIfGetHashCodeIsDifferentWhenTimeIsDifferent()
        {
            // Arrange
            var x = new Path<StopTimeInfo>
            {
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(1.23),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(2),
                    Route = "A"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(3),
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
                    DepartureTime = TimeSpan.FromHours(3),
                    Route = "D"
                }
            };

            // Act
            var resultX = _pathComparer.GetHashCode(x);
            var resultY = _pathComparer.GetHashCode(y);

            // Assert
            Assert.AreNotEqual(resultX, resultY);
        }

        [TestMethod]
        public void TestIfGetHashCodeIsDifferentWhenRoutesAreDifferent()
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
                    Route = "B"
                },
                new StopTimeInfo
                {
                    DepartureTime = TimeSpan.FromHours(3),
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
                    DepartureTime = TimeSpan.FromHours(3),
                    Route = "D"
                }
            };

            // Act
            var resultX = _pathComparer.GetHashCode(x);
            var resultY = _pathComparer.GetHashCode(y);

            // Assert
            Assert.AreNotEqual(resultX, resultY);
        }

        [TestInitialize]
        public void SetUp()
        {
            _pathComparer = new PathComparer();
        }
    }
}