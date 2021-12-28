using Assessment1.ModelView;
using Assessment1.ConcreteClass;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assessment1.Models;

namespace Assessment1.Validation
{
    public class BatchVMValidator : AbstractValidator<BatchVM>
    {
        public BatchVMValidator()
        {
            RuleFor(x => x.BusinessUnit).NotNull().NotEmpty().WithMessage("BusinessUnit Required!!!");
            //RuleForEach(x => x.attributes).NotNull().NotEmpty().WithMessage("readUsers cannot be blank!!!");
            RuleForEach(x => x.acl.readUsers).NotEmpty().WithMessage("readUsers cannot be blank!!!");
            RuleForEach(x => x.acl.readGroups).NotEmpty().WithMessage("readGroups cannot be blank!!!");
            RuleFor(x => x.expiryDate).NotNull().NotEmpty().WithMessage("expiryDate Required!!!")
                .Must(IsDateTime).WithMessage("Invalid expiryDate");

            RuleForEach(x => x.attributes).NotNull().NotEmpty().WithMessage("readUsers cannot be blank!!!")
                .Must(BeAValidKeyValue).WithMessage("Key and Value cannot be blank");
        }

        private bool BeAValidKeyValue(attributes value)
        {
            bool retval = true;
            if (string.IsNullOrWhiteSpace(value.key))
                retval = false;
            if (string.IsNullOrWhiteSpace(value.value))
                retval = false;
            return retval;
        }

        private bool BeAValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }
        public bool IsDateTime(string value)
        {
            DateTime fromDateValue;
            //var formats = new[] { "yyyy-MM-dd","MM/dd/yyyy", "dd/MM/yyyy h:mm:ss", "MM/dd/yyyy hh:mm tt", "yyyy'-'MM'-'dd'T'HH':'mm':'ss'" };
            var formats = new[] { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy h:mm:ss", "MM/dd/yyyy hh:mm tt", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", "yyyy-MM-dd HH:mm:ss.fff" };
            bool b = DateTime.TryParseExact(value, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue);
            return b;
        }

    }
}
