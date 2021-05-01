using UnityEngine.UIElements;

namespace AndorinhaEsporte.UI
{
    public class AboutPanelController
    {
        private VisualElement _container;
        private Label _welcomeLabel;
        private Label _creditsLabel;
        
        public AboutPanelController(VisualElement root)
        {
            _container = root.Q<VisualElement>("AboutContainer");
            _welcomeLabel = root.Q<Label>("WelcomeLabel");
            _creditsLabel = root.Q<Label>("CreditsLabel");

            _welcomeLabel.text = "Olá, obrigado por me ajudar nessa jornada, se quiser participar mais ativamente ou possuir uma idéia de melhoria/próximo projeto fique avontade para me enviar uma mensagem";
            _welcomeLabel.text += System.Environment.NewLine;
            _welcomeLabel.text += System.Environment.NewLine;
            _creditsLabel.text = "Music [birthofahero]: Royalty Free Music from Bensound" ;
        }

        public void Show()
        {
            _container.style.display = DisplayStyle.Flex;
            _container.visible = true;
        }

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
            _container.visible = false;
        }
    }
}