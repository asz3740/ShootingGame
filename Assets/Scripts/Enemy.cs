using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적의 상태 열거
    public enum State : int
    {
        None = -1, // 사용전
        Ready = 0, // 준비완료
        Appear,    // 등장
        Battle,    // 전투중
        Dead,      // 사망
        Disappear  // 퇴장
    }

    // 현재 상태 
    [SerializeField]
    private State CurrentState = State.None;
    
    // 최대 스피드
    private const float MaxSpeed = 10.0f;
    
    // 
    private const float MaxSpeedTime = 0.5f;

    // 타깃 위치
    [SerializeField]
    private Vector3 TargetPosition;

    //  현재 스피드
    [SerializeField] 
    private float CurrentSpeed;

    // 현재 속도
    private Vector3 CurrentVelocity;

    private float MoveStartTime = 0.0f;
    
    private float BattleStartTime = 0.0f;


    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Appear(new Vector3(7.0f, transform.position.y, transform.position.z));
        }
        

        switch (CurrentState)
        {
            case State.None:
                
            case State.Ready:
                break;
            case State.Dead:
                break;
            case State.Appear:
            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattle();
                break;
        }

    }

    void UpdateSpeed()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStartTime) / MaxSpeedTime);
    }

    void UpdateMove()
    {
        // Target과의 거리 
        float distance = Vector3.Distance(TargetPosition, transform.position);

        // Target도착
        if (distance == 0)
        {
            Arrived();
            return;
        }

        // 현재 속도
        CurrentVelocity = ( TargetPosition - transform.position).normalized * CurrentSpeed;
        
        // 부드러운 이동, 거리 = 시간 * 속력 
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed);
    }

    void Arrived()
    {
        if (CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            BattleStartTime = Time.time;
        }
        else
        {
            CurrentState = State.None;
        }
    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }

    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0;

        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }

    void UpdateBattle()
    {
        if (Time.time - BattleStartTime > 3.0f)
        {
            Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if(player)
            player.OnCrash(this);
    }
    
    public void OnCrash(Player player)
    {
        
    }
}
