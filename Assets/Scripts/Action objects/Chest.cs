using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata;
using UnityEngine;

public class Chest : MonoBehaviour, Iinteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    public void TriggerAction()
    {
        if (!isDone)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;        
        isDone = true;
        this.gameObject.tag = "Untagged";
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
