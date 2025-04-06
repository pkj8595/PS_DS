using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class Define
{
    public const int Pawn_Rune_Limit_Count = 3;
    public const int Rune_Count = 3;
    public const int Trait_Count = 10;
    public const int Affect_Count = 3;
    /// <summary>
    /// worldObject 상태
    /// </summary>

    public enum UnitType
    {
        Unknown,
        Pawn,
        Building,
    }

    public enum Layer
    {
        Water = 1 << 4,
        UI = 1 << 5,
        Ground = 1 << 6,
        Wall = 1 << 7,
        Building = 1 << 8,
        Pawn = 1 << 9,
        PawnGroup = 1 << 10,
    }

    public enum EPawnAniState
    {
        Idle,
        Ready,
        Running,
        Dead,
        Casting
    }

    public enum EPawnAniTriger
    {
        Slash,
        Shot,
        Hit,
        Heal,
        Cool
    }

    public enum EBuildingAniState
    {
        Idle,
        Ready,
        Dead,
        Casting
    }

    public enum Scene
    {
        Unknown,
        Login,
        GameScene,
        Ending,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Count,
    }

    public enum MouseEvent
    {
        LPress,
        LPointerDown,
        LPointerUp,
        LClick,
        RPress,
        RPointerDown,
        RPointerUp,
        RClick,
    }

    public enum ETargetType
    {
        Self,
        Enemy,
        Ally,
    }

    public enum ETeam
    {
        Player1,
        Player2,
    }


    public enum EAttributeType
    {
        none,
        Blow, //타격
        Penetration,//관통
        Fire,//불
        Posison,//독
        Ice,//얼음
        Dark,//어둠
        Light,//빛
    }

    public enum ESkillDistanceType
    {
        LessMin,
        Excuteable,
        MoreMax,
    }

    public enum EGoodsType
    {
        gold = 301001001,
        manaStone = 301001002,
        wood = 301001003,
        food = 301001004,
    }

    public enum EStatDataType
    {
        Character,
        Building,
        Upgrade,
        Skill,
    }

    public static class Path
    {
        public const string Sprite = "Sprites/";

        public const string Prefab_Trap = "Prefabs/Tiles/Traps/";
        public const string Sprite_Trap = "Sprites/Tiles/Traps/";

        public const string Prefab_Bullet = "Projectiles/";
        public const string Sprite_Bullet = "Sprites/Projectiles/";

        public const string UI = "UI/UIBase/";
        public const string UIPopup = "UI/Popup/";
        public const string UIIcon = "Sprites/UI/Icon/";

    }

}

