using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using WebApi.Model.Branch;
using WebApi.Model.Room;
using WebApi.Model.User;

namespace WebApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Landlord, UserProfileModel>();
            CreateMap<Tenant, UserProfileModel>();
            CreateMap<Admin, UserProfileModel>();
            CreateMap<Branch, BranchModel>().ForMember(
                                                    p => p.HouseType,
                                                    options => options.MapFrom(s => s.HouseType==HouseType.Row ? "row" : "floor")
                                                    );
            CreateMap<ServiceModel, Service>().ForMember(p => p.ServiceName,options => options.MapFrom(s => s.Name)); 
            CreateMap<BranchCreateModel, Branch>().ForMember( 
                                                    p => p.HouseType,
                                                    options => options.MapFrom(s=> s.HouseType=="floor" ? HouseType.Floor : HouseType.Row )
                                                    );
            CreateMap<Area, AreaModel>();
            CreateMap<Room, RoomModel>();
            CreateMap<RoomCreateModel, Room>();
            CreateMap<DeviceCreateModel, Device>();
            CreateMap<Device, DeviceModel>();
            CreateMap<ImageRoom, ImageRoomModel>();
        }
    }
}