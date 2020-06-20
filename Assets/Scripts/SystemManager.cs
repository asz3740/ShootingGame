using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;
    
    public static SystemManager Instance
    {
        get { return instance; }

    }
    
    [SerializeField] 
    private Player player;

    public Player Player
    {
        get { return player; }
    }

    void Awake()
    {
        // 한 개의 게임오브젝트만 존재 
        if (Instance != null)
        {
            Debug.LogError("SystemManager error! Singletone error!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
