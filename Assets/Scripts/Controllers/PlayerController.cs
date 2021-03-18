﻿using AndorinhaEsporte.Domain;
using AndorinhaEsporte.CommandHandlers;
using System;
using System.Linq;
using UnityEngine;

namespace AndorinhaEsporte.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerUIController UIController;

        [SerializeField]
        private GameObject _meshWrapper;

        [SerializeField]
        private Animator animator;

        private Rigidbody _rigidBody;
        private BallController _ball;

        private Player _player;
        private Team _team;

        public Guid PlayedId => _player.Id;
        public bool IsHomeTeamPlayer => _player.isHomeTeam;

        internal void Move()
        {
            _player.AddAction(PlayerAction.Move);
        }

        public bool IsUserControlled => _player.IsUserControlled;

        private PlayerCommandHandler playerCommandHandler;

        private Player _passTarget;

        void Start()
        {
            _rigidBody = gameObject.GetComponent<Rigidbody>();
            _ball = GameObject.FindObjectOfType<BallController>();
        }

        public void Initiate(Player playerData, Team team)
        {
            _player = playerData;
            _team = team;

            _rigidBody = gameObject.GetComponent<Rigidbody>();
            _ball = GameObject.FindObjectOfType<BallController>();
            var renderer = _meshWrapper.GetComponent<Renderer>();


            renderer.material.SetColor("_Color", _player.MainColor);
            UIController.ChangePosition(_player.FieldPosition);
            playerCommandHandler = new PlayerCommandHandler(_player, _ball, _rigidBody, transform, _team);
        }

        public void FixedUpdate()
        {
            if (_player == null) return;
            _player.UpdatePosition(transform.position, _rigidBody.velocity);

            var isSpiking = _player.IsSpiking;
            var isRunning = !_player.InAir && _rigidBody.velocity.magnitude > 0.1 && !isSpiking;
            var isIdle = !isRunning && !isSpiking;

            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsSpiking", isSpiking);
            animator.SetBool("IsIdle", isIdle);


            if (playerCommandHandler != null)
                playerCommandHandler.HandleCurrentAction(_passTarget);

        }

        private Player GetPassTarget(Vector3 direction)
        {
            Player target = null;
            if (!_player.Teammates.Any()) return target;
            target = _player.Teammates
                    .OrderByDescending(p => Vector3.Angle(direction, _ball.GetDirectionTo(p.Position)))
                    .First();
            return target;
        }

        public void OnDrawGizmos()
        {
            // Gizmos.DrawWireSphere(transform.position, _player.BallControlRange);
        }
    }


}

