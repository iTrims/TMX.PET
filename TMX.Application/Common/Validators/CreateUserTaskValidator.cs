using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.Domain.Entities;

namespace TMX.TaskService.Application.Common.Validators
{
    public class CreateUserTaskValidator : AbstractValidator<CreateUserTaskDto>
    {
        public CreateUserTaskValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithName("Title");
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(task => task.DueDate)
               .Must(date => date >= DateTime.Today)
               .WithMessage("Due date cannot be earlier than today")
               .When(x => x.DueDate.HasValue);
        }
    }
}
