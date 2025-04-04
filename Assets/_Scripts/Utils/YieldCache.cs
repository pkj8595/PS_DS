using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldCache
{
    private static readonly Dictionary<float, WaitForSeconds> _cacheWaitForSeconds = new();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_cacheWaitForSeconds.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
        {
            _cacheWaitForSeconds.Add(seconds, new WaitForSeconds(seconds));
        }
        return _cacheWaitForSeconds[seconds];
    }
   
}
