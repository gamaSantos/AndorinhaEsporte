using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using AndorinhaEsporte.Data;
using AndorinhaEsporte.Inputs;
using AndorinhaEsporte.Services;
using UnityEngine.InputSystem.UI;

namespace AndorinhaEsporte.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public static Guid SELECTED_TEAM_ID;
        public static Guid OPPONENT_TEAM_ID;

        Button _startButton;
        private TeamSelectController _teamSelectController;
        private AboutPanelController _aboutController;
        private AndorinhaUserActions _actions;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;


            BindMainMenu(root);

            _aboutController = new AboutPanelController(root);

            _teamSelectController = new TeamSelectController(root, new TeamRepository());
            _teamSelectController.TeamSelected += OnTeamSelected;

            BindGenericActions();

        }

        private void BindGenericActions()
        {
            var inputSystem = GetComponent<InputSystemUIInputModule>();
            //TODO add basic navigation in menu
            // inputSystem.move.action.performed += ctx =>
            // {

            // };
        }

        private void BindMainMenu(VisualElement root)
        {
            _startButton = root.Q<Button>(name: "Play");

            _startButton.clicked += PlayButtonClicked;
            _startButton.SetEnabled(true);
            root.Q<Button>(name: "About").clicked += () =>
            {
                _teamSelectController.Hide();
                _aboutController.Show();
            };
        }

        private void PlayButtonClicked()
        {
            _aboutController.Hide();
            _teamSelectController.Show();
        }

        private void OnTeamSelected(object sender, TeamSelectedEventArgs args)
        {
            SELECTED_TEAM_ID = args.SelectedTeamId;
            OPPONENT_TEAM_ID = args.OpponentTeamId;
            MatchService.CreateMatch(SELECTED_TEAM_ID, OPPONENT_TEAM_ID);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}