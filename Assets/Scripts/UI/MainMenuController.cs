using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using AndorinhaEsporte.Data;
using System.Collections.Generic;
using AndorinhaEsporte.Domain;

namespace AndorinhaEsporte.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public static Guid SELECTED_TEAM_ID;
        public static Guid OPPONENT_TEAM_ID;


        Button _startButton;
        private VisualElement _teamContainer;
        private VisualElement _mainMenuContainer;

        private int _selectedTeamIndex;
        private Label _teamNameLabel;
        private VisualElement _teamLogo;
        TeamRepository _teamRepository;
        IEnumerable<Team> _teams;

        void Start()
        {
            _teamRepository = new TeamRepository();
            _teams = _teamRepository.List();

            var document = GetComponent<UIDocument>().rootVisualElement;

            BindMainMenu(document);
            BindTeamContainer(document);            
        }

        private void BindMainMenu(VisualElement document)
        {
            _mainMenuContainer = document.Q<VisualElement>("MainMenuContainer");
            _startButton = document.Q<Button>(name: "Play");
            _startButton.clicked += PlayButtonClicked;
        }

        private void BindTeamContainer(VisualElement document)
        {
            _teamContainer = document.Q<VisualElement>("TeamContainer");
            _teamNameLabel = document.Q<Label>("TeamName");
            _teamLogo = document.Q<VisualElement>("TeamLogo");

            _teamContainer.Q<Button>("SelectTeamButton").clicked += SelectTeamButton;
            _teamContainer.Q<Button>("PreviousTeamButton").clicked += () => { _selectedTeamIndex = BindTeam(_selectedTeamIndex - 1); };
            _teamContainer.Q<Button>("NextTeamButton").clicked += () => { _selectedTeamIndex = BindTeam(_selectedTeamIndex + 1); };
            _selectedTeamIndex = BindTeam(_selectedTeamIndex);
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

        private void PlayButtonClicked()
        {
            if (_teamContainer.visible) return;
            _teamContainer.visible = true;
        }

        private void SelectTeamButton()
        {
            var team = _teams.ElementAt(_selectedTeamIndex);
            SELECTED_TEAM_ID = team.Id;
            OPPONENT_TEAM_ID = _teams.ElementAt(GetNextIndex(_selectedTeamIndex + 1)).Id;
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}