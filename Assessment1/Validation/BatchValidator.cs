using Assessment1.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Validation
{
    public class BatchValidator : AbstractValidator<Batch>
    {
        public BatchValidator()
        {
            RuleFor(x => x.BusinessUnitId).NotNull().NotEmpty().NotEqual(0).WithMessage("BusinessUnit Required");
            RuleFor(x => x.expiryDate).NotNull().NotEmpty().WithMessage("BusinessUnit Required");

        }
    }
}
