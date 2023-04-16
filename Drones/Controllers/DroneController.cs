using AutoMapper;
using Drones.Api.Validators;
using Drones.Core;
using Drones.Core.Dto;
using Drones.Core.Services;
using Drones.Core.Utils;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Drones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController : ControllerBase
    {
        private readonly IDroneService _droneService;
        private readonly IMedicationService _medicationService;
        private readonly IBaseService _baseService;
        private readonly IMapper _mapper;

        public DroneController(IBaseService baseService, IDroneService droneService,  IMapper mapper, IMedicationService medicationService)
        {
            this._baseService = baseService;
            this._droneService = droneService;
            this._mapper = mapper;
            this._medicationService = medicationService;
        }

        /// <summary>
        /// Check battery Level given the drone serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>Drone Serial Number with battery level</returns>
        /// <response code="200">Returns the battery level</response>
        /// <response code="400">If errors</response>
        /// <response code="404">If serial number not found</response>
        [HttpGet("check-battery/{serialNumber}")]
        public async Task<ActionResult> CheckBatteryLevelFromDroneSerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            try
            {
                var drone = await _droneService.FindBySerialNumber(serialNumber, cancellationToken);

                if (drone == null)
                {
                    return NotFound("Not drone founded with that serial number");
                }

                var response = new
                {
                    SerialNumber = drone.SerialNumber,
                    BatteryLevelPercent = drone.BatteryCapacity,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok();
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }

        /// <summary>
        /// List available drones for loading medications
        /// </summary>
        /// <returns>List with all drones that can be loaded with medications</returns>
        /// <response code="200">Returns the battery level</response>
        /// <response code="400">If errors</response>
        [HttpGet("available-for-loading")]
        public async Task<ActionResult> ListAvailableDronesForLoading(CancellationToken cancellationToken)
        {
            try
            {
                var listAvailableDrones = await _droneService.ListAvailableDronesForLoading(cancellationToken);

                return Ok(listAvailableDrones);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok(new List<DroneResource>());
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }

        /// <summary>
        /// Get loaded medications given drone serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>List with all medications</returns>
        /// <response code="200">Returns the battery level</response>
        /// <response code="400">If errors</response>
        /// <response code="404">If serial number not found</response>
        [HttpGet("loaded-medications/{serialNumber}")]
        public async Task<ActionResult> GetLoadedDroneMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            try
            {
                var drone = await _droneService.FindDroneWithMedicationBySerialNumber(serialNumber, cancellationToken);

                if (drone == null)
                {
                    return NotFound("Not drone founded with that serial number");
                }

                var loadedMedicationDrone = _mapper.Map<List<Medication>, List<MedicationDto>>(drone.Medications.ToList());

                return Ok(loadedMedicationDrone);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok(new List<Medication>());
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }

        /// <summary>
        /// Register a new drone
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "serialNumber": "string",
        ///         "model": "(Lightweight, Middleweight, Cruiserweight, Heavyweight)",
        ///         "weightLimit": float,
        ///         "batteryCapacity": float,
        ///         "state": "(IDLE, LOADING, LOADED, DELIVERING, DELIVERED, RETURNING)",
        ///         "medications": [
        ///         {
        ///             "name": "string",
        ///             "weight": float,
        ///             "code": "string",
        ///             "image": "IFormFile" //for testing pourpose this value can be null
        ///         }
        ///         ]
        ///     }
        ///     {        
        /// </remarks>
        /// <param name="saveDroneDto"></param>
        /// <returns>A newly registered Drone</returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If model is not valid</response>          
        [HttpPost("register")]
        public async Task<ActionResult> RegisterDrone([FromBody] SaveDroneDto saveDroneDto, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new SaveDroneDtoValidator();
                var validationResult = await validator.ValidateAsync(saveDroneDto);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var countDrones = await _droneService.CountDroneRegistered(cancellationToken);

                if (countDrones >= 10)
                    return BadRequest(new Result("No more than 10 drones can be registered", "Error"));  //remove this if you want to register more than 10 drones to the fleet

                var droneToCreate = _mapper.Map<SaveDroneDto, Drone>(saveDroneDto);

                if(droneToCreate.State == (int)DroneStateEnum.LOADING && droneToCreate.BatteryCapacity < 25)
                    return BadRequest(new Result($"Drone with serial number: {droneToCreate.SerialNumber} is in state LOADING and battery level must be greater than 25", "Error"));

                if (_droneService.CheckIfSerialNumberExist(droneToCreate.SerialNumber, cancellationToken))
                    return BadRequest(new Result($"Drone with serial number: {droneToCreate.SerialNumber} exist", "Error"));

                await _droneService.Create(droneToCreate, cancellationToken);

                await _baseService.SaveChanges(cancellationToken);

                return Ok(new Result("Drone Created", "Successful"));                
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok(new Result("Operation canceled"));
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }

        /// <summary>
        /// Load drone with medications
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///     {        
        ///         "serialNumber": "string",
        ///         "medicationDtoList": [
        ///         {
        ///             "name": "string",
        ///             "weight": float,
        ///             "code": "string",
        ///             "image": "IFormFile" //for testing pourpose this value can be null
        ///         }
        ///         ]
        /// </remarks>
        /// <param name="updateDroneDto"></param>
        /// <returns>NoContent</returns>
        /// <response code="204">Returns NoContent</response>
        /// <response code="400">If model is not valid</response>
        [HttpPut("load-drone")]
        public async Task<ActionResult> LoadDroneWithMedication([FromBody] UpdateDroneDto updateDroneDto, CancellationToken cancellationToken)
        {
            try
            {
                var droneToUpdate = await _droneService.FindDroneWithMedicationBySerialNumber(updateDroneDto.SerialNumber, cancellationToken);

                if (droneToUpdate == null)
                {
                    return NotFound(updateDroneDto);
                }

                var medicationWeight = updateDroneDto.MedicationDtoList.Sum(m => m.Weight);
                var medicationWeightLoaded = droneToUpdate.Medications.Sum(m => m.Weight);

                if(medicationWeight + medicationWeightLoaded > droneToUpdate.WeightLimit) 
                {
                    return BadRequest(new Result("The list of medications exceeds the drone's weight", "Error"));
                }

                if (droneToUpdate.BatteryCapacity < 25)
                {
                    return BadRequest(new Result("The drone's battery percent is under 25%", "Error"));
                }

                _mapper.Map<UpdateDroneDto, Drone>(updateDroneDto, droneToUpdate);

                List<Medication> medicationListFromDrone= new List<Medication>();

                foreach (var item in updateDroneDto.MedicationDtoList)
                {
                    Medication medication = new Medication();
                    medication.DroneId = droneToUpdate.Id;
                    _mapper.Map<SaveMedicationDto, Medication>(item, medication);
                    if (item.Image != null)
                    {
                        medication.Image = new byte[item.Image.Length];
                        using (var memoryStream = new MemoryStream(medication.Image))
                        {
                            item.Image.CopyTo(memoryStream);
                        }
                    }
                    medicationListFromDrone.Add(medication);
                }

                if(medicationWeight < droneToUpdate.WeightLimit)
                    droneToUpdate.State = (int)DroneStateEnum.LOADING;
                else
                    droneToUpdate.State = (int)DroneStateEnum.LOADED;

                await this._medicationService.AddDronesMedication(medicationListFromDrone, cancellationToken);

                this._droneService.Update(droneToUpdate, cancellationToken);

                await this._baseService.SaveChanges(cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok(new Result("Operation canceled"));
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }

        /// <summary>
        /// Get all drones
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List with all drones</returns>
        /// <response code="200">Returns the battery level</response>
        /// <response code="400">If errors</response>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DroneDto>>> GetAll([FromQuery] PaginationFilter filter, CancellationToken cancellationToken)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                PagedResponse<IEnumerable<DroneDto>> pageResponseFromServer = await _droneService.GetAllTransformedToDto(validFilter, cancellationToken);

                IEnumerable<DroneDto> itemResources = pageResponseFromServer.Data;

                return Ok(new PagedResponse<IEnumerable<DroneDto>>(itemResources, pageResponseFromServer.PageNumber, pageResponseFromServer.PageSize, pageResponseFromServer.TotalRecords));

            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        return Ok(new PagedResponse<IEnumerable<DroneDto>>(new List<DroneDto>(), 0, 0, 0));
                    default:
                        return BadRequest(new Result(ex.Message, "Error"));
                }
            }
        }
    }
}
