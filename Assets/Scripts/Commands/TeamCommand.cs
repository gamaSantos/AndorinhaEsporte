using AndorinhaEsporte.Controller;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers
{
    public class TeamCommand: PlayerCommand
    {
        public TeamCommand(Player player, BallController ball, Rigidbody rigidbody, Transform transform, Team team) : base(player, ball, rigidbody, transform)
        {
            Team = team;
        }

        public Team Team { get; }
    }
}