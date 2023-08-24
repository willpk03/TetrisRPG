using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Groups
    
    
    
    GameObject next;
    public GameObject[] groups;
    public GameObject[] ignores;
    public Sprite[] abilities;
    public static int currentBlock;
    public TMP_Text bossHealthtxt;
    public TMP_Text yourHealthtxt;
    public TMP_Text attacktxt;
    public TMP_Text enemyattacktxt;
    public TMP_Text speedtxt;

    public TMP_Text enemyChangetxt;
    public TMP_Text playerChangetxt;
    public TMP_Text enemyattackChangetxt;
    public TMP_Text playerAttackChangetxt;
    public double Dif = 1;
    public int Level = 0;
    public float rows;
    float lastenemyattempt = 0;
    public GameObject current;
    public static int abilityinUse;
    public static int abilityUsed;
    //FindObjectOfType<Spawner>().attacktxt
    
    // public void spawnNext() {
    //     int i = Random.Range(0, groups.Length);

    //     next = Instantiate(ignores[i], new Vector3(12, 12, 0), Quaternion.identity);
    //     Debug.Log("test");
    //     next.tag = "ignore";

    //     pre = i;
        
    // }


    public void enemyattackcheck() {
        if (Time.time - lastenemyattempt >= 5) {
            //Debug.Log(lastenemyattempt);
            
            lastenemyattempt = Time.time;
            PlayField.checkenemybag();
            //Debug.Log("It happened");
        }
    }

    public void spawn(int pre) {

        
        if (pre >= 0) {
            Instantiate(groups[pre], transform.position, Quaternion.identity);
            currentBlock = pre;
            Group.sameRound = 0;
            Group.cProcess = 0;
        } else {
            int i = Random.Range(0, groups.Length);
            Instantiate(groups[i], transform.position, Quaternion.identity);
            currentBlock = i;
            Group.cProcess = 0;
        }
    }

    public void cFunc(GameObject prevBlock) {
        Destroy(prevBlock);
        int newBlock = FindObjectOfType<holdBlock>().cPressed();
        if (newBlock >= 0) {
             spawnCert(newBlock);  
        }
            //Tell spawner to spawn held block
        
        else {
            Debug.Log("Firsttime");
            FindObjectOfType<NextBlock>().spawnFunc();
            
                        
        }
        
    }

    public void spawnCert(int pre) {
        current = Instantiate(groups[pre], transform.position, Quaternion.identity);
        foreach (Transform child in current.transform)
            {
                Debug.Log(child.gameObject.GetComponent<SpriteRenderer>());
                child.gameObject.GetComponent<SpriteRenderer>().sprite = abilities[abilityController.abilityPOS];
            }
        currentBlock = pre;
        abilityinUse = 1;
        abilityUsed = abilityController.abilityPOS;
        Debug.Log("Spawned in" + pre);
    }

     void Start() {
        
    }


    
    
}
