using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PGR
{
    public class HackingPuzzle : MonoBehaviour
    {
        [SerializeField] int pairCount, fixedPointPerPairCount;
        [SerializeField] Dictionary<FixedPoint, bool> lightOnDict;
        [SerializeField] int lightCount, lightTotal;
        [SerializeField] float puzzleScale;
        [SerializeField] IHackable target;
        [SerializeField] XRExclusiveSocketInteractor socketInteractor;
        [SerializeField] CableObject cable;
        public UnityEvent SelfDestroy;

        public void InitialPuzzle(IHackable _target, XRExclusiveSocketInteractor _socketInteractor, CableObject _cable, int _pairCount, int _fixedPointPerPairCount)
        {
            target = _target;
            socketInteractor = _socketInteractor;
            cable = _cable;
            pairCount = _pairCount;
            fixedPointPerPairCount = _fixedPointPerPairCount;

            lightOnDict = new Dictionary<FixedPoint, bool>();
            Vector3 randomPosition;

            for (int i = 0; i < pairCount; i++)
            {
                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                MovablePoint insMP = GameManager.Resource.Instantiate<MovablePoint>("Hacking/MovablePoint", randomPosition, Quaternion.identity, transform);
                SelfDestroy.AddListener(insMP.SelfDestroy);

                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                LightFixedPoint insLFP1 = GameManager.Resource.Instantiate<LightFixedPoint>("Hacking/LightFixedPoint", randomPosition, Quaternion.identity, transform);
                SelfDestroy.AddListener(insLFP1.SelfDestroy);
                insLFP1.SetLine(insMP.transform);
                
                randomPosition = transform.position + new Vector3(Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale), Random.Range(-puzzleScale, puzzleScale));
                LightFixedPoint insLFP2 = GameManager.Resource.Instantiate<LightFixedPoint>("Hacking/LightFixedPoint", randomPosition, Quaternion.identity, transform);
                SelfDestroy.AddListener(insLFP2.SelfDestroy);
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
                    SelfDestroy.AddListener(insFP.SelfDestroy);
                    insFP.SetLight(this, insMP.transform, insLFP1.transform, insLFP2.transform);
                    lightOnDict.Add(insFP, false);
                    lightTotal++;
                }
            }

            GameManager.Data.TimeScale = 0.01f;
        }

        public void TurnOn(FixedPoint fixedPoint)
        {
            if (lightOnDict[fixedPoint])
                return;
            lightOnDict[fixedPoint] = true;
            lightCount++;

            if(lightCount == lightTotal)
            {
                StopPuzzle(GameData.HackProgressState.Success);
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
            GameManager.Data.TimeScale = 1f;
            target.ChangeProgressState(result);
            socketInteractor.ChangeSocketUnusable();
            cable.FixExit();
            SelfDestroy?.Invoke();
            GameManager.Resource.Destroy(gameObject);
        }
    }

}