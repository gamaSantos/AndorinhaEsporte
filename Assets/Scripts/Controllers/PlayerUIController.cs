using System;
using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace AndorinhaEsporte.Controller
{
    public class PlayerUIController : MonoBehaviour
    {

        public Text PlayerFieldPosition;
        public UnityEngine.UI.Image EnergyBarContainer;
        public Image EnergyBarFill;
        public MeshRenderer UserControllerIndicator;
        private Transform cameraTransform;

        void Start()
        {
            PlayerFieldPosition.color = Color.white;

            HideEnergyBar();
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void HideEnergyBar()
        {
            EnergyBarFill.fillAmount = 0;
            EnergyBarContainer.gameObject.SetActive(false);
        }

        void LateUpdate()
        {
            var textTransform = PlayerFieldPosition.transform;
            textTransform.LookAt(textTransform.position + cameraTransform.forward);

        }

        // public void ChangePosition(FieldPosition newPosition)
        // {
        //     playerFieldPosition.text = Enum.GetName(typeof(PlayerPositionType), newPosition.Type);
        // }

        public void ChangeText(string text)
        {
            PlayerFieldPosition.text = text;
        }
        public void FillPowerBar(float percent)
        {
            EnergyBarContainer.gameObject.SetActive(true);
            EnergyBarFill.fillAmount = percent;
        }
        public void ChangeUserIndicatorState(bool enabled)
        {
            UserControllerIndicator.enabled = enabled;
        }

    }
}
