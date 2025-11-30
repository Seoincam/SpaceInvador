using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace TimeKit.Unity.Bootstrapper
{
    internal static class PlayerLoopUtils
    {
        internal static bool InsertSystem<T>(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem systemToInsert,
            int index)
        {
            if (loopSystem.type == typeof(T))
            {
                var list = new List<PlayerLoopSystem>();
                if (loopSystem.subSystemList != null)
                    list.AddRange(loopSystem.subSystemList);

                if (index < 0 || index > list.Count)
                    throw new IndexOutOfRangeException();
                
                list.Insert(index, systemToInsert);
                loopSystem.subSystemList = list.ToArray();
                return true;
            }
            
            return TryInsertIntoChildren<T>(ref loopSystem, in systemToInsert, index);
        }

        private static bool TryInsertIntoChildren<T>(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem systemToInsert, 
            int index)
        {
            if (loopSystem.subSystemList == null)
                return false;

            var children = loopSystem.subSystemList;

            for (int i = 0; i < children.Length; i++)
            {
                if (InsertSystem<T>(ref children[i], in systemToInsert, index))
                {
                    loopSystem.subSystemList = children;
                    return true;
                }
            }

            return false;
        }

        internal static bool RemoveSystem<T>(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem systemToRemove)
        {
            if (loopSystem.type == typeof(T))
            {
                if (loopSystem.subSystemList == null)
                    return false;
                
                var list = new List<PlayerLoopSystem>(loopSystem.subSystemList);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].type == systemToRemove.type && 
                        list[i].updateDelegate == systemToRemove.updateDelegate)
                    {
                        list.RemoveAt(i);
                        loopSystem.subSystemList = list.ToArray();
                        return true;
                    }
                }
            }
            
            return TryRemoveIntoChildren<T>(ref loopSystem, in systemToRemove);
        }

        private static bool TryRemoveIntoChildren<T>(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem systemToRemove)
        {
            if (loopSystem.subSystemList == null)
                return false;
            
            var children = loopSystem.subSystemList;
            for (int i = 0; i < children.Length; i++)
            {
                if (RemoveSystem<T>(ref children[i], in systemToRemove))
                {
                    loopSystem.subSystemList = children;
                    return true;
                }
            }
            return false;
        }
    }
}