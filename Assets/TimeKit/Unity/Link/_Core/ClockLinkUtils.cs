using System;
using UnityEngine;

namespace TimeKit.Unity.Link.Core
{
    internal static class ClockLinkUtils
    {
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