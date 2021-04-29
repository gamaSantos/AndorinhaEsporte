using AndorinhaEsporte.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AndorinhaEsporte.Data
{
    public class TeamRepository
    {
        private List<TeamInMatchInformation> teams;
        public TeamRepository()
        {
            teams = new List<TeamInMatchInformation>();
            teams.AddRange(new TeamInMatchInformation[]
                {
                    new TeamInMatchInformation(new Guid("790f5d8e-ca2d-4ba4-a99c-7b9e93014880"),"Red Team", new Color(.8f,.1f,.1f), Color.white),
                    new TeamInMatchInformation(new Guid("6a11a952-027f-47b6-aa48-27fd548c4f45"),"blue Team", Color.blue, Color.white),
                    new TeamInMatchInformation(new Guid("a43cf41a-a7f6-424c-8e74-3a7f8fc33abf"),"green Team", Color.green, Color.white)
                }
            );
        }
        public Team GetTeam(int index)
        {
            if (!teams.Any() || teams.Count() < index || index < 0) return GetRandomTeam();
            var teamData = teams[index];
            return new Team(teamData.TeamId, teamData);
        }

        private Team GetRandomTeam()
        {
            var id = System.Guid.NewGuid();
            return new Team(id, new TeamInMatchInformation(id, "TestTeam", Color.black, Color.white));
        }
    }
}