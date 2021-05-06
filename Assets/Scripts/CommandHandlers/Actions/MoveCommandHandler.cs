using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class MoveCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand command)
        {
            var transform = command.PlayerTransform;
            var foward = transform.forward;
            var player = command.Player;
            var target = player.FieldPosition.Type == Domain.PlayerPositionType.LeftBack ? new Vector3(0, 0, 5) : new Vector3(0, 0, -5);
            if(player.ArrivedInTarget(target))
            {
                player.RemoveAction(Domain.PlayerAction.Move);
                return;
            }
            MoveToTarget(target, command);


            return;
        }
    }
}
