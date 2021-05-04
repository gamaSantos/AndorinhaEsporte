using System.Collections.Generic;
using System.Linq;
using AndorinhaEsporte.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndorinhaEsporte.Controller
{
    public class UserController : MonoBehaviour
    {
        IEnumerable<PlayerController> _playerControllers;
        private AndorinhaUserActions _actions;


        private void Awake()
        {
            _actions = new AndorinhaUserActions();
        }

        private void OnEnable()
        {
            _actions.Enable();
        }
        private void OnDisable()
        {
            _actions.Disable();
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            _actions.Player.Fire.performed += ctx =>
            {               
                Debug.Log("Fire2");
            };
            _actions.Player.Move.performed += ctx => Debug.Log(ctx.ReadValue<Vector2>());
        }

        void Update()
        {

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
