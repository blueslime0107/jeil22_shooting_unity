using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Slider hpbar;
    [SerializeField]
    private Slider bossAppear;
    [SerializeField]
    private Text score_ui;
    [SerializeField]
    private Text text_Timer;
    [SerializeField]
    private Text level_text;
    [SerializeField]
    private Slider hpbar1;
    [SerializeField]
    private Slider hpbar2;
    [SerializeField]
    private Slider item_bar;
    [SerializeField]
    private Text maxPlayer;
    [SerializeField]
    private Text maxScore;
    public float _sec;
    int _min;

    public GameObject bulletA;
    bool diding = true;

    public GameObject player;
    public GameObject boss;
    public GameManager gameManager;
    private Boss bossObject;
    private Player playerObject;

    void Start()
    {
        bossObject = boss.GetComponent<Boss>();
        playerObject = player.GetComponent<Player>();
        Handle();
        Timer();

        maxPlayer.text = PlayerPrefs.GetString("name");
        maxScore.text = PlayerPrefs.GetInt("score").ToString("D10");


        //playerPower.fillAmount = (float)playerObject.min_power / (float)100;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStart){
            Handle();
            Timer();
        }
        
    }

    private void Handle(){
        hpbar.value = (float) bossObject.health /(float)bossObject.max_health;
        bossAppear.value = gameManager.curbossSD / gameManager.maxbossSD;
        //playerPower.fillAmount = (float)playerObject.min_power / (float)100;
        score_ui.text = gameManager.score.ToString("D10");
        level_text.text = gameManager.level.ToString();
        hpbar1.value = (float) playerObject.health /(float)playerObject.max_health;
        hpbar2.value = (float) playerObject.health /(float)playerObject.max_health;
        item_bar.value = gameManager.item_gague / gameManager.maxitem_gague;
    }

    private void Timer(){
        _sec += Time.deltaTime;
        float mill = _sec*100-Mathf.Floor(_sec)*100;
        text_Timer.text = string.Format("{0:D1}:{1:D2}:{2:D2}", _min, (int)_sec, (int)mill);
        if((int)_sec > 59){
            _sec = 0;
            _min++;
        }
        if((int)_min > 1 && diding){
            StartCoroutine("TimeOut");
            diding = false;
        }
    }

    IEnumerator TimeOut(){
        while (true)
        {
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
            if(gameManager.playerDied){
                StopAllCoroutines();
            }
            yield return new WaitForSecondsRealtime(1/60);
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
}
