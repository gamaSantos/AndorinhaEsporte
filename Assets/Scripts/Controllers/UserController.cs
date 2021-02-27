using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace AndorinhaEsporte.Controller
{
    public class UserController : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerable<PlayerController> _playerControllers;
        void Start()
        {
            Application.targetFrameRate = 60;
        }

        void Update()
        {
            // var playerController = GetPlayer();
            // if (playerController == null) return;

            // var horizontalInput = Input.GetAxis("Horizontal");
            // var verticalInput = Input.GetAxis("Vertical");

            // if (Input.GetKeyUp(KeyCode.C))
            // {
            //     playerController.KickAttheGoal();
            // }

            // if (Input.GetKeyUp(KeyCode.F))
            // {
            //     playerController.Pass();
            // }

            // if (Input.GetKey(KeyCode.J)) playerController.FollowBall();

            // if (horizontalInput != 0 || verticalInput != 0) playerController.Move(horizontalInput, verticalInput);
        }

        private PlayerController GetPlayer()
        {
            if (_playerControllers == null || _playerControllers.Count() == 0) LoadPlayers();
            if (_playerControllers == null || _playerControllers.Count() == 0) return null;
            return _playerControllers.Where(pc => pc.IsHomeTeamPlayer && pc.IsUserControlled).First();
        }

        private void LoadPlayers()
        {
            _playerControllers = GameObject.FindObjectsOfType<PlayerController>();
            if (_playerControllers == null || _playerControllers.Count() == 0) return;
            _playerControllers = _playerControllers.Where(p => p.IsHomeTeamPlayer);
        }
    }
}
