using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEventSO;
    public SceneLoadEventSO sceneLoadEventSO;
    public VoidEventSO backToMenuEvent;
    [Header("事件广播")]
    public FadeEventSO fadeEvent;
    public VoidEventSO afterSceneLoadedEventSO;
    public SceneLoadEventSO unLoadEventSO;
    [Header("参数")]
    public float fadeDuration;
    public Transform player;
    public Vector3 playerFirstPosition;
    public Vector3 menuPosition;
    [Header("场景")]
    public GameSceneSO firstScene;
    public GameSceneSO menuScene;
    [SerializeField] private GameSceneSO currentScene;
    private GameSceneSO locationToLoad;



    private Vector3 positionToGo;
    private bool fade;
    private bool isLoading;

    private void Awake()
    {
    }

    private void Start()
    {
        sceneLoadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        // NewGame();
    }
    private void OnEnable()
    {
        sceneLoadEventSO.LoadRequestedEvnet += OnLoadRequest;
        newGameEventSO.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackMenu;

        ISaveable saveable = this;
        saveable.RegisterSaveDate();
    }
    private void OnDisable()
    {
        sceneLoadEventSO.LoadRequestedEvnet -= OnLoadRequest;
        newGameEventSO.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackMenu;

        ISaveable saveable = this;
        saveable.UnRegisterSaveDate();
    }

    private void OnBackMenu()
    {
        locationToLoad = menuScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(locationToLoad, playerFirstPosition, true);
    }

    private void NewGame()
    {
        locationToLoad = firstScene;
        //OnLoadRequest(locationToLoad, playerFirstPosition, true);
        sceneLoadEventSO.RaiseLoadRequestEvent(locationToLoad, playerFirstPosition, true);
    }
    private void OnLoadRequest(GameSceneSO locationToLoad, Vector3 positionToGo, bool fade)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        this.locationToLoad = locationToLoad;
        this.positionToGo = positionToGo;
        this.fade = fade;

        if (currentScene != null)
        {
            StartCoroutine(UnloadScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnloadScene()
    {
        if (fade)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        unLoadEventSO.RaiseLoadRequestEvent(locationToLoad, positionToGo, true);

        yield return currentScene.sceneReference.UnLoadScene();
        //关闭人物
        player.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadsingOption = locationToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadsingOption.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        currentScene = locationToLoad;
        player.position = positionToGo;
        player.gameObject.SetActive(true);

        if (fade)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if (currentScene.sceneType == SceneType.Loaction)
            afterSceneLoadedEventSO?.RaiseEvent();

    }

    public DataDefinaltion GetDataID()
    {
        return GetComponent<DataDefinaltion>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentScene);
    }

    public void LoadData(Data data)
    {
        var playerID = player.GetComponent<DataDefinaltion>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID].ToVector3();
            locationToLoad = data.GetSavedScene();

            OnLoadRequest(locationToLoad, positionToGo, true);

        }
    }
}
