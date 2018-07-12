using Bim.Domain.General;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoTimber.DB.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        [DisplayName("Status")]
        public DesignState DesignState { get; set; }
        [DisplayName("Share To Gallery")]
        public bool AddToGallery { get; set; }
        public DesignOptions DesignOptions { get; set; }
        public WoodSetup WoodSetup { get; set; }
    }

}