using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Data.Entities
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Department name")]
        [StringLength(100, ErrorMessage = "The Department name cannot be more than 100 characters.")]
        public string DepartmentName { get; set; }
        public Category? Category { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public bool? Disable { get; set; }
        [Required]
        public string Slug { get; set; }
        public int Popular { get; set; }
       //public List<Category> lstCategories { get; set; }
    }
}
