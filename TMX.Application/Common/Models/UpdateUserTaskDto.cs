﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Domain.Enums;

namespace TMX.TaskService.Application.Common.Models
{
    public class UpdateUserTaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public UserTaskStatus Status { get; set; }
        public UserTaskPriority Priority { get; set; }
        public DateTimeOffset? DueDate { get; set; }
    }
}
