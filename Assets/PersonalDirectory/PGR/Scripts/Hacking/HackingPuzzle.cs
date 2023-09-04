using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class HackingPuzzle : MonoBehaviour
    {
        [SerializeField] int pairCount, fixedPointPerPairCount;
        [SerializeField] Dictionary<FixedPoint, bool> lightOnDict;
        [SerializeField] int lightCount, lightTotal;
        [SerializeField] float puzzleScale;
        [SerializeField] IHackable target;

        public void InitialPuzzle(IHackable _target, int _pairCount, int _fixedPointPerPairCount)
        {
            GameManager.Data.TimeState = 0.01f;
            target = _target;
            pairCount = _pairCount;
            fixedPointPerPairCount = _fixedPointPerPairCount;

            lightOnDict = new Dictionary<FixedPoint, bool>();
            Vector3 randomPosition;

            for (int i = 0; i < pairCount; i++)
            {
                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                MovablePoint insMP = GameManager.Resource.Instantiate<MovablePoint>("Hacking/MovablePoint", randomPosition, Quaternion.identity, transform);
                
                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                LightFixedPoint insLFP1 = GameManager.Resource.Instantiate<LightFixedPoint>("Hacking/LightFixedPoint", randomPosition, Quaternion.identity, transform);
                insLFP1.SetLine(insMP.transform);
                
                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                LightFixedPoint insLFP2 = GameManager.Resource.Instantiate<LightFixedPoint>("Hacking/LightFixedPoint", randomPosition, Quaternion.identity, transform);
                insLFP2.SetLine(insMP.transform);

                Vector3 route1 = insMP.transform.position - insLFP1.transform.position;
                Vector3 route2 = insMP.transform.position - insLFP2.transform.position;

                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                insMP.transform.position = randomPosition;

                for (int j = 0; j < fixedPointPerPairCount; j++)
                {
                    float distacne = Random.Range(0.1f, 0.9f);
                    FixedPoint insFP;
                    if (j % 2 == 0)
                    {
                        insFP = GameManager.Resource.Instantiate<FixedPoint>("Hacking/FixedPoint", insLFP1.transform.position + route1 * distacne, Quaternion.identity, transform);
                    }
                    else
                    {
                        insFP = GameManager.Resource.Instantiate<FixedPoint>("Hacking/FixedPoint", insLFP2.transform.position + route2 * distacne, Quaternion.identity, transform);
                    }
                    insFP.SetLight(this, insMP.transform, insLFP1.transform, insLFP2.transform);
                    lightOnDict.Add(insFP, false);
                    lightTotal++;
                }
            }

            //Time.timeScale = 0.001f;
        }

        public void TurnOn(FixedPoint fixedPoint)
        {
            if (lightOnDict[fixedPoint])
                return;
            lightOnDict[fixedPoint] = true;
            lightCount++;

            if(lightCount == lightTotal)
            {
                //StopPuzzle(GameData.HackProgressState.Success);
            }
        }

        public void TurnOff(FixedPoint fixedPoint)
        {
            if (!lightOnDict[fixedPoint])
                return;
            lightOnDict[fixedPoint] = false;
            lightCount--;
        }

        public void StopPuzzle(GameData.HackProgressState result)
        {
            target.ChangeProgressState(result);
            GameManager.Data.TimeState = 1f;
            GameManager.Resource.Destroy(gameObject);
        }
    }

}