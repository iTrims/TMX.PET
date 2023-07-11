using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.ContextLibrary.Contexts;
using TMX.TaskService.Application.Common.Mappings;
using TMX.Tests.UserTasks;

namespace TMX.Tests.Common
{
    public abstract class TestBase : IDisposable
    {
        protected readonly TaskServiceDbContext TaskServiceContext;
        protected readonly IHttpContextAccessor ContextAccessor;
        protected readonly IMapper Mapper = new MapperConfiguration(cfg => {
            cfg.AddProfile<UserTaskProfile>();
        }).CreateMapper();

        public TestBase()
        {
            TaskServiceContext = TaskServiceContextFactory.CreateDbContext("First");
            ContextAccessor = 
                TaskServiceContextFactory.CreateHttpContextAccessor(TaskServiceContextFactory.UserAId);

        }
        public void Dispose() 
        { 
            TaskServiceContextFactory.Destroy(TaskServiceContext);
        } 
    }
}
