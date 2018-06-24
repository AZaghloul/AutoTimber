using Algorithm.DB.Models;
using Bim.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.ViewModels
{
    public class SettingsVM
    {
        public WoodSetup WoodSetup { get; set; }
        public DesignOptions DesignOptions { get; set; }
    }
}
