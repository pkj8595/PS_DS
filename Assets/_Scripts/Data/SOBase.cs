using UnityEngine;


public abstract class SOBase : ScriptableObject { }

public abstract class SOSkillModuleBase : SOBase { }
public abstract class SOData : SOBase 
{
    public int Table { get; }
    public int tableNum;
    public string GetID() => name;

}