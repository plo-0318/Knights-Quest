// GENERATED AUTOMATICALLY FROM 'Assets/Control.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Control : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Control()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Control"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b13410c9-880c-4f35-9b0c-d9309efdd412"",
            ""actions"": [
                {
                    ""name"": ""MoveX"",
                    ""type"": ""Value"",
                    ""id"": ""a1b3f44d-33d5-4084-872c-e9822138c449"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveY"",
                    ""type"": ""Value"",
                    ""id"": ""b5e44eb4-7f7e-4ec7-bfae-5f27f880fbeb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""08a71c21-1c6f-4275-8344-1914bae6693a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DestroyObj"",
                    ""type"": ""Button"",
                    ""id"": ""6b35dd9d-fff2-4736-a91a-73fa84518ba1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f5196e46-62a8-4e40-9e97-b1f12151822c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""731fec84-2117-43c8-80a1-f572d27d3e66"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""cf4fe406-59bb-416f-b159-b768a90f8873"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""2f490fba-16b4-4e01-a21d-b355f4baa47d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveY"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ec753c44-140e-4278-9dcd-4db9ed324502"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4d09f71d-9f6c-4ff2-aecb-b9883b78f150"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a15ab34c-b3da-4ff3-b133-893a0d539f60"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad3ab7e4-7e8a-465d-a5a7-253f604e105a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DestroyObj"",
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
        m_Player_MoveX = m_Player.FindAction("MoveX", throwIfNotFound: true);
        m_Player_MoveY = m_Player.FindAction("MoveY", throwIfNotFound: true);
        m_Player_Mouse = m_Player.FindAction("Mouse", throwIfNotFound: true);
        m_Player_DestroyObj = m_Player.FindAction("DestroyObj", throwIfNotFound: true);
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
    private readonly InputAction m_Player_MoveX;
    private readonly InputAction m_Player_MoveY;
    private readonly InputAction m_Player_Mouse;
    private readonly InputAction m_Player_DestroyObj;
    public struct PlayerActions
    {
        private @Control m_Wrapper;
        public PlayerActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveX => m_Wrapper.m_Player_MoveX;
        public InputAction @MoveY => m_Wrapper.m_Player_MoveY;
        public InputAction @Mouse => m_Wrapper.m_Player_Mouse;
        public InputAction @DestroyObj => m_Wrapper.m_Player_DestroyObj;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveY.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveY;
                @MoveY.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveY;
                @MoveY.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveY;
                @Mouse.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @DestroyObj.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDestroyObj;
                @DestroyObj.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDestroyObj;
                @DestroyObj.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDestroyObj;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveX.started += instance.OnMoveX;
                @MoveX.performed += instance.OnMoveX;
                @MoveX.canceled += instance.OnMoveX;
                @MoveY.started += instance.OnMoveY;
                @MoveY.performed += instance.OnMoveY;
                @MoveY.canceled += instance.OnMoveY;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @DestroyObj.started += instance.OnDestroyObj;
                @DestroyObj.performed += instance.OnDestroyObj;
                @DestroyObj.canceled += instance.OnDestroyObj;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMoveX(InputAction.CallbackContext context);
        void OnMoveY(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnDestroyObj(InputAction.CallbackContext context);
    }
}
