using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    /// <summary>
    /// 맵의 최대가로 크기 세로 크기 저장 배열기준
    /// 배열에 가로 세로 크기 프리팹 순서를 랜덤으로 저장
    /// 방 프리팹을 사이즈 별로 만들어 놓고 랜덤으로 배열순으로 그 사이즈중 하나를 가져온다
    /// 룸종류 room0 {2,2}, room1{3,2}, room2{2,3}, room3{3,3}
    /// array 생성된 룸의 게임으브 젝트를 저장
    /// 
    /// 순서
    /// maxXSize, maxYSize
    /// Array 크기 지정
    /// position 지정
    /// 랜덤 프리팹 생성 Array에 저장
    /// 프리팹들 사이에 길 생성 , 바닥 , 옆벽, 천장
    /// 프리팹들 벽 생성
    /// </summary>
    [SerializeField] int maxXSize;
    [SerializeField] int maxYSize;
    [SerializeField] int RoomMaxSize;
    int x;
    int y;
    enum arrow { up, down, left, right };

    [SerializeField] GameObject wall;
    [SerializeField] GameObject roomDoor;
    [SerializeField] GameObject passageTile;
    [SerializeField]
    GameObject[,] array;
    Vector3[,] position;

    [SerializeField] GameObject[] room0;
    [SerializeField] GameObject[] room1;
    [SerializeField] GameObject[] room2;
    [SerializeField] GameObject[] room3;

    private void Start()
    {
        PositionSetting();
        RandomPrefab();
        SpawnPrefab();
        PassageCreate(0, 0);
        WallCreate();
    }

    // 프리팹을 생성할 위치를 지정
    private void PositionSetting()
    {
        position = new Vector3[maxXSize, maxYSize];
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                position[j,i] = new Vector3(i * 30, 0, -j * 30);
            }
        }
    }

    // 랜덤으로 프리팹을 받아 array에 저장 후 x,y 값을 저장
    private void RandomPrefab()
    {
        array = new GameObject[maxYSize, maxXSize];
        // 초기에는 x,y 2~3으로 설정
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                x = Random.Range(2, RoomMaxSize+1);
                y = Random.Range(2, RoomMaxSize+1);
                if (x == 2 && y == 2)
                    array[j, i] = Instantiate(room0[Random.Range(0, 1)], transform);
                else if(x == 2 && y == 3)
                    array[j, i] = Instantiate(room1[Random.Range(0, 1)], transform);
                else if(x == 3 && y == 2)
                    array[j, i] = Instantiate(room2[Random.Range(0, 1)], transform);
                else if(x == 3 && y == 3)
                    array[j, i] = Instantiate(room3[Random.Range(0, 1)], transform);

                array[j, i].GetComponent<RoomData>().x = x;
                array[j, i].GetComponent<RoomData>().y = y;
            }
        }
    }

    // 프리팹을 position 위치에 생성
    private void SpawnPrefab()
    {
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                array[j, i].transform.position = position[j, i] + new Vector3(0, 0.1f, 0);
            }
        }
    }

    /// <summary>
    /// 재귀 함수로 구현
    /// i, j가 array 배열을 순회
    /// j,i가 배열의 인덱스를 넘으면 return
    /// </summary>
    private void PassageCreate(int m, int n)
    {
        if (n + 1 < maxXSize && array[m, n].GetComponent<RoomData>().right == false)
        {
            PassageRightCreate(m, n);
            PassageCreate(m, n + 1);
        }
        if (m+1 < maxYSize && array[m, n].GetComponent<RoomData>().down == false)
        {
            PassageDownCreate(m, n);
            PassageCreate(m + 1, n);
        }
    }
    private void PassageRightCreate(int m, int n)
    {
        int x = 6;
        while (position[m, n].x +x < position[m, n+1].x)
        {
            GameObject tile = Instantiate(passageTile);
            tile.transform.position = position[m, n] + new Vector3(x,0,0);
            tile.GetComponent<Passage>().arrow = Passage.Arrow.right;
            x += 6;
        }
        array[m, n].GetComponent<RoomData>().right = true;
        array[m, n+1].GetComponent<RoomData>().left = true;
    }
    private void PassageDownCreate(int m, int n)
    {
        int z = 6;
        while (position[m, n].z - z > position[m+1, n].z)
        {
            GameObject tile = Instantiate(passageTile);
            tile.transform.position = position[m, n] + new Vector3(0, 0, -z);
            tile.GetComponent<Passage>().arrow = Passage.Arrow.down;
            z += 6;
        }
        array[m, n].GetComponent<RoomData>().down = true;
        array[m+1, n].GetComponent <RoomData>().up = true;
    }
    
    // 배열에 저장된 게임오브젝트를 순회하며 RoomData 스크립트를 확인하여 통로가 없는 방향에는 그냥 벽 있는 방향에는 문과 벽을 사이즈에 맞게 생성
    // 통로가 있냐 없냐에 따라 다르게 실행 하며 있으면 
    private void WallCreate()
    {
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                RoomData roomData = array[j, i].GetComponent<RoomData>();
                ArrowWallCreater(roomData.x, roomData.y, roomData.up, arrow.up, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.right, arrow.right, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.down, arrow.down, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.left, arrow.left, roomData.transform);
            }
        }
    }

    private void ArrowWallCreater(int x, int y, bool door, arrow arrow, Transform room)
    {
        GameObject leftWall = new GameObject();
        GameObject rightWall = new GameObject();
        switch (arrow)
        {
            case arrow.up:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                for (int i = 1; i <= x * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                }
                if (x % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(1.5f, 0, 0);
                    leftWall.transform.position += new Vector3(1.5f, 0, 0);
                }
                break;
            case arrow.down:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                for (int i = 1; i <= x * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                }
                if (x % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(1.5f, 0, 0);
                    leftWall.transform.position += new Vector3(1.5f, 0, 0);
                }
                break;
            case arrow.right:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                for (int i = 1; i <= y * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room);
                }
                if (y % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(0, 0, 1.5f);
                    leftWall.transform.position += new Vector3(0, 0, 1.5f);
                }
                break;
            case arrow.left:
                if(door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                for (int i = 1; i <= y * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room);
                }
                if (y % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(0, 0, 1.5f);
                    leftWall.transform.position += new Vector3(0, 0, 1.5f);
                }
                break;
        }
    }
    private void RightWallCreater(int x, int y)
    {

    }
    private void LeftWallCreater(int x, int y)
    {

    }
}
