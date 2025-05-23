using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPanel : MonoBehaviour
{
    [SerializeField]
    public int startingIndex; // 시작 위치 인덱스로 설정
    private int targetIndex;
    [SerializeField]
    public Transform[] waypoints;
    private int index;
    private float elapsedTime = 0f;
    [SerializeField]
    public float maxSpeed;
    //일반적 이동에 필요
    [SerializeField]
    public AnimationCurve movingCurve;
    //부드러운 이동에 필요
    private Vector3 startPos;
    private Vector3 targetPos;
    private float distance;
    private float travelTime;

    void Awake()
    {
        startPos = waypoints[startingIndex].position;
        targetIndex = startingIndex + 1;
        targetPos = waypoints[targetIndex].position;
        CalculateTravelTime();
    }

    void Update()
    {
               
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / travelTime;
        t = movingCurve.Evaluate(Mathf.Clamp01(t)); // 부드럽게 보간
        transform.position = Vector3.Lerp(startPos, targetPos, t);

        if (t >= 1f)
        {
            // 다음 포인트로
            targetIndex = (targetIndex + 1) % waypoints.Length;
            startPos = targetPos;
            targetPos = waypoints[targetIndex].position;
            CalculateTravelTime();
            elapsedTime = 0f;
        }
    }

    private void CalculateTravelTime()
    {
        distance = Vector3.Distance(startPos, targetPos);
        travelTime = distance / maxSpeed;

    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
        Debug.Log("충돌 시작함");
    }
    void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
        Debug.Log("충돌 빠져나옴");
    }
}
