using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable(){
        Invoke("Disable", 0.5f);
    }

    void Disable(){
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void StartExplosion(string target)
    {
        anim.SetTrigger("OnExplosion");
        transform.localScale = Vector3.one * 2f;
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.name == "Enemy_S(Clone)"){
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.OnHit(1);
        }
        if(collision.gameObject.name == "Small(Clone)") // ź��迡 ���������
        {
            Destroy(collision.gameObject); // �� �ڽ� ���� gameObject �� �� �ڽ�
        }
        
    }

}
