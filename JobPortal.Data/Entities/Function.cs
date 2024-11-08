using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JobPortal.Data.Entities
{
    public class tblFunction
    {
        [Key]
        public int FunctionId { get; set; }

        [Required(ErrorMessage = "Please enter Function name")]
        [StringLength(100, ErrorMessage = "The Function name cannot be more than 100 characters.")]
        [DisplayName("Function Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Function Description name")]
        [StringLength(100, ErrorMessage = "The Function Description name cannot be more than 100 characters.")]
        public String Description { get; set; }

        public int SystemId { get; set; }
        public int ModuleId { get; set; }

        public bool IsActive { get; set; }

        public tblSystem system { get; set; }
        public tblModule module { get; set; }

    }
}
