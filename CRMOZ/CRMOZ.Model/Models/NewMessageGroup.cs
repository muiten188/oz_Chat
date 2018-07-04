using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("NewMessageGroups")]
    public class NewMessageGroup
    {
        [Key]
        [Column(Order = 1)]
        [StringLength(128)]
        public string UserID { set; get; }

        [Key]
        [Column(Order = 2)]
        public int GroupID { set; get; }

        [Key]
        [Column(Order = 3)]
        public int MessageID { set; get; }

        public int Count { set; get; }

        [ForeignKey("UserID")]
        public virtual HubUser HubUser { set; get; }

        [ForeignKey("GroupID")]
        public virtual Group Group { set; get; }

        [ForeignKey("MessageID")]
        public virtual MessageGroup MessageGroup { set; get; }
    }
}