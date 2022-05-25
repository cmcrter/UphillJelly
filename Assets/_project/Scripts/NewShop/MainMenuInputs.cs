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
            ""name"": ""Menu"",
            ""id"": ""244ae9c0-44bd-4c53-8055-8c9d65e2c6d5"",
            ""actions"": [
                {
                    ""name"": ""Menu_Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""cac6d42c-4031-41f8-ad77-183a220f49ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""380354a1-efa9-476b-84a2-dce430a2a311"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Up"",
                    ""type"": ""Button"",
                    ""id"": ""105f0176-c83a-47d2-8be9-80ff7b1cd42c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Right"",
                    ""type"": ""Button"",
                    ""id"": ""0b2e45c0-7a42-47ca-bf8c-31abccc96bf5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Down"",
                    ""type"": ""Button"",
                    ""id"": ""72de4800-903e-4d96-87ed-7f40e1fafdb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Left"",
                    ""type"": ""Button"",
                    ""id"": ""6935af17-21de-4d30-b12b-3d10a97011ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Unpause"",
                    ""type"": ""Button"",
                    ""id"": ""4438326d-edc0-40e4-aa97-3cd00783273e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Character_Rotation"",
                    ""type"": ""Button"",
                    ""id"": ""b10047ce-a132-41d3-a636-904e137c42e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""79650ad3-fc15-4825-8dfd-b439dbe0c868"",
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
                    ""id"": ""0dd628fa-1443-4ddb-ad47-5d996c88de1d"",
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
                    ""id"": ""7fcb7681-8ae0-4578-976b-5719de67b119"",
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
                    ""id"": ""b09ea4dc-93f4-42a8-a0b1-a0745085c553"",
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
                    ""id"": ""5dce8e45-f8d8-4295-8a60-b0ece8bf7a49"",
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
                    ""id"": ""88ea4e17-8987-4e02-a981-ff2dc6d8178e"",
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
                    ""id"": ""1e879c48-8ee9-4153-8659-c79770fe31a8"",
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
                    ""id"": ""c6a4679b-62fd-49a8-9c0b-d422b3dde4e0"",
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
                    ""id"": ""98a3f7de-bf05-4a31-8ffe-ef6b01291701"",
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
                    ""id"": ""026010f8-9e51-41ff-814e-61dd0c3d053e"",
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
                    ""id"": ""4358d714-d600-407a-b759-e6740a20bd0d"",
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
                    ""id"": ""6cabc9de-b51c-4e9f-8e53-c29bd6bfb0b3"",
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
                    ""id"": ""dce5ae29-3d99-4988-872b-23e48dae8884"",
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
                    ""id"": ""6465da50-c0a5-4408-9278-525c5fbb8915"",
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
                    ""id"": ""01536be6-6045-4a93-a003-21da8c0b1313"",
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
                    ""id"": ""bc00b573-045a-497a-9877-8e9fcf5d1c02"",
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
                    ""id"": ""352c748e-b8f1-4a68-a0ef-b9042b21de16"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c1a0d62-4481-47b4-95f3-a6388c0907d9"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu_Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""009e76da-594d-4588-8c47-c60961c3e6a7"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Ps4"",
                    ""action"": ""Character_Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6992ffa1-1fe1-4b47-af75-c0bd33a1a137"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Ps4"",
                    ""action"": ""Character_Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MainMenuNav"",
            ""id"": ""85a66949-a3b0-4df4-9694-7708b7c5f0da"",
            ""actions"": [
                {
                    ""name"": ""TabLeft"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5b412b76-3196-4797-96dc-aedf79046a6e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TabRight"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0c94ddd6-b5ad-43f9-b629-cda20fedabd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Right"",
                    ""type"": ""Button"",
                    ""id"": ""1d331c0a-4229-493c-b08e-c75fb76d59f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Left"",
                    ""type"": ""Button"",
                    ""id"": ""6f04e442-6706-492c-a54e-6898545bf466"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Up"",
                    ""type"": ""Button"",
                    ""id"": ""cf22e4ee-21c4-45b2-9524-e96aee050306"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Down"",
                    ""type"": ""Button"",
                    ""id"": ""f3b84219-c84d-4348-9531-1d81983ecced"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""8ca993fd-d5be-451f-be5c-d97f59f2817b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu_Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""8f249425-ad01-4452-bb0b-948eb85c3850"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0d276a07-ec84-4234-b945-e1374f16beb2"",
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
                    ""id"": ""8e2ba67f-d3c6-41e2-b018-4e8f53938489"",
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
                    ""id"": ""9b064cef-3b48-4d71-a5f1-e6289e18a1c3"",
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
                    ""id"": ""077cbc8a-1c36-47b8-8471-c0a4e08a1244"",
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
                    ""id"": ""ab6025f8-a6d8-44d8-8de2-f12b4f3cb0b7"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""TabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""691e9584-3137-490c-9eac-c74732df70ab"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57d410df-3dd2-4c96-a2d3-d4df2ca8967e"",
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
                    ""id"": ""2f2911ed-65ca-4b18-b6c8-f02c970d616b"",
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
                    ""id"": ""7db4ae8d-166c-4fb9-ac0f-77e072e5f099"",
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
                    ""id"": ""9a5d9c4c-5002-4f7e-ab43-882c3f586949"",
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
                    ""id"": ""911d703d-b8c3-4bb5-96b6-440f9a37deef"",
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
                    ""id"": ""d20f06cb-724f-409c-99de-11c60b948824"",
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
                    ""id"": ""a832a90a-9aab-482b-97fb-3b06d4957b6a"",
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
                    ""id"": ""e8135390-1589-4d38-b1b2-0def054838dd"",
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
                    ""id"": ""c74941da-1662-4792-bf96-3351489a8661"",
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
                    ""id"": ""e34676c4-dc7e-48e8-86d3-0323f4a5c412"",
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
                    ""id"": ""cd58e989-6e1a-469c-9cb2-4ab8d88a821c"",
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
                    ""id"": ""95df8d67-4be9-4fb1-a0b2-fb44d8b2cae8"",
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
                    ""id"": ""47a58a61-6ff5-4f40-be53-c96caa279db4"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""TabRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dd46522-25ee-45fb-ae91-304f9131a0f5"",
                    ""path"": ""<Keyboard>/rightShift"",
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
        },
        {
            ""name"": ""Ps4"",
            ""bindingGroup"": ""Ps4"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShock4GampadiOS>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Menu_Confirm = m_Menu.FindAction("Menu_Confirm", throwIfNotFound: true);
        m_Menu_Menu_Cancel = m_Menu.FindAction("Menu_Cancel", throwIfNotFound: true);
        m_Menu_Menu_Up = m_Menu.FindAction("Menu_Up", throwIfNotFound: true);
        m_Menu_Menu_Right = m_Menu.FindAction("Menu_Right", throwIfNotFound: true);
        m_Menu_Menu_Down = m_Menu.FindAction("Menu_Down", throwIfNotFound: true);
        m_Menu_Menu_Left = m_Menu.FindAction("Menu_Left", throwIfNotFound: true);
        m_Menu_Menu_Unpause = m_Menu.FindAction("Menu_Unpause", throwIfNotFound: true);
        m_Menu_Character_Rotation = m_Menu.FindAction("Character_Rotation", throwIfNotFound: true);
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

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Menu_Confirm;
    private readonly InputAction m_Menu_Menu_Cancel;
    private readonly InputAction m_Menu_Menu_Up;
    private readonly InputAction m_Menu_Menu_Right;
    private readonly InputAction m_Menu_Menu_Down;
    private readonly InputAction m_Menu_Menu_Left;
    private readonly InputAction m_Menu_Menu_Unpause;
    private readonly InputAction m_Menu_Character_Rotation;
    public struct MenuActions
    {
        private @MainMenuInputs m_Wrapper;
        public MenuActions(@MainMenuInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Menu_Confirm => m_Wrapper.m_Menu_Menu_Confirm;
        public InputAction @Menu_Cancel => m_Wrapper.m_Menu_Menu_Cancel;
        public InputAction @Menu_Up => m_Wrapper.m_Menu_Menu_Up;
        public InputAction @Menu_Right => m_Wrapper.m_Menu_Menu_Right;
        public InputAction @Menu_Down => m_Wrapper.m_Menu_Menu_Down;
        public InputAction @Menu_Left => m_Wrapper.m_Menu_Menu_Left;
        public InputAction @Menu_Unpause => m_Wrapper.m_Menu_Menu_Unpause;
        public InputAction @Character_Rotation => m_Wrapper.m_Menu_Character_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Menu_Confirm.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Confirm;
                @Menu_Confirm.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Confirm;
                @Menu_Confirm.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Confirm;
                @Menu_Cancel.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Cancel.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Cancel.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Cancel;
                @Menu_Up.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Up;
                @Menu_Up.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Up;
                @Menu_Up.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Up;
                @Menu_Right.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Right;
                @Menu_Right.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Right;
                @Menu_Right.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Right;
                @Menu_Down.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Down;
                @Menu_Down.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Down;
                @Menu_Down.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Down;
                @Menu_Left.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Left;
                @Menu_Left.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Left;
                @Menu_Left.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Left;
                @Menu_Unpause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Unpause;
                @Menu_Unpause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Unpause;
                @Menu_Unpause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMenu_Unpause;
                @Character_Rotation.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCharacter_Rotation;
                @Character_Rotation.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCharacter_Rotation;
                @Character_Rotation.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCharacter_Rotation;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Menu_Confirm.started += instance.OnMenu_Confirm;
                @Menu_Confirm.performed += instance.OnMenu_Confirm;
                @Menu_Confirm.canceled += instance.OnMenu_Confirm;
                @Menu_Cancel.started += instance.OnMenu_Cancel;
                @Menu_Cancel.performed += instance.OnMenu_Cancel;
                @Menu_Cancel.canceled += instance.OnMenu_Cancel;
                @Menu_Up.started += instance.OnMenu_Up;
                @Menu_Up.performed += instance.OnMenu_Up;
                @Menu_Up.canceled += instance.OnMenu_Up;
                @Menu_Right.started += instance.OnMenu_Right;
                @Menu_Right.performed += instance.OnMenu_Right;
                @Menu_Right.canceled += instance.OnMenu_Right;
                @Menu_Down.started += instance.OnMenu_Down;
                @Menu_Down.performed += instance.OnMenu_Down;
                @Menu_Down.canceled += instance.OnMenu_Down;
                @Menu_Left.started += instance.OnMenu_Left;
                @Menu_Left.performed += instance.OnMenu_Left;
                @Menu_Left.canceled += instance.OnMenu_Left;
                @Menu_Unpause.started += instance.OnMenu_Unpause;
                @Menu_Unpause.performed += instance.OnMenu_Unpause;
                @Menu_Unpause.canceled += instance.OnMenu_Unpause;
                @Character_Rotation.started += instance.OnCharacter_Rotation;
                @Character_Rotation.performed += instance.OnCharacter_Rotation;
                @Character_Rotation.canceled += instance.OnCharacter_Rotation;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

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
    private int m_Ps4SchemeIndex = -1;
    public InputControlScheme Ps4Scheme
    {
        get
        {
            if (m_Ps4SchemeIndex == -1) m_Ps4SchemeIndex = asset.FindControlSchemeIndex("Ps4");
            return asset.controlSchemes[m_Ps4SchemeIndex];
        }
    }
    public interface IMenuActions
    {
        void OnMenu_Confirm(InputAction.CallbackContext context);
        void OnMenu_Cancel(InputAction.CallbackContext context);
        void OnMenu_Up(InputAction.CallbackContext context);
        void OnMenu_Right(InputAction.CallbackContext context);
        void OnMenu_Down(InputAction.CallbackContext context);
        void OnMenu_Left(InputAction.CallbackContext context);
        void OnMenu_Unpause(InputAction.CallbackContext context);
        void OnCharacter_Rotation(InputAction.CallbackContext context);
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
