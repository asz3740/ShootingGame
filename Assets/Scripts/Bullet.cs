using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public enum OwnerSide : int
{
    Player = 0,
    Enemy
}
public class Bullet : MonoBehaviour
{
    private OwnerSide ownerSide = OwnerSide.Player; // enmey, player 총알 구별

    [SerializeField] 
    private Vector3 moveDirection = Vector3.zero; // 총알 방향

    [SerializeField] 
    private float movespeed = 0.0f; // 이동 속도

    [SerializeField] 
    private bool fire = false; // 발사 가능 여부

    private bool Hited = false;
    
    void Start()
    {
        
    }

   
    void Update()
    {
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
    
    public void Fire(OwnerSide fireOwner, Vector3 firePosition , Vector3 direction, float speed)
    {
        ownerSide = fireOwner;
        transform.position = firePosition;
        moveDirection = direction;
        movespeed = speed;

        fire = true;
    }

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
}
