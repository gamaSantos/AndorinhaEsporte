using System;
using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.Domain.State;

namespace AndorinhaEsporte.Domain
{
    public class Match
    {
        public Match(Guid id, Team homeTeam, Team awayTeam)
        {
            Id = id;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Stats = new MatchStats(new TimeSpan(0, 59, 0));
        }

        public Guid Id { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public MatchStats Stats { get; private set; }

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

        public bool IsChangingSides()
        {
            return HomeTeam.IsChangingSides() || HomeTeam.IsChangingSides();
        }

        public void ChangeSides()
        {
            if(Stats.IsFinished) return;
            Stats.ResetScore();
            HomeTeam.ChangeSides();
            AwayTeam.ChangeSides();            
        }

        public bool FinishedServe()
        {
            var players = GetAllPlayers();
            return players.Any(p => p.ServeState == ServeStateEnum.Finished);
        }


        private List<Player> GetAllPlayers()
        {
            var players = HomeTeam.Formation.ToList();
            players.AddRange(AwayTeam.Formation);
            return players;
        }


    }
}
