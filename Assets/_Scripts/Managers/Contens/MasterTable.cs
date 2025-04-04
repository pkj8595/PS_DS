using System.Collections.Generic;
using UnityEngine;

public class MasterTable<T> where T : SOData
{
    private Dictionary<string, T> tableDictionary = new ();

    public void Initialize(List<T> dataList)
    {
        tableDictionary.Clear();
        foreach (var data in dataList)
        {
            tableDictionary[data.GetID()] = data;
        }
    }

    public T GetData(string id)
    {
        tableDictionary.TryGetValue(id, out var data);
        return data;
    }
}