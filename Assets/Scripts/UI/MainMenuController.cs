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
        private TeamSelectController _teamSelectController;
        private AboutPanelController _aboutController;      

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            BindMainMenu(root);

            _aboutController = new AboutPanelController(root);

            _teamSelectController = new TeamSelectController(root, new TeamRepository());
            _teamSelectController.TeamSelected += OnTeamSelected;
        }

        private void BindMainMenu(VisualElement root)
        {
            _startButton = root.Q<Button>(name: "Play");
            _startButton.clicked += PlayButtonClicked;

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
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}