using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적의 상태 
    public enum State : int
    {
        None = -1, // 사용전
        Ready = 0, // 준비완료
        Appear,    // 등장
        Battle,    // 전투중
        Dead,      // 사망
        Disappear  // 퇴장
    }
    
    [SerializeField]
    private State CurrentState = State.None; // 현재 상태 
    
    private const float MaxSpeed = 10.0f; // 최대 스피드
    
    private const float MaxSpeedTime = 0.5f;
    
    [SerializeField]
    private Vector3 TargetPosition; // 타깃 위치

    [SerializeField] 
    private float CurrentSpeed; // 현재 스피드

    private Vector3 CurrentVelocity; // 현재 속도 

    private float MoveStartTime = 0.0f;

    [Header("총알 속성")]
    
    [SerializeField] 
    private GameObject Bullet; // 총알
    
    [SerializeField]
    private Transform FireTransform; // 발사 위치
    
    [SerializeField] 
    private float bulletSpeed = 1; // 총알 스피드
    
    private float LastBattleUpdateTime = 0.0f;

    [SerializeField] 
    private int FireRemainCount = 1;

    void Update()
    {
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
            LastBattleUpdateTime = Time.time;
        }
        else
        {
            CurrentState = State.None;
        }
    }

    // 적 등장
    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }

    
    // 적 퇴장
    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0;

        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }
    
    void UpdateBattle()
    {
        if (Time.time - LastBattleUpdateTime > 1.0f)
        {
            if (FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
            }
            
            LastBattleUpdateTime = Time.time;
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
    
    public void Fire()
    {
        GameObject go = Instantiate(Bullet);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Fire(OwnerSide.Enemy, FireTransform.position, -FireTransform.right, bulletSpeed);
    }
}
