using AndorinhaEsporte.Domain.State;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class RotateCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var player = command.Player;
            if (player.RotateState == RotateStateEnum.Initial)
            {
                player.RotatePosition();
                return;
            }

            if (player.RotateState == RotateStateEnum.Rotating)
            {
                var target = player.FieldPosition.GetStartPosition(player.TeamFoward);
                MoveToTarget(target, command);
            }
            var distanceFromTarget = player.FieldPosition.GetStartPosition(player.TeamFoward) - player.Position;
            if (distanceFromTarget.sqrMagnitude < 0.01f)
            {
                player.FinishRotation();
                command.PlayerTransform.forward = player.TeamFoward;
                return;
            }
        }
    }
}