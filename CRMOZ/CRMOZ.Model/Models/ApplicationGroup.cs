using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMOZ.Model.Models
{
    [Table("ApplicationGroups")]
    public class ApplicationGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }

        public int? OrderBy { set; get; }

        public bool Status { set; get; }

        [DefaultValue(false)]
        public bool IsDelete { set; get; }
    }
}