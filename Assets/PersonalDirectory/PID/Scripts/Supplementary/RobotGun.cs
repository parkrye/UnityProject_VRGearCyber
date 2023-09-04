using PID;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class RobotGun : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] TrailRenderer bulletTrail; 
    int currentAmmo;
    public int CurrentAmmo => currentAmmo; 
    int maxAmmo;
    bool reloading;
    float nextFire = 0f;
    float attackRange;
    int attackDamage; 

    [SerializeField] float fireInterval;
    [SerializeField] float randomShotRadius;
    [SerializeField] float reloadInterval;
    WaitForSeconds reloadWaitInterval;
    LineRenderer debugLine;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] LayerMask targetMask;
    [SerializeField] string targetTag; 
    GuardEnemy gunOwner; 

    public bool Reloading { get => reloading; set => reloading = value; }

    private void Awake()
    {
        reloading = false; 
        debugLine = GetComponent<LineRenderer>();
        gunOwner = GetComponentInParent<GuardEnemy>(); 
        reloadWaitInterval = new WaitForSeconds(reloadInterval);
    }
    public void UponFire(Vector3 fireDir, float distance)
    {
        muzzleFlash.Play();
        //BulletTrail Works 
        currentAmmo--; 
    }
    Coroutine reloadCoroutine; 
    public void AttemptFire(Transform target)
    {
        if (Time.time < nextFire || reloading)
            return;
        if (currentAmmo <= 0)
            reloadCoroutine = StartCoroutine(ReloadRoutine()); 
            //Reload; 
        nextFire = Time.time + fireInterval;
        Fire(target); 
        //Animator Plays Fire 
    }
    Vector3 shotAttempt;
    RaycastHit hitAttempt;
    bool struck;
    GameObject appropriateObj; 
    public void Fire(Transform target)
    {
        struck = false; 
        gunOwner.anim.SetTrigger("GunFire");
        debugLine.SetPosition(0, muzzlePoint.position);
        //shotAttempt = FinalShotDir(muzzlePoint.positionempt.norma, playerBody.position, attackRange, randomShotRadius);
        shotAttempt = (target.position - muzzlePoint.position).normalized;
        debugLine.SetPosition(1, target.position);
        //RaycastHit[] queries;
        //queries = Physics.RaycastAll(muzzlePoint.position, shotAttempt, attackRange);

        //if (queries.Length > 0)
        //    return;
        if (Physics.Raycast(muzzlePoint.position, shotAttempt, out hitAttempt, attackRange))
        {
            if (hitAttempt.collider.gameObject.tag != targetTag)
            {
                //obstacle should take damage; 
                IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
                hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
            }
            else
            {
                IHitable hitable = hitAttempt.collider.GetComponent<IHitable>(); 
                if (hitable != null)
                {
                    hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
                    return;
                }

                appropriateObj = SearchForComponentInParent(hitAttempt.collider.gameObject); 
                if (appropriateObj != null)
                {
                    GiveDamage(appropriateObj);
                    appropriateObj = null; 
                    return; 
                }
                else
                {
                    appropriateObj = SearchForComponentInChildren(hitAttempt.collider.gameObject);
                    if (appropriateObj != null)
                    {
                        GiveDamage(appropriateObj); 
                        appropriateObj = null;
                        return; 
                    }
                }
            }
        }
    }

    public void GiveDamage(GameObject obj)
    {
        IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
        hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
    }

    public GameObject SearchForComponentInParent(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return null; 
        }

        // Check if the current GameObject has the desired component
        IHitable hitable = gameObject.GetComponent<IHitable>();

        if (hitable != null)
        {
            return gameObject; 
        }

        // Recursively search through the parent
        if (gameObject.transform.parent != null)
        {
            return SearchForComponentInParent(gameObject.transform.parent.gameObject);
        }
        else
        {
            return null;
        }
    }

    public GameObject SearchForComponentInChildren(GameObject gameObject)
    {
        GameObject target = null; 
        foreach (Transform child in gameObject.transform)
        {
            IHitable hitable = child.GetComponent<IHitable>();
            if (hitable != null)
            {
                target = child.gameObject;
                break;
            }
        }
        return target; 
    }
    IEnumerator ReloadRoutine()
    {
        reloading = true;
        yield return reloadWaitInterval; 
        reloading = false;
    }

    IEnumerator TrailRendererRoutine()
    {
        yield return null;
    }

    public void SyncStatData(EnemyStat stat)
    {
        attackDamage = stat.attackDamage;
        attackRange = stat.attackRange;
        maxAmmo = stat.maxAmmo;
        currentAmmo = maxAmmo; 
    }
}
