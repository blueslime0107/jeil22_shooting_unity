using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int dmg;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // gameObject�� collision��,  �� ���� ������Ʈ
        if(collision.gameObject.tag == "BulletBorder") // ź��迡 ���������
        {
            Destroy(gameObject); // �� �ڽ� ���� gameObject �� �� �ڽ�
        }
    }

    
}
