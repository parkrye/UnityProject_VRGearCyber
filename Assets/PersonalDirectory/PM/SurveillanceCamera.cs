using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace PM
{
    public class SurveillanceCamera : MonoBehaviour, IHitable, IHackable
    {
        [SerializeField] GameData.HackProgressState state;
        [SerializeField]
        GameObject[] securities;
        private float range;
        [SerializeField] int hp;
        [SerializeField] Transform SpotLight;
        Ray ray;
        private Vector3 lightPosition;
        private float angle;
        private float cos;
        private float sin;
        public GameObject player;

        private void Start()
        {
            lightPosition = SpotLight.transform.position;
            ray = new Ray(lightPosition, SpotLight.forward);
            StartCoroutine(RangeSetting());
            StartCoroutine(Checking());
        }

        public virtual void Hack()
        {
            StartCoroutine(WaitingHackResultRoutine());
        }

        public virtual void ChangeProgressState(GameData.HackProgressState value)
        {
            state = value;
        }

        public virtual void Failure()
        {
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);
            StartCoroutine(CallSecurity(hitData.point));
        }

        public virtual void Success()
        {
            StartCoroutine(Break());
        }

        public virtual IEnumerator WaitingHackResultRoutine()
        {
            yield return null;
           state = GameData.HackProgressState.Progress;
           yield return new WaitUntil(() => state != GameData.HackProgressState.Progress );
           switch(state)
           {
                case GameData.HackProgressState.Failure:
                    Failure();
                    break;
                case GameData.HackProgressState.Success:
                    Success();
                    break;
           }
        }
        IEnumerator RangeSetting()
        {
            angle = SpotLight.GetComponent<Light>().spotAngle;
            cos = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
            sin = Mathf.Sin(angle * 0.5f * Mathf.Deg2Rad);
            Ray ray = new Ray(lightPosition, (SpotLight.forward * cos + SpotLight.up * sin));
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);
            range = hitData.distance;
            yield return null;
        }
        //private void Update()
        //{
        //    Debug.DrawRay(lightPosition, SpotLight.forward*10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos + SpotLight.right*sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos - SpotLight.right * sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos + SpotLight.up * sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos - SpotLight.up * sin) * 10, Color.red);
        //}
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lightPosition, range);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(lightPosition, (player.transform.position - lightPosition));
        }
        IEnumerator Checking()
        {
            while (true)
            {
                Collider[] colliders = Physics.OverlapSphere(lightPosition, range);
                foreach (Collider collider in colliders)
                {
                    // 만약 콜라이더가 플레이어면 실행
                    if (collider.tag == "Player")
                    {
                        Vector3 dirTarget = (collider.transform.position - lightPosition).normalized;
                        if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                            continue;
                        // 다시 레이를쏴 카메라와 플레이어 사이에 장애물이 없으면 실행
                        Ray ray = new Ray(lightPosition, (collider.transform.position - lightPosition));
                        RaycastHit hitData;
                        Physics.Raycast(ray, out hitData);
                        Debug.Log(hitData.collider.tag);
                        if (hitData.collider.tag == "Player")
                        {
                            Debug.Log("player!!!!");
                            StartCoroutine(CallSecurity(collider.transform.position));
                        }
                    }
                    yield return null;
                }
                yield return new WaitUntil(() => GameManager.Data.TimeState != 1);
                yield return new WaitForSeconds(0.3f);
            }
        }

        IEnumerator CallSecurity(Vector3 destination)
        {
            foreach (GameObject guard in securities)
            {
                // 로봇의 위치를 추적하는 함수를 가져와서 플레이어의 위치를 변수로 값을 넘겨줌 guard.GetComponent<>
                yield return null;
            }
            yield return null;
        }

        public IEnumerator Break()
        {
            SpotLight.gameObject.SetActive(false);
            Destroy(this);
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }
    }
}

