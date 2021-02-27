using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class CenterBack : FieldPosition
    {
        public CenterBack()
        {
            Type = PlayerPositionType.CenterBack;
        }

        protected override Vector3 StartPosition => new Vector3(0, 0, BackPosition);

        protected override Vector3 ChangeSidePosition => new Vector3(SideLine - 1, 0, 7);
        public override int RotationOrder => 6;


    }
}