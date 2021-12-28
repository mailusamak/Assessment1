using Assessment1.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Validation
{
    public class attributesValidator : AbstractValidator<attributes>
    {
        public attributesValidator()
        {
            RuleFor(x => x.key).NotEmpty().WithMessage("Attributes Key Required");
            RuleFor(x => x.value).NotEmpty().WithMessage("Attributes value Required");
        }
    }
}
