using CRMOZ.Model.Models;

namespace CRMOZ.Web.Models
{
    public class HubUserViewModel
    {
        public string ID { set; get; }

        public string Email { set; get; }

        public string UserName { set; get; }

        public string FullName { set; get; }

        public string Avatar { set; get; }

        //public Connection ConnectionId { set; get; }

        public bool Connected { set; get; }

        public int CountNew { set; get; }
    }
}