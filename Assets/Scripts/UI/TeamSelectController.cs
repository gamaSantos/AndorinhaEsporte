using System;
using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.Data;
using AndorinhaEsporte.Domain;
using UnityEngine.UIElements;

namespace AndorinhaEsporte.UI
{
    public class TeamSelectController
    {
        private int _selectedTeamIndex;
        private VisualElement _container;
        private Label _teamNameLabel;
        private VisualElement _teamLogo;
        TeamRepository _teamRepository;
        IEnumerable<Team> _teams;

        public bool Visible => _container.visible;

        public event EventHandler<TeamSelectedEventArgs> TeamSelected;

        public TeamSelectController(VisualElement root, TeamRepository teamRepository)
        {
            _teamRepository = new TeamRepository();
            _teams = _teamRepository.List();
            
            _container = root.Q<VisualElement>("TeamContainer");
            _teamNameLabel = root.Q<Label>("TeamName");
            _teamLogo = root.Q<VisualElement>("TeamLogo");

            _container.Q<Button>("SelectTeamButton").clicked += SelectTeamButton;
            _container.Q<Button>("PreviousTeamButton").clicked += () => { _selectedTeamIndex = BindTeam(_selectedTeamIndex - 1); };
            _container.Q<Button>("NextTeamButton").clicked += () => { _selectedTeamIndex = BindTeam(_selectedTeamIndex + 1); };
            _selectedTeamIndex = BindTeam(_selectedTeamIndex);
        }

        public void Show()
        {
            _container.style.display = DisplayStyle.Flex;
            _container.visible = true;
        }

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
            _container.visible = false;
        }

        private int BindTeam(int index)
        {
            if (_teamNameLabel == null) return 0;
            if (_teamLogo == null) return 0;
            index = GetNextIndex(index);
            var team = _teams.ElementAt(index);
            _teamNameLabel.text = team.InMatchInformation.Name.ToUpper();
            _teamLogo.style.unityBackgroundImageTintColor = team.InMatchInformation.MainColor;
            return index;
        }

        private int GetNextIndex(int index)
        {
            index = index >= _teams.Count() ? 0 : index;
            index = index < 0 ? _teams.Count() - 1 : index;
            return index;
        }

        private void SelectTeamButton()
        {
            var team = _teams.ElementAt(_selectedTeamIndex);
            var teamId = team.Id;
            var opponentId = _teams.ElementAt(GetNextIndex(_selectedTeamIndex + 1)).Id;
            TeamSelected?.Invoke(this, new TeamSelectedEventArgs(teamId, opponentId));
        }
    }

    public class TeamSelectedEventArgs : EventArgs
    {
        public TeamSelectedEventArgs(Guid selectedTeamId, Guid opponentTeamId)
        {
            SelectedTeamId = selectedTeamId;
            OpponentTeamId = opponentTeamId;
        }

        public Guid SelectedTeamId { get; }
        public Guid OpponentTeamId { get; }
    }
}