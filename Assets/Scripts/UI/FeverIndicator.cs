using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FeverIndicator : MonoBehaviour
{
    public RawImage image;
    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.onFeverTime += FeverTime;
    }

    public void FeverTime(float amount)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        coroutine = StartCoroutine(FeverUIOn(amount));
    }

    private IEnumerator FeverUIOn(float duration)
    {
        float elapsedTime = 0f;

        Color originalColor = image.color;

        while (elapsedTime < duration)
        {
            float blinkDuration = 2f;
            float cycleTime = 0f;
            while (cycleTime < blinkDuration)
            {
                float t = cycleTime / (blinkDuration / 2f);
                float alpha = (t < 1f) ? Mathf.Lerp(1f, 0f, t) : Mathf.Lerp(0f, 1f, t - 1f);
                // 증가와 감소 모두 천천히 알파값 0~1~0 반복
                Color color = image.color;
                color.a = alpha;
                image.color = color;
                cycleTime += Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null; // 다음프레임에 루프 다시 실행
            }
        }
        image.enabled = false;

        // 알파 3단 복원
        Color reset = image.color;
        reset.a = 1f;
        image.color = reset;

        coroutine = null;
    }

}
