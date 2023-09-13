using System.Collections;
using UnityEngine;

namespace PGR
{
    public class ElevatorDirection : SceneMover
    {
        [SerializeField] GameObject LDoor, RDoor;
        [SerializeField] AudioSource ringAudio;

        void Start()
        {
            StartCoroutine(Routine1());
        }

        IEnumerator Routine1()
        {
            for(int i = 0; i < 100; i++)
            {
                LDoor.transform.Translate(-LDoor.transform.right * 0.5f * 0.01f);
                RDoor.transform.Translate(RDoor.transform.right * 0.5f * 0.01f);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1f);
            ringAudio.Play();
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.enabled);
            GameManager.Data.Player.Display.ModifyText("Good Luck");
            yield return new WaitForSeconds(3f);
            GameManager.Data.Player.Display.ModifyText("");

            MoveScene(0);
        }
    }

}