using UnityEngine;

namespace TimeKit.Core.Linked
{
    internal interface IClockLinked
    {
        ClockType ClockType { get; }
        object Target { get; }
        GameObject GameObject { get; }
    }
}