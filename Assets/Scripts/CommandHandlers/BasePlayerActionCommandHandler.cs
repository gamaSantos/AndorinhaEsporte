using System.Linq;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers
{
    public class BasePlayerActionCommandHandler
    {
        protected static void MoveToTarget(Vector3 target, BasePlayerCommand command, float precisionStart = 1f, bool lookAtBall = false)
        {
            var player = command.Player;
            if (player == null || player.InAir) return;
            target.y = 0;
            var transform = command.PlayerTransform;
            var rigidBody = command.PlayerRigidBody;

            var heading = target - transform.position;
            var distance = heading.magnitude;

            var direction = heading / distance;
            var faceDirection = direction;
            var collisionRange = 2f;
            
            var teamMateInCollisionRange = player.Teammates.Any(teammate => teammate.Position.Distance(player.Position) < collisionRange);

            if (teamMateInCollisionRange) direction += transform.right;
            var moveSpeed = distance > precisionStart ? command.Player.MoveSpeed : command.Player.PreciseMoveSpeed;
            if (lookAtBall)
            {
                moveSpeed = moveSpeed * 0.7f;
            }
            var force = direction * moveSpeed * Time.deltaTime;
            if (distance < precisionStart && rigidBody.velocity.magnitude > 2f)
            {
                force = (force / 2) * -1;
            }

            rigidBody.AddForce(force, ForceMode.Force);
            if (lookAtBall)
            {
                var ballPosition = command.Ball.transform.position;
                faceDirection = new Vector3(ballPosition.x, 0, ballPosition.z);
               
            }
             transform.LookAt(faceDirection);

        }

        protected static void Jump(BasePlayerCommand command)
        {
            var player = command.Player;
            var rigidBody = command.PlayerRigidBody;

            if (player.InAir) return;

            rigidBody.AddForce(new Vector3(0, 1, 0.1f) * 1800);
        }
    }
}
