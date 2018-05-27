using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;

namespace Bim.Domain.Ifc
{
    public enum UnitName
    {
        MILLIMETRE,
        SQUAREMETRE,
        CUBICMETRE,
        SQUAREFOOT,
        CUBICFOOT,
        FOOT,
        METRE,
    }
    public class IfUnit : IfObject
    {
        public IfModel IfModel { get; set; }
        public UnitName LengthUnit { get; set; }
        public UnitName AreaUnit { get; set; }
        public UnitName VolumeUnit { get; set; }

        public IfUnit(IfModel ifModel)
        {
            IfModel = ifModel;
            Initialize();
            IfModel.Instances.Add(this);

        }

        private void Initialize()
        {
            var unitCtx = (IfcUnitAssignment)IfModel.IfcStore.Instances.OfType<IIfcProject>()
                .FirstOrDefault().UnitsInContext;

            //set Length unit
            if (unitCtx.LengthUnitName.Contains("MILLI"))
            {
                LengthUnit = UnitName.MILLIMETRE;
            }
            else if (unitCtx.LengthUnitName.Contains("FOOT"))
            {
                LengthUnit = UnitName.FOOT;
            }
            else
            {
                LengthUnit = UnitName.METRE;
            }

            //set area unit
            if (unitCtx.AreaUnit.FullName.Contains("SQUAREMETRE"))
            {
                AreaUnit = UnitName.SQUAREMETRE;
            }
            else
            {
                AreaUnit = UnitName.SQUAREFOOT;
            }

            //Set volume Unit
            if (unitCtx.VolumeUnit.FullName.Contains("CUBICMETRE"))
            {
                VolumeUnit = UnitName.CUBICMETRE;
            }
            else
            {
                VolumeUnit = UnitName.CUBICFOOT;
            }
            //VolumeUnit = unitCtx.VolumeUnit.FullName;

        }






    }
}
