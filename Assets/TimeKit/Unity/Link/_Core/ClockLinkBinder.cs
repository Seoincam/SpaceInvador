using UnityEngine;

namespace TimeKit.Unity.Link.Core
{
    internal static class ClockLinkBinder
    {
        internal static void Bind<TTarget, TLink>(ClockType type, TTarget target)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            var link = ClockLinkUtils.GetOrAddLinkComponent<TTarget, TLink>(target);
            link.Bind(type, target);
        }

        internal static bool Unbind<TTarget, TLink>(ClockType type, TTarget target)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            if (!ClockLinkUtils.TryGetLink<TTarget, TLink>(target, out var link))
                return false;
            
            link.Unbind();
            return true;
        }
    }
}