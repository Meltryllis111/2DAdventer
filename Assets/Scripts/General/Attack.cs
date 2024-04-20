using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本属性")]
    public int Damage;
    public float AttackRange;
    public float AttackRate;

    private void OnTriggerStay2D(Collider2D other) {
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
