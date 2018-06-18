using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bim.BOQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain.Ifc;

namespace Bim.BOQ.Tests
{
    [TestClass()]
    public class GeometryCollectionTests
    {
        static List<IfDimension> ifDimensions = new List<IfDimension>()
            {
                new IfDimension(2,6,120),
                new IfDimension(2,8,120),
                new IfDimension(3,6,120),
                new IfDimension(2,6,130),
            };

        static List<IfElement> ifElements = new List<IfElement>()
            {
                new IfJoist(){ IfDimension = ifDimensions[0]},
                new IfStud() { IfDimension = ifDimensions[1]},
                new IfSill() { IfDimension = ifDimensions[2]},
                new IfJoist(){ IfDimension = ifDimensions[3]},
            };
        static GeometryCollection GC1 = new GeometryCollection()
            {
                ElementCollection = ifElements
            };

        /*[TestMethod()]
        public void CheckElementTest()
        {
            List<IfDimension> ifDimensions = new List<IfDimension>()
            {
                new IfDimension(2,6,120),
                new IfDimension(2,8,120),
                new IfDimension(3,6,120),
                new IfDimension(2,6,130),
            };

            List<IfElement> ifElements = new List<IfElement>()
            {
                new IfJoist(){ IfDimension = ifDimensions[0]},
                new IfStud() { IfDimension = ifDimensions[1]},
                new IfSill() { IfDimension = ifDimensions[2]},
                new IfJoist(){ IfDimension = ifDimensions[3]},
            };

            GeometryCollection GC1 = new GeometryCollection()
            {
                ElementCollection = ifElements
            };

            IfJoist joist = new IfJoist()
            {
                IfDimension = new IfDimension(2, 6, 150)
            };
            Assert.IsTrue(GC1.CheckElement(joist));
        }*/

        [TestMethod()]
        public void AddToCollectionTest()
        {
            List<IfDimension> ifDimensions = new List<IfDimension>()
            {
                new IfDimension(2,6,120), //1
                new IfDimension(2,8,120), //2
                new IfDimension(2,6,120),
                new IfDimension(3,10,120),//3
                new IfDimension(2,12,130),//4
                new IfDimension(3,10,120),
                new IfDimension(3,8,120),//5
                new IfDimension(2,4,60),//6
                new IfDimension(3,6,120),//7
                new IfDimension(2,6,130),//8
            };

            List<IfElement> ifElements = new List<IfElement>()
            {
                new IfJoist(){ IfDimension = ifDimensions[0]},
                new IfJoist(){ IfDimension = ifDimensions[1]},
                new IfJoist(){ IfDimension = ifDimensions[2]},
                new IfJoist(){ IfDimension = ifDimensions[3]},
                new IfJoist(){ IfDimension = ifDimensions[4]},

                new IfStud() { IfDimension = ifDimensions[5]},
                new IfStud() { IfDimension = ifDimensions[6]},
                new IfStud() { IfDimension = ifDimensions[7]},
                new IfStud() { IfDimension = ifDimensions[8]},
                new IfStud() { IfDimension = ifDimensions[9]},
                new IfStud() { IfDimension = ifDimensions[0]},

                new IfSill() { IfDimension = ifDimensions[1]},
                new IfSill() { IfDimension = ifDimensions[2]},
                new IfSill() { IfDimension = ifDimensions[3]},
                new IfSill() { IfDimension = ifDimensions[4]},

                new IfJoist(){ IfDimension = ifDimensions[2]},
            };

            GeometryCollection GC2 = new GeometryCollection();
            GC2.AddToCollection(ifElements);

            Assert.Fail();
        }
    }
}