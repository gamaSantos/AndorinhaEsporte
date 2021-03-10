using System;
using System.Linq;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Domain.Events;
using UnityEngine;

namespace AndorinhaEsporte.Controller
{
    public class BallController : MonoBehaviour
    {
        public AudioSource audioSource;

        Rigidbody _rigidbody;
        MatchController _matchController;
        PhysicsSceneController _simulationController;
        Player _currentPlayer;

        public Vector3 Velocity => _rigidbody.velocity;

        public Vector3? LandingSpot => Trajectory?.Last;
        public Vector3 Position => transform.position;

        public Trajectory Trajectory { get; set; }

        private Guid _lastTeamId;

        public event EventHandler<BallChangedDirectionEventArgs> ChangedDirection;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public Vector3 GetDirectionTo(Vector3 target)
        {
            var headingDirecton = target - transform.position;
            return headingDirecton / headingDirecton.magnitude;
        }

        public void MoveInDirection(Vector3 direction, float strength, Guid teamId)
        {
            Trajectory = null;
            var force = direction * strength;
            var torque = (direction * -1) * strength;
            _simulationController.SimulateLandingSpot(gameObject, force, torque);
            _lastTeamId = teamId;
            _rigidbody.AddTorque(torque);
            _rigidbody.AddForce(force, ForceMode.Force);
            ChangedDirection?.Invoke(this, new BallChangedDirectionEventArgs
            {
                LandingSpot = LandingSpot,
                TeamId = teamId
            });
            DrawTrajectory();
            audioSource?.Play();
        }

        public float GetNeededForceFromSimulation(Vector3 startPosition, Vector3 target, Vector3 direction)
        {
            return _simulationController.TryGetNeededForce(gameObject, startPosition, target, direction);
        }

        public void Stop(Player player = null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }



        public void SetMatchController(MatchController controller)
        {
            _matchController = controller;
            _simulationController = _matchController.GetComponent<PhysicsSceneController>();
        }
        public void disableGravity()
        {
            _rigidbody.useGravity = false;
        }
        public void EnableGravity() => _rigidbody.useGravity = true;

        void OnCollisionEnter(Collision collision)
        {
            if (_matchController == null) return;
            var touchedTheGround = collision.contacts.Any(x => x.point.y <= 0.5);
            if (!IsInPlayField())
            {
                _matchController.AddScoreAgainst(_lastTeamId);
            }
            if (!touchedTheGround) return;
            if (IsInCourt()) _matchController.AddScore(_lastTeamId);
            else _matchController.AddScoreAgainst(_lastTeamId);
        }

        private bool IsInPlayField() => Position.IsInPlayField();

        private bool IsInCourt() => Position.IsInCourt();

        public void DrawTrajectory()
        {
            if (Trajectory == null) return;
            var component = GetComponent<LineRenderer>();
            if (component == null) return;
            var positionArray = Trajectory.Points.ToArray();
            component.startWidth = 0.05f;
            component.endWidth = 0.05f;
            
            component.positionCount = positionArray.Count();
            component.SetPositions(positionArray);
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            if (LandingSpot.HasValue)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(LandingSpot.Value, 0.2f);
            }

        }
    }
}