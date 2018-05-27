using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.PresentationAppearanceResource;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Bim.Domain.Configuration;

namespace Bim.Domain.Ifc
{
    public class IfMaterial : IfObject
    {
        public static Setup Setup { get; set; }
        public IfModel IfModel { get; set; }
        public IfColor IfColor { get; set; }
        public double Transparency { get; set; }
        public double SpecularColor { get; set; }
        public double SpecularHighlight { get; set; }
        public IfcMaterial IfcMaterial { get; set; }
        public List<IfElement> IfElements { get; set; }
        private IfcRelAssociatesMaterial ifcRelAssociates;
        public IfMaterial(IfModel ifModel,IfColor color):this(ifModel,color,.5,65,0)
        {
          
        }

        public IfMaterial(IfModel ifModel,IfColor color,double specularColor=.5,double SpecularHighlight=65,double transparency=0)
        {
            IfColor = color;
            IfModel = ifModel;
            IfElements = new List<IfElement>();
            SpecularColor = specularColor;
            SpecularHighlight = specularColor;
            Transparency = transparency;
            if (IfModel == null) return;
            New();
        }
        public void New()
        {
            var ifcModel = IfModel.IfcStore;
            using (var ctx=ifcModel.BeginTransaction("Adding Material"))
            {

                var material = ifcModel.Instances.New<IfcMaterial>();
                material.Name = Name; //set material name

                //set material style
                var reps = ifcModel.Instances.New<IfcMaterialDefinitionRepresentation>();
                var styles = ifcModel.Instances.New<IfcStyledRepresentation>();
                var styleItem = ifcModel.Instances.New<IfcStyledItem>();
                var styleAssign = ifcModel.Instances.New<IfcPresentationStyleAssignment>();
                var surfaceStyle = ifcModel.Instances.New<IfcSurfaceStyle>();
                var surfaceStyleRender = ifcModel.Instances.New<IfcSurfaceStyleRendering>();
                var ctxPres = ifcModel.Instances.
                    OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
                //Color
                var color = ifcModel.Instances.New<IfcColourRgb>();
                color.Red = IfColor.Red;
                color.Green = IfColor.Green; color.Blue = IfColor.Blue;
                surfaceStyleRender.SurfaceColour = color;
                // set material properties
                surfaceStyleRender.SpecularColour = new IfcNormalisedRatioMeasure(SpecularColor);
                surfaceStyleRender.SpecularHighlight = new IfcSpecularExponent(SpecularHighlight);
                surfaceStyleRender.Transparency = new IfcNormalisedRatioMeasure(Transparency);

                //ifcsurfacestyle
                surfaceStyle.Styles.Add(surfaceStyleRender);
                surfaceStyle.Side = IfcSurfaceSide.BOTH;
                //IfcPresentationStyleAssignment
                styleAssign.Styles.Add(surfaceStyle);
                //style Item
                styleItem.Styles.Add(styleAssign);
                //style represntation
                styles.Items.Add(styleItem);
                styles.RepresentationIdentifier = "Style";
                styles.RepresentationType = "Material";
                styles.ContextOfItems = ctxPres;
                reps.Representations.Add(styles);
                reps.RepresentedMaterial = material;
                ifcRelAssociates = ifcModel.Instances.New<IfcRelAssociatesMaterial>();
                ifcRelAssociates.RelatingMaterial = material;
                Label = ifcRelAssociates.EntityLabel;
                ctx.Commit();
            }
           
            IfModel.Instances.Add(this);
        }
        public void AttatchTo(IfElement element)
        {
            IfElements.Add(element);
            var ifcModel = IfModel.IfcStore;
            element.IfMaterial = this;

            using (var ctx=ifcModel.BeginTransaction("Attatching Elment"))
            {
                //attach material to specific element
                var m = (IfcRelAssociatesMaterial)ifcModel.Instances.Where(e => e.EntityLabel == Label).FirstOrDefault();
                m.RelatedObjects.Add(element.IfcElement);
                ctx.Commit();
            }
        }
    }
}
