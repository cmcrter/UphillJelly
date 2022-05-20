// GENERATED AUTOMATICALLY FROM 'Assets/Settings/MainMenuInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MainMenuInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainMenuInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainMenuInputs"",
    ""maps"": [
        {
            ""name"": ""MainMenuNav"",
            ""id"": ""3324ea46-a061-4469-9bd6-bbd8cf80fbcc"",
            ""actions"": [
                {
                    ""name"": ""TabLeft"",
                    ""type"": ""Button"",
                    ""id"": ""da9c387b-6d33-40a5-9a8c-c4ceb35bcdfe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TabRight"",
                    ""type"": ""Button"",
                    ""id"": ""c1338eb7-4220-450a-b4d7-3f0c4a05ff6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Right"",
                    ""type"": ""Button"",
                    ""id"": ""db07910d-1b10-404a-b194-252cdd85f7f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Left"",
                    ""type"": ""Button"",
                    ""id"": ""9ab2572e-4d60-4819-8f36-0c7d8181a19c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Up"",
                    ""type"": ""Button"",
                    ""id"": ""10a3e621-00f2-42d4-9ab4-58f8eb36431b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Down"",
                    ""type"": ""Button"",
                    ""id"": ""9bded14a-e772-42f2-bb69-f68dfab93a0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""779e3d5b-ca26-47dd-8ed4-47f2de39a1ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""eb25f436-fedf-4808-a9e2-7b566aa13dad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""421d93d3-1b9c-46eb-90d4-3644d80865f8"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4be2f51-6d25-42f0-85b7-5fb38b61e1de"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8d5be4a-7f81-49a5-81d6-659e2a0a9ec6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5b6b85e-10b0-44b5-a810-799b2dd9e40e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ada67757-0773-45b5-a98f-0c8639509dd7"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03e98875-9c67-4f99-afd4-bf18ffef32ec"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb96a1cd-2bb7-44b2-bf9e-dee496c20c05"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef9ebefc-389f-443b-8b83-bc000290e002"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""034baa2b-aca3-4625-be84-9ad5af28dddc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73bb2ddf-9fd0-4d08-8837-56bbbd709508"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62ef53c1-a507-4c83-8909-f9c9bbb12f9d"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f7e5022-ac27-40a5-8e1c-ceaf8ef91e98"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a424fdb-e264-436c-a477-54211c5a59d2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bd4d0f5-fe62-4dd4-9cce-ce4da0fcd60f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""050eee47-a4b4-4bb1-aa95-9525e19dcac4"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""890ba5ae-6edc-45cb-9467-6bd703dcaf43"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fe98377-1c20-4527-a9ba-323c2b16d050"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89379547-ce63-4685-95ab-76f7654f0c2b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // MainMenuNav
        m_MainMenuNav = asset.FindActionMap("MainMenuNav", throwIfNotFound: true);
        m_MainMenuNav_TabLeft = m_MainMenuNav.FindAction("TabLeft", throwIfNotFound: true);
        m_MainMenuNav_TabRight = m_MainMenuNav.FindAction("TabRight", throwIfNotFound: true);
        m_MainMenuNav_Menu_Right = m_MainMenuNav.FindAction("Menu_Right", throwIfNotFound: true);
        m_MainMenuNav_Menu_Left = m_MainMenuNav.FindAction("Menu_Left", throwIfNotFound: true);
        m_MainMenuNav_Menu_Up = m_MainMenuNav.FindAction("Menu_Up", throwIfNotFound: true);
        m_MainMenuNav_Menu_Down = m_MainMenuNav.FindAction("Menu_Down", throwIfNotFound: true);
        m_MainMenuNav_Menu_Cancel = m_MainMenuNav.FindAction("Menu_Cancel", throwIfNotFound: true);
        m_MainMenuNav_Menu_Confirm = m_MainMenuNav.FindAction("Menu_Confirm", throwIfNotFound: true);
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

    // MainMenuNav
    private readonly InputActionMap m_MainMenuNav;
    private IMainMenuNavActions m_MainMenuNavActionsCallbackInterface;
    private readonly InputAction m_MainMenuNav_TabLeft;
    private readonly InputAction m_MainMenuNav_TabRight;
    private readonly InputAction m_MainMenuNav_Menu_Right;
    private readonly InputAction m_MainMenuNav_Menu_Left;
    private readonly InputAction m_MainMenuNav_Menu_Up;
    private readonly InputAction m_MainMenuNav_Menu_Down;
    private readonly InputAction m_MainMenuNav_Menu_Cancel;
    private readonly InputAction m_MainMenuNav_Menu_Confirm;
    public struct MainMenuNavActions
    {
        private @MainMenuInputs m_Wrapper;
        public MainMenuNavActions(@MainMenuInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @TabLeft => m_Wrapper.m_MainMenuNav_TabLeft;
        public InputAction @TabRight => m_Wrapper.m_MainMenuNav_TabRight;
        public InputAction @Menu_Right => m_Wrapper.m_MainMenuNav_Menu_Right;
        public InputAction @Menu_Left => m_Wrapper.m_MainMenuNav_Menu_Left;
        public InputAction @Menu_Up => m_Wrapper.m_MainMenuNav_Menu_Up;
        public InputAction @Menu_Down => m_Wrapper.m_MainMenuNav_Menu_Down;
        public InputAction @Menu_Cancel => m_Wrapper.m_MainMenuNav_Menu_Cancel;
        public InputAction @Menu_Confirm => m_Wrapper.m_MainMenuNav_Menu_Confirm;
        public InputActionMap Get() { return m_Wrapper.m_MainMenuNav; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainMenuNavActions set) { return set.Get(); }
        public void SetCallbacks(IMainMenuNavActions instance)
        {
            if (m_Wrapper.m_MainMenuNavActionsCallbackInterface != null)
            {
                @TabLeft.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabLeft;
                @TabLeft.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabLeft;
                @TabLeft.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabLeft;
                @TabRight.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabRight;
                @TabRight.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabRight;
                @TabRight.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnTabRight;
                @Menu_Right.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Right;
                @Menu_Right.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Right;
                @Menu_Right.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Right;
                @Menu_Left.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Left;
                @Menu_Left.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Left;
                @Menu_Left.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Left;
                @Menu_Up.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Up;
                @Menu_Up.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Up;
                @Menu_Up.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Up;
                @Menu_Down.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Down;
                @Menu_Down.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Down;
                @Menu_Down.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Down;
                @Menu_Cancel.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Cancel.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Cancel.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Confirm.started -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Confirm;
                @Menu_Confirm.performed -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Confirm;
                @Menu_Confirm.canceled -= m_Wrapper.m_MainMenuNavActionsCallbackInterface.OnMenu_Confirm;
            }
            m_Wrapper.m_MainMenuNavActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TabLeft.started += instance.OnTabLeft;
                @TabLeft.performed += instance.OnTabLeft;
                @TabLeft.canceled += instance.OnTabLeft;
                @TabRight.started += instance.OnTabRight;
                @TabRight.performed += instance.OnTabRight;
                @TabRight.canceled += instance.OnTabRight;
                @Menu_Right.started += instance.OnMenu_Right;
                @Menu_Right.performed += instance.OnMenu_Right;
                @Menu_Right.canceled += instance.OnMenu_Right;
                @Menu_Left.started += instance.OnMenu_Left;
                @Menu_Left.performed += instance.OnMenu_Left;
                @Menu_Left.canceled += instance.OnMenu_Left;
                @Menu_Up.started += instance.OnMenu_Up;
                @Menu_Up.performed += instance.OnMenu_Up;
                @Menu_Up.canceled += instance.OnMenu_Up;
                @Menu_Down.started += instance.OnMenu_Down;
                @Menu_Down.performed += instance.OnMenu_Down;
                @Menu_Down.canceled += instance.OnMenu_Down;
                @Menu_Cancel.started += instance.OnMenu_Cancel;
                @Menu_Cancel.performed += instance.OnMenu_Cancel;
                @Menu_Cancel.canceled += instance.OnMenu_Cancel;
                @Menu_Confirm.started += instance.OnMenu_Confirm;
                @Menu_Confirm.performed += instance.OnMenu_Confirm;
                @Menu_Confirm.canceled += instance.OnMenu_Confirm;
            }
        }
    }
    public MainMenuNavActions @MainMenuNav => new MainMenuNavActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IMainMenuNavActions
    {
        void OnTabLeft(InputAction.CallbackContext context);
        void OnTabRight(InputAction.CallbackContext context);
        void OnMenu_Right(InputAction.CallbackContext context);
        void OnMenu_Left(InputAction.CallbackContext context);
        void OnMenu_Up(InputAction.CallbackContext context);
        void OnMenu_Down(InputAction.CallbackContext context);
        void OnMenu_Cancel(InputAction.CallbackContext context);
        void OnMenu_Confirm(InputAction.CallbackContext context);
    }
}
