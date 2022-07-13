using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    GameObject[] enemyBullet;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] explosion;
    GameObject[] pbull1;
    GameObject[] pbull2;
    GameObject[] small_b;

    void Awake(){
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];
        explosion = new GameObject[10];
        pbull1 = new GameObject[10];
        pbull2 = new GameObject[10];
        small_b = new GameObject[10];

        Generate();
    }

    void Generate(){
        
    }
}
