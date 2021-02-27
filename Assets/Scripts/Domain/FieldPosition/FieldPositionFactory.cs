using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class FieldPositionFactory
    {
        public static FieldPosition Create(PlayerPositionType type)
        {
            switch (type)
            {
                case PlayerPositionType.CenterBack:
                    return new CenterBack();
                case PlayerPositionType.LeftBack:
                    return new LeftBack();
                case PlayerPositionType.LeftStriker:
                    return new LeftStriker();
                case PlayerPositionType.Center:
                    return new CenterFoward();
                case PlayerPositionType.RightBack:
                    return new RightBack();
                case PlayerPositionType.RightStriker:
                    return new RightStriker();
                default:
                    return null;
            }
        }
    }
}