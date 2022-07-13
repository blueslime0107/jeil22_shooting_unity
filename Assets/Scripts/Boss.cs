using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public string enemyName;
    public int max_health;
    [HideInInspector] public int health;    
    public Sprite[] sprites;
    public GameManager gameManager;
    public ParticleSystem effect;

    public GameObject[] bullet;

    public float count;
    public int bossNum;
    int sub_bossNum;
    public GameObject player;
    public Transform[] pose;
    Vector3 targetPos;
    public bool targetMoving;
    float targetSpeed;

    bool battleStart;

    SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        if(gameManager.level < 8){
            bossNum = gameManager.level;
        }            
        else
        {
            bossNum = (int)Random.Range(1f,8f);
        }
        sub_bossNum = bossNum;
        health = max_health;
        count = 0f;
        targetMoving = false;
        
    }

    void ReadyToFight(){
        battleStart = true;
    }

    // Update is called once per frame


    void Update()
    {
        if(battleStart){
            Fight();
            MoveToTarget();
        }
        else{
            FirstMove();
        }
        
    }

    private void FirstMove(){
        transform.position = Vector3.Lerp(transform.position, pose[1].position, 4f*Time.deltaTime);
        if(Vector3.Distance(transform.position,pose[1].position) <= 0.01f){
            transform.position = pose[1].position;
            ReadyToFight();
        }

            
    }

    public void OnHit(int dmg)
    {
        if(!battleStart)
            return;
        health -= dmg;
        gameManager.score += gameManager.scorePointer[0];
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        if (health <= 0){
            EffectPlay();
            gameManager.ShakeScreen();
            gameManager.score += gameManager.scorePointer[2];
            transform.position = new Vector3(-11,8,0);
            battleStart = false;
            gameManager.bossSpawned = false;
            StopCoroutine("Boss"+sub_bossNum.ToString());
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EBullet");
            for(int i = 0; i < bullets.Length; i++){
                Destroy(bullets[i]);
            }
            gameManager.level++;            
            gameObject.SetActive(false);
        }
            
            
    }
    
    void Fight()
    {
        if(bossNum != 0)
            StartCoroutine("Boss"+bossNum.ToString());
        bossNum = 0;
             
    }

    IEnumerator Boss1(){
        yield return new WaitForSecondsRealtime(milSec(10));
        if(count==10){
            SetTargetMove(new Vector3(transform.position.x+Random.Range(-1f,1f),transform.position.y+Random.Range(-1f,1f),0),2f);
            count = 0;
        }
        Bullet(bullet[3],transform.position,LookatPos(pose[4].position),5); 
        
        count += 1;
        StartCoroutine("Boss1");
           
    }

    IEnumerator Boss2(){
        yield return new WaitForSecondsRealtime(milSec(60));
        if(count == 3){
            SetTargetMove(pose[4].position,4f);
        }
        Bullet(bullet[3],transform.position,LookatPos(player.transform.position),3); 

        float rand = Random.Range(0f,15f);
        for(int i = 0;i<360;i+=15){
            Bullet(bullet[2],transform.position,i+rand,5); 
        }
        count += 1;
        StartCoroutine("Boss2"); 
    }

    IEnumerator Boss3(){
        yield return new WaitForSecondsRealtime(milSec(5));

        for(int i = 0;i<360;i+=90){
            Bullet(bullet[1],transform.position,(float)i+count,5); 
        }
        count += 10.3f;
        StartCoroutine("Boss3");  
    }

    IEnumerator Boss4(){
        yield return new WaitForSecondsRealtime(milSec(30));
        count++;
        if(count % 2 == 0){
            for(int i = 0;i<360;i+=30){
                Bullet(bullet[2],transform.position,(float)i+count,5); 
            }
        }
        SetTargetMove(pose[(int)Random.Range(0f,9f)].position,5f);
        
        StartCoroutine("Boss4");  
    }

    IEnumerator Boss5(){
        yield return new WaitForSecondsRealtime(milSec(20));
        count++;
        if(count == 2){
            for(int i = 10;i<100;i+=10)
            Bullet(bullet[3],transform.position,LookatPos(player.transform.position)+i,4f);
        }
        if(count == 4){
            for(int i = 10;i<100;i+=10)
            Bullet(bullet[3],transform.position,LookatPos(player.transform.position)-i,4f);
            count = 0;
        }
        
        StartCoroutine("Boss5");  
    }

    IEnumerator Boss6(){
        yield return new WaitForSecondsRealtime(milSec(2));
        count++;
        if(count == 30){
            SetTargetMove(pose[(int)Random.Range(0f,3f)].position,5f);
            for(int i = 0;i<360;i+=10){
            Bullet(bullet[2],transform.position,(float)i+count,5); 
            }
        }
        if(count >= 35 && count % 2 == 0){
            Bullet(bullet[3],transform.position,LookatPos(player.transform.position),6f);
        }
        if(count == 60)
            count = 0;
        
        StartCoroutine("Boss6");  
    }

    IEnumerator Boss7(){
        yield return new WaitForSecondsRealtime(milSec(2));
        count++;
        if(count == 5){
            SetTargetMove(pose[(int)Random.Range(0f,3f)].position,2f);
        }
        if(count == 30){
            for(int i = 0;i<360;i+=10){
                for(int j = 2;j<6;j+=1){
                    yield return null;
                    Bullet(bullet[3],transform.position,(float)i+count,(float)j); 
            }
        }
        }
        if(count == 90)
            count = 0;
        
        StartCoroutine("Boss7");  
    }








    void SetTargetMove(Vector3 pos, float speed){
        if(targetMoving)
            return;
        targetMoving = true;
        targetPos = pos;
        targetSpeed = speed;

    }

    void MoveToTarget(){
        if(targetMoving){
            transform.position = Vector3.Lerp(transform.position, targetPos, 4f*Time.deltaTime);
            if(Vector3.Distance(transform.position,targetPos) <= 0.01f){
                transform.position = targetPos;
                targetMoving = false;
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.tag == "BulletBorder")
        //     Destroy(gameObject);
        if (collision.gameObject.tag == "Pbullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }
            
    }

    void EffectPlay(){
        effect.transform.position = transform.position;
        effect.Play();
    }

    void Bullet(GameObject bull, Vector3 pos, float dir, float sped){
        //dir = dir+90f; 
        Vector3 dVec = new Vector3(Mathf.Cos(dir/180*Mathf.PI),Mathf.Sin(dir/180*Mathf.PI),0);
        GameObject bullet = Instantiate(bull, pos, Quaternion.Euler(0,0,dir-90f));
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(dVec.normalized * sped, ForceMode2D.Impulse);
    }
    Vector3 Intdir(float dir){
        Vector3 dVec = new Vector3(Mathf.Cos(dir/180*Mathf.PI),Mathf.Sin(dir/180*Mathf.PI),0);
        return dVec;
    }

    float LookatPos(Vector3 pos){
        Vector3 v = pos - transform.position;
        return Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg;
    }


    float milSec(int time){
        return (float)time / 60f;
    }
    
    
    
    
    // bool whentime(float time){
    //     return count == time;
    // }

    // bool whiletime(float time){
    //     if(count >= time){
    //         count = time;
    //     }
    //     return count % time == 0;        
    // }




}
