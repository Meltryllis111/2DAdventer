using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoadEventSO sceneLoadEventSO;
    public GameSceneSO sceneToLoad;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
    }
}
