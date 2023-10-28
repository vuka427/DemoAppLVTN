using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using WebApi.Model.Branch;
using WebApi.Model.Contract;
using WebApi.Model.Invoice;
using WebApi.Model.Room;
using WebApi.Model.RoomIndex;
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
            CreateMap<RoomEditModel, Room>();
            CreateMap<DeviceEditModel, Device>();

            //room index
            CreateMap<Branch,BranchForIndexModel>().ForMember(
                                                    p => p.HouseType,
                                                    options => options.MapFrom(s => s.HouseType==HouseType.Row ? "row" : "floor")
                                                    ); 
            CreateMap<Area,AreaForIndexModel>();

            CreateMap<Room, RoomForIndexModel>()
				 .ForMember(p => p.TenantName, options => options.MapFrom(c => (c.Contracts.FirstOrDefault(a=>a.Status==ContractStatus.Active).B_Lessee)??""))
				 .ForMember(p => p.Status, options => options.MapFrom(c => (c.Contracts.FirstOrDefault(a => a.Status==ContractStatus.Active).Status ==ContractStatus.Active)?true:false));


			//contract
			CreateMap<ContractCreateModel, Contract>();
            CreateMap<Contract, ContractModel>()
                .ForMember(p => p.A_DateOfBirth,options => options.MapFrom(c=>c.A_DateOfBirth.ToShortDateString()))
                .ForMember(p => p.A_DateOfIssuance, options => options.MapFrom(c => c.A_DateOfIssuance.ToShortDateString()))
                .ForMember(p => p.B_DateOfBirth, options => options.MapFrom(c => c.B_DateOfBirth.ToShortDateString()))
                .ForMember(p => p.B_DateOfIssuance, options => options.MapFrom(c => c.B_DateOfIssuance.ToShortDateString()))
                .ForMember(p => p.Status, options => options.MapFrom(c => c.Status.ToString()))
                .ForMember(p => p.CommencingOn, options => options.MapFrom(c => c.CommencingOn.ToShortDateString()))
                .ForMember(p => p.EndingOn, options => options.MapFrom(c => c.EndingOn.ToShortDateString()))
                .ForMember( p => p.HouseType, options => options.MapFrom(s => s.HouseType==HouseType.Row ? "row" : "floor"))

                 ;
			CreateMap<Contract, ContractDetailModel>()
				.ForMember(p => p.B_DateOfBirth, options => options.MapFrom(c => c.B_DateOfBirth.ToShortDateString()))
				.ForMember(p => p.B_DateOfIssuance, options => options.MapFrom(c => c.B_DateOfIssuance.ToShortDateString()))
				.ForMember(p => p.Status, options => options.MapFrom(c => c.Status.ToString()))
				.ForMember(p => p.CommencingOn, options => options.MapFrom(c => c.CommencingOn.ToShortDateString()))
				.ForMember(p => p.EndingOn, options => options.MapFrom(c => c.EndingOn.ToShortDateString()))
				.ForMember(p => p.HouseType, options => options.MapFrom(s => s.HouseType==HouseType.Row ? "row" : "floor"))

				 ;

            // invoice 
            CreateMap<Invoice, InvoiceModel>();
            CreateMap<Service, ServiceItemModel>();
            CreateMap<ServiceItemModel, ServiceItem>();
			CreateMap<ServiceItem, ServiceItemModel>();
			CreateMap<Invoice, InvoiceDataTableModel>()
				.ForMember(p => p.Lessee, options => options.MapFrom(s => s.Contract.B_Lessee)) 
                .ForMember(p => p.RoomNumber , options => options.MapFrom(s =>"P."+ s.Contract.RoomNumber +  ((s.Contract.HouseType==HouseType.Row)? ", dãy " : ", tầng ") + s.Contract.AreaName   ))
				.ForMember(p => p.BranchName, options => options.MapFrom(s => s.Contract.BranchName))
				;


		}
    }
}