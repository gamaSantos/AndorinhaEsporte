using AndorinhaEsporte.Data;
using AndorinhaEsporte.Domain;
using System;
using UnityEngine;

namespace AndorinhaEsporte.Services
{
    public class MatchService
    {
        private readonly TeamRepository _teamRepository;
        public MatchService()
        {
            _teamRepository = new TeamRepository();
        }
        public Match GetMatch(Guid matchId)
        {
            var homeTeam = _teamRepository.GetTeam(0);
            homeTeam.SetFoward(Vector3.forward);
            GetTeamPlayers(homeTeam, true);
            var awayTeam = _teamRepository.GetTeam(1);
            awayTeam.SetFoward(Vector3.back);
            GetTeamPlayers(awayTeam, false);
            var match = new Match(Guid.NewGuid(), homeTeam, awayTeam);

            foreach (var player in homeTeam.Players) player.SetMatchStatus(match.Stats);
            foreach (var player in awayTeam.Players) player.SetMatchStatus(match.Stats);
            return match;
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
                team.AddPlayer(player);
            }
        }
    }
}