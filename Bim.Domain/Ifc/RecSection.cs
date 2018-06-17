using Bim.Common.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class RecSection : IEquatable<RecSection>
    {
        public Length Width { get; set; }
        public Length Depth { get; set; }
        public RecSection(double Width, double Depth)
        {
            this.Width = Length.FromInches(Width);
            this.Depth = Length.FromInches(Depth);
        }
        public RecSection() : this(0, 0) { }

        public static bool operator== (RecSection RS1 , RecSection RS2)
        {
            return RS1.Width == RS2.Width && RS1.Depth == RS2.Depth;
        }
        public static bool operator !=(RecSection RS1, RecSection RS2)
        {
            return !(RS1 == RS2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RecSection);
        }

        public bool Equals(RecSection other)
        {
            return other != null &&
                   Width == other.Width &&
                   Depth == other.Depth;
        }

        public override int GetHashCode()
        {
            var hashCode = -405052895;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            return hashCode;
        }
    }
}
