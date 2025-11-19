using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity
{
    internal static class ClockLinkComponentUtils
    {
        internal static TLink GetOrAddLinkComponent<TLink, TTarget>(Component from) 
            where TLink : ClockLinkComponent<TTarget>
            where TTarget : Component
        {
            TLink link = null;
            
            TLink[] links = from.GetComponents<TLink>();
            if (links != null)
            {
                foreach (var l in links)
                {
                    if (!l || l.Target != from) continue;
                    
                    link = l;
                    break;
                }
            }

            if (!link)
                link = from.gameObject.AddComponent<TLink>();

            return link;
        }
    }
}