using api.data.repositories;
using api.models;
using api.services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace api.tests
{
    public class PartServiceTests
    {
        private readonly Mock<IPartRepository> _mockPartRepository;
        private readonly Mock<IStationRepository> _mockStationRepository;
        private readonly Mock<IFlowHistoryRepository> _mockHistoryRepository;
        private readonly PartService _partService;

        public PartServiceTests()
        {
            _mockPartRepository = new Mock<IPartRepository>();
            _mockStationRepository = new Mock<IStationRepository>();
            _mockHistoryRepository = new Mock<IFlowHistoryRepository>();

            _partService = new PartService(
                _mockPartRepository.Object,
                _mockStationRepository.Object,
                _mockHistoryRepository.Object);
        }

        [Fact]
        public async Task CreatePartAsync_WhenPartNumberAlreadyExists_ShouldThrowInvalidOperationException()
        {
            var existingPartNumber = "PN-123";
            _mockPartRepository.Setup(repo => repo.PartNumberExistsAsync(existingPartNumber))
                               .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _partService.CreatePartAsync(existingPartNumber, "Test Part")
            );

            Assert.Equal($"A part with number '{existingPartNumber}' already exists.", exception.Message);
        }

        [Fact]
        public async Task CreatePartAsync_WithValidData_ShouldCreateAndReturnPart()
        {
            var partNumber = "PN-VALID";
            var partName = "Valid Part";
            _mockPartRepository.Setup(repo => repo.PartNumberExistsAsync(partNumber))
                               .ReturnsAsync(false);

            var result = await _partService.CreatePartAsync(partNumber, partName);

            Assert.NotNull(result);
            Assert.Equal(partNumber, result.PartNumber);
            _mockPartRepository.Verify(repo => repo.AddAsync(It.IsAny<Part>()), Times.Once);
        }

        [Fact]
        public async Task MovePartAsync_WithValidPartInFirstStation_ShouldMoveToSecondStation()
        {
            var partNumber = "PN-001";
            var responsible = "Test User";
            var station1 = new Station { StationId = Guid.NewGuid(), Order = 1, IsActive = true, StationName = "Receiving" };
            var station2 = new Station { StationId = Guid.NewGuid(), Order = 2, IsActive = true, StationName = "Assembly" };
            var stations = new List<Station> { station1, station2 };
            var partToMove = new Part { PartId = Guid.NewGuid(), PartNumber = partNumber, PartName = "Test Part", CurrentStationId = station1.StationId };

            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(partNumber)).ReturnsAsync(partToMove);
            _mockStationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stations);

            var result = await _partService.MovePartAsync(partNumber, responsible);

            Assert.True(result);
            Assert.Equal(station2.StationId, partToMove.CurrentStationId);
            _mockPartRepository.Verify(repo => repo.UpdateAsync(partToMove), Times.Once);
            _mockHistoryRepository.Verify(repo => repo.AddAsync(It.IsAny<FlowHistory>()), Times.Once);
        }
        
        [Fact]
        public async Task MovePartAsync_FromLastStation_ShouldCompletePart()
        {
            var partNumber = "PN-002";
            var lastStation = new Station { StationId = Guid.NewGuid(), Order = 1, StationName = "Last Station" };
            var stations = new List<Station> { lastStation };
            var partToMove = new Part { PartId = Guid.NewGuid(), PartNumber = partNumber, PartName = "Test Part", CurrentStationId = lastStation.StationId };

            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(partNumber)).ReturnsAsync(partToMove);
            _mockStationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stations);

            var result = await _partService.MovePartAsync(partNumber, "Test User");

            Assert.True(result);
            Assert.True(partToMove.IsCompleted);
            Assert.Null(partToMove.CurrentStationId);
            _mockPartRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Part>()), Times.Once);
        }
        
        [Fact]
        public async Task MovePartAsync_WhenPartIsAlreadyCompleted_ShouldReturnFalse()
        {
            var partNumber = "PN-003";
            var completedPart = new Part { PartNumber = partNumber, PartName = "Completed Part", IsCompleted = true };
            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(partNumber)).ReturnsAsync(completedPart);

            var result = await _partService.MovePartAsync(partNumber, "Test User");

            Assert.False(result);
        }

        [Fact]
        public async Task MovePartAsync_WhenPartDoesNotExist_ShouldReturnFalse()
        {
            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(It.IsAny<string>()))
                               .ReturnsAsync((Part?)null);

            var result = await _partService.MovePartAsync("PN-FAKE", "Test User");

            Assert.False(result);
        }

        [Fact]
        public async Task DeletePartAsync_WithExistingPart_ShouldDeactivateAndReturnTrue()
        {
            var partNumber = "PN-004";
            var partToDelete = new Part { PartNumber = partNumber, PartName = "Part to Delete", IsActive = true };
            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(partNumber)).ReturnsAsync(partToDelete);
            
            var result = await _partService.DeletePartAsync(partNumber);
            
            Assert.True(result);
            Assert.False(partToDelete.IsActive);
            _mockPartRepository.Verify(repo => repo.UpdateAsync(partToDelete), Times.Once);
        }

        [Fact]
        public async Task DeletePartAsync_WhenPartDoesNotExist_ShouldReturnFalse()
        {
            _mockPartRepository.Setup(repo => repo.GetByNumberAsync(It.IsAny<string>()))
                               .ReturnsAsync((Part?)null);
            
            var result = await _partService.DeletePartAsync("PN-FAKE");
            
            Assert.False(result);
        }
    }
}