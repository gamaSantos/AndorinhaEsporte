using System.Linq;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class PassCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;

            if (!ball.LandingSpot.HasValue) return;
            var target = ball.LandingSpot.Value;

            if (player.InPassRange(ball.transform.position) && !player.Passing)
            {
                player.Passing = true;
                ball.disableGravity();
                ball.Stop();

                var horizontalDirection = UnityEngine.Random.Range(-1, 1);
                var targetPlayer = horizontalDirection >= 0 ?
                    player.Teammates.FirstOrDefault(p => p.CurrentFunction == PlayerPositionType.RightStriker) :
                    player.Teammates.FirstOrDefault(p => p.CurrentFunction == PlayerPositionType.LeftStriker);


                var fowardMargin = -1f * player.TeamFoward.z;
                var targetBallPosition = new Vector3(targetPlayer.Position.x, 2.5f, fowardMargin);
                var direction = ball.Position.DirectionTo(targetBallPosition);
                direction.y = 1;
                var passStrength = ball.GetNeededForceFromSimulation(ball.Position, targetBallPosition, direction);
                
                ball.EnableGravity();
                foreach(var teammate in player.Teammates)
                {
                    var isPassTarget = teammate.Id == targetPlayer.Id;
                    teammate.ChangePassTargeState(isPassTarget);
                }
                ball.MoveInDirection(direction, passStrength, player.TeamId);
                player.RemoveAction(PlayerAction.Pass);
            } 

            if (!player.ArrivedInTarget(target))
            {
                MoveToTarget(target, command);
                return;
            }


            return;
        }
    }
}
