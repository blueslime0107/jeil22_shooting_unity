using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float speed = 100f;

    public GameManager gameManager;
    public Rigidbody2D rb;
    public Sprite[] sprites;
    
    float randomX, randomY;
    public int itemCode;

    

    SpriteRenderer spriteRenderer;

    void Start(){
        rb = GetComponent<Rigidbody2D>();

        randomX = 1f;
        randomY = 1f;

        Vector2 dir = new Vector2(randomX,randomY).normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();  
        spriteRenderer.sprite = sprites[itemCode-1];

        rb.AddForce(dir * speed);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            gameManager.ItemEquiped(itemCode);
            gameManager.score += gameManager.scorePointer[3];
            gameObject.SetActive(false);
        }
    }
}
