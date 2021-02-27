using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class CenterFoward : FieldPosition
    {
        public CenterFoward()
        {
            Type = PlayerPositionType.Center;
        }

        protected override Vector3 StartPosition => new Vector3(0, 0, FowardPosition);
        protected override Vector3 ChangeSidePosition => new Vector3(SideLine, 0, 1f);

         public override int RotationOrder => 3;
    }
}