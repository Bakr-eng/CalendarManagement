using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Application.Services;
using CalendarManagement.Domain.Entities;
using Moq;

namespace CalendarManagement.Tests.Services
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _repositoryMock;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            // Körs före varje test – varje test får en ny, tom mock
            _repositoryMock = new Mock<ILocationRepository>();
            _service = new LocationService(_repositoryMock.Object);
        }


        // ---------- GetAllAsync ----------
        [Fact]
        public async Task GetAllAsync_ReturnsAllLocations()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Location 1" },
                new Location { Id = 2, Name = "Location 2" }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(locations);
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Location 1", result[0].Name);
            Assert.Equal("Location 2", result[1].Name);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoLocations_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Location>());
            // Act
            var result = await _service.GetAllAsync();
            // Assert
            Assert.Empty(result);
        }


        // ---------- GetByIdAsync ----------
        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsLocation()
        {
            // Arrange
            var location = new Location { Id = 1, Name = "Location 1" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(location);
            // Act
            var result = await _service.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Location 1", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Location?)null);
            // Act
            var result = await _service.GetByIdAsync(999);
            // Assert
            Assert.Null(result);
        }


        // ---------- CreateAsync ----------
        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsCreatedLocation()
        {
            // Arrange
            var request = new CreateLocationRequest("Location 1", "City 1", "Address 1");
            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            // Act
            var result = await _service.CreateAsync(request);
            // Assert
            Assert.Equal("Location 1", result.Name);
            Assert.Equal("City 1", result.City);
            Assert.Equal("Address 1", result.Address);

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CreateAsync_EmptyName_ThrowsArgumentException(string? name)
        {
            // Arrange
            var request = new CreateLocationRequest(name!, "City 1", null);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CreateAsync_EmptyCity_ThrowsArgumentException(string? city)
        {
            // Arrange
            var request = new CreateLocationRequest("Location 1", city!, null);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }



    }
}
