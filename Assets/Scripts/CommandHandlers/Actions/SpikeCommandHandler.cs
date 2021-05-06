using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Domain.State;
using System.Linq;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class SpikeCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;
            var isRightSide = player.Position.x > 0;
            var side = isRightSide ? 4f : -4f;
            var state = player.SpikeState;

            var target = new Vector3(side, 0, player.TeamFoward.z * -1);
            

            if (player.IsPassTarget && ball.LandingSpot.HasValue)
            {
                var newTarget = ball.Trajectory.Points.Where(ballPosition =>
                        ballPosition.y < player.SpikeHeight && ballPosition.y > 2.4f).LastOrDefault();

                target = newTarget != default(Vector3) ? newTarget : ball.LandingSpot.Value;
            }

            if (state == SpikeStateEnum.Initial
            || state == SpikeStateEnum.Moving
            || state == SpikeStateEnum.AwaitingOrMovingToPassTarget)
            {
                if (state == SpikeStateEnum.Initial) state = SpikeStateEnum.Moving;

                if (!player.ArrivedInTarget(target))
                {
                    MoveToTarget(target, command);
                    return;
                }
                else
                {
                    state = state == SpikeStateEnum.Moving ? SpikeStateEnum.AwaitingOrMovingToPassTarget : SpikeStateEnum.PreJump;
                    command.PlayerTransform.forward = player.TeamFoward;
                    player.SpikeState = state;
                    return;
                }
            }

            if (!player.IsPassTarget)
            {
                // player.RemoveAction(PlayerAction.Spike);
                // player.SpikeState = SpikeStateEnum.Initial;
                return;
            } 

            if (state == SpikeStateEnum.PreJump && player.CanStartSpikeJump(ball.transform.position, ball.Velocity))
            {
                jumpSpike(command, player);
                player.SpikeState = SpikeStateEnum.Jumping;
                return;
            }

            if (state == SpikeStateEnum.Jumping && player.InSpikeRange(ball.Position))
            {
                spike(ball, player, isRightSide);
                player.SpikeState = SpikeStateEnum.Finished;
                return;
            }


        }

        private static void jumpSpike(PlayerCommand command, Player player)
        {
            Jump(command);
            player.IsSpiking = true;
        }

        private static void spike(Controller.BallController ball, Player player, bool isRightSide)
        {
            player.IsPassTarget = false;
            player.IsSpiking = false;

            var ballHorizontalDirection = isRightSide ? -0.5f : 0.5f;
            var direction = new Vector3(ballHorizontalDirection, 0.1f, player.TeamFoward.z);

            ball.MoveInDirection(direction, 6, player.TeamId);
            player.RemoveAction(PlayerAction.Spike);
        }
    }
}
