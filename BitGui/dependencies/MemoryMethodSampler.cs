using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public static class MemoryMethodSampler
{

    public static bool Enabled;

    private static Dictionary<string, long> memoryMap = new Dictionary<string, long>();
    private static Dictionary<string, long> totalMemoryMap = new Dictionary<string, long>();

    public static void Begin(string key)
    {
        if (!Enabled)
            return;
        totalMemoryMap[key] = GC.GetTotalMemory(false);
    }

    public static void End(string key)
    {
        if (!Enabled)
            return;
        long diff = GC.GetTotalMemory(false) - totalMemoryMap[key];
        long value;
        if (!memoryMap.TryGetValue(key, out value))
        {
            value = 0;
            memoryMap[key] = 0;
        }
        memoryMap[key] = value + diff;
    }

    public static List<KeyValuePair<string, long>> Content()
    {
        List<KeyValuePair<string, long>> list = SortDictionary(memoryMap);
        memoryMap.Clear();
        return list;
    }

    public static List<KeyValuePair<string, long>> SortDictionary(Dictionary<string, long> data)
    {
        List<KeyValuePair<string, long>> result =
              new List<KeyValuePair<string, long>>(data);
        result.Sort(
          delegate(
            KeyValuePair<string, long> first,
            KeyValuePair<string, long> second)
          {
              return second.Value.CompareTo(first.Value);
          }
          );
        return result;
    }

}

