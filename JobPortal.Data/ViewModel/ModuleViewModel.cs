using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Data.ViewModel
{
    public class ModuleViewModel
    {
        [Key]
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public bool IsActive { get; set; }
        public String ModuleUrl { get; set; }

    }
}
