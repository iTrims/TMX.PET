using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Domain.Entities;

namespace TMX.ContextLibrary.EntityTypeConfiguration
{
    public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.Priority).HasMaxLength(32);
            builder.Property(x => x.Status).HasMaxLength(32);
        }
    }
}
