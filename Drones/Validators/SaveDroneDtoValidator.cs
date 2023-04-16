using Drones.Core.Dto;
using FluentValidation;

namespace Drones.Api.Validators
{
    public class SaveDroneDtoValidator : AbstractValidator<SaveDroneDto>
    {
        public SaveDroneDtoValidator()
        {
            RuleFor(a => a.SerialNumber)
                .NotEmpty().WithMessage("Serial Number is required.")
                .Length(1, 100).WithMessage("Serial Number must have between 1 and 100 characters.");
            RuleFor(a => a.BatteryCapacity)
                .NotEmpty().WithMessage("Battery Capacity is required.")
                .InclusiveBetween(1,100).WithMessage("Battery Capacity must be a value less than 100.");
            RuleFor(a => a.WeightLimit)
                .NotEmpty().WithMessage("Weight limit is required.")
                .InclusiveBetween(1,500).WithMessage("Weight limit must be a value less than 500g.");
            RuleFor(a => a.Model)
                .NotEmpty().WithMessage("Model is required.");
            RuleFor(a => a.State)
                .NotEmpty().WithMessage("State is required.");
            RuleFor(a => a.Medications.Sum(x => x.Weight))
                .LessThanOrEqualTo(x => x.WeightLimit)
                .When(a => a.Medications.Any())
                .WithMessage("Medications sum weight must be less than drone weight limit.");
            RuleForEach(x => x.Medications).ChildRules(medication =>
            {
                medication.RuleFor(x => x.Weight).NotNull().WithMessage("Weight is required.").GreaterThan(0).WithMessage("Weight must be greather than 0"); ;
                medication.RuleFor(x => x.Name).NotNull().WithMessage("Name is required.").Matches("^[a-zA-Z0-9_-]*$").WithMessage("Character not allowed.");
                medication.RuleFor(x => x.Code).NotNull().WithMessage("Code is required.").Matches("^[A-Z0-9_]*$").WithMessage("Character not allowed.");
            });
        }
    }
}
