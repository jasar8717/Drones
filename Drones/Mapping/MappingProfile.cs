using AutoMapper;
using Drones.Core;
using Drones.Core.Dto;
using Drones.Core.Utils;
using Drones.Entities.Models;

namespace Drones.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SaveDroneDto, Drone>()
                .ForMember(dest => dest.Model, act => act.MapFrom(src => (int)Enum.Parse(typeof(DroneModelEnum), src.Model)))
                .ForMember(dest => dest.State, act => act.MapFrom(src => (int)Enum.Parse(typeof(DroneStateEnum), src.State)));
            CreateMap<UpdateDroneDto, Drone>();
            CreateMap<SaveMedicationDto, Medication>()
                .ForMember(dest => dest.Image, act => act.Ignore());
            CreateMap<Medication, MedicationDto>();
        }
    }
}
