using UnityEngine;

namespace Attributes
{
    public class EnableIf : PropertyAttribute
    {
        public string ConditionMemberName { get; }
        
        public EnableIf(string conditionMemberName)
        {
            ConditionMemberName = conditionMemberName;
        }
    }
}