using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class TeamInMatchInformation
    {
        public TeamInMatchInformation(Vector3 foward, Color mainColor, Color secondaryColor)
        {
            Foward = foward;
            MainColor = mainColor;
            SecondaryColor = secondaryColor;
        }


        public Vector3 Foward { get; set; }
        public Color MainColor { get; }
        public Color SecondaryColor { get; }

    }
}
