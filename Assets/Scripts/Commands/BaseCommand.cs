using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Controller;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers
{
    public class BasePlayerCommand
    {
        public BasePlayerCommand(
            Player player,
            BallController ball,
            Rigidbody rigidbody,
            Transform transform)
        {
            Player = player;
            Ball = ball;
            PlayerRigidBody = rigidbody;
            PlayerTransform = transform;
        }

        public Player Player { get; }
        public Rigidbody PlayerRigidBody { get; }
        public BallController Ball { get; }
        public Transform PlayerTransform { get;}
    }
}
