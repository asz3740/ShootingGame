using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

// BGScrollData클래스 변수들을 인스팩터창에 노출
[System.Serializable]

// 배경을 움직이는데 필요한 데이터
public class BGScrollData
{
    public Renderer Renderer; // Renderer컴포넌트와 연결
    public float speed; // 움직이는 속도
    public float offsetX; // 텍스처를 움직이기 위한 변수
}
public class BGScroller : MonoBehaviour
{
    [SerializeField] 
    private BGScrollData[] ScrollDatas;
    void Start()
    {
        
    }
    
    void Update()
    {
        UpdateScroll();
    }

    // 모든 배경에 SetTextureOffset메소드를 호출해 주는 메소드
    void UpdateScroll()
    {
        for (int i = 0; i < ScrollDatas.Length; i++)
        {
            SetTextureOffset(ScrollDatas[i]);
        }
    }

    // Offset를 변경해 주는 메소드
    void SetTextureOffset(BGScrollData scrollData)
    {
        // 이동량 계산 
        scrollData.offsetX += (float)(scrollData.speed) * Time.deltaTime;
        
        // 오버플로 방지
        if (scrollData.offsetX > 1)
            scrollData.offsetX = scrollData.offsetX % 1.0f;
        
        // Vector2에 이동량 할당
        Vector2 Offset = new Vector2(scrollData.offsetX, 0);
        
        // 배경 오프셋 설정
        scrollData.Renderer.material.SetTextureOffset("_MainTex", Offset);
    }
}
