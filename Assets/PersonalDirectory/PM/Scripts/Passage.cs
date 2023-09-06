using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class Passage : MonoBehaviour
{
    // 타일 방향에 따라 벽을 세우기 위해 enum 사용
    public enum Arrow { right, down};
    [SerializeField] GameObject passageCeiling;
    [SerializeField] GameObject harfPassageCeiling;
    [SerializeField] GameObject passageWall;
    [SerializeField] GameObject harfPassageWall;
    public Arrow arrow;
    // 타일이 네 방향으로 레이를 쏴서 조건에 따라 통로벽을 생성
    Ray rightRay;
    Ray leftRay;
    Ray upRay;
    Ray downRay;

    public int right;
    public int left;
    public int up;
    public int down;
    private void Start()
    {
        rightRay = new Ray(transform.position + new Vector3(3, -0.2f,0), transform.up);
        leftRay = new Ray(transform.position + new Vector3(-3, -0.2f, 0), transform.up);
        upRay = new Ray(transform.position + new Vector3(0, -0.2f, 3), transform.up);
        downRay = new Ray(transform.position + new Vector3(0, -0.2f, -3), transform.up);
        RayCast();
        CreateWall();
    }
    //private void Update()
    //{
    //    Debug.DrawRay(transform.position + new Vector3(3, -0.2f, 0), transform.up);
    //    Debug.DrawRay(transform.position + new Vector3(-3, -0.2f, 0), transform.up);
    //    Debug.DrawRay(transform.position + new Vector3(0, -0.2f, 3), transform.up);
    //    Debug.DrawRay(transform.position + new Vector3(0, -0.2f, -3), transform.up);
    //}
    private void RayCast()
    {
        right = Physics.Raycast(rightRay, 1) ? 1 : 0;
        left = Physics.Raycast(leftRay, 1) ? 1 : 0;
        up = Physics.Raycast(upRay, 1) ? 1 : 0;
        down = Physics.Raycast(downRay, 1) ? 1 : 0;
    }

    private void CreateWall()
    {
        int sum = right + left + up + down;
        if (sum == 0 || sum == 1)
        {
            if(arrow == Arrow.right)
            {
                Instantiate(passageWall, transform.position, Quaternion.Euler(0, 90, 0), transform);
                Instantiate(passageCeiling, transform.position, Quaternion.Euler(0, 90, 0), transform);
            }
            else if(arrow == Arrow.down)
            {
                Instantiate(passageWall, transform.position, Quaternion.Euler(0, 0, 0), transform);
                Instantiate(passageCeiling, transform.position, Quaternion.Euler(0, 0, 0), transform);
            }
        }
        else if (sum == 3)
        {
            if(right == 0)
            {
                Instantiate(harfPassageWall, transform.position + new Vector3(1.5f, 0, 0), Quaternion.Euler(0, 90, 0), transform);
                Instantiate(harfPassageCeiling, transform.position + new Vector3(1.5f, 0, 0), Quaternion.Euler(0, 90, 0), transform);
            }
            else if (left == 0)
            {
                Instantiate(harfPassageWall, transform.position + new Vector3(-1.5f, 0, 0), Quaternion.Euler(0, 90, 0), transform);
                Instantiate(harfPassageCeiling, transform.position + new Vector3(-1.5f, 0, 0), Quaternion.Euler(0, 90, 0), transform);
            }
            else if (up == 0)
            {
                Instantiate(harfPassageWall, transform).transform.position = transform.position + new Vector3(0, 0, 1.5f);
                Instantiate(harfPassageCeiling, transform).transform.position = transform.position + new Vector3(0, 0, 1.5f);
            }
            else if (down == 0)
            {
                Instantiate(harfPassageWall, transform).transform.position = transform.position + new Vector3(0, 0, -1.5f);
                Instantiate(harfPassageCeiling, transform).transform.position = transform.position + new Vector3(0, 0, -1.5f);
            }
        }
    }
}
