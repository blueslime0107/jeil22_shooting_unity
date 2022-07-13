using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float curTime = 0f;
    public int damageing = 0;
    public bool damageActive = true;
    public float maxTime;

    void Update(){
        curTime += Time.deltaTime;
        if(curTime>maxTime){
            curTime = 0;
            damageing += 1;
            damageActive = true;
            
            
        }
        if(curTime>maxTime/2){
            damageActive = false;
        }

        if(damageing > 4){
            damageing = 0;
            damageActive = true;
            gameObject.SetActive(false);
        }
         
    }

    void OnDisable() {
        curTime = 0f;
        damageing = 0;
        damageActive = false;
    }

    void OnTriggerStay2D(Collider2D collision) {
        if(damageActive){
            if(collision.gameObject.tag == "EBullet"){
                Destroy(collision.gameObject);
            }
            if(collision.gameObject.tag == "Boss"){
                Boss objectBoss = collision.gameObject.GetComponent<Boss>();
                objectBoss.OnHit(5);
            }
            if(collision.gameObject.tag == "Enemy"){
                Enemy objectEnemy = collision.gameObject.GetComponent<Enemy>();
                objectEnemy.OnHit(5);
            }
            
        }
        
        
    }

}
