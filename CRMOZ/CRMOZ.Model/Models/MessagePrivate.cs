using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("MessagePrivates")]
    public class MessagePrivate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public string Content { set; get; }

        [StringLength(128)]
        public string FromID { set; get; }

        [StringLength(128)]
        public string ReceiveID { set; get; }

        [StringLength(256)]
        public string FromFullName { set; get; }

        [StringLength(256)]
        public string FromAvatar { set; get; }

        public DateTime CreatedDate { set; get; }

        [StringLength(50)]
        public string StrDateTime { set; get; }

        public bool IsFile { set; get; }

        [ForeignKey("FromID")]
        public virtual HubUser HubUser { set; get; }
    }
}