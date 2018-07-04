using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("UserMessagePrivates")]
    public class UserMessagePrivate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public string FromUserID { set; get; }
        public string RecieveUserID { set; get; }
        public int NewMessage { set; get; }
    }
}