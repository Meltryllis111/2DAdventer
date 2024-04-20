using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PLayStateBar : MonoBehaviour
{
    private Character currentCharacter;
    public Image healthImage;
    public Image healhDelayImage;
    public Image powerImage;
    private bool isPowerRecovering;

    private void Update()
    {
        if (healhDelayImage.fillAmount > healthImage.fillAmount)
        {
            healhDelayImage.fillAmount -= Time.deltaTime;
        }

        if (isPowerRecovering)
        {
            float percent = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = percent;

            if (percent >= 1)
            {
                isPowerRecovering = false;
                return;
            }
        }
    }
    /// <summary>
    /// 接受血量变更百分比
    /// </summary>
    /// <param name="percent">百分比</param>
    /// 
    public void OnHealthChange(float percent)
    {
        healthImage.fillAmount = percent;
    }
    public void OnPowerChange(Character character)
    {
        isPowerRecovering = true;
        currentCharacter = character;
    }
}
