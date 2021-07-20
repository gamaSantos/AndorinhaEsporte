using AndorinhaEsporte.Data;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.UI;
using System;
using UnityEngine;

namespace AndorinhaEsporte.Services
{
    public class MatchService
    {
        private readonly TeamRepository _teamRepository;
        private static MatchService _matchService;
        private static Match _currentMatch;
        private MatchService()
        {
            _teamRepository = new TeamRepository();
        }

        public static Match Current
        {
            get
            {
                if (_matchService == null) _matchService = new MatchService();
                if (_currentMatch == null)
                {
                    _currentMatch = _matchService.GetMatch(Guid.NewGuid());
                }
                return _currentMatch;
            }
        }

        private Match GetMatch(Guid matchId)
        {

            var homeTeam = _teamRepository.GetTeam(0);
            var awayTeam = _teamRepository.GetTeam(2);
            if (MainMenuController.SELECTED_TEAM_ID != Guid.Empty)
            {
                homeTeam = _teamRepository.GetTeam(MainMenuController.SELECTED_TEAM_ID);
                awayTeam = _teamRepository.GetTeam(MainMenuController.OPPONENT_TEAM_ID);
            }

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

        private void GetTeamPlayers(Team team, bool homeTeam)
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