﻿using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.CommandHandlers;
using AndorinhaEsporte.CommandHandlers.UserActions;
using AndorinhaEsporte.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndorinhaEsporte.Controller
{
    public class UserController : MonoBehaviour
    {
        IEnumerable<PlayerController> _playerControllers;
        private AndorinhaUserActions _actions;
        private BallController _ball;


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
            Application.targetFrameRate = 60;
            _actions.Player.Fire.performed += ctx =>
            {

            };
            BindMovimentEvents();
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

        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if(_moving || _movingDirection == Vector2.zero) return;
            var player = GetPlayer();
            var handler = new MoveInDirectionCommandHandler();
            var command = new BasePlayerCommand<Vector2>(
                player.GetPlayer(),
                _ball,
                player.GetRigidbody(),
                player.GetTransform(),
                _movingDirection);
            handler.Handle(command);
        }

        private PlayerController GetPlayer()
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
