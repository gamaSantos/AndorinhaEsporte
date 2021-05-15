using System;
using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace AndorinhaEsporte.Controller
{
    public class PlayerUIController : MonoBehaviour
    {

        public Text playerFieldPosition;
        public MeshRenderer UserControllerIndicator;
        private Transform cameraTransform;
        
        void Start()
        {
            playerFieldPosition.color = Color.white;
            
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        void LateUpdate()
        {
            var textTransform = playerFieldPosition.transform;
            textTransform.LookAt(textTransform.position + cameraTransform.forward);
        }

        // public void ChangePosition(FieldPosition newPosition)
        // {
        //     playerFieldPosition.text = Enum.GetName(typeof(PlayerPositionType), newPosition.Type);
        // }

        public void ChangeText(string text)
        {
            playerFieldPosition.text = text;
        }

        public void ChangeUserIndicatorState(bool enabled)
        {
            UserControllerIndicator.enabled= enabled;
        }

    }
}
