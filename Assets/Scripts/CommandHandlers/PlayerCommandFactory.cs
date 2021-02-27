
using AndorinhaEsporte.Controller;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers
{
    public class PlayerCommandFactory
    {
        private BallController _ball;
        private Player _player;
        private Rigidbody _rigidBody;
        private readonly Transform _transform;
        private readonly Team _team;

        public PlayerCommandFactory(BallController ball, Player player, Rigidbody rigidBody, Transform transform, Team team)
        {
            _ball = ball;
            _player = player;
            _rigidBody = rigidBody;
            _transform = transform;
            _team = team;
        }

        public BasePlayerCommand CreateBaseCommand()
        {
            return new BasePlayerCommand(_player, _ball, _rigidBody, _transform);
        }

        public TeamCommand CreateTeamCommand()
        {
            return new TeamCommand(_player, _ball, _rigidBody, _transform, _team);
        }
    }
}