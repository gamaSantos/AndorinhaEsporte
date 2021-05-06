using AndorinhaEsporte.Domain;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class ResetPositionCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var player = command.Player;
            var target = command.Player.FieldPosition.GetStartPosition(player.TeamFoward);
            if (!player.ArrivedInTarget(target))
            {
                MoveToTarget(target, command);
            }
            else
            {
                command.PlayerTransform.forward = player.TeamFoward;
                player.RemoveAction(PlayerAction.ResetPosition);
            }

        }
    }
}