using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AtlasPackConfig", menuName = "@Tool/Atlas", order = 1)]
public class AtlasPackConfig : ScriptableObject
{
    public List<SpriteAtlas> _atlasList;
}
