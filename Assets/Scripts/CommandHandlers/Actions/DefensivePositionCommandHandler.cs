using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class DefensivePositionCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(BasePlayerCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;

            if (!ball.LandingSpot.HasValue) return;
            var landingSpot = ball.LandingSpot.Value;
            if (player.IsDefenseNecessary(landingSpot))
            {
                player.RemoveAction(PlayerAction.MoveToDenfensivePosition);
                return;
            }
            Vector3 target = player.GetDefensivePosition(ball.Position);
            MoveToTarget(target, command, precisionStart: 1, lookAtBall: true);
            return;
        }
    }
}