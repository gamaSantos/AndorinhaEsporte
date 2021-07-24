using AndorinhaEsporte.CommandHandlers.Actions;
using AndorinhaEsporte.Controller;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.CommandHandlers
{
    public class PlayerCommandHandler
    {
        private Player _player;
        PlayerCommandFactory _commandFactory;
        public PlayerCommandHandler(Player player, BallController ball, Rigidbody rigidbody, Transform transform, Team team)
        {
            _player = player;
            _commandFactory = new PlayerCommandFactory(ball, player, rigidbody, transform, team);
        }

        public void HandleCurrentAction(Player target)
        {
            var baseCommand = _commandFactory.CreateBaseCommand();
            switch (_player.CurrentAction)
            {
                case PlayerAction.Serve:
                    new ServeCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.Rotate:
                    new RotateCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.ChangeSides:
                    new ChangeSidesCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.Defend:
                    new DefendCommandHandler().Handle(_commandFactory.CreateTeamCommand());
                    break;
                case PlayerAction.Block:
                    new BlockCommandHandler().Handle(_commandFactory.CreateTeamCommand());
                    break;
                case PlayerAction.Pass:
                    new PassCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.ResetPosition:
                    new ResetPositionCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.Spike:
                    new SpikeCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.MoveToDenfensivePosition:
                    new DefensivePositionCommandHandler().Handle(baseCommand);
                    break;
                case PlayerAction.Move:
                    new MoveCommandHandler().Handle(baseCommand);
                    break;
            }

        }

    }
}