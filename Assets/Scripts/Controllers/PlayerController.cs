using AndorinhaEsporte.Domain;
using AndorinhaEsporte.CommandHandlers;
using System;
using System.Linq;
using UnityEngine;
using AndorinhaEsporte.Domain.State;

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
        
        public SpikeStateEnum spikeState;
        public int JerseyNumber = 0;

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

            playerCommandHandler = new PlayerCommandHandler(_player, _ball, _rigidBody, transform, _team);
        }

        public void FixedUpdate()
        {
            if (_player == null) return;
            _player.UpdatePosition(transform.position, _rigidBody.velocity);
            spikeState = _player.SpikeState;
            UpdateAnimator();

            UIController.ChangeText($"{_player.CurrentFunction} - {_player.CurrentAction}");
            if (playerCommandHandler != null)
                playerCommandHandler.HandleCurrentAction(_passTarget);

        }

        private void UpdateAnimator()
        {
            var isSpiking = _player.IsSpiking;
            var isDefending = _player.IsDefending;
            var isPassing = _player.IsPassing;
            var isBlocking = _player.IsBlocking;
            var isRunning = !_player.InAir && _rigidBody.velocity.magnitude > 0.1 && !(isSpiking || isDefending);
            var isIdle = !isRunning && !isSpiking && !isDefending;

            if (isSpiking)
            {
                animator.SetTrigger("IsSpiking");
                _player.IsSpiking = false;
            }
            if (isDefending)
            {
                animator.SetTrigger("IsDefending");
                _player.IsDefending = false;
            }
            if(isPassing){
                animator.SetTrigger("IsPassing");
                _player.IsPassing = false;
            }

            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsIdle", isIdle);
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

        internal Player GetPlayer() => _player;
        internal Rigidbody GetRigidbody() => _rigidBody;
        internal Transform GetTransform() => transform;
        public void OnDrawGizmos()
        {
            // Gizmos.DrawWireSphere(transform.position, _player.BallControlRange);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!_player.InAir)
            {
                _player.IsJumping = false;
            }
        }
    }


}

