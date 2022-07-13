using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int health;
    public Sprite[] sprites;

    public GameObject bulletA;
    public GameObject bulletB;
    public float maxSD;
    public float curSD;
    public GameObject player;
    public GameManager gameManager;
    
    SpriteRenderer spriteRenderer;
    Player playerObject;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Start(){
        playerObject = player.GetComponent<Player>();
    }
  
    public void OnHit(int dmg)
    {
        health -= dmg;
        gameManager.score += 5;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        if (health <= 0){
            OnDeath();
        }
    }

    private void OnDeath(){
        gameManager.CallExplosion(transform.position,"E");            
        
        switch(enemyName){
            case "L":
                gameManager.item_gague += 6;
                gameManager.score += gameManager.scorePointer[1]*4;
                break;
            case "M":
                gameManager.item_gague += 3;
                gameManager.score += gameManager.scorePointer[1]*2;
                break;
            case "S":
                gameManager.item_gague += 1;
                gameManager.score += gameManager.scorePointer[1];
                break;
        }

        if(gameManager.level > 2 && enemyName == "S"){
            float rand = Random.Range(1f,4.5f);
            switch((int)rand){
                case 1:
                    Bullet(bulletA,new Vector3(Random.Range(-7f,7f),6f,0),Intdir(Random.Range(210f,330f)),Random.Range(2f,4f));
                    break;
                case 2:
                    Bullet(bulletA,new Vector3(10f,Random.Range(-5f,5f),0),Intdir(Random.Range(120f,240f)),Random.Range(2f,4f));
                    break;
                case 3:
                    Bullet(bulletA,new Vector3(-10f,Random.Range(-5f,5f),0),Intdir(Random.Range(-90f,90f)),Random.Range(2f,4f));
                    break;
                case 4:
                    Bullet(bulletA,new Vector3(Random.Range(-7f,7f),-6f,0),Intdir(Random.Range(30f,150f)),Random.Range(2f,4f));
                    break;
        }
        }

        
        Destroy(gameObject);
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Pbullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }
            
    }

    void Bullet(GameObject bull, Vector3 pos, Vector3 dir, float sped){
        GameObject bullet = Instantiate(bull, pos, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector3 dirVec = dir;
        rigid.AddForce(dirVec.normalized * sped, ForceMode2D.Impulse);
    }

    Vector3 Intdir(float dir){
        Vector3 dVec = new Vector3(Mathf.Cos(dir/180*Mathf.PI),Mathf.Sin(dir/180*Mathf.PI),0);
        return dVec;
    }

    Vector3 Lookpdir(){
        Vector3 dVec = player.transform.position - transform.position;
        return dVec;
    }
}
