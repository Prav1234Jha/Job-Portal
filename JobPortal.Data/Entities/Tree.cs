using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JobPortal.Data.Entities
{
    public class tblTree
    {
        [Key]
        public int TreeId { get; set; }

        [Required(ErrorMessage = "Please enter Tree name")]
        [StringLength(100, ErrorMessage = "The Tree name cannot be more than 100 characters.")]
        [DisplayName("Tree Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Tree Description name")]
        [StringLength(100, ErrorMessage = "The Tree Description name cannot be more than 100 characters.")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Please enter NodeUrl name")]
        [StringLength(100, ErrorMessage = "The NodeUrl name cannot be more than 100 characters.")]

        [DisplayName("Node URL")]
        public String NodeUrl { get; set; }

        public int SystemId { get; set; }
        public int ModuleId { get; set; }

        public bool IsActive { get; set; }

        public tblSystem system { get; set; }
        public tblModule module { get; set; }
    }
}
