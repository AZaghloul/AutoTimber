using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.Models
{
   public class WoodSetup
    {
        [Key]
        public Guid ProjectId { get; set; }
        public HeaderEnum HeaderSection { get; set; }
        public StudEnum StudSection { get; set; }
        public JoistEnum JoistSection { get; set; }
        public WoodGrade WoodGrade { get; set; }
        public WoodSpecies WoodSpecies { get; set; }

    }
}
