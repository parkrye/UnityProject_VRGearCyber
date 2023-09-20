using UnityEngine;

namespace PGR
{
    public class EndSceneDirection : MonoBehaviour
    {
        public void TryAgainButton()
        {
            GameManager.Data.DestroyPlayer();
            GameManager.Scene.LoadScene("PGR_ApartmentScene");
        }

        public void QuitGameButton()
        {
            Application.Quit();
        }
    }

}