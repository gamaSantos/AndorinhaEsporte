using AndorinhaEsporte.CommandHandlers.Actions;
using AndorinhaEsporte.CommandHandlers.UserActions;
using AndorinhaEsporte.CommandHandlers;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Inputs;
using AndorinhaEsporte.Services;
using AndorinhaEsporte.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

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
        private Team _userTeam;

        private bool _moving = false;
        private Vector2 _movingDirection = Vector2.zero;
        private bool _countingEnery = false;
        private float _currentEnergy = 0f;

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

            _userTeam = MatchService.Current.HomeTeam;
            _userTeam.ChangePlayerControlledByUser();

            Application.targetFrameRate = 60;
            BindSpike();
            BindMovimentEvents();
            BindDefend();
            BindPass();
            BindChangePlayer();
            BindMenu();
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
            var action = _actions.Player.Spike;
            action.started += ctx =>
            {
                _countingEnery = true;
            };
            action.performed += ctx =>
            {
                var command = new PlayerCommand(_player, _ball, _playerController.GetRigidbody(), _playerController.GetTransform());
                var handler = new SpikeCommandHandler();
                handler.HandleImmediate(command);
            };
            action.canceled += ctx =>
            {
                _countingEnery = false;
            };
        }
        private void BindDefend()
        {
            _actions.Player.Defend.performed += ctx =>
           {
               var command = new PlayerCommand(_player, _ball, _playerController.GetRigidbody(), _playerController.GetTransform());
               if (_player.InBlockPosition)
               {
                   var handler = new BlockCommandHandler();
                   handler.HandleImmediate(command);
               }
               else
               {
                   var handler = new DefendCommandHandler();
                   handler.HandleImmediate(command);
               }

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
        private void BindChangePlayer()
        {
            _actions.Player.ChangePlayer.started += ctx =>
            {
                _userTeam.ChangePlayerControlledByUser();
            };
        }
        private void BindMenu()
        {
            _actions.Player.OpenMenu.started += ctx =>
            {
                var pauseMenu = FindObjectOfType<PauseMenuController>();
                if (pauseMenu == null) return;

                pauseMenu.ToogleVisualization();

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
            UpdateEnergyMeter();
        }

        private void UpdateEnergyMeter()
        {
            if (!_countingEnery) return;
            if (_currentEnergy < 1)
            {
                _currentEnergy += Time.deltaTime;
                if (_currentEnergy >= 1) _currentEnergy = 1;
            }
            _playerController.UpdateEnergyMeter(_currentEnergy);
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
            return _playerControllers.Where(pc => pc.IsUserControlled).First();
        }

        private void LoadPlayers()
        {
            _playerControllers = GameObject.FindObjectsOfType<PlayerController>();
            if (_playerControllers == null || _playerControllers.Count() == 0) return;
            _playerControllers = _playerControllers.Where(p => p.IsHomeTeamPlayer);
        }


    }
}
