using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Application.Services;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Application.Exceptions;
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
        public async Task CreateAsync_EmptyName_ThrowsValidationException(string? name)
        {
            // Arrange
            var request = new CreateLocationRequest(name!, "City 1", null);

            // Act + Assert
            await Assert.ThrowsAsync<RequestValidationException>(() => _service.CreateAsync(request));

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CreateAsync_EmptyCity_ThrowsValidationException(string? city)
        {
            // Arrange
            var request = new CreateLocationRequest("Location 1", city!, null);

            // Act + Assert
            await Assert.ThrowsAsync<RequestValidationException>(() => _service.CreateAsync(request));

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }



        // ---------- UpdateAsync ----------

        [Fact]
        public async Task UpdateAsync_ExistingLocation_UpdatesFieldsAndReturnsDto()
        {
            // Arrange
            var existing = new Location { Id = 3, Name = "Gammalt namn", City = "Gammal stad" };
            var request = new UpdateLocationRequest("  Avicii Arena  ", " Stockholm ", " Globentorget 2 ");
            Location? updatedLocation = null;

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            _repositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()))
                .Callback<Location, CancellationToken>((location, _) => updatedLocation = location)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(3, request);

            // Assert – det som returneras
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("Avicii Arena", result.Name);
            Assert.Equal("Stockholm", result.City);
            Assert.Equal("Globentorget 2", result.Address);

            // Assert – det som skickades till repositoryn
            Assert.NotNull(updatedLocation);
            Assert.Equal("Avicii Arena", updatedLocation.Name);

            _repositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingLocation_ReturnsNullAndDoesNotUpdate()
        {
            // Arrange
            var request = new UpdateLocationRequest("Avicii Arena", "Stockholm", null);

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Location?)null);

            // Act
            var result = await _service.UpdateAsync(999, request);

            // Assert
            Assert.Null(result);

            _repositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

       

       

        // ---------- DeleteAsync ----------

        [Fact]
        public async Task DeleteAsync_ExistingLocation_ReturnsTrue()
        {
            // Arrange
            _repositoryMock
                .Setup(repo => repo.DeleteAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingLocation_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }


    }
}
