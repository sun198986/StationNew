using AutoMapper;
using Station.Entity.DB2Admin;

namespace Station.Model.Sap_MaraDto
{
    public class Sap_MaraProfile : Profile
    {
        public Sap_MaraProfile ()
        {
            CreateMap<Sap_Mara, Sap_MaraDto>();
            CreateMap<Sap_MaraAddDto, Sap_Mara>();
            CreateMap<Sap_MaraUpdateDto, Sap_Mara>();
            CreateMap<Sap_MaraSearchDto, Sap_Mara>();
        }
    }
}