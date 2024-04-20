using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDateEvent;


    public static DataManager Instance;

    //私有变量
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private string jsonFolder;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/SAVE";

        ReadSaveDate();
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveDate(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveDate(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDateEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDateEvent.OnEventRaised -= Load;
    }

    public void Save()
    {
        foreach (var item in saveableList)
        {
            item.GetSaveData(saveData);
            
        }
        var resultPath = jsonFolder + "data.save";
        var jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(resultPath, jsonData);
    }
    public void Load()
    {
        foreach (var item in saveableList)
        {
            item.LoadData(saveData);
        }
    }

    private void ReadSaveDate()
    {
        var resultPath = jsonFolder + "data.save";

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
