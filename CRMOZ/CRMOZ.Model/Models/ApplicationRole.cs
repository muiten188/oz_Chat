using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRMOZ.Model.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }

        [StringLength(256)]
        public string Description { set; get; }

        public int? OrderBy { set; get; }

        public bool Status { set; get; }

        [DefaultValue(false)]
        public bool IsDelete { set; get; }

        [DefaultValue(false)]
        public bool IsDefault { set; get; }
    }
}