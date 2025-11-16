using System;
using UnityEngine;

namespace Attributes
{
    /// <summary>
    /// 커스텀 Attribute 테스트 클래스. 
    /// </summary>
    public class AttributesTestClass : MonoBehaviour
    {
        [Header("VisibleOnly")]

        [VisibleOnly] 
        public bool alwaysVisibleOnly;

        [VisibleOnly(VisibleOnly.Mode.EditModeOnly)]
        public bool editModeOnly;

        [VisibleOnly(VisibleOnly.Mode.PlayModeOnly)]
        public bool playModeOnly;

        
        [Header("EnableIf")]

        public bool enableIf;

        [EnableIf(nameof(enableIf))]
        public MyStruct myStruct;

        [Header("DisableIf")]
        
        public bool disableIf;

        [DisableIf(nameof(disableIf))]
        public MyClass myClass;

        
        [Serializable]
        public struct MyStruct
        {
            public float value;
        }

        [Serializable]
        public class MyClass
        {
            public enum MyEnum { Option1, Option2, Option3 }
            public MyEnum option;
        }
    }
}