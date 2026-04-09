using AutoMapper;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;

namespace TaskManagementApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BaseUser, AddUserDTO>().ReverseMap();
            CreateMap<BaseUser, UpdateUserDTO>().ReverseMap();
            CreateMap<BaseUser, GetUserDTO>().ReverseMap();
            CreateMap<BaseProject, GetProjectDTO>().ReverseMap();
            CreateMap<BaseProject, AddProjectDTO>().ReverseMap();
            CreateMap<BaseProject, UpdateProjectDTO>().ReverseMap();
            CreateMap<BaseTask, AddTaskDTO>().ReverseMap();
            CreateMap<BaseTask, UpdateTaskDTO>().ReverseMap();
            CreateMap<BaseTask, GetTaskDTO>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ReverseMap();
            CreateMap<TaskComment, GetTaskCommentDTO>().ReverseMap();
            CreateMap<TaskComment, AddTaskCommentDTO>().ReverseMap();
            CreateMap<ImageUpload, AddImageRequestDTO>().ReverseMap();
        }
    }
}
