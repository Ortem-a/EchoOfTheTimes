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

            return b1.Inner.Id == b2.Outer.Id ||
                b1.Outer.Id == b2.Inner.Id ||
                b1.Inner.Id == b2.Inner.Id && b1.Outer.Id == b2.Outer.Id;
        }

        public int GetHashCode(Bridge obj)
        {
            return obj.GetHashCode();
        }
    }
}