using Bim.Domain.General;
using System;
using System.ComponentModel.DataAnnotations;

namespace Algorithm.DB.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public DesignState DesignState { get; set; }
        public bool AddToGallery { get; set; }
        public DesignOptions DesignOptions { get; set; }
    }

}