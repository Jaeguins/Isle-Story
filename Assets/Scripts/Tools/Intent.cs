using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Intent {
    static Dictionary<string, object> Data = new Dictionary<string, object>();
    public static void InputData(string key, object value) {
        if (Data.ContainsKey(key))
            Data[key] = value;
        else
            Data.Add(key, value);
    }
    public static T GetData<T>(string key) {
        return (T)Data[key];
    }
    public static void DeleteData(string key) {
        Data.Remove(key);
    }
}
