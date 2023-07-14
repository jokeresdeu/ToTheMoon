using System;
using System.Collections;
using System.IO;
using Game.Figure;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssetBundles
{
    public class AssetBundlesLoader
    {
        public CrossBehaviour CrossBehaviour { get; private set; }
        public CircleBehaviour CircleBehaviour { get; private set; }
        public GameObject Background { get; private set; }
        
        public IEnumerator LoadBundlesAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("AssetBundle Name must be specified!");
                yield break;
            }
            
            var loadRequest =  AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "graphics"));
            yield return loadRequest;
            var graphicsBundle = loadRequest.assetBundle;
            yield return graphicsBundle.LoadAllAssetsAsync();
            
            var assetBundlePath = Path.Combine(Application.streamingAssetsPath, name.ToLower());
            loadRequest = AssetBundle.LoadFromFileAsync(assetBundlePath);
            yield return loadRequest;
            var assetBundle = loadRequest.assetBundle;
            if (assetBundle == null)
                throw new NullReferenceException("Failed to load AssetBundle at path: " + assetBundlePath);

            CrossBehaviour = assetBundle.LoadAsset<GameObject>("Cross").GetComponent<CrossBehaviour>();
            CircleBehaviour = assetBundle.LoadAsset<GameObject>("Circle").GetComponent<CircleBehaviour>();
            Background = assetBundle.LoadAsset<GameObject>("BackGround");
        }
    }
}