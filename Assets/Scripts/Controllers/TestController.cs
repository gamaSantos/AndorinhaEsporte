
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
            var position = FieldPositionFactory.Create(PlayerPositionType.Center);
            var teamInfo = new TeamInMatchInformation(Guid.NewGuid(), Vector3.forward, Color.red, Color.red);
            var playerA = new Domain.Player(position, teamInfo);
            var playerB = new Domain.Player(position, teamInfo);

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