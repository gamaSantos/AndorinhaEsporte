using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using AndorinhaEsporte.Domain.Events;

namespace AndorinhaEsporte.Domain
{
    public class Team
    {
        public Team(Guid id, bool isHomeTeam)
        {
            Id = id;
            Players = new List<Player>();
            var foward = isHomeTeam ? Vector3.forward : Vector3.back;

            UnityEngine.Random.InitState(id.GetHashCode());
            InMatchInformation = new TeamInMatchInformation(Id, foward, UnityEngine.Random.ColorHSV(0, 1, 0.7f, 1), UnityEngine.Random.ColorHSV());
        }
        public Guid Id { get; }
        public Vector3 Foward => InMatchInformation.Foward;
        public TeamInMatchInformation InMatchInformation { get; private set; }
        public List<Player> Players { get; private set; }
        public IEnumerable<Player> Formation => Players;

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
            var setter = Formation.FirstOrDefault(player => player.CurrentFunction == PlayerPositionType.Center);

            setter.Pass();
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
                if (player.InBlockPosition)
                {
                    player.AddAction(PlayerAction.Block);
                }
                else
                {
                    player.AddAction(PlayerAction.Defend);
                }

            }
        }

        public void AddPlayer(Player newPlayer)
        {
            foreach (var player in Players)
            {
                player.AddTeammate(newPlayer);
                newPlayer.AddTeammate(player);
            }
            Players.Add(newPlayer);
        }

        internal void CheckBall(object sender, BallChangedDirectionEventArgs e)
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
        }

        public bool InRotation()
        {
            return Formation.Any(p => p.RotateState != State.RotateStateEnum.Finished);
        }
    }
}