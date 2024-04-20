using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraControl : MonoBehaviour
{
    public VoidEventSO afterSceneLoadedEvent;
    private CinemachineConfiner2D cinemachineConfiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO onCameraShake;

    private void Awake() {
        cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable() {
        onCameraShake.OnEventRaised += OnCameraShake;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoaded;
    }


    private void OnDisable() {
        onCameraShake.OnEventRaised -= OnCameraShake;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoaded;
    }
    private void OnAfterSceneLoaded()
    {
        GetNewCameraBounds();
    }
    private void OnCameraShake()
    {
        impulseSource.GenerateImpulse();
    }
   
    // private void Start() {
    //     GetNewCameraBounds();
    // }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        cinemachineConfiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        cinemachineConfiner2D.InvalidateCache();
    }
}
