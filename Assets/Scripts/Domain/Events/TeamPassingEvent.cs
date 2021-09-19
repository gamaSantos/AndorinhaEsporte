using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain.Events
{
    public class TeamPassingEvent
    {
        public TeamPassingEvent(Guid teamId, bool isPassing)
        {
            TeamId = teamId;
            IsPassing = isPassing;
        }

        public Guid TeamId { get; set; }
        public bool IsPassing { get; set; }
    }
}