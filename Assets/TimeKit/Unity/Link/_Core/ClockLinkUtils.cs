using System;
using UnityEngine;

namespace TimeKit.Unity.Link.Core
{
    internal static class ClockLinkUtils
    {
        /// <summary>
        /// Attempts to get an unused <typeparamref name="TLink"/> component attached to the target.
        /// </summary>
        /// <param name="target">Component to search for an available link.</param>
        /// <param name="targetLink">Found link component, or null if none exist.</param>
        /// <returns>True if an available link component was found.</returns>
        internal static bool TryGetLink<TTarget, TLink>(Component target, out TLink targetLink)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            targetLink = null;

            if (!target)
                throw new ArgumentNullException();
            
            TLink[] links = target.GetComponents<TLink>();
            if (links != null)
            {
                foreach (var link in links)
                {
                    if (!link || link.Target) 
                        continue;
                    targetLink = link;
                    break;
                }
            }

            return targetLink;
        }
        
        /// <summary>
        /// Gets an existing unused <typeparamref name="TLink"/> component or adds a new one if necessary.
        /// </summary>
        /// <param name="target">Component to search or attach the link to.</param>
        /// <returns>A valid link component ready for binding.</returns>
        internal static TLink GetOrAddLinkComponent<TTarget, TLink>(Component target)
            where TTarget : Component
            where TLink : ClockLinkComponent<TTarget>
        {
            if (!TryGetLink<TTarget, TLink>(target, out var targetLink))
            {
                targetLink = target.gameObject.AddComponent<TLink>();
            }
            
            return targetLink;
        }
    }
}