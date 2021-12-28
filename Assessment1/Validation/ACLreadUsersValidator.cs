using Assessment1.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Validation
{
    public class ACLreadUsersValidator : AbstractValidator<ACLreadUsers>
    {
        public ACLreadUsersValidator()
        {
            RuleFor(x => x.readUsers).NotNull().NotEmpty().WithMessage("readUsers cannot be blank!!!");
        }
    }
}
