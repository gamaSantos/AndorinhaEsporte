using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class RightBack : FieldPosition
    {
        public RightBack()
        {
            Type = PlayerPositionType.RightBack;
        }

        protected override Vector3 StartPosition => new Vector3(SidePosition, 0, BackPosition);
        protected override Vector3 ChangeSidePosition => new Vector3(SideLine - 1, 0, 3);
        public override int RotationOrder => 1;
        public override bool InFrontRow => true;
    }
}