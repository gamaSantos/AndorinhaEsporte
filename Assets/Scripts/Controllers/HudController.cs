using System;
using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace AndorinhaEsporte.Controller
{
    public class HudController : MonoBehaviour
    {


        [SerializeField]
        Text timer;
        [SerializeField]
        Text currentSet;


        [SerializeField]
        Text homeScore;
        [SerializeField]
        Text homeSets;

        [SerializeField]
        Text awayScore;
        [SerializeField]
        Text awaySets;

        public void UpdateHud(MatchStats stats)
        {
            if(stats == null) return;
            timer.text = $"{stats.CurrentTime.ToString("mm")}:{stats.CurrentTime.ToString("ss")}";
            currentSet.text = stats.CurrentSet.ToString();

            homeScore.text = stats.HomeScore.ToString();
            homeSets.text = stats.HomeSetCount.ToString();

            awayScore.text = stats.AwayScore.ToString();
            awaySets.text = stats.AwaySetCount.ToString();
        }
    }
}