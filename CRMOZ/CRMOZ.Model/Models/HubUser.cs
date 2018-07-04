using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("HubUsers")]
    public class HubUser
    {
        [Key]
        [StringLength(128)]
        public string ID { set; get; }

        [StringLength(256)]
        public string Email { set; get; }

        [StringLength(256)]
        public string UserName { set; get; }

        [StringLength(256)]
        public string FullName { set; get; }

        [StringLength(256)]
        public string Avatar { set; get; }

        public string ConnectionId { set; get; }

        public bool Connected { set; get; }

        public virtual ICollection<MessagePrivate> MessagePrivates { set; get; }
    }
}