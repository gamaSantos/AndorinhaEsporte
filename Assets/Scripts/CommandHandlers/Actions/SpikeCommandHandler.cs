using AndorinhaEsporte.Domain;
using System.Linq;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class SpikeCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(BasePlayerCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;
            var isRightSide = player.Position.x > 0;
            var side = isRightSide ? 4.65f : -4.65f;

            var target = new Vector3(side, 0, player.TeamFoward.z * -1);

            if (player.IsPassTarget && ball.LandingSpot.HasValue)
            {
                var newTarget = ball.Trajectory.Points.Where(ballPosition =>
                        ballPosition.y < (player.SpikeHeight - 0.5f) && ballPosition.y > 2.4f).LastOrDefault();

                target = newTarget != default(Vector3) ? newTarget : ball.LandingSpot.Value;

                ball.DrawTrajectory();
            }

            if (!player.ArrivedInTarget(target))
            {
                MoveToTarget(target, command);
                return;
            }
            else
            {
                command.PlayerTransform.forward = player.TeamFoward;
            }
            if (!player.IsPassTarget) return;

            if (player.InSpikeRange(ball.Position))
            {
                Jump(command);
                var ballHorizontalDirection = isRightSide ? -0.5f : 0.5f;
                var direction = new Vector3(ballHorizontalDirection, 0.1f, player.TeamFoward.z);

                ball.MoveInDirection(direction, 7, player.TeamId);
                player.RemoveAction(PlayerAction.Spike);
                player.IsPassTarget = false;
            }
        }
    }
}
