using System;
using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace AndorinhaEsporte.Controller
{
    public class PlayerUIController : MonoBehaviour
    {

        public Text PlayerFieldPosition;
        public Transform WrapperTransformer;
        public UnityEngine.UI.Image EnergyBarContainer;
        public Image EnergyBarFill;
        public MeshRenderer UserControllerIndicator;
        private Transform cameraTransform;
        private DateTime _energyStartTime;

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
            // var textTransform = PlayerFieldPosition.transform;
            WrapperTransformer.LookAt(WrapperTransformer.position + cameraTransform.forward);
            if ((DateTime.Now - _energyStartTime).Seconds > 2)
            {
                HideEnergyBar();
            }

        }
        public void ChangeText(string text)
        {
            PlayerFieldPosition.text = text;
        }
        public void FillEnergyBar(float percent)
        {
            EnergyBarContainer.gameObject.SetActive(true);
            EnergyBarFill.fillAmount = percent;
            _energyStartTime = DateTime.Now;
        }
        public void ChangeUserIndicatorState(bool enabled)
        {
            UserControllerIndicator.enabled = enabled;
        }

    }
}
