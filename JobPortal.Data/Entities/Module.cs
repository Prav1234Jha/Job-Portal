using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JobPortal.Data.Entities
{
    public class tblModule
    {
        [Key]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Please enter Module name")]
        [StringLength(100, ErrorMessage = "The Module name cannot be more than 100 characters.")]
        
        [DisplayName("Module Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Description name")]
        [StringLength(100, ErrorMessage = "The Description name cannot be more than 100 characters.")]
        public String Description { get; set; }

        [DisplayName("System Name")]
        public int SystemId { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter ModuleUrl name")]
        [StringLength(100, ErrorMessage = "The ModuleUrl name cannot be more than 100 characters.")]
        
        [DisplayName("Module URL")]
        public String ModuleUrl { get; set; }
        public tblSystem system { get; set; }


    }
}
