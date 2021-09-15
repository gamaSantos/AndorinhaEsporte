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
        public GameObject KeyIndicator;
        public UnityEngine.UI.Image EnergyBarContainer;
        public Image EnergyBarFill;
        public MeshRenderer UserControllerIndicator;
        private Transform cameraTransform;
        private DateTime _energyStartTime;


        private Vector3 keyStart;
        private Vector3 KeyEnd;
        private float keyElapsedTime = 0f;
        private float keyAnimationDuration = .5f;
        private bool keyMovingback = true;

        void Start()
        {
            PlayerFieldPosition.color = Color.white;
            InitPlayerButton();
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
            WrapperTransformer.LookAt(WrapperTransformer.position + cameraTransform.forward);

            keyElapsedTime += Time.deltaTime;
            BouncePlayerButton();

            if ((DateTime.Now - _energyStartTime).Seconds > 2)
            {
                HideEnergyBar();
            }

        }

        private void BouncePlayerButton()
        {
            if (!KeyIndicator.activeInHierarchy) return;
            var keyTransform = KeyIndicator.transform;
            var lerpValue = Mathf.Clamp(keyElapsedTime / keyAnimationDuration, 0, 1);
            if (keyMovingback)
                keyTransform.localPosition = Vector3.Lerp(keyStart, KeyEnd, lerpValue);
            else
                keyTransform.localPosition = Vector3.Lerp(KeyEnd, keyStart, lerpValue);
            if (lerpValue >= 1)
            {
                keyMovingback = !keyMovingback;
                keyElapsedTime = 0f;
            }
        }
        private void InitPlayerButton()
        {
            var movingFactor = 5f;
            KeyIndicator.SetActive(false);
            keyStart = KeyIndicator.transform.localPosition;
            KeyEnd = KeyIndicator.transform.localPosition + (Vector3.forward * movingFactor);
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
