using UnityEngine;

namespace Attributes
{
    public class DisableIf : PropertyAttribute
    {
        public string ConditionMemberName { get; }

        public DisableIf(string conditionMemberName)
        {
            ConditionMemberName = conditionMemberName;
        }
    }
}