using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class TeamInMatchInformation
    {
        public TeamInMatchInformation(Guid teamId, string name, Color mainColor, Color secondaryColor)
        {
            Name = name;
            MainColor = mainColor;
            SecondaryColor = secondaryColor;
            TeamId = teamId;
        }

        public Guid TeamId { get; }
        public string Name { get; }
        public Vector3 Foward { get; set; }
        public Color MainColor { get; }
        public Color SecondaryColor { get; }
    }
}
