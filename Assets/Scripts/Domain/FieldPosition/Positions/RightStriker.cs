using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class RightStriker : FieldPosition
    {
        public RightStriker()
        {
            Type = PlayerPositionType.RightStriker;
        }

        protected override Vector3 StartPosition => new Vector3(SidePosition, 0, FowardPosition);
        protected override Vector3 ChangeSidePosition => new Vector3(SideLine, 0, 0.6f);

        public override int RotationOrder => 2;
    }
}