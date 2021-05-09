using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.CommandHandlers;
using AndorinhaEsporte.CommandHandlers.Actions;
using AndorinhaEsporte.CommandHandlers.UserActions;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Inputs;
using UnityEngine;

namespace AndorinhaEsporte.Controller
{
    public class UserController : MonoBehaviour
    {
        IEnumerable<PlayerController> _playerControllers;
        private AndorinhaUserActions _actions;
        private BallController _ball;
        private CameraController _cameraController;
        private PlayerController _playerController;
        private Player _player;

        private bool _moving = false;
        private Vector2 _movingDirection = Vector2.zero;

        private void Awake()
        {
            _actions = new AndorinhaUserActions();
        }

        private void OnEnable()
        {
            _actions.Enable();
        }
        private void OnDisable()
        {
            _actions.Disable();
        }

        void Start()
        {
            _ball = GameObject.FindObjectOfType<BallController>();
            _cameraController = Camera.main.GetComponent<CameraController>();
            Application.targetFrameRate = 60;
            BindSpike();
            BindMovimentEvents();
            BindDefend();
            BindPass();
        }

        private void BindMovimentEvents()
        {
            var action = _actions.Player.Move;
            action.started += ctx =>
            {
                _moving = true;
            };
            action.performed += ctx =>
            {
                _movingDirection = ctx.ReadValue<Vector2>();
            };
            action.canceled += ctx =>
            {
                _moving = false;
                _movingDirection = Vector2.zero;
            };
        }
        private void BindSpike()
        {
            _actions.Player.Spike.performed += ctx =>
            {
                var command = new PlayerCommand(_player, _ball, _playerController.GetRigidbody(), _playerController.GetTransform());
                var handler = new SpikeCommandHandler();
                handler.HandleImmediate(command);
            };
        }
        private void BindDefend()
        {
             _actions.Player.Defend.performed += ctx =>
            {
                var command = new PlayerCommand(_player, _ball, _playerController.GetRigidbody(), _playerController.GetTransform());
                var handler = new DefendCommandHandler();
                handler.HandleImmediate(command);
            };
        }
        
        private void BindPass()
        {
              _actions.Player.Pass.performed += ctx =>
            {
                var command = new PlayerCommand(_player, _ball, _playerController.GetRigidbody(), _playerController.GetTransform());
                var handler = new PassCommandHandler();
                handler.HandleImmediate(command);
            };
        }

        void Update()
        {
            _playerController = GetPlayerController();
            _player = _playerController.GetPlayer();
        }
        void FixedUpdate()
        {
            if (_player == null) return;
            if (_player.IsInForcedAction) return;
            Move();
        }

        private void Move()
        {
            if (!_moving || _movingDirection == Vector2.zero) return;

            var direction = _cameraController.facingOposingDirection ? _movingDirection * -1 : _movingDirection;
            var handler = new MoveInDirectionCommandHandler();
            var command = new PlayerCommand<Vector2>(
                _player,
                _ball,
                _playerController.GetRigidbody(),
                _playerController.GetTransform(),
                direction);
            handler.Handle(command);
        }

        private PlayerController GetPlayerController()
        {
            if (_playerControllers == null || _playerControllers.Count() == 0) LoadPlayers();
            if (_playerControllers == null || _playerControllers.Count() == 0) return null;
            return _playerControllers.Where(pc => pc.IsHomeTeamPlayer && pc.IsUserControlled).First();
        }

        private void LoadPlayers()
        {
            _playerControllers = GameObject.FindObjectsOfType<PlayerController>();
            if (_playerControllers == null || _playerControllers.Count() == 0) return;
            _playerControllers = _playerControllers.Where(p => p.IsHomeTeamPlayer);
        }


    }
}
