using System.Collections.Generic;

namespace Systems.Leveling
{
    public class BridgeEqualityComparer : IEqualityComparer<Bridge>
    {
        public bool Equals(Bridge b1, Bridge b2)
        {
            if (ReferenceEquals(b1, b2))
            {
                return true;
            }

            if (b1 is null || b2 is null)
            {
                return false;
            }

            return b1.Inner.Equals(b2.Inner) && b1.Outer.Equals(b2.Outer) ||
                b1.Inner.Equals(b2.Outer) ||
                b1.Outer.Equals(b2.Inner);
        }

        public int GetHashCode(Bridge obj)
        {
            return obj.GetHashCode();
        }
    }
}