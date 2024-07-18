using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Web.Validations
{
    public class CustomDateRangeAttribute : ValidationAttribute
    {
      

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime?)validationContext.ObjectType.GetProperty("StartDate")?.GetValue(validationContext.ObjectInstance, null);
            var endDate = (DateTime?)value;

            var currentDate = DateTime.Today;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    return new ValidationResult("End Date must be greater than or equal to Start Date.");
                }

                if (startDate < currentDate || endDate < currentDate)
                {
                    return new ValidationResult("Both Start Date and End Date must be after today's date.");
                }
            }

            return ValidationResult.Success;
        }

    }
}
