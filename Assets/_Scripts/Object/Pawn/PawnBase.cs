using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public abstract class PawnBase : Unit
{
    //data
    public SOCharacterData CharacterData { get; private set; }

    //pawn 기능
    [field: SerializeField] public PawnAnimationController PawnAni { get; private set; }
    [field: SerializeField] public PawnMove PawnMove { get; private set; }
    [field: SerializeField] public Stat Stat { get; protected set; }
    [field: SerializeField] public PawnDamageable PawnDamageable { get; private set; }
    public UnitSkill PawnSkills;
    public UnitAI AI { get; private set; }




}
