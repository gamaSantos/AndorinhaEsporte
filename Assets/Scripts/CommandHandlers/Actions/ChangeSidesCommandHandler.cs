using AndorinhaEsporte.Domain.State;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class ChangeSidesCommandHandler : BasePlayerActionCommandHandler
    {

        public void Handle(PlayerCommand command)
        {
            var player = command.Player;
            var ball = command.Ball;
            var state = player.ChangeSideState;
            Vector3 target = player.Position; ;
            ChangeSideStateEnum nextAction = ChangeSideStateEnum.Initial;
            
            if (state == ChangeSideStateEnum.Initial)
            {
                target = player.FieldPosition.GetChangeSidesPosition(player.TeamFoward);
                nextAction = ChangeSideStateEnum.ChangingSides;
            }
            if (state == ChangeSideStateEnum.ChangingSides)
            {
                var positionZ = player.FieldPosition.GetStartPosition(player.TeamFoward).z;
                var targetZ = System.Math.Abs(positionZ) < 2  ? positionZ : player.TeamFoward.z * -3;
                target = new Vector3(player.Position.x, 0, targetZ);
                nextAction = ChangeSideStateEnum.MovingToPosition;
            }
            if (state == ChangeSideStateEnum.MovingToPosition)
            {
                target = player.FieldPosition.GetStartPosition(player.TeamFoward);
                nextAction = ChangeSideStateEnum.Finished;
            }
            if (state == ChangeSideStateEnum.Finished)
            {
                command.PlayerTransform.forward = player.TeamFoward;
                player.RemoveAction(PlayerAction.ChangeSides);
            }

            if (player.Position.Distance(target) > 0.2)
            {
                MoveToTarget(target, command);
            }
            else
            {
                player.ChangeSideState = nextAction;
            }
        }
    }
}