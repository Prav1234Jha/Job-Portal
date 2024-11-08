using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JobPortal.Data.ViewModel
{
    public class UpdateCityModelView
    {
        [Required(ErrorMessage = "Please enter City name")]
        [StringLength(50, ErrorMessage = "The City name cannot be more than 50 characters.")]
        public string Name { get; set; }
    }
}
