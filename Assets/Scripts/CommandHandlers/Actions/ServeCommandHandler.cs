using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Domain.State;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class ServeCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(BasePlayerCommand command)
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
                ball.MoveInDirection(Vector3.up, 5f, player.TeamId);
                player.ServeState = ServeStateEnum.Serving;
            }
            if (state == ServeStateEnum.Serving && ball.transform.position.y < 2.1f && ball.Velocity.y < 0)
            {
                Jump(command);
                var foward = player.TeamFoward.z;
                var horizontalDirection = foward < 0 ? 0.25f : -0.25f;
                var force = Random.Range(6.8f, 7.3f);
                
                ball.MoveInDirection(new Vector3(horizontalDirection, 1, foward), force, player.TeamId);
                Finalizar(player);
            }
        }

        private static void Finalizar(Player player)
        {
            player.ServeState = ServeStateEnum.Finished;
            player.RemoveAction(PlayerAction.Serve);
            player.AddAction(PlayerAction.ResetPosition);
        }

        private void MoveToServePosition(BasePlayerCommand command)
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