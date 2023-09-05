using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    /// <summary>
    /// 맵의 최대가로 크기 세로 크기 저장 배열기준
    /// 배열에 가로 세로 크기 프리팹 순서를 랜덤으로 저장
    /// 방 프리팹을 사이즈 별로 만들어 놓고 랜덤으로 배열순으로 그 사이즈중 하나를 가져온다
    /// 
    /// 
    /// </summary>
    [SerializeField] int roomMaxSize;
    [SerializeField] int roomMinSize;

    //test
    [SerializeField] GameObject wall;
    [SerializeField] GameObject door;
    [SerializeField] GameObject smallWall;
    [SerializeField] GameObject smallTile;
    [SerializeField] Vector3[] position;
    [SerializeField]
    GameObject[] array = new GameObject[2];
    //room1
    [SerializeField] GameObject room;

    //room2
    [SerializeField] GameObject room2;
    private void Start()
    {
        array[0] = room;
        array[1] = room2;
        position = new Vector3[10];
        position[0] = new Vector3(0, 0, 0);
        position[1] = new Vector3(24, 0, 0);
        int x = 3;
        Instantiate(room).transform.position = position[0];
        Instantiate(room2).transform.position = position[1];
        Debug.Log(room.transform.position);
        while (true)
        {
            Instantiate(smallTile, room.transform.position+ new Vector3(x,0,0), Quaternion.identity);
            x += 3;
            if (x >= position[1].x)
            {
                break;
            }
        }
    }
}
