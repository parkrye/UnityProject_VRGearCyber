using PID;
using System.Collections;
using System.Collections.Generic;
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
    public void Fire(Transform target)
    {
        gunOwner.anim.SetTrigger("GunFire");
        debugLine.SetPosition(0, muzzlePoint.position);
        //shotAttempt = FinalShotDir(muzzlePoint.position, playerBody.position, attackRange, randomShotRadius);
        shotAttempt = (target.position - muzzlePoint.position).normalized;
        debugLine.SetPosition(1, target.position);
        if (Physics.Raycast(muzzlePoint.position, shotAttempt, out hitAttempt, attackRange))
        {
            IHitable hitable = hitAttempt.collider.GetComponent<IHitable>();
            hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal);
        }
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
