using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour, ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("基本属性")]
    public float MaxHealth;
    public float CurrentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("受伤无敌")]
    public float InvincibleTime;
    [HideInInspector] public float InvincibleCounter;
    public bool IsInvincible;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    public UnityEvent<Character> OnHealthChange;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveDate();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveDate();
    }
    private void NewGame()
    {
        CurrentHealth = MaxHealth;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }
    private void Update()
    {
        if (IsInvincible)
        {
            InvincibleCounter -= Time.deltaTime;
            if (InvincibleCounter <= 0)
            {
                IsInvincible = false;
            }
        }
        if (currentPower < maxPower)
        {
            currentPower += powerRecoverSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead Zone"))
        {
            //触发死亡,更新血量
            CurrentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDead?.Invoke();
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (IsInvincible)
            return;

        if (CurrentHealth - attacker.Damage > 0 && attacker.Damage > 0)
        {
            CurrentHealth -= attacker.Damage;
            TriggerInvincible();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else if (CurrentHealth - attacker.Damage <= 0 && attacker.Damage > 0)
        {
            CurrentHealth = 0;
            //触发死亡
            OnDead?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }
    private void TriggerInvincible()
    {
        IsInvincible = true;
        InvincibleCounter = InvincibleTime;
    }

    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefinaltion GetDataID()
    {
        return GetComponent<DataDefinaltion>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerializeVector3(transform.position);
            data.floatSaveData[GetDataID().ID + "health"] = this.CurrentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;

        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID + "health", this.CurrentHealth);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();
            this.CurrentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];

            OnHealthChange?.Invoke(this);
        }
    }
}
