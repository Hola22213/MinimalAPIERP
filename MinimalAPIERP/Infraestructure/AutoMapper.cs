using AutoMapper;
using ERP.Data;
using ERP.Api.Models;
using ERP; 

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>(); 
    }
}
