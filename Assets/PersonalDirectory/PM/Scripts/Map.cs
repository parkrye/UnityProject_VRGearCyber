using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace PM
{
    public class Map : MonoBehaviour
    {
        /// <summary>
        /// 맵의 최대가로 크기 세로 크기 저장 배열기준
        /// 배열에 가로 세로 크기 프리팹 순서를 랜덤으로 저장
        /// 방 프리팹을 사이즈 별로 만들어 놓고 랜덤으로 배열순으로 그 사이즈중 하나를 가져온다
        /// 룸종류 room0 {2,2}, room1{2,3}, room2{3,2}, room3{3,3}, room4{2,4}, room5{4,2}, room6{3,4}, room7{4,3}, room8{4,4}
        /// array 생성된 룸의 게임오브 젝트를 저장
        /// </summary>
        [SerializeField] int maxXSize;
        [SerializeField] int maxYSize;
        [SerializeField] int RoomMaxSize;
        enum arrow { up, down, left, right };

        [SerializeField] GameObject wall;
        [SerializeField] GameObject roomDoor;
        [SerializeField] GameObject passageTile;
        [SerializeField]
        GameObject[,] array;
        RoomData[,] roomData;

        [SerializeField] GameObject[] room0;
        [SerializeField] GameObject[] room1;
        [SerializeField] GameObject[] room2;
        [SerializeField] GameObject[] room3;
        [SerializeField] GameObject[] room4;
        [SerializeField] GameObject[] room5;
        [SerializeField] GameObject[] room6;
        [SerializeField] GameObject[] room7;
        [SerializeField] GameObject[] room8;
        [SerializeField] GameObject bossRoom;

        Transform rooms;
        Transform passages;
        Vector3[,] position;


        private void Start()
        {
            rooms = transform.GetChild(0);
            passages = transform.GetChild(1);

            PositionSetting();
            RandomPrefab();
            SpawnPrefab();
            EllerAlghorithm();
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
                    position[j, i] = new Vector3(i * 30, 0, -j * 30);
                }
            }
        }

        // 랜덤으로 프리팹을 받아 array에 저장 후 x,y 값을 저장
        private void RandomPrefab()
        {
            array = new GameObject[maxYSize, maxXSize];
            roomData = new RoomData[maxYSize, maxXSize];
            // 초기에는 x,y 2~4으로 설정
            for (int j = 0; j < maxYSize; j++)
            {
                for (int i = 0; i < maxXSize; i++)
                {
                    int x = Random.Range(2, RoomMaxSize + 1);
                    int z = Random.Range(2, RoomMaxSize + 1);
                    // 마지막 배열방에는 보스룸을 생성
                    if (j == maxYSize - 1 && i == maxXSize - 1)
                    {
                        x = 4;
                        z = 4;
                        array[j, i] = Instantiate(bossRoom, rooms);
                    }
                    else
                    {
                        if (x == 2 && z == 2)
                            array[j, i] = Instantiate(room0[Random.Range(0, 1)], rooms);
                        else if (x == 2 && z == 3)
                            array[j, i] = Instantiate(room1[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 2)
                            array[j, i] = Instantiate(room2[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 3)
                            array[j, i] = Instantiate(room3[Random.Range(0, 1)], rooms);
                        else if (x == 2 && z == 4)
                            array[j, i] = Instantiate(room4[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 2)
                            array[j, i] = Instantiate(room5[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 4)
                            array[j, i] = Instantiate(room6[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 3)
                            array[j, i] = Instantiate(room7[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 4)
                            array[j, i] = Instantiate(room8[Random.Range(0, 1)], rooms);
                    }
                    roomData[j, i] = array[j, i].GetComponent<RoomData>();
                    roomData[j, i].x = x;
                    roomData[j, i].z = z;
                    roomData[j, i].aggregate = maxXSize * j + i;
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

        private void EllerAlghorithm()
        {
            for (int j = 0; j < maxYSize; j++)
            {
                RightAggregater(j);
                DownAggregater(j);
            }
        }

        // array배열의 열을 매개변수로 받음
        private void RightAggregater(int j)
        {
            for (int i = 0; i < maxXSize - 1; i++)
            {
                // j가 roomData의 마지막 행이 아닐때
                if (j != roomData.GetLength(1) - 1)
                {
                    // 옆칸을 전염시킬 확률 지금은 2분의 1
                    if (Random.Range(0, 2) == 0)
                    {
                        // 크거나 작거나 같을때
                        if (roomData[j, i].aggregate < roomData[j, i + 1].aggregate)
                            roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                        else if (roomData[j, i].aggregate > roomData[j, i + 1].aggregate)
                            roomData[j, i].aggregate = roomData[j, i + 1].aggregate;
                        else
                            continue;
                        PassageRightCreate(j, i);
                    }
                }
                else
                {
                    // 보스방에 만약 이미 위로 연결 되어있으면 위로 연결된 방중 왼쪽으로 연결된지 확인 후 연결 안되어 있으면
                    // 위로 연결된 방중 하나를 왼쪽으로 연결
                    if (i == roomData.GetLength(0) - 2)
                    {
                        if (roomData[j, i + 1].up)
                        {
                            for (int m = 1; m <= roomData.GetLength(1); m++)
                            {
                                if (roomData[j - m, i + 1].left)
                                {
                                    break;
                                }
                                else if (!roomData[j - m, i + 1].up)
                                {
                                    PassageRightCreate(j - Random.Range(1, m + 1), i);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                            {
                                roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                                PassageRightCreate(j, i);
                            }
                        }
                    }
                    else
                    {
                        if (roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                        {
                            roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                            PassageRightCreate(j, i);
                        }
                    }
                }
            }
        }
        private void DownAggregater(int j)
        {
            if (j == roomData.GetLength(1) - 1)
                return;

            for (int i = 0; i < maxXSize; i++)
            {
                // i가 roomData 열의 마지막이 아닐때
                if (i != roomData.GetLength(0) - 1)
                {
                    Debug.Log(i + " " + (roomData.GetLength(0) - 1));
                    if (Random.Range(0, 2) == 0 || roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0 || roomData[j, i].aggregate != roomData[j, i - 1].aggregate)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                }

            }
            // 만약 행의 값이 다같을시 아래 통로를 안만들기 때문에 조건문으로 추가
            int sum = 0;
            for (int i = 0; i < maxXSize; i++)
            {
                if (roomData[j, i].down)
                    sum++;
                // 마지막 열의 값이 이전 값과 같고 같은 값의 아래통로가 없거나 룸값이 다음 배열의 값과 다르고 아래통로의 합이 0이면
                // 새로 아래로 통로 생성
                if (i == roomData.GetLength(0) - 1 || roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                {
                    if (sum == 0)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                    sum = 0;
                }
            }
        }
        /// <summary>
        /// 재귀 함수로 구현
        /// i, j가 array 배열을 순회
        /// j,i가 배열의 인덱스를 넘으면 return
        /// </summary>
        //private void PassageCreate(int m, int n)
        //{
        //    if (n + 1 < maxXSize && array[m, n].GetComponent<RoomData>().right == false)
        //    {
        //        PassageRightCreate(m, n);
        //        PassageCreate(m, n + 1);
        //    }
        //    if (m + 1 < maxYSize && array[m, n].GetComponent<RoomData>().down == false)
        //    {
        //        PassageDownCreate(m, n);
        //        PassageCreate(m + 1, n);
        //    }
        //}
        private void PassageRightCreate(int m, int n)
        {
            int x = 6;
            while (position[m, n].x + x < position[m, n + 1].x)
            {
                GameObject tile = Instantiate(passageTile, passages);
                tile.transform.position = position[m, n] + new Vector3(x, 0, 0);
                tile.GetComponent<Passage>().arrow = Passage.Arrow.right;
                x += 6;
            }
            roomData[m, n].right = true;
            roomData[m, n + 1].left = true;
        }
        private void PassageDownCreate(int m, int n)
        {
            int z = 6;
            while (position[m, n].z - z > position[m + 1, n].z)
            {
                GameObject tile = Instantiate(passageTile, passages);
                tile.transform.position = position[m, n] + new Vector3(0, 0, -z);
                tile.GetComponent<Passage>().arrow = Passage.Arrow.down;
                z += 6;
            }
            roomData[m, n].down = true;
            roomData[m + 1, n].up = true;
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
                    ArrowWallCreater(roomData.x, roomData.z, roomData.up, arrow.up, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.right, arrow.right, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.down, arrow.down, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.left, arrow.left, roomData.transform);
                }
            }
        }

        private void ArrowWallCreater(int x, int y, bool door, arrow arrow, Transform room)
        {
            GameObject leftWall = array[0, 0];
            GameObject rightWall = array[0, 0];
            switch (arrow)
            {
                case arrow.up:
                    if (door)
                        Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    for (int i = 1; i <= x * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
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
                        Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 270, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    for (int i = 1; i <= x * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
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
                        Instantiate(roomDoor, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.Euler(0, 180, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    for (int i = 1; i <= y * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room.GetChild(2));
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
                    if (door)
                        Instantiate(roomDoor, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    for (int i = 1; i <= y * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room.GetChild(2));
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

        //private void DoorConnectings()
        //{
        //    for (int j = 0; j < maxYSize; j++)
        //    {
        //        for (int i = 0; i < maxXSize; i++)
        //        {
        //            RoomData roomData = array[j, i].GetComponent<RoomData>();
                    
        //            if(roomData.up || roomData.down || roomData.right || roomData.left)
        //            {
        //                SyberDoor[] doors = array[j, i].GetComponentsInChildren<SyberDoor>();
        //                foreach(SyberDoor door in doors)
        //                {
        //                    if(door.arrow == SyberDoor.Arrow.up)
        //                        //door.connetingDoor = array[j-1, i];
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
