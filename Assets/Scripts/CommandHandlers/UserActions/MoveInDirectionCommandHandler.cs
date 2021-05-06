using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.UserActions
{
    public class MoveInDirectionCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(PlayerCommand<Vector2> command)
        {
            var ball = command.Ball;
            var player = command.Player;
            var inputDirection = command.Data;
            
            MoveInDirection(new Vector3(inputDirection.x, 0, inputDirection.y), command);
        }
    }
}

