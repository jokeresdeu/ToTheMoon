using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AssetBundleBuilderWindow : EditorWindow
    {
        private GameObject _cross;
        private GameObject _circle;
        private GameObject _backGround;
        
        private string _assetBundleName;

        [MenuItem("Window/AssetBundles/AssetBundle Builder")]
        private static void OpenWindow()
        {
            var window = GetWindow<AssetBundleBuilderWindow>();
            window.titleContent = new GUIContent("AssetBundle Builder");
            window.Show();
        }

        private void OnGUI()
        {
            _cross = EditorGUILayout.ObjectField("Cross", _cross, typeof(GameObject), true) as GameObject;
            _circle = EditorGUILayout.ObjectField("Circle", _circle, typeof(GameObject), true) as GameObject;
            _backGround = EditorGUILayout.ObjectField("BackGround", _backGround, typeof(GameObject), true) as GameObject;
            _assetBundleName = EditorGUILayout.TextField("AssetBundle Name", _assetBundleName);

            if (GUILayout.Button("Build AssetBundle"))
            {
                BuildAssetBundle();
            }
        }

        private void BuildAssetBundle()
        {
            if (_cross == null || _circle == null || _backGround == null)
            {
                Debug.LogError("Cross and Circle GameObjects must be assigned!");
                return;
            }

            if (string.IsNullOrEmpty(_assetBundleName))
            {
                Debug.LogError("AssetBundle Name must be specified!");
                return;
            }

            var streamingAssetsPath = Application.streamingAssetsPath;
            if (!Directory.Exists(streamingAssetsPath))
                Directory.CreateDirectory(streamingAssetsPath);

            var assetBundleBuild = new AssetBundleBuild
            {
                assetBundleName = _assetBundleName,
                assetNames = new[]
                {
                    AssetDatabase.GetAssetPath(_cross),
                    AssetDatabase.GetAssetPath(_circle),
                    AssetDatabase.GetAssetPath(_backGround)
                }
            };
            
            BuildPipeline.BuildAssetBundles(streamingAssetsPath, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            BuildPipeline.BuildAssetBundles(streamingAssetsPath, new[] { assetBundleBuild }, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();

            Debug.Log("AssetBundle built and saved to: " + streamingAssetsPath);
        }
    }
}