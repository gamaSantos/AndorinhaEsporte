using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using AndorinhaEsporte.Domain.Events;

namespace AndorinhaEsporte.Domain
{
    public class Team
    {
        public Team(TeamInMatchInformation inMatchInfo)
        {
            Id = inMatchInfo.TeamId;
            Players = new List<Player>();
            InMatchInformation = inMatchInfo;
        }
        public Guid Id { get; }
        public Vector3 Foward => InMatchInformation.Foward;
        public TeamInMatchInformation InMatchInformation { get; private set; }
        public bool HasControllerAssociated { get; set; }
        private int _currentPlayerIndex = 0;
        public List<Player> Players { get; private set; }
        public IEnumerable<Player> Formation => Players;
        public event EventHandler<TeamPassingEvent> PassingStateChange;
        private bool _nextBallTransitionIsServe = true;
        private Player _setter;

        public void SetFoward(Vector3 direction)
        {
            InMatchInformation.Foward = direction;
        }

        public Guid? GetNearestPlayerIdFrom(Vector3 targetPosition)
        {
            if (!Formation.Any()) return null;
            Guid? playerId = null;
            PlayerPositionType type;
            float minDistance = 10000f;
            foreach (var player in Players)
            {
                var heading = targetPosition - player.Position;
                var distance = heading.sqrMagnitude;
                if (playerId.HasValue && distance > minDistance) continue;
                playerId = player.Id;
                type = player.CurrentFunction;
                minDistance = heading.sqrMagnitude;
            }
            return playerId;
        }



        internal void Spike()
        {
            var players = Formation.Where(p => p.CurrentFunction == PlayerPositionType.LeftStriker || p.CurrentFunction == PlayerPositionType.RightStriker);
            foreach (var player in players) player.StartSpike();
        }

        internal void Pass()
        {
            _setter = Formation.FirstOrDefault(player => player.CurrentFunction == PlayerPositionType.Center);
            if (PassingStateChange != null) PassingStateChange.Invoke(this, new TeamPassingEvent(this.Id, true));
            _setter.Pass();
        }

        internal bool IsServing()
        {
            return Formation.Any(player => player.ServeState != State.ServeStateEnum.Initial);
        }

        internal bool IsChangingSides()
        {
            return Formation.Any(player => player.IsChangingSides);
        }

        internal bool IsResetingPosition()
        {
            return Formation.Any(p => p.CurrentAction == PlayerAction.ResetPosition);
        }

        internal void ChangeSides()
        {
            InMatchInformation.Foward = Foward * -1;
            foreach (var player in Formation)
                player.ChangeSides();
        }

        public void Defend()
        {
            foreach (var player in Formation)
            {
                if (player.InBlockPosition && !_nextBallTransitionIsServe)
                {
                    player.AddAction(PlayerAction.Block);
                }
                else
                {
                    player.AddAction(PlayerAction.Defend);
                }

            }
            _nextBallTransitionIsServe = false;
        }

        public void AddPlayer(Player newPlayer)
        {
            foreach (var player in Players)
            {
                player.AddTeammate(newPlayer);
                newPlayer.AddTeammate(player);
            }
            newPlayer.ChangedState += OnPlayerStateChange;
            Players.Add(newPlayer);
        }

        private void OnPlayerStateChange(object sender, EventArgs e)
        {
            var player = sender as Player;
            if (_setter == null) return;
            if (player.Id == _setter.Id && _setter.CurrentAction != PlayerAction.Pass)
            {
                PassingStateChange.Invoke(this, new TeamPassingEvent(Id, false));
                _setter = null;
            }

        }

        internal void CheckBall(object sender, BallChangedDirectionEvent e)
        {
            if (Id == e.LastTouchTeamId) return;
            if (!e.LandingSpot.HasValue) return;
            var landingSpot = e.LandingSpot.Value;
            var foward = this.InMatchInformation.Foward.z;

            var incomming = (foward < 0 && e.FowardDirection > 0) || (foward > 0 && e.FowardDirection < 0);
            if (!incomming)
            {
                foreach (var player in Formation)
                {
                    player.AddAction(PlayerAction.MoveToDenfensivePosition);
                }
            }
            else
            {
                Defend();
            }

        }

        public void Serve()
        {
            var playerInServePosition = Formation.OrderBy(x => x.FieldPosition.RotationOrder).First();
            playerInServePosition.StartServing();
        }

        internal void Rotate()
        {
            if (InRotation()) return;
            _nextBallTransitionIsServe = true;
            foreach (var player in Formation)
            {
                player.AddRotationAction();
            }
        }

        public bool IsWaitingForServe()
        {
            return Formation.Any(p => p.ServeState == State.ServeStateEnum.AwaitingApproval);
        }

        public void ResetState()
        {
            foreach (var player in Formation)
            {
                player.ServeState = State.ServeStateEnum.Initial;
                player.ClearActions();
            }
        }

        public void ResetPosition()
        {
            foreach (var player in Formation)
            {
                player.AddAction(PlayerAction.ResetPosition);
            }
            _nextBallTransitionIsServe = true;
        }

        public bool InRotation()
        {
            return Formation.Any(p => p.RotateState != State.RotateStateEnum.Finished);
        }


        internal void ChangePlayerControlledByUser()
        {
            var maxIndex = Players.Count - 1;

            var newIndex = _currentPlayerIndex + 1 > maxIndex ? 0 : _currentPlayerIndex + 1;
            var currentPlayer = Players.ElementAt(_currentPlayerIndex);
            var newPlayer = Players.ElementAt(newIndex);

            currentPlayer.IsUserControlled = false;
            newPlayer.IsUserControlled = true;
            _currentPlayerIndex = newIndex;
        }
    }
}