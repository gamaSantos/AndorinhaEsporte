using System.Collections.Generic;
using System.Linq;
using System;
using AndorinhaEsporte.Domain.State;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class Player
    {
        private static PlayerAction[] ForcedPlayerActions = new PlayerAction[]
        {
            PlayerAction.Serve,
            PlayerAction.Rotate,
            PlayerAction.ResetPosition,
            PlayerAction.ChangeSides
        };

        private readonly Guid _id;
        private readonly List<PlayerAction> _actions;
        private readonly TeamInMatchInformation _team;
        private readonly List<Player> _teammates;
        private readonly List<Player> _opponents;
        private MatchStats _matchStatus;
        public Player(FieldPosition fieldPosition, TeamInMatchInformation teamInfo)
        {
            _id = Guid.NewGuid();
            _actions = new List<PlayerAction>();
            _teammates = new List<Player>();
            _team = teamInfo;
            FieldPosition = fieldPosition;
            if (fieldPosition.Type == PlayerPositionType.RightStriker) IsUserControlled = true;
            _opponents = new List<Player>();
            ResetStates();
        }

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }

        public Guid Id => _id;
        public Guid TeamId => _team.TeamId;
        public bool isHomeTeam { get; set; }
        public PlayerPositionType CurrentFunction => this.FieldPosition.Type;
        public FieldPosition FieldPosition { get; private set; }
        public IReadOnlyCollection<Player> Teammates => _teammates;

        public IReadOnlyCollection<Player> Opponents => _opponents;


        public float MoveSpeed = 9000f;
        public float PreciseMoveSpeed = 6000;
        public float SpikeHeight => 3.3f;


        public bool IsUserControlled { get; private set; }
        public bool IsInForcedAction => ForcedPlayerActions.Contains(CurrentAction);

        public Vector3 TeamFoward => _team.Foward;
        public Color MainColor => _team.MainColor;
        public PlayerAction CurrentAction => _actions.Any() ? _actions.First() : PlayerAction.Idle;

        internal void StartSpike()
        {
            SpikeState = SpikeStateEnum.Initial;
            IsSpiking = false;
            AddAction(PlayerAction.Spike);
        }

        public RotateStateEnum RotateState { get; set; }
        public ServeStateEnum ServeState { get; set; }
        public ChangeSideStateEnum ChangeSideState { get; set; }
        public SpikeStateEnum SpikeState { get; set; }
        public bool IsChangingSides => ChangeSideState != ChangeSideStateEnum.Finished;

        public bool Passing { get; set; }

        public bool IsPassTarget { get; set; }

        public bool InAir => Position.y >= 0.2f;
        public bool IsJumping = false;
        public bool InBlockPosition => Position.Distance(new Vector3(Position.x, 0, 0)) < 2;

        public bool IsSpiking { get; set; }
        public bool IsDefending { get; set; }
        public bool IsPassing { get; set; }
        public bool IsBlocking { get; set; }

        public void UpdatePosition(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
        public void SetMatchStatus(MatchStats matchStats)
        {
            _matchStatus = matchStats;
        }

        public void AddTeammate(Player teammate, Guid? subId = null)
        {
            if (teammate.TeamId != TeamId || teammate.Id == Id) return;
            if (subId.HasValue)
            {
                var sub = _teammates.FirstOrDefault(x => x.Id == subId.Value);
                if (teammate != null)
                {
                    _teammates.Remove(_teammates.FirstOrDefault(x => x.Id == subId.Value));
                }
            }
            _teammates.Add(teammate);
        }


        public void AddAction(PlayerAction action)
        {
            if (_actions.Contains(action) || action == PlayerAction.Idle) return;
            _actions.Add(action);
        }

        public void RemoveAction(PlayerAction action)
        {
            var actions = _actions.FirstOrDefault(x => x == action);
            if (actions == default(PlayerAction)) return;
            _actions.Remove(actions);
            Passing = false;
        }

        public void ClearActions()
        {
            _actions.Clear();
        }

        public void RemoveUserControl()
        {
            IsUserControlled = false;
        }

        internal void Pass()
        {
            ClearActions();
            ResetStates();
            _actions.Add(PlayerAction.Pass);
        }

        public void ChangePassTargeState(bool isPassTarget)
        {
            IsPassTarget = isPassTarget;
            if (SpikeState == SpikeStateEnum.AwaitingOrMovingToPassTarget && !isPassTarget)
            {
                SpikeState = SpikeStateEnum.Finished;
                RemoveAction(PlayerAction.Spike);
                IsSpiking = false;
            }
        }

        public void ChangeSides()
        {
            this.ChangeSideState = ChangeSideStateEnum.Initial;
            AddAction(PlayerAction.ChangeSides);
        }
        public void RotatePosition()
        {
            this.FieldPosition = FieldPosition.GetNextFieldPosition();
            RotateState = RotateStateEnum.Rotating;
        }

        public void FinishRotation()
        {
            RotateState = RotateStateEnum.Finished;
            RemoveAction(PlayerAction.Rotate);
        }

        public void AddRotationAction()
        {
            ResetStates();
            RotateState = RotateStateEnum.Initial;
            AddAction(PlayerAction.Rotate);
        }

        public void StartServing()
        {
            ServeState = ServeStateEnum.Initial;
            AddAction(PlayerAction.Serve);
        }


        public Vector3 GetDefensivePosition(Vector3 ballPosition)
        {
            if (InBlockPosition)
            {
                return GetBlockDefensivePosition(ballPosition);
            }
            else
            {
                return GetSpikeDefensivePosition(Position);
            }
        }

        private Vector3 GetBlockDefensivePosition(Vector3 ballPosition)
        {
            if (_matchStatus?.IsServe ?? false) return Position;
            var fowardDirection = TeamFoward.z;
            var netDistance = 0.1f * fowardDirection;
            if(!Opponents.Any()) return Position;
            var horizontalPosition = Opponents.OrderBy(x => Position.Distance(x.Position)).First().Position.x;
            return new Vector3(horizontalPosition, 0, netDistance);
        }

        private Vector3 GetSpikeDefensivePosition(Vector3 position)
        {
            return FieldPosition.GetStartPosition(TeamFoward);
        }

        public bool IsDefenseNecessary(Vector3 landingSpot)
        {
            return TeamFoward.IsFowardOfNet() != landingSpot.IsFowardOfNet();
        }

        private void ResetStates()
        {
            RotateState = RotateStateEnum.Finished;
            ServeState = ServeStateEnum.Initial;
            ChangeSideState = ChangeSideStateEnum.Finished;
            IsPassTarget = false;
            IsSpiking = false;
        }
        private Vector3 GetGroundPosition(Vector3 target)
        {
            return new Vector3(target.x, 0, target.z);
        }

        public bool ArrivedInTarget(Vector3 target)
        {
            var groundTarget = GetGroundPosition(target);
            return Position.Distance(groundTarget) <= 0.25f;
        }

        public bool InDefenseRange(Vector3 ballPosition)
        {
            var groundTarget = GetGroundPosition(ballPosition);
            return Position.Distance(groundTarget) < 1f && ballPosition.y < 2;
        }

        public bool InBlockRange(Vector3 ballPosition)
        {
            var target = GetGroundPosition(ballPosition);
            return Position.Distance(target) < 1f;
        }
        public bool CanBlock(Vector3 ballPosition)
        {
            if (Position.IsFowardOfNet() != ballPosition.IsFowardOfNet()) return true;

            var ballDistanceFromNet = 0 + Mathf.Abs(ballPosition.z + .1f);
            var playerDistanceFromNet = 0 + Mathf.Abs(Position.z);

            return playerDistanceFromNet >= ballDistanceFromNet;
        }
        public bool InPassRange(Vector3 ballPosition)
        {
            var groundTarget = GetGroundPosition(ballPosition);
            return Position.Distance(groundTarget) < 0.5f && ballPosition.y < 2.3;
        }

        public bool InExtendedPassRange(Vector3 ballPosition)
        {
            return Position.Distance(ballPosition) < 3.5f;
        }

        public bool InSpikeRange(Vector3 ballPosition)
        {
            var groundTarget = GetGroundPosition(ballPosition);
            return isBallIn2DSpikeRange(groundTarget) && ballPosition.y < SpikeHeight;
        }

        private bool isBallIn2DSpikeRange(Vector3 groundTarget, float maxDistance = 0.8f) => Position.Distance(groundTarget) < maxDistance;
        public bool CanStartSpikeJump(Vector3 ballPosition, Vector3 ballVelocity)
        {
            if (InAir) return false;
            var ballHeight = ballPosition.y;
            var groundTarget = new Vector3(ballPosition.x, 0, ballPosition.z);
            return isBallIn2DSpikeRange(groundTarget, 1f) &&
            ballHeight < SpikeHeight + 2.3f && ballVelocity.y < 0;
        }


    }
}