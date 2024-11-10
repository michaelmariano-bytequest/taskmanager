using AutoMapper;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento para User e UserDTO
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Ignora PasswordHash no mapeamento

            // Mapeamento para Project e ProjectDTO
            CreateMap<Project, ProjectDTO>();

            // Mapeamento para TodoTask e TaskDTO (supondo que exista um TaskDTO)
            CreateMap<TodoTask, TodoTaskDTO>(); // Esse mapeamento assume que você criou um TaskDTO para o TodoTask

            // Mapeamento para History, se necessário (exemplo)
            CreateMap<History, HistoryDTO>(); // Esse mapeamento também assume que existe um HistoryDTO
        }
    }
}