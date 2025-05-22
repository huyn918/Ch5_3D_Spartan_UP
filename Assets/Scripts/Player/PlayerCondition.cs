using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    private Coroutine buffCoroutine;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;
    public event Action<float> onFeverTime;
    public float noStaminaHealthDecay;

    private bool isBuffOn = false;

    private void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        
        if (health.curValue < 0f)
        {
            Die();
        }
    }

    public void HealHP(float amount)
    {
        health.Add(amount);
    }

    public void HealStamina(float amount)
    {
        stamina.Add(amount);
    }

    public void StaminaBuff(float amount)
    {
        if (isBuffOn)
        {
            StopCoroutine(buffCoroutine);
        }
        buffCoroutine = StartCoroutine(StaminaFever(amount)); // 버프 갱신
    }

    private IEnumerator StaminaFever(float value)
    {
        onFeverTime?.Invoke(value);
        if (!isBuffOn) // 버프가 오프상태일 때만 추가스텟을 부여해서 중복으로 버프가 걸리지 않게 방지
        {
            isBuffOn = true;
            stamina.passiveValue += 1000; // 스테미나 자연 회복량 증가
            CharacterManager.Instance.Player.controller.maxSpeed += 10; // 최대속도 증가
            CharacterManager.Instance.Player.controller.acceleration += 10; // 가속력도 증가
            CharacterManager.Instance.Player.controller.jumpPower += 30; // 점프력도 증가
        }
        yield return new WaitForSeconds(value); // 버프시간 갱신
        stamina.passiveValue -= 1000;
        CharacterManager.Instance.Player.controller.maxSpeed -= 10;
        CharacterManager.Instance.Player.controller.acceleration -= 10; 
        CharacterManager.Instance.Player.controller.jumpPower -= 30; 
        isBuffOn = false; // 버프 끄기
        buffCoroutine = null;
    }

    

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
    public void UseStamina(int damageAmount)
    {
        int value = (int)stamina.curValue - damageAmount;
        if (value > 0)
        {
            stamina.Subtract(damageAmount);
        }
        else
        {
            stamina.Subtract(stamina.curValue);
            health.Subtract(-value); // value가 음수라서 체력 감소
            onTakeDamage?.Invoke();
        }
    }


}
