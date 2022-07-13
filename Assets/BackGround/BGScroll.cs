using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    private MeshRenderer render;

    public float speed;
    private float offsetx;
    private float offsety;

    void Start(){
        render = GetComponent<MeshRenderer>();
    }

    void Update(){
        offsetx += Time.deltaTime * speed;
        offsety += Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2(offsetx, offsety);
    }
}
