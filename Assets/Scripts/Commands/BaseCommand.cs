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
        public Transform PlayerTransform { get; }
    }


    public class BasePlayerCommand<T> : BasePlayerCommand
    {
        public BasePlayerCommand(Player player, BallController ball, Rigidbody rigidbody, Transform transform, T data) : base(player, ball, rigidbody, transform)
        {
            Data = data;
        }

        public void SetData(T data)
        {
            if (Data != null) return;
            Data = data;
        }
        public T Data { get; private set; }

    }
}
