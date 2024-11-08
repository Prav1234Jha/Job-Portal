using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JobPortal.Data.Entities
{
    public class tblSystem
    {
        [Key]
        public int SystemId { get; set; }

        [Required(ErrorMessage = "Please enter System name")]
        [StringLength(100, ErrorMessage = "The System name cannot be more than 100 characters.")]

        [DisplayName("System Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Description name")]
        [StringLength(100, ErrorMessage = "The Description name cannot be more than 100 characters.")]
        public String Description { get; set; }

        public bool IsActive { get; set; }
        
    }
}
