using System.Collections.Generic;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;
using api.models.dtos;
using api.services;
using Moq;
using Xunit;

namespace api.tests
{
    public class StationServiceTests
    {
        private readonly Mock<IStationRepository> _mockStationRepository;
        private readonly StationService _stationService;

        public StationServiceTests()
        {
            _mockStationRepository = new Mock<IStationRepository>();
            _stationService = new StationService(_mockStationRepository.Object);
        }

        [Fact]
        public async Task CreateStationAsync_WhenInsertingInMiddle_ShouldReorderSubsequentStations()
        {
            // Arrange
            var station1 = new Station { StationName = "Station 1", Order = 1 };
            var station2 = new Station { StationName = "Station 2", Order = 2 };
            var initialStations = new List<Station> { station1, station2 };

            var newStationDto = new CreateStationDto
            {
                StationName = "New Station In Middle",
                Order = 2
            };

            _mockStationRepository.Setup(r => r.StationNameExistsAsync(newStationDto.StationName)).ReturnsAsync(false);
            _mockStationRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(initialStations);

            // Act
            await _stationService.CreateStationAsync(newStationDto);

            // Assert
            Assert.Equal(3, station2.Order);
            _mockStationRepository.Verify(r => r.UpdateAsync(station2), Times.Once);
            _mockStationRepository.Verify(r => r.AddAsync(It.Is<Station>(s => s.Order == 2)), Times.Once);
        }
    }
}