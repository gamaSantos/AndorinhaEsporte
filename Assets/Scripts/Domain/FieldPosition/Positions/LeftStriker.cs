using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class LeftStriker : FieldPosition
    {
        public LeftStriker()
        {
            Type = PlayerPositionType.LeftStriker;
        }
        protected override Vector3 StartPosition => new Vector3(SidePosition * -1, 0, FowardPosition);
        protected override Vector3 ChangeSidePosition => new Vector3(SideLine, 0, 1.5f);
        public override int RotationOrder => 4;
    }
}