using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AndorinhaEsporte.Domain
{
    public abstract class FieldPosition
    {
        public PlayerPositionType Type { get; protected set; }
        protected abstract Vector3 StartPosition { get; }
        protected abstract Vector3 ChangeSidePosition { get; }
        public abstract int RotationOrder { get; }

        private Dictionary<int, FieldPosition> positions = new Dictionary<int, FieldPosition>();
        public FieldPosition GetNextFieldPosition()
        {
            var newPositionIndex = RotationOrder == 1 ? 6 : RotationOrder - 1;
            if (positions == null || !positions.Any())
            {
                positions.Add(new CenterBack().RotationOrder, new CenterBack());
                positions.Add(new CenterFoward().RotationOrder, new CenterFoward());
                positions.Add(new LeftBack().RotationOrder, new LeftBack());
                positions.Add(new RightBack().RotationOrder, new RightBack());
                positions.Add(new LeftStriker().RotationOrder, new LeftStriker());
                positions.Add(new RightStriker().RotationOrder, new RightStriker());
            }
            return positions[newPositionIndex];
        }

        public Vector3 GetStartPosition(Vector3 teamfoward)
        {
            return StartPosition * GetTeamFoward(teamfoward);
        }
        public Vector3 GetChangeSidesPosition(Vector3 teamfoward)
        {
            var target = ChangeSidePosition;
            target.x = ChangeSidePosition.x * GetTeamFoward(teamfoward);
            target.z = ChangeSidePosition.z * GetTeamFoward(teamfoward);
            return target;
        }
        protected float GetTeamFoward(Vector3 foward) => Mathf.Clamp(foward.z, -1, 1);
        public abstract bool InFrontRow { get; }

        protected float FowardPosition => -1.2f;
        protected float BackPosition => -7.5f;
        protected float SidePosition => 3f;
        protected float SideLine => -6.5f;

    }
}