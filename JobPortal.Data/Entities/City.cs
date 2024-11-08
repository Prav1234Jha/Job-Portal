using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Data.Entities
{
    public class City
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter City name")]
        [StringLength(100, ErrorMessage = "The City name cannot be more than 100 characters.")]
        public string Name { get; set; }
        public Category? Category { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public bool? Disable { get; set; }
        [Required]
        public string Slug { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public int Popular { get; set; }
    }
}
