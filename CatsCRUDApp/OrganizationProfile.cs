using AutoMapper;
using BLL.Entities;
using CatsCRUDApp.Models;

namespace CatsCRUDApp
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<CatViewModel, Cat>();
            CreateMap<Cat, CatViewModel>();
            CreateMap<IEnumerable<Cat>, IEnumerable<CatViewModel>>();
            CreateMap<IEnumerable<CatViewModel>, IEnumerable<Cat>>();
        }
    }
}
