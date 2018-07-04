using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("HubUserGroups")]
    public class HubUserGroup
    {
        [Key]
        [Column(Order = 1)]
        [StringLength(128)]
        public string UserID { set; get; }

        [Key]
        [Column(Order = 2)]
        public int GroupID { set; get; }

        public bool IsCreater { set; get; }

        [ForeignKey("UserID")]
        public virtual HubUser HubUser { set; get; }

        [ForeignKey("GroupID")]
        public virtual Group Group { set; get; }
    }
}