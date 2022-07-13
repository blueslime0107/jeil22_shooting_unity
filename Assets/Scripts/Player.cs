using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float max_health;
    public float health;
    public bool gihapeti = true;
    public float speed;
    public float slow_speed;
    float curSpd;
    public int min_power;
    public int power;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public GameObject bulletA;
    public GameObject bulletB;
    public float maxSD;
    public float curSD;

    float angle;
    Vector2 mouse;

    public GameManager manager;

    public bool isRespawnTime;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    void OnEnable() {
        UnbeatAble();
        Invoke("UnbeatAble", 2f);      
    }

    void UnbeatAble(){
        isRespawnTime = !isRespawnTime;
        if(isRespawnTime){
            spriteRenderer.color = new Color(1,1,1,0.5f);
        }else{
            spriteRenderer.color = new Color(1,1,1,1);
        }
    }

    void Update()
    {
        
        Move();
        Fire();
        Reload();
        // 플레이어 방향으로 바라보기
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
   	    angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
   	    this.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
    }

    void Move()
    {
        // �⺻���� �̵�
        float h = Input.GetAxisRaw("Horizontal"); // ����Ű�� ���� �� ��ȯ
        // if ���� ���ٸ� ���� ��ȣ�� ������ �� �ִ�
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)) // �����ʿ� ��� �������̵�? �Ǵ� ���ʿ� ��� �������� �̵�?
            h = 0; // 0 ���� �ٲ� ���� �������� �ʰ��Ѵ�. �Ʒ��� ����������

        float v = Input.GetAxisRaw("Vertical"); // -1, 0, 1 �� ��ȯ ���� ���ϵ�

        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;

        if (Input.GetButton("Jump"))
            curSpd = slow_speed;
        else
            curSpd = speed;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) *curSpd* Time.deltaTime; // transform �̵����� deltaTime �����ֱ�
        transform.position = curPos + nextPos; // ��������� �����ϸ� ����

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;
        if (curSD < maxSD)
            return;
        ShootBullet(bulletA, transform.position, Intdir(angle-3),20f);
        ShootBullet(bulletA, transform.position, Intdir(angle),20f);
        ShootBullet(bulletA, transform.position, Intdir(angle+3),20f);
        if(manager.getPower){
            ShootBullet(bulletB, transform.position, Intdir(angle+5),20f);
            ShootBullet(bulletB, transform.position, Intdir(angle-5),20f);
            ShootBullet(bulletB, transform.position, Intdir(angle+20),20f);
            ShootBullet(bulletB, transform.position, Intdir(angle-20),20f);
        }

        curSD = 0;

    }

    Vector3 Intdir(float dir){
        Vector3 dVec = new Vector3(Mathf.Cos(dir/180*Mathf.PI),Mathf.Sin(dir/180*Mathf.PI),0);
        return dVec;
    }

    void ShootBullet(GameObject objec,Vector3 pos,Vector3 pdir,float speed)
    {
        GameObject bullet = Instantiate(objec, pos,transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(pdir*speed, ForceMode2D.Impulse);
    }

    void Reload()
    {
        curSD += Time.deltaTime;
    }

    // #���# �Լ� �̸��� ���� OnTrigger�� �ִ�. �׷��� Boxcollider���� Ʈ���Ÿ� ������
    void OnTriggerEnter2D(Collider2D collision) // collision ���� ������Ʈ�� ������
    {
        if(collision.gameObject.tag == "Border") // ������Ʈ�� ��ũ�� Border?
        {
            switch (collision.gameObject.name) 
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }  
        else if (collision.gameObject.tag == "EBullet"){ // 탄에 닿았을때=
            if(isRespawnTime)
                return;
            manager.ShakeScreen();
            if(!manager.getShield){
                health -= manager.level;
            }
            else{
                manager.perfectShield = false;
                manager.getShieldTime = 0.1f;
            }
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EBullet");
            for(int i = 0; i < bullets.Length; i++){
                Destroy(bullets[i]);
            }

            UnbeatAble();
            Invoke("UnbeatAble",2f);
            if(health<=0){
                if(gihapeti){
                    health = 1;
                    gihapeti = false;
                }   
                else{
                    manager.PlayerDied();
                }
            }
                
                



        }
    }

    void OnTriggerExit2D(Collider2D collision) // ���� ������Ʈ���� ����
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
  
}
