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
        // Start is called before the first frame update
        Button _startButton;
        ListView _teamListView;
        TeamRepository _teamRepository;
        IEnumerable<Team> _teams;

        public VisualTreeAsset TeamListTemplate;

        void Start()
        {
            var document = GetComponent<UIDocument>();
            _startButton = document.rootVisualElement.Q<Button>(name: "Play");
            _teamListView = document.rootVisualElement.Q<ListView>("TeamListView");
            _startButton.clicked += PlayButtonClicked;

            _teamRepository = new TeamRepository();
            _teams = _teamRepository.List();
            
            BindTeamListView();

        }

        private void BindTeamListView()
        {
            if (_teamListView != null)
            {
                if (_teamListView.makeItem == null)
                    _teamListView.makeItem = MakeItem;
                if (_teamListView.bindItem == null)
                    _teamListView.bindItem = BindItem;
                _teamListView.itemsSource = _teams.ToList();

                _teamListView.Refresh();
            }
            else
            {
                Debug.Log("List view is null");
            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        private Action PlayButtonClicked = () =>
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        };

        private VisualElement MakeItem()
        {
            if (TeamListTemplate != null)
                return TeamListTemplate.CloneTree();
            throw new Exception("Team list template not binded in UI.MainMenuController");
        }

        private void BindItem(VisualElement element, int index)
        {
            var team = _teams.ElementAt(index).InMatchInformation;
            element.Q<Label>("TeamName").text = "Player " + team.Name;

            var mainColor = team.MainColor;
            var secondaryColor = team.SecondaryColor;

            element.Q("TeamLogo").style.unityBackgroundImageTintColor = mainColor;

            element.userData = team;
        }
    }
}