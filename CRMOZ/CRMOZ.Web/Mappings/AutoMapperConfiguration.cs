using AutoMapper;
using CRMOZ.Model.Models;

namespace CRMOZ.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configuration()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<ApplicationUser, HubUser>();
            });
        }
    }
}