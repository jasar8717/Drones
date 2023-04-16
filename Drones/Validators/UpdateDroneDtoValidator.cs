using Drones.Core.Dto;
using FluentValidation;

namespace Drones.Api.Validators
{
    public class UpdateDroneDtoValidator : AbstractValidator<UpdateDroneDto>
    {
        public UpdateDroneDtoValidator()
        {
            RuleFor(a => a.SerialNumber)
                .NotEmpty().WithMessage("Serial Number is required.")
                .Length(1, 100).WithMessage("Serial Number must have between 1 and 100 characters.");
            RuleForEach(x => x.MedicationDtoList).NotNull().WithMessage("Medications is required.");
            RuleForEach(x => x.MedicationDtoList).ChildRules(medication =>
            {
                medication.RuleFor(x => x.Weight).NotNull().WithMessage("Weight is required.").GreaterThan(0).WithMessage("Weight must be greather than 0");
                medication.RuleFor(x => x.Name).NotNull().WithMessage("Name is required.").Matches("^[a-zA-Z0-9_-]*$").WithMessage("Character not allowed.");
                medication.RuleFor(x => x.Code).NotNull().WithMessage("Code is required.").Matches("^[A-Z0-9_]*$").WithMessage("Character not allowed.");
            });
        }
    }
}
