using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerDirectInteractor : XRDirectInteractor
    {
        [Header("Player Direct Interactor Parameters")]
        [SerializeField] List<IXRInteractable> customSortedValidTargets;

        public override void GetValidTargets(List<IXRInteractable> targets)
        {
            targets.Clear();

            if (!isActiveAndEnabled)
                return;

            var filter = targetFilter;
            if (filter != null && filter.canProcess)
                filter.Process(this, customSortedValidTargets, targets);
            else
            {
                customSortedValidTargets = new List<IXRInteractable>();
                PriorityQueue<IXRInteractable, ulong> pq = new();
                foreach(var target in unsortedValidTargets)
                {
                    ulong priority = 5;
                    if (target is PlayerInteractable)
                        priority = (target as PlayerInteractable).Priority;

                    priority *= (ulong)(Vector3.SqrMagnitude(target.transform.position - transform.position));

                    pq.Enqueue(target, priority);
                }

                while(pq.Count > 0)
                    customSortedValidTargets.Add(pq.Dequeue());

                targets.AddRange(customSortedValidTargets);
            }
        }
    }
}