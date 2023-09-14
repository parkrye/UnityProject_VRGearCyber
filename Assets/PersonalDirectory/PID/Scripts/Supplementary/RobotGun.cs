using PID;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace PID
{
    public class RobotGun : MonoBehaviour
    {
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] TrailRenderer bulletTrail;
        int currentAmmo;
        public int CurrentAmmo => currentAmmo;
        bool outofAmmo;
        public bool OutOfAmmo => outofAmmo;
        int maxAmmo;
        bool reloading;
        float nextFire = 0f;
        float attackRange;
        int attackDamage;

        [SerializeField] float fireInterval;
        [SerializeField] float missChance;
        [SerializeField] float reloadInterval;
        WaitForSeconds reloadWaitInterval;
        [SerializeField] Transform muzzlePoint;
        [SerializeField] LayerMask targetMask;
        [SerializeField] string targetTag;
        GuardEnemy gunOwner;

        //Debug 

        public bool Reloading { get => reloading; set => reloading = value; }

        private void Awake()
        {
            reloading = false;
            outofAmmo = false; 
            gunOwner = GetComponentInParent<GuardEnemy>();
            reloadWaitInterval = new WaitForSeconds(reloadInterval);
        }
        public void UponFire(Vector3 fireDir, float distance)
        {
            muzzleFlash.Play();
            currentAmmo--;
        }
        Coroutine reloadCoroutine;
        
        public void AttemptFire(Transform target)
        {
            if (Time.time < nextFire || reloading || outofAmmo)
                return;
            if (currentAmmo <= 0)
            {
                outofAmmo = true;
                //reloadCoroutine = StartCoroutine(ReloadRoutine());
                return; 
            }
            //Reload; 
            nextFire = Time.time + fireInterval;
            Fire(target);
            //Animator Plays Fire 
        }
        Vector3 shotAttempt;
        RaycastHit hitAttempt;
        GameObject appropriateObj;
        const float trailDestroyTime = 1.5f; 
        public void Fire(Transform target)
        {
            
            gunOwner.anim.SetTrigger("GunFire");
            currentAmmo--;
            muzzleFlash.Play(); 
            //debugLine.SetPosition(0, muzzlePoint.position);
            shotAttempt = RobotHelper.FinalShotPoint(target.position, attackRange, missChance);
            shotAttempt = (shotAttempt - muzzlePoint.position).normalized;
            //RaycastHit[] queries;
            //queries = Physics.RaycastAll(muzzlePoint.position, shotAttempt, attackRange);
            //if (queries.Length > 0)
            //    return;
            TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, muzzlePoint.position, Quaternion.identity, true);
            GameManager.Resource.Destroy(trail, trailDestroyTime); 
            if (Physics.Raycast(muzzlePoint.position, shotAttempt, out hitAttempt, attackRange))
            {
                if (hitAttempt.collider.gameObject.tag != targetTag)
                {
                    //obstacle should take damage; 
                    //debugLine.SetPosition(1, hitAttempt.point);
                    IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
                    hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
                    StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, hitAttempt.point));
                    return; 
                }
                else
                {
                    IHitable playerDirect = GameManager.Data.Player.Data.GetComponent<IHitable>();
                    playerDirect?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
                    StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, hitAttempt.point));
                    return; 
                }
            }
            else 
            {
                //Trail Renderer effect for the nonHits 
                StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, shotAttempt * attackRange)); 
                return; 
            }
        }

        public void Reload()
        {
            if (reloading || !outofAmmo)
                return; 
            reloadCoroutine = StartCoroutine(ReloadRoutine());
        }

        public void GiveDamage(GameObject obj)
        {
            IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
            hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
        }
       
        IEnumerator ReloadRoutine()
        {
            reloading = true;
            yield return new WaitForEndOfFrame(); 
            gunOwner.anim.SetTrigger("Reload"); 
            yield return reloadWaitInterval;
            currentAmmo = maxAmmo;
            outofAmmo = false; 
            reloading = false;
        }
        const float deltaDistThreshold = 1f; 
        IEnumerator TrailRendererRoutine(TrailRenderer trail, Vector3 startPos, Vector3 endPos)
        {
            float deltaDist = Vector3.SqrMagnitude(endPos - startPos);
            trail.Clear();
            while (deltaDist > deltaDistThreshold)
            {
                trail.transform.position = Vector3.Lerp(trail.transform.position, endPos, .275f);
                deltaDist = Vector3.SqrMagnitude(endPos - startPos);
                yield return null;
            }
            trail.Clear();

        }
        public void SyncStatData(EnemyStat stat)
        {
            attackDamage = stat.attackDamage;
            attackRange = stat.attackRange;
            missChance = stat.missChance;
            maxAmmo = stat.maxAmmo;
            currentAmmo = maxAmmo;
        }
    }
}
#region DEPRECATED 
//IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
//if (hitable != null)
//{
//    hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
//    StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, hitAttempt.point));
//    //debugLine.SetPosition(1, hitAttempt.point);
//    return;
//}

//appropriateObj = SearchForComponentInParent(hitAttempt.collider.gameObject);
//if (appropriateObj != null)
//{
//    GiveDamage(appropriateObj);
//    StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, hitAttempt.point));
//    //debugLine.SetPosition(1, hitAttempt.point);
//    appropriateObj = null;
//    return;
//}
//else
//{
//    appropriateObj = SearchForComponentInChildren(hitAttempt.collider.gameObject);
//    if (appropriateObj != null)
//    {
//        GiveDamage(appropriateObj);
//        StartCoroutine(TrailRendererRoutine(trail, muzzlePoint.position, hitAttempt.point));
//        //debugLine.SetPosition(1, hitAttempt.point);
//        appropriateObj = null;
//        return;
//    }
//}
//public GameObject SearchForComponentInParent(GameObject gameObject)
//{
//    if (gameObject == null)
//    {
//        return null;
//    }
//    // Check if the current GameObject has the desired component
//    IHitable hitable = gameObject.GetComponent<IHitable>();
//    if (hitable != null)
//    {
//        return gameObject;
//    }
//    // Recursively search through the parent
//    if (gameObject.transform.parent != null)
//    {
//        return SearchForComponentInParent(gameObject.transform.parent.gameObject);
//    }
//    else
//    {
//        return null;
//    }
//}
//public GameObject SearchForComponentInChildren(GameObject gameObject)
//{
//    GameObject target = null;
//    foreach (Transform child in gameObject.transform)
//    {
//        IHitable hitable = child.GetComponent<IHitable>();
//        if (hitable != null)
//        {
//            target = child.gameObject;
//            break;
//        }
//    }
//    return target;
//}
#endregion