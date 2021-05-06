using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Domain.State;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class ServeCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var player = command.Player;
            var ball = command.Ball;
            var state = player.ServeState;
            if (state == ServeStateEnum.Initial)
            {

                player.ServeState = ServeStateEnum.Moving;
            }
            if (state == ServeStateEnum.Moving)
            {
                MoveToServePosition(command);
            }


            if (state == ServeStateEnum.Approved)
            {
                ball.Stop();
                ball.EnableGravity();
                ball.MoveInDirection(Vector3.up, 5.5f, player.TeamId);
                player.ServeState = ServeStateEnum.Serving;
            }
            if (state == ServeStateEnum.Serving && ball.Velocity.y < 0)
            {
                var ballHeight = ball.transform.position.y;
                if (ballHeight < player.SpikeHeight)
                {
                    var foward = player.TeamFoward.z;
                    var horizontalDirection = Random.Range(-.3f, .1f) * foward;
                    var verticalDirection = Random.Range(0.7f, 1f);
                    var forwardDirection = Random.Range(0.6f, 1f) * foward;
                    var force = Random.Range(7f, 8.5f);
                    var spikeDirection = new Vector3(horizontalDirection, verticalDirection, forwardDirection);
                    
                    player.IsSpiking = false;
                    ball.MoveInDirection(spikeDirection, force, player.TeamId);
                    Finalizar(player);
                    return;
                }

                if (player.CanStartSpikeJump(ball.transform.position, ball.Velocity))
                {
                    Jump(command);
                    player.IsSpiking = true;
                    return;
                }

            }
        }

        private static void Finalizar(Player player)
        {
            player.ServeState = ServeStateEnum.Finished;
            player.RemoveAction(PlayerAction.Serve);
            player.AddAction(PlayerAction.ResetPosition);
        }

        private void MoveToServePosition(PlayerCommand command)
        {
            var player = command.Player;
            var servePosition = GetServePosition(player);
            MoveToTarget(servePosition, command);
            var distance = servePosition - player.Position;

            if (distance.magnitude < 0.1f)
            {
                command.PlayerTransform.forward = player.TeamFoward;

                var ball = command.Ball;
                var ballMargin = 0.5f * player.TeamFoward.z;
                ball.disableGravity();
                ball.Stop();
                ball.transform.position = new Vector3(player.Position.x, 1f, player.Position.z + ballMargin);

                player.ServeState = ServeStateEnum.AwaitingApproval;
            }
        }

        private Vector3 GetServePosition(Player player)
        {
            var servePosition = new Vector3(3, 0, -11f) * player.TeamFoward.z;

            return servePosition;
        }

    }
}