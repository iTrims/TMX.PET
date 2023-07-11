using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.ContextLibrary.Contexts;

namespace TMX.TaskService.Persistence
{
    public class DbInitializer
    {
        public static void DbInitialize(TaskServiceDbContext context)
        {
            context.Database.EnsureCreated();
        } 
    }
}
