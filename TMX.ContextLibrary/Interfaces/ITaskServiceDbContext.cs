using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Domain.Entities;

namespace TMX.ContextLibrary.Interfaces
{
    public interface ITaskServiceDbContext
    {
        DbSet<UserTask> UserTasks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
