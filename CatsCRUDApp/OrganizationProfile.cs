using AutoMapper;
using BLL.Entities;
using CatsCRUDApp.Models;
using DAL.CacheAllocation;

namespace CatsCRUDApp
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<CatViewModel, Cat>();
            CreateMap<Cat, CatViewModel>();
            CreateMap<Cat, CatStreamModel>();
            CreateMap<CatStreamModel, Cat>();
        }
    }
}
