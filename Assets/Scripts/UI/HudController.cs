using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace AndorinhaEsporte.Controller
{
    public class HudController : MonoBehaviour
    {
        private Label _homeScore;
        private Label _homeSets;
        private Label _homeName;

        private Label _awayScore;
        private Label _awaySets;
        private Label _awayName;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _homeScore = root.Q<Label>("home-score");
            _homeSets = root.Q<Label>("home-sets");
            _homeName = root.Q<Label>("home-name");

            _awayScore = root.Q<Label>("away-score");
            _awaySets = root.Q<Label>("away-sets");
            _awayName = root.Q<Label>("away-name");
        }

        public void UpdateHud(MatchStats stats)
        {
            if (stats == null) return;
            UpdateHome(stats.HomeStats);
            UpdateAway(stats.AwayStats);
        }

        private void UpdateAway(TeamStats stats)
        {
            _awayName.text = stats.Name;
            _awayScore.text = stats.Score.ToString("00");
            _awaySets.text = stats.WinnedSetCount.ToString();
        }

        private void UpdateHome(TeamStats stats)
        {
            _homeName.text = stats.Name;
            _homeScore.text = stats.Score.ToString("00");
            _homeSets.text = stats.WinnedSetCount.ToString();
        }
    }
}