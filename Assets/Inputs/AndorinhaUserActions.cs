// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/UserActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AndorinhaEsporte.Inputs
{
    public class @AndorinhaUserActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @AndorinhaUserActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""eeb47464-2b18-485c-88ae-ebea097a8f36"",
            ""actions"": [
                {
                    ""name"": ""Spike"",
                    ""type"": ""Button"",
                    ""id"": ""1eefc72b-5967-4070-8dd6-b9b82c499e0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""97b20195-060e-4149-9d20-895af95fc5ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangePlayer"",
                    ""type"": ""Button"",
                    ""id"": ""46469c67-62c0-4486-b2f0-faee717042db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cd2741c0-e89c-47b2-b720-e8ad3b3c54a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Spike"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""06e3e7b9-d754-4bdc-87e4-51b15525fe6e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7c20c874-1ea2-4083-a1af-26b2318d9343"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4ba40644-2461-4489-9f03-4563dc587136"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""221619fb-3c85-4932-b039-30d65872ed8e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""17b09410-b24f-45b6-a491-51ff8ca407fa"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""69f5471a-5c32-4ea1-862b-764179fafcec"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Spike = m_Player.FindAction("Spike", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_ChangePlayer = m_Player.FindAction("ChangePlayer", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Spike;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_ChangePlayer;
        public struct PlayerActions
        {
            private @AndorinhaUserActions m_Wrapper;
            public PlayerActions(@AndorinhaUserActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Spike => m_Wrapper.m_Player_Spike;
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @ChangePlayer => m_Wrapper.m_Player_ChangePlayer;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Spike.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpike;
                    @Spike.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpike;
                    @Spike.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpike;
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @ChangePlayer.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangePlayer;
                    @ChangePlayer.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangePlayer;
                    @ChangePlayer.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangePlayer;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Spike.started += instance.OnSpike;
                    @Spike.performed += instance.OnSpike;
                    @Spike.canceled += instance.OnSpike;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @ChangePlayer.started += instance.OnChangePlayer;
                    @ChangePlayer.performed += instance.OnChangePlayer;
                    @ChangePlayer.canceled += instance.OnChangePlayer;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnSpike(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnChangePlayer(InputAction.CallbackContext context);
        }
    }
}
