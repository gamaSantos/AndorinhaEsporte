using System.Collections.Generic;
using System.Linq;
using System;
using AndorinhaEsporte.Domain.State;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class Player
    {
        private readonly List<PlayerAction> _actions;
        private readonly TeamInMatchInformation _team;
        private readonly List<Player> _teammates;

        public Player(FieldPosition fieldPosition, TeamInMatchInformation teamInfo)
        {
            _actions = new List<PlayerAction>();
            _teammates = new List<Player>();
            _team = teamInfo;
            FieldPosition = fieldPosition;
            if (fieldPosition.Type == PlayerPositionType.RightStriker) IsUserControlled = true;
            ResetStates();
        }

        public Vector3 GetDefensivePosition(Vector3 position)
        {
            if (InBlockPosition)
            {
                return GetBlockPosition(Position);

            }
            else
            {
                return GetSpikeDefensivePosition(Position);
            }
        }

        private Vector3 GetBlockPosition(Vector3 position)
        {
            return FieldPosition.GetStartPosition(TeamFoward);
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
        }

        public bool ArrivedInTarget(Vector3 target)
        {
            var groundTarget = new Vector3(target.x, 0, target.z);
            return Position.Distance(groundTarget) <= 0.25f;
        }

        public bool InDefenseRange(Vector3 ballPosition)
        {
            var groundTarget = new Vector3(ballPosition.x, 0, ballPosition.z);
            return Position.Distance(ballPosition) < 1f;
        }

        public bool InPassRange(Vector3 ballPosition)
        {
            var groundTarget = new Vector3(ballPosition.x, 0, ballPosition.z);
            return Position.Distance(groundTarget) < 0.5f && ballPosition.y < 2.4;
        }

        public bool InSpikeRange(Vector3 ballPosition)
        {
            var groundTarget = new Vector3(ballPosition.x, 0, ballPosition.z);
            return Position.Distance(groundTarget) < 0.5f && ballPosition.y < SpikeHeight;
        }


        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }

        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public bool isHomeTeam { get; set; }
        public PlayerPositionType CurrentFunction => this.FieldPosition.Type;
        public FieldPosition FieldPosition { get; private set; }
        public IReadOnlyCollection<Player> Teammates => _teammates;


        private float _cooldown;
        public float MoveSpeed = 9000f;
        public float PreciseMoveSpeed = 6000;
        internal float BallControlRange => 1f;

        internal void Pass()
        {
            ClearActions();
            ResetStates();
            _actions.Add(PlayerAction.Pass);
        }

        public bool IsUserControlled { get; private set; }

        public Vector3 TeamFoward => _team.Foward;
        public Color MainColor => _team.MainColor;
        public PlayerAction CurrentAction => _actions.Any() ? _actions.First() : PlayerAction.Idle;


        public RotateStateEnum RotateState { get; set; }
        public ServeStateEnum ServeState { get; set; }
        public ChangeSideStateEnum ChangeSideState { get; set; }
        public bool IsChangingSides => ChangeSideState != ChangeSideStateEnum.Finished;

        public bool Passing { get; internal set; }
        public bool InAir => Position.y >= 0.05f && Velocity.y < 0;

        public bool IsPassTarget { get; internal set; }
        public float SpikeHeight => 3.3f;

        public bool InBlockPosition => FieldPosition.InFrontRow;

        public void UpdatePosition(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
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

        public void NotifyIsPassTarget()
        {
            IsPassTarget = true;
        }

        public void ChangeSides()
        {
            this.ChangeSideState = ChangeSideStateEnum.Initial;
            AddAction(PlayerAction.ChangeSides);
        }
        internal void RotatePosition()
        {
            this.FieldPosition = FieldPosition.GetNextFieldPosition();
            RotateState = RotateStateEnum.Rotating;
        }

        internal void FinishRotation()
        {
            RotateState = RotateStateEnum.Finished;
            RemoveAction(PlayerAction.Rotate);
        }

        public void AddRotationAction()
        {
            RotateState = RotateStateEnum.Initial;
            AddAction(PlayerAction.Rotate);
        }

        public void StartServing()
        {
            ServeState = ServeStateEnum.Initial;
            AddAction(PlayerAction.Serve);
        }
    }
}