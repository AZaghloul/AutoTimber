using Bim.Domain.Ifc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.QuantityResource;

namespace Bim.Domain.Ifc
{
    public class IfProperties
    {
        public Dictionary<string, List<IfSingleValue>> SingleValueProperties { get; set; }
        public Dictionary<string, List<IfQuantity>> Quanatities { get; set; }

        public IfElement IfElement { get; set; }
        private IfcBuildingElement ifcElment { get; set; }
        public IfProperties(IfElement ifElement)
        {
            IfElement = ifElement;
            SingleValueProperties = new Dictionary<string, List<IfSingleValue>>();
            Quanatities = new Dictionary<string, List<IfQuantity>>();
        }

        public IfProperties(IfcBuildingElement ifcElement)
        {

            ifcElment = ifcElement;
            SingleValueProperties = new Dictionary<string, List<IfSingleValue>>();
            Quanatities = new Dictionary<string, List<IfQuantity>>();
        }
        public void AddSingleValue(string setName, List<IfSingleValue> propertiesList)
        {
            if (SingleValueProperties.ContainsKey(setName))
            {
                SingleValueProperties[setName].AddRange(propertiesList);

            }
            else
            {
                SingleValueProperties.Add(setName, new List<IfSingleValue>());
                SingleValueProperties[setName].AddRange(propertiesList);
            }
        }
        public void AddQuantities(string setName, List<IfQuantity> propertiesList)
        {

            if (Quanatities.ContainsKey(setName))
            {
                Quanatities[setName].AddRange(propertiesList);

            }
            else
            {
                Quanatities.Add(setName, new List<IfQuantity>());
                Quanatities[setName].AddRange(propertiesList);
            }

        }

        public void FindByName(string name)
        {
            //find in Single Value first
            var rels = ifcElment.IsDefinedBy
                .OfType<IfcRelDefinesByProperties>().Select(e => e.RelatingPropertyDefinition);


            var ss = rels.SelectMany(d => ((IfcPropertySet)d).HasProperties).Where(n => n.Name == name);

        }
        public void FindByValue(string value)
        {
            //find in Single Value first
            var rels = ifcElment.IsDefinedBy
                .OfType<IfcRelDefinesByProperties>().Select(e => e.RelatingPropertyDefinition);
            //var set = rels.Where(e => ((IfcPropertySet)e).Name==setName)
            //    .FirstOrDefault();/*Where(s => ((IfcPropertySet)s).Name == name);*/
            //  var props = ((IfcPropertySet)set).HasProperties.Where(n=>n.Name==name);

            var ss = rels.SelectMany(d => ((IfcPropertySet)d).HasProperties)
                .Where(n => ((IfcPropertySingleValue)n).NominalValue.Value.ToString() == value);

        }
        public List<IfSingleValue> FindSVProperty(IfSingleValue value)
        {
            var rels = ifcElment.IsDefinedBy
               .OfType<IfcRelDefinesByProperties>().Select(e => e.RelatingPropertyDefinition);


            var ss = rels.SelectMany(d => ((IfcPropertySet)d).HasProperties)
                .Where(n => n.Name == value.Name &&
                ((IfcPropertySingleValue)n).NominalValue.Value.ToString() == value.Value).ToList();
            var res = new List<IfSingleValue>();
            ss.ForEach(s => res
            .Add(
                new IfSingleValue
                    (s.Name,
                         ((IfcPropertySingleValue)s).NominalValue.Value.ToString()
                         )
                {
                    IfcSVProperty = (IfcPropertySingleValue)s
                }
                )

                );
            return res;
        }


