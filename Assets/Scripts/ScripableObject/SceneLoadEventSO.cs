using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO", order = 0)]
public class SceneLoadEventSO : ScriptableObject
{

    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestedEvnet;
/// <summary>
/// 场景加载请求
/// </summary>
/// <param name="locationToLoad">目标场景</param>
/// <param name="positionToGo">目标坐标</param>
/// <param name="fadeScreen">是否淡入淡出</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad,Vector3 positionToGo,bool fadeScreen)
    {
        LoadRequestedEvnet?.Invoke(locationToLoad,positionToGo,fadeScreen);
    }
}