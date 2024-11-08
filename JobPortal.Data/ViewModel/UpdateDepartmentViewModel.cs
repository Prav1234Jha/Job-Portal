using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JobPortal.Data.ViewModel
{
    public class UpdateDepartmentViewModel
    {
        [Required(ErrorMessage = "Please enter Department name")]
        [StringLength(50, ErrorMessage = "The Department name cannot be more than 50 characters.")]
        public string DepartmentName { get; set; }
    }
}
