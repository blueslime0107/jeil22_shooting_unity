using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    [SerializeField] float m_force;
    [SerializeField] Vector3 m_offset;
    [HideInInspector] public bool shakeing;


    Quaternion m_originRot;
    void Start()
    {
        m_originRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeing){
            shakeing = !shakeing;
            StartCoroutine(ShakeCoroutine());
            Invoke("StopShake",0.2f);
        }
       
    }

    void StopShake(){
        StopAllCoroutines();
        StartCoroutine(Reset());
    }

    IEnumerator ShakeCoroutine(){
        Vector3 t_originEuler = transform.eulerAngles;
        while(true){
            float t_rotX = Random.Range(-m_offset.x, m_offset.x);
            float t_rotY = Random.Range(-m_offset.y, m_offset.y);
            float t_rotZ = Random.Range(-m_offset.z, m_offset.z);

            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX,t_rotY,t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);

            while(Quaternion.Angle(transform.rotation,t_rot) > 0.1f){
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Reset(){
        while(Quaternion.Angle(transform.rotation,m_originRot) > 0f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return null;
        }
        transform.rotation = m_originRot;
    }
}
