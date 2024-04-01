using System;
using System.Collections.Generic;
using UnityEngine;

namespace ptl.bezier
{
    public static class KnotSegmentResolutionManager
    {
        public static Dictionary<int, int> _data = new();

        public static int GetValue(int key)
        {
            return _data.TryGetValue(key, out var value) ? value : 0;
        }

        public static void SetValue(int key, int value)
        {
            _data[key] = value;
            Debug.Log($"SetDataValue: {_data[key]}");
        }
    }

    public class SerializableValue
    {
        public List<int> keys;
        public List<int> values;
    }
}