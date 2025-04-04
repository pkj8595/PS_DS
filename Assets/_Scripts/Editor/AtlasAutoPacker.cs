#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;

[CustomEditor(typeof(AtlasPackConfig))]
public class AtlasAutoPacker : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AtlasPackConfig config = (AtlasPackConfig)target;
        if (GUILayout.Button("PackPreview"))
        {
            PackAtlases(config);
        }

    }

    public void PackAtlases(AtlasPackConfig config)
    {
        if (config._atlasList == null || config._atlasList.Count == 0)
        {
            Debug.LogWarning("❌ No atlases found in the list!");
            return;
        }
        Debug.Log($"🔄 Packing {config._atlasList.Count} atlases...");
        SpriteAtlasUtility.PackAtlases(config._atlasList.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("✅ Atlas packing complete!");

    }

    [MenuItem("Atlas Tools/Pack All Atlases")]
    public static void PackAtlases()
    {
        // 모든 AtlasPackConfig 찾기
        string[] guids = AssetDatabase.FindAssets("t:SpriteAtlas");
        var configs = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<AtlasPackConfig>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(config => config != null)
            .ToList();

        if (configs.Count == 0)
        {
            Debug.LogWarning("❌ No AtlasPackConfig assets found in the project.");
            return;
        }

        // 모든 아틀라스를 모아서 패킹
        var allAtlases = configs.SelectMany(config => config._atlasList).Where(atlas => atlas != null).ToArray();

        if (allAtlases.Length == 0)
        {
            Debug.LogWarning("❌ No atlases found in any AtlasPackConfig.");
            return;
        }

        Debug.Log($"🔄 Packing {allAtlases.Length} atlases...");
        SpriteAtlasUtility.PackAtlases(allAtlases, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("✅ All atlas packing complete!");
    }
}

#endif