using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Application.Common.Models;

namespace TMX.TaskService.Application.Common.Validators
{
    public class UpdateUserTaskValidator : AbstractValidator<UpdateUserTaskDto>
    {
        public UpdateUserTaskValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().WithName("Title");
            RuleFor(task => task.DueDate)
               .Must(date => date >= DateTime.Today)
               .WithMessage("Due date cannot be earlier than today")
               .When(x => x.DueDate.HasValue);
        }
    }
}
