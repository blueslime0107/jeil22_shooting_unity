using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuide : MonoBehaviour
{
    bool enemyTouched = false;
    bool bossTouched = false;

    SpriteRenderer render;

    void Awake(){
        render = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision) // ���� ������Ʈ���� ����
    {
        //Debug.Log("colli");
        if (collision.gameObject.tag == "Enemy"){
            enemyTouched = true;
            changeColor();
        }
        if (collision.gameObject.tag == "Boss"){
            bossTouched = true;
            changeColor();
        }
    }

    void OnTriggerExit2D(Collider2D collision) // ���� ������Ʈ���� ����
    {
        if (collision.gameObject.tag == "Enemy"){
            enemyTouched = false;
            changeColor();
        }
        if (collision.gameObject.tag == "Boss"){
            bossTouched = false;
            changeColor();
        }
    }

    void changeColor(){
        if(enemyTouched || bossTouched){
            render.color = new Color32(255,255,255,255);
        }
        else{         
            render.color = new Color32(85,85,85,255);
        }
    }
}
