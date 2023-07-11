using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.ContextLibrary.EntityTypeConfiguration;
using TMX.ContextLibrary.Interfaces;
using TMX.TaskService.Domain.Entities;

namespace TMX.ContextLibrary.Contexts
{
    public class TaskServiceDbContext : DbContext, ITaskServiceDbContext
    {
        public DbSet<UserTask> UserTasks { get ; set ; }

        public TaskServiceDbContext(DbContextOptions<TaskServiceDbContext> options)
            :base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
            base.OnModelCreating(modelBuilder);
        }


    }
}
