using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdBlock : MonoBehaviour
{
    int holdedBlocked = -1;
    int newBlock = 0;
    public GameObject[] groups;
    GameObject heldBlock;
    public Sprite[] abilities;
    int cPressedOnce = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int cPressed() {
        cPressedOnce = 1;
        //If done once before
        if (holdedBlocked >= 0) {
            //Gives the new current block.
            newBlock = holdedBlocked;
            holdedBlocked = Spawner.currentBlock; //Holds the current block
            Destroy(heldBlock); //Destroys old block
            heldBlock = Instantiate(groups[holdedBlocked], transform.position, Quaternion.identity);//Puts it in the box
            
            return newBlock;
            
        } else {
        //First time
            holdedBlocked = Spawner.currentBlock;
            
            heldBlock = Instantiate(groups[holdedBlocked], transform.position, Quaternion.identity);
            
            return -1; //Returns telling the system this is the first time
            
        } 
        FindObjectOfType<abilityController>().changedheld();
        
    }

    public void abilitychanged(int num) {
        if (cPressedOnce  == 1) {
            Debug.Log("Running abilitychanged");
            //Grabs each block and changes the image - goal
            foreach (Transform child in heldBlock.transform)
            {
                Debug.Log(child.gameObject.GetComponent<SpriteRenderer>());
                child.gameObject.GetComponent<SpriteRenderer>().sprite = abilities[num];
            }
        }
        
    }
}
