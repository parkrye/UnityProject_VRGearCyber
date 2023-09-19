using UnityEngine;

namespace PGR
{
    public class PlayerPokeVisible : MonoBehaviour
    {
        [SerializeField] GameObject visible;
        [SerializeField] bool isRight;

        void LateUpdate()
        {
            if(!visible.activeSelf && GameManager.Data.Player.HandMotion.IsPokeHandMotion(isRight))
                visible.SetActive(true);
            else if(visible.activeSelf && !GameManager.Data.Player.HandMotion.IsPokeHandMotion(isRight))
                visible.SetActive(false);
        }
    }

}