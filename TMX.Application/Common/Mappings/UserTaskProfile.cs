using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.Domain.Entities;

namespace TMX.TaskService.Application.Common.Mappings
{
    public class UserTaskProfile : Profile
    {
        public UserTaskProfile()
        {
            CreateMap<UserTask, UserTaskDto>().ReverseMap();
            CreateMap<UserTask, CreateUserTaskDto>().ReverseMap();
            CreateMap<UserTask, UpdateUserTaskDto>().ReverseMap();
        }

    }
}
