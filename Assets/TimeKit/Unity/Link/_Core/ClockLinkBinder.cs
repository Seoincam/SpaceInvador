using UnityEngine;

namespace TimeKit.Unity.Link.Core
{
    /// <summary>
    /// Utility methods for binding and unbinding clock link components.
    /// </summary>
    internal static class ClockLinkBinder
    {
        /// <summary>
        /// Binds the target to a clock by obtaining or creating a link component.
        /// </summary>
        /// <param name="type">Clock type to bind.</param>
        /// <param name="target">Target component.</param>
        internal static void Bind<TTarget, TLink>(ClockType type, TTarget target)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            TryUnbindAny<TTarget, TLink>(target);
            
            var link = ClockLinkUtils.GetOrAddLinkComponent<TTarget, TLink>(target);
            link.Bind(type, target);
        }

        /// <summary>
        /// Unbinds the target from the specified clock type if a matching link exists.
        /// </summary>
        /// <param name="type">Clock type to unbind.</param>
        /// <param name="target">Target component.</param>
        /// <returns>True if a matching link was unbound.</returns>
        internal static bool TryUnbind<TTarget, TLink>(ClockType type, TTarget target)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            if (!ClockLinkUtils.TryGetLink<TTarget, TLink>(target, out var link))
                return false;

            if (link.Type != type)
                return false;
            
            link.Unbind();
            return true;
        }
        
        /// <summary>
        /// Unbinds the target from any existing clock link.
        /// </summary>
        /// <param name="target">Target component.</param>
        /// <returns>True if any link was unbound.</returns>
        internal static bool TryUnbindAny<TTarget, TLink>(TTarget target)
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