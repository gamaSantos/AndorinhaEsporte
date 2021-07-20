using System.Collections;
using System.Collections.Generic;
using AndorinhaEsporte.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AndorinhaEsporte.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        private VisualElement _container;
        private Button _resumeButton;
        private Button _quitButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _container = root.Q<VisualElement>("MenuContainer");
            _resumeButton = _container.Q<Button>("ResumeButton");
            _quitButton = _container.Q<Button>("QuitButton");

            //FIXME CLICK EVENT
            _resumeButton.clicked += () =>
           {
               MatchService.ResumeMatch();
               this.ToogleVisualization();
           };

            _quitButton.clicked += () =>
            {
                //TODO implement scene constant
                SceneManager.LoadScene("MainMenu");
            };
        }

        public void ToogleVisualization()
        {
            if (_container == null) return;
            _container.visible = !_container.visible;
            if (_container.visible)
            {
                MatchService.StopMatch();
                _container.style.display = DisplayStyle.Flex;
            }
            else
            {
                MatchService.ResumeMatch();
                _container.style.display = DisplayStyle.None;
            }
        }
    }
}