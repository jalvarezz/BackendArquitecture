using AutoMapper;
using Core.Common;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using MyBoilerPlate.Web.Infrastructure.Models;
using MyBoilerPlate.Web.Models;

namespace MyBoilerPlate.Web.Api.Infrastructure.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        : this("MipeProfile")
        {
        }
        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            this.RecognizePrefixes("Config");
            this.CreateMap<EmployeeDTO, EmployeeViewModel>().ReverseMap();

            this.CreateMap<EmployeeTypeDTO, EmployeeTypeViewModel>().ReverseMap();
            this.CreateMap<EmployeeDTO, Employee>().ReverseMap();
            this.CreateMap<EmployeeTypeDTO, EmployeeType>().ReverseMap();

            this.CreateMap<PagedList<EmployeeDTO>, PagedListViewModel<EmployeeViewModel>>().ReverseMap();
        }
    }
}
