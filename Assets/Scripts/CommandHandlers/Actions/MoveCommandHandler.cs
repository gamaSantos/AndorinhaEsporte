using System.Linq;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers.Actions
{
    public class MoveCommandHandler : BasePlayerActionCommandHandler
    {
        public void Handle(BasePlayerCommand command)
        {
            var transform = command.PlayerTransform;
            var foward = transform.forward;
            MoveToTarget(foward, command);


            return;
        }
    }
}
