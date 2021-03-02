using System;
using AndorinhaEsporte.Domain.Events;

namespace AndorinhaEsporte.Domain
{
    public class MatchStats
    {
        private const int SET_POINT_LIMIT = 1;
        private const int MAX_SET_COUNT = 3;
        public MatchStats(TimeSpan matchLenght)
        {
            matchTime = matchLenght.TotalSeconds;
            CurrentSet = 1;
        }

        private double matchTime { get; set; }
        public TimeSpan CurrentTime => TimeSpan.FromSeconds(matchTime);
        public bool IsFinished => (matchTime <= 0 || ReachedPointLimit()) && (HomeSetCount > (MAX_SET_COUNT / 2) || AwaySetCount > (MAX_SET_COUNT / 2));
        public bool IsSetFinished => matchTime <= 0 || ReachedPointLimit();
        public bool IsHomeWinner => HomeScore > AwayScore;
        public int HomeScore { get; private set; }

        public int AwayScore { get; private set; }

        public int CurrentSet { get; private set; }
        public int HomeSetCount { get; private set; }
        public int AwaySetCount { get; private set; }

        private Guid _lastTouchTeamId { get; set; }

        public int BallTouchCount { get; private set; }
        public bool IsServe { get; private set; }


        public void UpdateMatchTimer(double timeFromLastUpdate)
        {
            if (IsFinished) return;
            matchTime -= timeFromLastUpdate;
        }

        private bool ReachedPointLimit()
        {
            var pointDif = Math.Abs(HomeScore - AwayScore);
            if (pointDif < 2) return false;
            return HomeScore >= SET_POINT_LIMIT || AwayScore >= SET_POINT_LIMIT;
        }

        internal void ResetScore()
        {
            if (!IsFinished)
            {
                HomeScore = 0;
                AwayScore = 0;
            }
        }

        public void AddScore(bool isHomeTeam)
        {
            if (isHomeTeam)
            {
                HomeScore++;
            }
            else
            {
                AwayScore++;
            }
            if (IsSetFinished)
            {
                if (IsHomeWinner) HomeSetCount++;
                else AwaySetCount++;
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
            var teamId = e.TeamId;
            if (teamId != _lastTouchTeamId)
            {
                BallTouchCount = 1;
                _lastTouchTeamId = teamId;
                IsServe = false;
                return;
            }
            if(IsServe) return;
            BallTouchCount++;            
        }
    }
}