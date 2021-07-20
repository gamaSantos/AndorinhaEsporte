using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class DefendCommandHandler : BasePlayerActionCommandHandler
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
                player.RemoveAction(PlayerAction.Defend);
                return;
            }

            var playerID = team.GetNearestPlayerIdFrom(landingSpot);
            if (player.Id != playerID)
            {
                player.RemoveAction(PlayerAction.Defend);
                return;
            }
            var inDefenseRange = player.InDefenseRange(ball.transform.position);
            if (player.Position.Distance(ball.Position) < 2.5f
                && !inDefenseRange)
            {
                player.IsDefending = true;
            }
            if (inDefenseRange)
            {
                var foward = player.TeamFoward.z * 0.15f;
                ball.Stop();
                ball.MoveInDirection(new Vector3(0, 1, foward), 6, player.TeamId);
                player.RemoveAction(PlayerAction.Defend);
                team.Pass();
                team.Spike();
                return;
            }
            var margin = 0.5f * team.Foward.z;
            var target = new Vector3(landingSpot.x, 0, landingSpot.z + margin);
            var arrivedInPosition = player.ArrivedInTarget(target);
            if (!arrivedInPosition)
            {
                MoveToTarget(target, command, 0, lookAtBall: true);
                return;
            }
        }

        public void HandleImmediate(PlayerCommand command)
        {
            var ball = command.Ball;
            var player = command.Player;
            player.IsDefending = true;
            if (player.InDefenseRange(ball.transform.position))
            {
                var foward = player.TeamFoward.z * 0.15f;
                ball.Stop();
                ball.MoveInDirection(new Vector3(0, 1, foward), 6, player.TeamId);
                player.RemoveAction(PlayerAction.Defend);
                //FIXME FIX implementation, pass team as parameter.??
                return;
            }
        }
    }
}