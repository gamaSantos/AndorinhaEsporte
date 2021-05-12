using System;
using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.Domain.State;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class Match
    {
        public Match(Guid id, Team homeTeam, Team awayTeam)
        {
            Id = id;
            _teams = new List<Team>();
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            _teams.Add(homeTeam);
            _teams.Add(awayTeam);
            Stats = new MatchStats(homeTeam, awayTeam);
        }

        public Guid Id { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public MatchStats Stats { get; private set; }
        private List<Team> _teams;

        public bool IsTeamBusy()
        {
            return IsTeamInRotation() || IsTeamInRotation();
        }
        public bool IsTeamInRotation()
        {
            return HomeTeam.InRotation() || AwayTeam.InRotation();
        }

        internal bool IsServing()
        {
            return HomeTeam.IsServing() || AwayTeam.IsServing();
        }

        public void ApproveServe()
        {
            if (!HomeTeam.IsWaitingForServe() && !AwayTeam.IsWaitingForServe()) return;
            List<Player> players = GetAllPlayers();

            var playerWaiting = players.FirstOrDefault(p => p.ServeState == ServeStateEnum.AwaitingApproval);
            if (playerWaiting == null) return;
            playerWaiting.ServeState = ServeStateEnum.Approved;
        }

        public Player GetServingPlayer()
        {
            var players = GetAllPlayers();
            return players.FirstOrDefault(p => p.ServeState == ServeStateEnum.AwaitingApproval);
        }

        public bool IsChangingSides()
        {
            return HomeTeam.IsChangingSides() || HomeTeam.IsChangingSides();
        }

        public void ChangeSides()
        {
            if (Stats.IsFinished) return;
            Stats.ResetScore();
            HomeTeam.ChangeSides();
            AwayTeam.ChangeSides();
        }

        public bool FinishedServe()
        {
            var players = GetAllPlayers();
            return players.Any(p => p.ServeState == ServeStateEnum.Finished);
        }
        public Guid GetOponnentId(Guid teamId) => _teams.First(x => x.Id != teamId).Id;
        public Guid GetTeamIdFromContactPoint(Vector3 contactPoint)
        {
            var horizontalPosition = contactPoint.z;
            if (horizontalPosition < 0) return _teams.First(team => team.Foward.z > 0).Id;
            else return _teams.First(team => team.Foward.z < 0).Id;
        }
        private List<Player> GetAllPlayers()
        {
            var players = HomeTeam.Formation.ToList();
            players.AddRange(AwayTeam.Formation);
            return players;
        }


    }
}
