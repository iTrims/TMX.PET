using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TMX.TaskService.Domain.Enums;

namespace TMX.TaskService.Domain.Entities
{
    public class UserTask : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } 
        public string? Description { get; set; }
        public UserTaskStatus Status { get; set; }
        public UserTaskPriority Priority { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public bool NotificationSent { get; set; } = false;
            
    }
}
