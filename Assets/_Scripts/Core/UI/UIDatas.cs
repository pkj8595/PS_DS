using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI에 전달하는 기본 객체 쿨래스
/// </summary>
public abstract class UIData
{
    
}

public class UIStoryData : UIData
{
    public int StoryDataNum { get; set; }
}

public class UIUnitData : UIData
{
    public Unit unitGameObject { get; set; }
}