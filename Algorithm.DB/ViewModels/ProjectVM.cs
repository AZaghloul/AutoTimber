
using Algorithm.DB.Models;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.ViewModels
{
    class ProjectVM
    {
        public Guid Id { get; set; }
        public string Thumbnail { get; set; }
        public string FileName { get; set; }
        public DesignOptions DesignOptions { get; set; }
        public DesignState DesignState { get; set; }
        public bool AddedToGallery { get; set; }


    }
}
