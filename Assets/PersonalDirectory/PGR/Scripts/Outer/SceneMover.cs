using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class SceneMover : MonoBehaviour
    {
        [SerializeField] string SceneName;
        public void MoveScene(int locationNum)
        {
            GameManager.Scene.LoadScene(SceneName);
            PlayerPrefs.SetInt("Load Location", locationNum);
        }
    }

}