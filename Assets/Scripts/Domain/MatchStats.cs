using System;
using AndorinhaEsporte.Domain.Events;

namespace AndorinhaEsporte.Domain
{
    public class MatchStats
    {
        private const int SET_POINT_LIMIT = 1;
        private const int MAX_SET_COUNT = 3;
        public MatchStats(Team hometeam, Team awayteam)
        {
            CurrentSet = 1;
            HomeStats = new TeamStats() { Name = hometeam.InMatchInformation.Name };
            AwayStats = new TeamStats() { Name = awayteam.InMatchInformation.Name };
        }

        public bool IsFinished => ReachedPointLimit() && (HomeStats.WinnedSetCount > (MAX_SET_COUNT / 2) || AwayStats.WinnedSetCount > (MAX_SET_COUNT / 2));
        public bool IsSetFinished => ReachedPointLimit();
        public bool IsHomeWinner => HomeStats.Score > AwayStats.Score;


        public TeamStats HomeStats { get; set; }
        public TeamStats AwayStats { get; set; }



        public int CurrentSet { get; private set; }

        private Guid _lastTouchTeamId { get; set; }

        public int BallTouchCount { get; private set; }
        public bool IsServe { get; private set; }

        private bool ReachedPointLimit()
        {
            var pointDif = Math.Abs(HomeStats.Score - AwayStats.Score);
            if (pointDif < 2) return false;
            return HomeStats.Score >= SET_POINT_LIMIT || AwayStats.Score >= SET_POINT_LIMIT;
        }

        internal void ResetScore()
        {
            if (!IsFinished)
            {
                HomeStats.Score = 0;
                AwayStats.Score = 0;
            }
        }

        public void AddScore(bool isHomeTeam)
        {
            if (isHomeTeam)
            {
                HomeStats.Score++;
            }
            else
            {
                AwayStats.Score++;
            }
            if (IsSetFinished)
            {
                if (IsHomeWinner) HomeStats.WinnedSetCount++;
                else AwayStats.WinnedSetCount++;
                CurrentSet++;
            }


        }

        public void StartedServe(Guid teamId)
        {
            IsServe = true;
            _lastTouchTeamId = teamId;
        }
        internal void CountTouches(object sender, BallChangedDirectionEventArgs e)
        {
            var teamId = e.LastTouchTeamId;
            if (teamId != _lastTouchTeamId)
            {
                BallTouchCount = 1;
                _lastTouchTeamId = teamId;
                IsServe = false;
                return;
            }
            if (IsServe) return;
            BallTouchCount++;
        }
    }

    public class TeamStats
    {
        public string Name { get; set; }
        public int WinnedSetCount { get; set; }
        public int Score { get; set; }
    }
}