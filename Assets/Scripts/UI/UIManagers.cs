using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManagers : MonoBehaviour
{
    public PLayStateBar playStateBar;
    [Header("事件监听")]
    public CharacterEventSO HealthEvent;
    public SceneLoadEventSO unLoadSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO exitGameEvent;
    [Header("获取组件")]
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject moblieTouch;
    public Button settingsButton;
    public GameObject pausePanel;

    private void Awake()
    {
#if UNITY_STANDALONE
        moblieTouch.SetActive(false);
#endif

        settingsButton.onClick.AddListener(TogglePausePanel);
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnEnable()
    {
        HealthEvent.OnEventRaised += OnHealthEvent;
        unLoadSceneEvent.LoadRequestedEvnet += OnUnLoadRequest;
        loadDataEvent.OnEventRaised += OnLoadData;
        gameOverEvent.OnEventRaised += OnGameOver;
        backToMenuEvent.OnEventRaised += OnLoadData;
        exitGameEvent.OnEventRaised += OnExitGame;
    }

    private void OnDisable()
    {
        HealthEvent.OnEventRaised -= OnHealthEvent;
        unLoadSceneEvent.LoadRequestedEvnet -= OnUnLoadRequest;
        loadDataEvent.OnEventRaised -= OnLoadData;
        gameOverEvent.OnEventRaised -= OnGameOver;
        backToMenuEvent.OnEventRaised -= OnLoadData;
        exitGameEvent.OnEventRaised -= OnExitGame;
    }

    private void OnExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();        
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    private void OnLoadData()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnUnLoadRequest(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playStateBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.CurrentHealth / character.MaxHealth;
        playStateBar.OnHealthChange(percentage);
        playStateBar.OnPowerChange(character);
    }
}
