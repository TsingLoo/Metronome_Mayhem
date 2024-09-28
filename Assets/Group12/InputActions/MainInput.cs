//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputActions/MainInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MainInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInput"",
    ""maps"": [
        {
            ""name"": ""inLevel"",
            ""id"": ""4f844909-2d9a-4c3f-854e-7c07d1126599"",
            ""actions"": [
                {
                    ""name"": ""inputChannel0"",
                    ""type"": ""Button"",
                    ""id"": ""eeb29230-6e1c-4d52-8499-323a7a2eb8a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel1"",
                    ""type"": ""Button"",
                    ""id"": ""1d063f87-7934-4a1b-a9c5-f77083156e8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel2"",
                    ""type"": ""Button"",
                    ""id"": ""5509cae3-5220-4456-99d9-6742edb80dd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel3"",
                    ""type"": ""Button"",
                    ""id"": ""b9086da5-a542-4a9b-8a8d-65cea0db035d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel4"",
                    ""type"": ""Button"",
                    ""id"": ""1e1da4ff-f89f-43f5-9c07-b142c246eb83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel5"",
                    ""type"": ""Button"",
                    ""id"": ""2df5ecec-bfec-4b0f-8cb7-62c3f02e2511"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel6"",
                    ""type"": ""Button"",
                    ""id"": ""a3aa7a5f-c31b-426c-bee1-953cd05e91f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""inputChannel7"",
                    ""type"": ""Button"",
                    ""id"": ""05620491-57d6-43fa-8a60-c901b9615692"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b6c10ed0-93c1-4af4-8a62-b76b60975527"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d54f881-37f0-4f39-bbbf-44098a75911e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ad5d0d6-5abe-4759-91ef-9b8a9ec7d014"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62d718ea-a244-4436-ae48-0714822a4102"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cba4ccaf-1423-43b4-82b5-10c163e93356"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9cdb1f57-db0f-4205-957d-7d4120d4dd09"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58a4b020-0494-44a2-8766-a75a3197eec6"",
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf6f7645-7a3e-4eab-83a0-b56baccf5b90"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC-Keyboard"",
                    ""action"": ""inputChannel6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC-Keyboard"",
            ""bindingGroup"": ""PC-Keyboard"",
            ""devices"": []
        }
    ]
}");
        // inLevel
        m_inLevel = asset.FindActionMap("inLevel", throwIfNotFound: true);
        m_inLevel_inputChannel0 = m_inLevel.FindAction("inputChannel0", throwIfNotFound: true);
        m_inLevel_inputChannel1 = m_inLevel.FindAction("inputChannel1", throwIfNotFound: true);
        m_inLevel_inputChannel2 = m_inLevel.FindAction("inputChannel2", throwIfNotFound: true);
        m_inLevel_inputChannel3 = m_inLevel.FindAction("inputChannel3", throwIfNotFound: true);
        m_inLevel_inputChannel4 = m_inLevel.FindAction("inputChannel4", throwIfNotFound: true);
        m_inLevel_inputChannel5 = m_inLevel.FindAction("inputChannel5", throwIfNotFound: true);
        m_inLevel_inputChannel6 = m_inLevel.FindAction("inputChannel6", throwIfNotFound: true);
        m_inLevel_inputChannel7 = m_inLevel.FindAction("inputChannel7", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // inLevel
    private readonly InputActionMap m_inLevel;
    private List<IInLevelActions> m_InLevelActionsCallbackInterfaces = new List<IInLevelActions>();
    private readonly InputAction m_inLevel_inputChannel0;
    private readonly InputAction m_inLevel_inputChannel1;
    private readonly InputAction m_inLevel_inputChannel2;
    private readonly InputAction m_inLevel_inputChannel3;
    private readonly InputAction m_inLevel_inputChannel4;
    private readonly InputAction m_inLevel_inputChannel5;
    private readonly InputAction m_inLevel_inputChannel6;
    private readonly InputAction m_inLevel_inputChannel7;
    public struct InLevelActions
    {
        private @MainInput m_Wrapper;
        public InLevelActions(@MainInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @inputChannel0 => m_Wrapper.m_inLevel_inputChannel0;
        public InputAction @inputChannel1 => m_Wrapper.m_inLevel_inputChannel1;
        public InputAction @inputChannel2 => m_Wrapper.m_inLevel_inputChannel2;
        public InputAction @inputChannel3 => m_Wrapper.m_inLevel_inputChannel3;
        public InputAction @inputChannel4 => m_Wrapper.m_inLevel_inputChannel4;
        public InputAction @inputChannel5 => m_Wrapper.m_inLevel_inputChannel5;
        public InputAction @inputChannel6 => m_Wrapper.m_inLevel_inputChannel6;
        public InputAction @inputChannel7 => m_Wrapper.m_inLevel_inputChannel7;
        public InputActionMap Get() { return m_Wrapper.m_inLevel; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InLevelActions set) { return set.Get(); }
        public void AddCallbacks(IInLevelActions instance)
        {
            if (instance == null || m_Wrapper.m_InLevelActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InLevelActionsCallbackInterfaces.Add(instance);
            @inputChannel0.started += instance.OnInputChannel0;
            @inputChannel0.performed += instance.OnInputChannel0;
            @inputChannel0.canceled += instance.OnInputChannel0;
            @inputChannel1.started += instance.OnInputChannel1;
            @inputChannel1.performed += instance.OnInputChannel1;
            @inputChannel1.canceled += instance.OnInputChannel1;
            @inputChannel2.started += instance.OnInputChannel2;
            @inputChannel2.performed += instance.OnInputChannel2;
            @inputChannel2.canceled += instance.OnInputChannel2;
            @inputChannel3.started += instance.OnInputChannel3;
            @inputChannel3.performed += instance.OnInputChannel3;
            @inputChannel3.canceled += instance.OnInputChannel3;
            @inputChannel4.started += instance.OnInputChannel4;
            @inputChannel4.performed += instance.OnInputChannel4;
            @inputChannel4.canceled += instance.OnInputChannel4;
            @inputChannel5.started += instance.OnInputChannel5;
            @inputChannel5.performed += instance.OnInputChannel5;
            @inputChannel5.canceled += instance.OnInputChannel5;
            @inputChannel6.started += instance.OnInputChannel6;
            @inputChannel6.performed += instance.OnInputChannel6;
            @inputChannel6.canceled += instance.OnInputChannel6;
            @inputChannel7.started += instance.OnInputChannel7;
            @inputChannel7.performed += instance.OnInputChannel7;
            @inputChannel7.canceled += instance.OnInputChannel7;
        }

        private void UnregisterCallbacks(IInLevelActions instance)
        {
            @inputChannel0.started -= instance.OnInputChannel0;
            @inputChannel0.performed -= instance.OnInputChannel0;
            @inputChannel0.canceled -= instance.OnInputChannel0;
            @inputChannel1.started -= instance.OnInputChannel1;
            @inputChannel1.performed -= instance.OnInputChannel1;
            @inputChannel1.canceled -= instance.OnInputChannel1;
            @inputChannel2.started -= instance.OnInputChannel2;
            @inputChannel2.performed -= instance.OnInputChannel2;
            @inputChannel2.canceled -= instance.OnInputChannel2;
            @inputChannel3.started -= instance.OnInputChannel3;
            @inputChannel3.performed -= instance.OnInputChannel3;
            @inputChannel3.canceled -= instance.OnInputChannel3;
            @inputChannel4.started -= instance.OnInputChannel4;
            @inputChannel4.performed -= instance.OnInputChannel4;
            @inputChannel4.canceled -= instance.OnInputChannel4;
            @inputChannel5.started -= instance.OnInputChannel5;
            @inputChannel5.performed -= instance.OnInputChannel5;
            @inputChannel5.canceled -= instance.OnInputChannel5;
            @inputChannel6.started -= instance.OnInputChannel6;
            @inputChannel6.performed -= instance.OnInputChannel6;
            @inputChannel6.canceled -= instance.OnInputChannel6;
            @inputChannel7.started -= instance.OnInputChannel7;
            @inputChannel7.performed -= instance.OnInputChannel7;
            @inputChannel7.canceled -= instance.OnInputChannel7;
        }

        public void RemoveCallbacks(IInLevelActions instance)
        {
            if (m_Wrapper.m_InLevelActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInLevelActions instance)
        {
            foreach (var item in m_Wrapper.m_InLevelActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InLevelActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InLevelActions @inLevel => new InLevelActions(this);
    private int m_PCKeyboardSchemeIndex = -1;
    public InputControlScheme PCKeyboardScheme
    {
        get
        {
            if (m_PCKeyboardSchemeIndex == -1) m_PCKeyboardSchemeIndex = asset.FindControlSchemeIndex("PC-Keyboard");
            return asset.controlSchemes[m_PCKeyboardSchemeIndex];
        }
    }
    public interface IInLevelActions
    {
        void OnInputChannel0(InputAction.CallbackContext context);
        void OnInputChannel1(InputAction.CallbackContext context);
        void OnInputChannel2(InputAction.CallbackContext context);
        void OnInputChannel3(InputAction.CallbackContext context);
        void OnInputChannel4(InputAction.CallbackContext context);
        void OnInputChannel5(InputAction.CallbackContext context);
        void OnInputChannel6(InputAction.CallbackContext context);
        void OnInputChannel7(InputAction.CallbackContext context);
    }
}