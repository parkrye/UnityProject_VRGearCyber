using UnityEngine;

namespace PID
{
    public static class MeleeCombatHelper
    {
        const float flankThreshold = 0.3f;
        public const int flankDamageMultiplier = 4; 
        public static void FlankJudgement(Transform attacker, Transform target, 
            float damage, Vector3 hitPoint, Vector3 hitNormal,
            System.Action<float, Vector3, Vector3, bool> flankAttempt)
        {
            if(attacker == null || target == null)
                return;
            Vector3 playerAttackDir = target.position - attacker.position;
            playerAttackDir.y = 0;
            playerAttackDir.Normalize();
            Vector3 enemyLookDir = target.transform.forward;
            if (Vector3.Dot(enemyLookDir, playerAttackDir) > flankThreshold)
            {
                Debug.Log($"Player has flanked {target.gameObject.name}");
                flankAttempt(damage, hitPoint, hitNormal, true);
            }
            flankAttempt(damage, hitPoint, hitNormal, false);
        }
        //Example Script for meleestrike, 
        //Could Insert Damage float variable to compute base damage based on the swinging velocity 
        //public static void TryFlank(Transform target, bool contestSuccess)
        //{
        //    if (contestSuccess)
        //    {
        //        IHitable hittable = target.GetComponent<IHitable>();
        //        //Insert Damage Here 
        //        hittable?.TakeDamage(0, Vector3.zero, Vector3.zero);
        //    }
        //    else
        //    {
        //        IHitable hittable = target.GetComponent<IHitable>();
        //        //Insert Damage Here 
        //        hittable?.TakeDamage(0, Vector3.zero, Vector3.zero);
        //    }
        //}

        //public void FindComponentThroughParent(GameObject gameObject)
        //{
        //    Transform currentTransform = gameObject.transform;

        //    while (currentTransform.parent != null)
        //    {
        //        currentTransform = currentTransform.parent;
        //    }

        //    return currentTransform.gameObject;
        //}
        //private GameObject FindComponentThroughChildren(GameObject obj)
        //{
        //    for (int i = 0; i < obj.transform.childCount; i++)
        //    {
        //        if ()
        //    }
    }

    //public static void SearchForComponent(this GameObject gameObject)
    //{
    //    if (gameObject == null)
    //    {
    //        return;
    //    }

    //    // Check if the current GameObject has the desired component
    //    Component component = gameObject.GetComponent(componentNameToSearchFor);

    //    if (component != null)
    //    {
    //        // Do something with the component, e.g., print its name
    //        Debug.Log("Found " + componentNameToSearchFor + " in GameObject: " + gameObject.name);
    //    }

    //    // Recursively search through children using the foreach loop
    //    foreach (Transform child in gameObject.transform)
    //    {
    //        SearchForComponent(child.gameObject);
    //    }

    //    // Recursively search through the parent
    //    if (gameObject.transform.parent != null)
    //    {
    //        SearchForComponent(gameObject.transform.parent.gameObject);
    //    }
    //}
    
}
    
