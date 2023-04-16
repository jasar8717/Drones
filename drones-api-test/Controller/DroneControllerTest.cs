using AutoMapper;
using Drones.Api.Controllers;
using Drones.Api.Mapping;
using Drones.Core.Dto;
using Drones.Core.Services;
using Drones.Entities.Models;
using drones_api_test.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drones_api_test.Controller
{
    public class DroneControllerTest
    {
        private DroneController _controller;
        private IDroneService _service;
        private readonly Mock<IMedicationService> _mockMedicationRepo;
        private readonly Mock<IBaseService> _baseServiceMock;
        private static IMapper _mapper;

        public DroneControllerTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _service = new DroneServiceTest();
            _mockMedicationRepo = new Mock<IMedicationService>();
            _baseServiceMock = new Mock<IBaseService>();
            _controller = new DroneController(_baseServiceMock.Object, _service, mapper, _mockMedicationRepo.Object);
        }

        [Fact]
        public void GetBySerialNumber_UnknownSerialNumberPassed_ReturnsNotFoundResult()
        {
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var notFoundResult = _controller.CheckBatteryLevelFromDroneSerialNumber("7777", cancellationToken);
            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetBySerialNumber_NotExistingSerialNumberPassed_ReturnsOkResult()
        {
            // Arrange
            string serialNumber = "123";
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var okResult = _controller.CheckBatteryLevelFromDroneSerialNumber(serialNumber, cancellationToken);
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void RegisterDrone_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new SaveDroneDto()
            {
                SerialNumber = "123"
            };
            _controller.ModelState.AddModelError("Model", "Required");
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.RegisterDrone(nameMissingItem, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void RegisterDrone_ExistingSerialNumber_ReturnsBadRequest()
        {
            // Arrange
            var serialNumberDuplicatedItem = new SaveDroneDto()
            {
                SerialNumber = "123",
                Model = "Lightweight",
                State = "IDLE",
                BatteryCapacity = 100,
                WeightLimit = 500
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.RegisterDrone(serialNumberDuplicatedItem, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void RegisterDrone_BetteryLevelLowAndStateLoading_ReturnsBadRequest()
        {
            // Arrange
            var serialNumberDuplicatedItem = new SaveDroneDto()
            {
                SerialNumber = "123",
                Model = "Lightweight",
                State = "LOADING",
                BatteryCapacity = 20,
                WeightLimit = 500
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.RegisterDrone(serialNumberDuplicatedItem, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void RegisterDrone_ObjectValid_ReturnsOkResult()
        {
            // Arrange
            var serialNumberDuplicatedItem = new SaveDroneDto()
            {
                SerialNumber = "222",
                Model = "Lightweight",
                State = "IDLE",
                BatteryCapacity = 20,
                WeightLimit = 500,
                Medications = new List<SaveMedicationDto>() { new SaveMedicationDto() { Code = "ABC1", Name = "Hydrocodone", Weight = 10 } }
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var okResult = _controller.RegisterDrone(serialNumberDuplicatedItem, cancellationToken);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Add_MoreMedicationWeightThanAllowed_ReturnsBadRequest()
        {
            // Arrange
            var medicationList = new UpdateDroneDto()
            {
                SerialNumber = "123",
                MedicationDtoList = new List<SaveMedicationDto> {
                    new SaveMedicationDto() { Code = "ABC1", Name = "Hydrocodone", Weight = 600 }
                }
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.LoadDroneWithMedication(medicationList, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Add_MedicationToDroneWithBatteryLevelNotPermitted_ReturnsBadRequest()
        {
            // Arrange
            var medicationList = new UpdateDroneDto()
            {
                SerialNumber = "456",
                MedicationDtoList = new List<SaveMedicationDto> {
                    new SaveMedicationDto() { Code = "ABC1", Name = "Hydrocodone", Weight = 200 }
                }
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.LoadDroneWithMedication(medicationList, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Add_MedicationToDrone_ReturnsOkResult()
        {
            // Arrange
            var medicationList = new UpdateDroneDto()
            {
                SerialNumber = "123",
                MedicationDtoList = new List<SaveMedicationDto> {
                    new SaveMedicationDto() { Code = "ABC1", Name = "Hydrocodone", Weight = 60 }
                }
            };

            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var okResult = _controller.LoadDroneWithMedication(medicationList, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(okResult.Result);
        }

        [Fact]
        public void GetLoadedDroneMedication_NotExistingSerialNumber_ReturnsBadRequest()
        {
            // Arrange
            string serialNumber = "222";
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var badResponse = _controller.GetLoadedDroneMedicationBySerialNumber(serialNumber, cancellationToken);
            // Assert
            var objectResponse = badResponse.Result as NotFoundObjectResult;

            // Assert
            Assert.Equal(404, objectResponse.StatusCode);
        }

        [Fact]
        public void GetLoadedDroneMedication_ExistingSerialNumber_ReturnsOkResult()
        {
            // Arrange
            string serialNumber = "123";
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var okResult = _controller.GetLoadedDroneMedicationBySerialNumber(serialNumber, cancellationToken);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetListAvailableDrones_ReturnsOkResult()
        {
            // Act
            CancellationToken cancellationToken = default(CancellationToken);
            var okResult = _controller.ListAvailableDronesForLoading(cancellationToken);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
    }
}
