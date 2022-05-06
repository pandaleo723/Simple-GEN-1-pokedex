#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using TargetSceneNameEnum = SceneName;

public enum SceneName { }

namespace JwDeveloper.Utility
{
    [CreateAssetMenu(fileName = "LoadSceneObject - ", menuName = "ScriptableObjects/Jw Utility/LoadSceneObject")]
    [ExecuteInEditMode]
    public class LoadSceneObject : ScriptableObject
    {
        public TargetSceneNameEnum sceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), sceneName.ToString());
        }
#endif
    }
}