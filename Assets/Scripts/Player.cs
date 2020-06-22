using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 MoveVector = Vector3.zero;

    [SerializeField] 
    private float speed;

    [SerializeField] 
    private BoxCollider boxCollider;
    

    [SerializeField] 
    private Transform MainBGTransform;

    [Header("총알 속성")]
    
    [SerializeField] 
    private GameObject Bullet; // 총알
    
    [SerializeField]
    private Transform FireTransform; // 발사 위치

    
    [SerializeField] 
    private float bulletSpeed = 1; // 총알 스피드
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        // moveVector값이 있는지 확인
        if (MoveVector.sqrMagnitude == 0)
            return;

        MoveVector = AdjustMoveVector(MoveVector);
        
        // MoveVector값만큼 이동
        transform.position += MoveVector;
    }

    // 이동량 계산
    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * speed * Time.deltaTime;
    }

    // 카메라 밖으로 Player가 나가지 못하도록 Vector값 조정
    private Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;

        if (result.x - boxCollider.size.x * 0.5f < -MainBGTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0;
        }
        
        if (result.x + boxCollider.size.x * 0.5f > MainBGTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0;
        }
        
        if (result.y - boxCollider.size.y * 0.5f < -MainBGTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0;
        }
        
        if (result.y + boxCollider.size.y * 0.5f > MainBGTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0;
        }
        
        return moveVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
            enemy.OnCrash(this);
    }

    public void OnCrash(Enemy enemy)
    {
        
    }

    public void Fire()
    {
        GameObject go = Instantiate(Bullet);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Fire(OwnerSide.Player, FireTransform.position, FireTransform.right, bulletSpeed);
    }
}
