using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

// Enmey, Player 총알 구별
public enum OwnerSide : int
{
    Player = 0,
    Enemy
}
public class Bullet : MonoBehaviour
{
    private const float LifeTime = 15;
    private OwnerSide ownerSide = OwnerSide.Player; // Player로 기본값 설정

    [SerializeField] 
    private Vector3 moveDirection = Vector3.zero; // 총알 방향

    [SerializeField] 
    private float movespeed = 0.0f; // 이동 속도

    [SerializeField] 
    private bool fire = false; // 발사 가능 여부

    private bool Hited = false;

    private float firedTime;
    
    void Update()
    {
        if (ProcessDisapperCondiction())
            return;
        UpdateMove();    
    }

    void UpdateMove()
    {
        // 발사하지 않을 때 
        if (!fire)
            return;
        
        // 이동할 양
        Vector3 moveVector = moveDirection.normalized * movespeed * Time.deltaTime;

        moveVector = AdjustMove(moveVector);
        // 이동
        transform.position += moveVector;
    }
    
    // 발사 위치 
    public void Fire(OwnerSide fireOwner, Vector3 firePosition , Vector3 direction, float speed)
    {
        ownerSide = fireOwner;
        transform.position = firePosition;
        moveDirection = direction;
        movespeed = speed;

        fire = true;
        firedTime = Time.time;
    }
    
    // 총알 움직임 조정 (콜라이더 무시 방지)
    Vector3 AdjustMove(Vector3 moveVector)
    {
        RaycastHit hitInfo;
        if(Physics.Linecast(transform.position, transform.position + moveVector, out hitInfo))
        {
            moveVector = hitInfo.point - transform.position;
            OnBulletCollision(hitInfo.collider);
        }
        return moveVector;
    }

    // 
    void OnBulletCollision(Collider collider)
    {
        if (Hited)
            return;

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;
        
        Hited = true;
        fire = false;
        
        if (ownerSide == OwnerSide.Player)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
        }
        else
        {
            Player player = collider.GetComponentInParent<Player>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);
    }
    
    void OnBecameInvisible() //화면밖으로 나가 보이지 않게 되면 호출이 된다.
    {
        Destroy(gameObject); //객체를 삭제한다.
    }
    
    bool ProcessDisapperCondiction()
    {
        if (transform.position.x > 15.0f || transform.position.x < -15.0f || 
            transform.position.y > 15.0f || transform.position.y < -15.0f)
        {
            Disapper();
            return true;
        }
        else if (Time.time - firedTime > LifeTime)
        {    
            Disapper();
            return true;
        }

        return false;
    }

    void Disapper()
    {
        Destroy(gameObject);
    }
}
