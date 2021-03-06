using AndorinhaEsporte.Domain;
using System;

namespace AndorinhaEsporte.Services
{
    public class MatchService
    {

        public Match GetMatch(Guid matchId)
        {
            var homeTeam = new Team(Guid.NewGuid(), true);
            GetTeamPlayers(homeTeam, true);
            var awayTeam = new Team(Guid.NewGuid(), false);
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