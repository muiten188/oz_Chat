using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [StringLength(256)]
        public string Name { set; get; }

        public virtual ICollection<MessageGroup> MessageGroups { set; get; }
    }
}