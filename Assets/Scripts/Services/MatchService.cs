using AndorinhaEsporte.Data;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.UI;
using System;
using UnityEngine;

namespace AndorinhaEsporte.Services
{
    public static class MatchService
    {

        private static Match _currentMatch;

        public static Match Current
        {
            get
            {
                if (_currentMatch == null)
                {
                    _currentMatch = CreateRandomMatch(Guid.NewGuid());
                }
                return _currentMatch;
            }
        }

        public static void ResumeMatch()
        {
            if (_currentMatch == null) return;
            _currentMatch.IsPaused = false;
        }

        public static void StopMatch()
        {
            if (_currentMatch == null) return;
            _currentMatch.IsPaused = true;
        }

        public static Match CreateMatch(Guid homeTeamId, Guid awayTeamId)
        {
            var teamRepository = new TeamRepository();
            var homeTeam = teamRepository.GetTeam(homeTeamId);
            var awayTeam = teamRepository.GetTeam(awayTeamId);

            homeTeam.SetFoward(Vector3.forward);
            homeTeam.HasControllerAssociated = true;
            GetTeamPlayers(homeTeam, true);

            awayTeam.SetFoward(Vector3.back);
            GetTeamPlayers(awayTeam, false);
            var match = new Match(Guid.NewGuid(), homeTeam, awayTeam);
            foreach (var player in homeTeam.Players) player.SetMatchStatus(match.Stats);
            foreach (var player in awayTeam.Players) player.SetMatchStatus(match.Stats);
            _currentMatch = match;
            return match;
        }

        private static Match CreateRandomMatch(Guid matchId)
        {
            var teamRepository = new TeamRepository();
            var homeTeam = teamRepository.GetTeam(0);
            var awayTeam = teamRepository.GetTeam(2);

            homeTeam.SetFoward(Vector3.forward);
            homeTeam.HasControllerAssociated = true;
            GetTeamPlayers(homeTeam, true);

            awayTeam.SetFoward(Vector3.back);
            GetTeamPlayers(awayTeam, false);
            var match = new Match(Guid.NewGuid(), homeTeam, awayTeam);

            foreach (var player in homeTeam.Players) player.SetMatchStatus(match.Stats);
            foreach (var player in awayTeam.Players) player.SetMatchStatus(match.Stats);
            return match;
        }

        private static void GetTeamPlayers(Team team, bool homeTeam)
        {
            var teamLength = 6;
            for (int i = 0; i < teamLength; i++)
            {
                var position = FieldPositionFactory.Create((PlayerPositionType)i + 1);
                var player = new Player(position, team.InMatchInformation)
                {
                    isHomeTeam = homeTeam,
                };
                if (!team.HasControllerAssociated) player.IsUserControlled = false;
                team.AddPlayer(player);
            }
        }
    }
}