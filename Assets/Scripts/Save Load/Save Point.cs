using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, Iinteractable
{
    [Header("事件广播")]
    public VoidEventSO saveGameEvent;
     
    public SpriteRenderer spriteRenderer;
    [HideInInspector] public bool isDone;
    public Sprite darkSprite;
    public GameObject lightObj;
    public Sprite lightSprite;

    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TriggerAction()
    {
        if (!isDone)
        { 
            lightObj.SetActive(true);
            saveGameEvent.OnEventRaised();
            ChengeImage();
        }
    }

    private void ChengeImage()
    {
        spriteRenderer.sprite = lightSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        lightObj.SetActive(isDone);
    }
}
