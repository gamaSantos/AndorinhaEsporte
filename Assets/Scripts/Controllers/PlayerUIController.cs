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
        public GameObject PassIndicator;
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
            InitPassIndicator();
            HideEnergyBar();
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }
        void LateUpdate()
        {
            WrapperTransformer.LookAt(WrapperTransformer.position + cameraTransform.forward);

            keyElapsedTime += Time.deltaTime;
            BouncePassIndicator();

            if ((DateTime.Now - _energyStartTime).Seconds > 2)
            {
                HideEnergyBar();
            }

        }

        private void HideEnergyBar()
        {
            EnergyBarFill.fillAmount = 0;
            EnergyBarContainer.gameObject.SetActive(false);
        }
        private void InitPassIndicator()
        {
            var movingFactor = 5f;
            PassIndicator.SetActive(false);
            keyStart = PassIndicator.transform.localPosition;
            KeyEnd = PassIndicator.transform.localPosition + (Vector3.forward * movingFactor);
        }
        private void BouncePassIndicator()
        {
            if (!PassIndicator.activeInHierarchy) return;
            var keyTransform = PassIndicator.transform;
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
        public void ChangePassIndicatorVisibility(bool isPassing)
        {
            if (isPassing) ShowPassIndicator();
            else HidePassIndicator();
        }
        private void ShowPassIndicator() => PassIndicator.SetActive(true);
        private void HidePassIndicator() => PassIndicator.SetActive(false);

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
