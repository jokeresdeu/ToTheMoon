using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AssetBundles;
using Game;
using Game.Grid;
using Game.TouchController;
using UI.Core.Context;
using UI.Enum;
using UI.ViewProvider;
using UI.ViewProvider.Data;
using UnityEngine;
using UnityEngine.UI;

[SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
public sealed class AppFlowController : MonoBehaviour
{
    [SerializeField] private Image _fader;
    [SerializeField] private GridView _gridView;
    [SerializeField] private ScreenViewsStorage _screenViewsStorage;
    [SerializeField] private string _originalBundlesKey;
    [SerializeField] private string _alternativeBundlesKey;
    [SerializeField] private bool _useAlternativeBundles;
    
    private IInput _input;
    private List<IDisposable> _disposables;

    private IEnumerator Start()
    {
        _fader.gameObject.SetActive(true);
        var assetBundlesLoader = new AssetBundlesLoader();
        var bundlesKey = _useAlternativeBundles ? _alternativeBundlesKey : _originalBundlesKey;
        yield return assetBundlesLoader.LoadBundlesAsync(bundlesKey);
        Instantiate(assetBundlesLoader.Background, Vector3.zero, Quaternion.identity);
        _disposables = new List<IDisposable>();
        _gridView.Initialize(assetBundlesLoader.CrossBehaviour, assetBundlesLoader.CircleBehaviour);
        _input = GetInput();
        var gameLauncher = InitializeGameLauncher();
        InitializeUIContext(gameLauncher);
        _fader.gameObject.SetActive(false);
    }

    private void Update() => _input?.Update();

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();
    }

    private IInput GetInput()
    {
#if UNITY_EDITOR
        return new EditorInput();
#endif
        return new MobileInput();
    }

    private GameLauncher InitializeGameLauncher() => new GameLauncher(_gridView, _fader, _input);

    private void InitializeUIContext(GameLauncher gameLauncher)
    {
        var uiContext = new UIContext(new ViewProviderFromStorage(_screenViewsStorage), gameLauncher);
        uiContext.RegisterScreenOpener(gameLauncher);
        _disposables.Add(uiContext);
        uiContext.OpenOpenScreen(ScreenType.MainMenu);
    }
}