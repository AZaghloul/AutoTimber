using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.General
{
    public class DesignOptions
    {
        [Key]
        public Guid ProjectId { get; set; }
        public Option FrameWalls { get; set; }
        public Option FrameFloors { get; set; }
        public Option FrameRafter { get; set; }
        public Option DetectExternalWalls { get; set; }
        public Option OptimizeWalls { get; set; }
        public Option OptimizeFloors { get; set; }
        public Option OptimizeRafter { get; set; }
        public Option Exclude { get; set; }
        public Option DeleteArcWalls { get; set; }
    }
}
