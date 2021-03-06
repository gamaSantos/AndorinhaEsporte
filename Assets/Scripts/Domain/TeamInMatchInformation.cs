using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class TeamInMatchInformation
    {
        public TeamInMatchInformation(Guid teamId, Vector3 foward, Color mainColor, Color secondaryColor)
        {
            Foward = foward;
            MainColor = mainColor;
            SecondaryColor = secondaryColor;
            TeamId = teamId;
        }

        public Guid TeamId { get; }
        public Vector3 Foward { get; set; }
        public Color MainColor { get; }
        public Color SecondaryColor { get; }

    }
}
