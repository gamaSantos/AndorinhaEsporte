using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class LeftBack : FieldPosition
    {
        public LeftBack()
        {
            Type = PlayerPositionType.LeftBack;
        }

        protected override Vector3 StartPosition => new Vector3(SidePosition * -1, 0, BackPosition);
        protected override Vector3 ChangeSidePosition => new Vector3(SideLine - 1, 0, 7);
        public override int RotationOrder => 5;
        public override bool InFrontRow => false;
    }
}