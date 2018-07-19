using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMOZ.Model.Models
{
        [Table("FcmConnection")]
        public class FcmConnection
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public Guid ID { set; get; }

            public string UserID { set; get; }

            public string DeviceToken { set; get; }

            [ForeignKey("UserID")]
            public virtual HubUser HubUser { set; get; }
        }
}
