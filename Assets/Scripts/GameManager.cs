using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStart = false;
    public bool playerDied = false;
    public RectTransform gameIntroImg;

    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    //public GameObject cameraObject;
    public cameraShake camman;

    public GameObject explo1; 

    public int[] scorePointer;

    public float maxSD;
    float curSD;
    public int curEnemy;
    public int maxEnemy;
    public bool bossSpawned;
    public float curbossSD;
    public float maxbossSD;
    int ranPoint;
    public int level;

    
    [HideInInspector] public float item_gague;
    public float maxitem_gague;

    int itemArray = 1;
    public bool getLazer;
    public bool getBomb;
    public float getShieldTime = 12f;
    public bool getShield;
    public bool perfectShield;
    float getPowerTime = 4f;
    public bool getPower;

    //public 
    public UIFirst startimg;
    public GameObject player;
    public GameObject boss;
    public GameObject item;
    public GameObject bomb;
    public GameObject barrier;

    public int score = 0;

    void Start(){
        ranPoint = Random.Range(0, spawnPoints.Length);
        bossSpawned = false;
    }
    
    void Update()
    {        
        if(gameStart){ 
            gameIntroImg.anchoredPosition = Vector2.Lerp(gameIntroImg.anchoredPosition,new Vector2(-960,-550),4f*Time.deltaTime);
            SpawnSmall();
            if(!bossSpawned){
                curbossSD += Time.deltaTime;
                SpawnBoss();
            }
            if(item_gague > maxitem_gague){
                GameObject itemed =Instantiate(item,Vector3.zero,transform.rotation);
                Item itemLogic = itemed.GetComponent<Item>();
                itemLogic.gameManager = this;
                switch(itemArray){
                    case 1:
                        itemLogic.itemCode = 2;
                        itemArray = 2;
                        break;
                    case 2:
                        itemLogic.itemCode = 3;
                        itemArray = 3;
                        break;
                    case 3:
                        itemLogic.itemCode = 4;
                        itemArray = 1;
                        break;
                }

                item_gague = 0;
            }

            ItemActive();
            
            
        }
        if(playerDied){
            boss.SetActive(false);
            gameIntroImg.anchoredPosition = Vector2.Lerp(gameIntroImg.anchoredPosition,new Vector2(-960,-540),4f*Time.deltaTime);
        }
            

        
    }

    public void PlayerDied(){
        player.SetActive(false); // 모양숨기기
        playerDied = true;
        gameStart = false;
        boss.SetActive(false);
        startimg.PlayerDied();
    }

    void SpawnSmall()
    {
        curSD += Time.deltaTime;
        if (curSD > maxSD)
        {
            if (curEnemy > maxEnemy){
            ranPoint = Random.Range(0, spawnPoints.Length);
            curEnemy = 0;
            }

            int spawnObj = 0;
            if (curEnemy == maxEnemy/2)
                spawnObj = 1;
            if (curEnemy == maxEnemy)
                spawnObj = 2;
            GameObject enemy = Instantiate(enemyObjs[spawnObj], spawnPoints[ranPoint].position,
                                spawnPoints[ranPoint].rotation);
            Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
            Enemy enemyLogic = enemy.GetComponent<Enemy>();
            enemyLogic.player = player;
            enemyLogic.gameManager = this;

            switch (ranPoint)
            {
                case 0:
                    rigid.velocity = new Vector2(0, -enemyLogic.speed);
                    break;
                case 1:
                    rigid.velocity = new Vector2(0, -enemyLogic.speed);
                    break;
                case 2:
                    rigid.velocity = new Vector2(-enemyLogic.speed, 0);
                    break;
                case 3:
                    rigid.velocity = new Vector2(-enemyLogic.speed, 0);
                    break;
                case 4:
                    rigid.velocity = new Vector2(enemyLogic.speed, 0);
                    break;
                case 5:
                    rigid.velocity = new Vector2(enemyLogic.speed, 0);
                    break;
                case 6:
                    rigid.velocity = new Vector2(0, enemyLogic.speed);
                    break;
                case 7:
                    rigid.velocity = new Vector2(0, enemyLogic.speed);
                    //enemy.transform.Rotate(Vector3.forward * 90);                
                    break;
            }


            curEnemy += 1;
            curSD = 0;
        }
        //int ranEnemy = Random.Range(0, enemyObjs.Length);
        

    }

    void SpawnBoss(){
        if(curbossSD < maxbossSD)
            return;
        
        bossSpawned = true;
        boss.SetActive(true);
        curbossSD = 0;
    }

    void ItemActive(){
        if(getPower){
            getPowerTime -= Time.deltaTime;
            if(getPowerTime <= 0){
                getPower = false;
                getPowerTime = 4f;
            }
        }
        if(getShield){
            if(getShieldTime == 12f)
                barrier.SetActive(true);
            getShieldTime -= Time.deltaTime;
            if(getShieldTime <= 0){
                barrier.SetActive(false);
                getShield = false;
                getShieldTime = 12f;
                if(perfectShield)
                    score += scorePointer[4];
            }
        }
        if(getBomb){
            bomb.SetActive(true);
            getBomb = false;
        }
        
    }

    public void ItemEquiped(int var){
        switch(var){
            case 2:
                bomb.SetActive(false);
                getBomb = true;
                break;
            case 3:
                barrier.SetActive(false);
                getShield = true;
                getShieldTime = 12f;
                perfectShield = true;
                break;
            case 4:
                getPower = true;
                getPowerTime = 4f;
                break;

        }

    }

    public void ShakeScreen(){
        camman.shakeing = true;
    }

    public void OnClickStart()
    {
        if(playerDied)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameStart = true;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }
















    public void RespawnPlayer(){ // 2초 뒤에 소환
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe(){
        player.transform.position = Vector3.down * 3f;
        player.SetActive(true);
    }

    public void CallExplosion(Vector3 pos, string type){
        GameObject explosion = Instantiate(explo1, transform.position, transform.rotation);
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }
}