        public void New()
        {
            using (var txs = ifcElment.Model.BeginTransaction("ggg"))
            {
                //create new Qunatities Properties
                #region Quantities

                // var instances = IfElement.IfModel.IfcStore.Instances;
                var instances = ifcElment.Model.Instances;
                //

                //add single value properties to if Element
                foreach (var collection in Quanatities)
                {
                    var eq = instances.New<IfcElementQuantity>();
                    eq.Name = collection.Key;
                    var Qvalues = collection.Value;
                    //create Quantity Values
                    foreach (var Qvalue in Qvalues)
                    {
                        //check if already defined value
                        var q = instances.OfType<IfcQuantityLength>().
                             Where(e => e.Name == Qvalue.Name && (e.LengthValue.Value.ToString()) == Qvalue.Value)
                             .FirstOrDefault();
                        //    if found
                        if (q != null)
                        {
                            eq.Quantities.Add(q);
                        }
                        //if not found create new one
                        else
                        {
                            IfcPhysicalSimpleQuantity quantity;
                            switch (Qvalue.IfUnitEnum)
                            {
                                case IfUnitEnum.AREAUNIT:
                                    quantity = instances.New<IfcQuantityArea>(e =>
                                    {
                                        e.AreaValue = Convert.ToDouble(Qvalue.Value);
                                        e.Name = Qvalue.Name;
                                    });
                                    eq.Quantities.Add(quantity);

                                    break;
                                case IfUnitEnum.LENGTHUNIT:
                                    quantity = instances.New<IfcQuantityLength>(e =>
                                    {
                                        e.LengthValue = Convert.ToDouble(Qvalue.Value);
                                        e.Name = Qvalue.Name;
                                    });
                                    eq.Quantities.Add(quantity);
                                    break;
                                case IfUnitEnum.VOLUMEUNIT:

                                    quantity = instances.New<IfcQuantityVolume>(e =>
                                    {
                                        e.VolumeValue = Convert.ToDouble(Qvalue.Value);
                                        e.Name = Qvalue.Name;
                                    });
                                    eq.Quantities.Add(quantity);
                                    break;
                                default:
                                    break;
                            }



                        }


                    }

                    // create the relationship to realte Quantities to the element
                    instances.New<IfcRelDefinesByProperties>(rd =>
                    {
                        rd.Name = "Element Quantities";
                        rd.Description = "IfcElementQuantity associated to Elemnt";
                        // rd.RelatedObjects.Add(IfElement.IfcElement);
                        rd.RelatedObjects.Add(ifcElment);
                        rd.RelatingPropertyDefinition = eq;
                    });
                }


                #endregion
                //create new Single Value Properties
                #region Single Value 

                //add single value properties to if Element
                foreach (var collection in SingleValueProperties)
                {
                    //create new property Set
                    var sv = instances.New<IfcPropertySet>();

                    sv.Name = collection.Key;
                    var Svalues = collection.Value;
                    //create Quantity Values
                    foreach (var Svalue in Svalues)
                    {
                        //check if already defined value
                        var qq = instances.OfType<IfcPropertySingleValue>();
                        var q = qq.
                         Where(e => e.Name == Svalue.Name /*&& (e.NominalValue.Value.ToString()) == Svalue.Value*/)
                         .FirstOrDefault();
                        //    if found
                        if (q != null)
                        {
                            sv.HasProperties.Add(q);
                        }
                        //if not found create new one
                        else
                        {
                            var v = instances.New<IfcPropertySingleValue>(e =>
                              {
                                  e.NominalValue = new IfcLabel(Svalue.Value);
                                  e.Name = Svalue.Name;
                              });

                            sv.HasProperties.Add(v);
                        }




                    }


                    // create the relationship to realte Quantities to the element
                    instances.New<IfcRelDefinesByProperties>(rd =>
                    {

                        rd.Name = "Element Properties";
                        rd.Description = "IfcElement PRoperties associated to Elemnt";
                        // rd.RelatedObjects.Add(IfElement.IfcElement);
                        rd.RelatedObjects.Add(ifcElment);
                        //add single value property set
                        rd.RelatingPropertyDefinition = sv;
                    });
                }




                #endregion

                txs.Commit();
            }

        }
    }
}
