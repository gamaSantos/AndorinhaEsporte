using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class BlockCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(TeamCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;
            var team = command.Team;

            if (!ball.LandingSpot.HasValue) return;
            var landingSpot = ball.LandingSpot.Value;
            
            if (!player.IsDefenseNecessary(landingSpot))
            {
                player.RemoveAction(PlayerAction.Block);
                return;
            }

            if (player.InBlockRange(ball.Position))
            {
                Jump(command);
                player.RemoveAction(PlayerAction.Block);
                return;
            }
            if(!player.CanBlock(ball.Position))
            {
                player.RemoveAction(PlayerAction.Block);
                return;
            }
        }
    }
}