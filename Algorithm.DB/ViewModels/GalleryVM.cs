using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.ViewModels
{
   public class GalleryVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string Thumbnail { get; set; }
        public string UserName { get; set; }
    }
}
