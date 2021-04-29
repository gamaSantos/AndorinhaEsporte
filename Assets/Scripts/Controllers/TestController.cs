
using System;
using System.Linq;
using AndorinhaEsporte.Domain;
using UnityEngine;

namespace AndorinhaEsporte.Controller
{
    public class TestController : MonoBehaviour
    {
        void Start()
        {
            var controllers = FindObjectsOfType<PlayerController>();
            var teamInfo = new TeamInMatchInformation(Guid.NewGuid(), "testName", Color.red, Color.red);
            var playerA = new Domain.Player(FieldPositionFactory.Create(PlayerPositionType.RightBack), teamInfo);
            var playerB = new Domain.Player(FieldPositionFactory.Create(PlayerPositionType.LeftBack), teamInfo);

            playerA.AddTeammate(playerB);
            playerB.AddTeammate(playerA);
            for (var i = 0; i < controllers.Count(); i++)
            {
                var controller = controllers[i];
                if (i == 0)
                {
                    controller.Initiate(playerA, null);
                }
                else
                {
                    controller.Initiate(playerB, null);
                }
                controller.Move();
            }
        }
    }
}