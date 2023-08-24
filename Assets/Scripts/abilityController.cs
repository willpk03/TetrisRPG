using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityController : MonoBehaviour

{

    public GameObject[] groups;
    public static int ability = 0;
    public static int abilityPOS = 0;
    int initialPOS = 0;
    GameObject current;
    // Start is called before the first frame update
    void Start()
    {
        
        changedAbility();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) { 
            if (groups.Length == ability + 1 ) {
                ability = -1;
            } 
            ability = ability + 1;
            // Debug.Log(ability);
            changedAbility();
        } else if (Input.GetKeyDown(KeyCode.Z)) { 
            if (ability == 0) {
                ability = groups.Length;
            } 
            ability = ability - 1;
            // Debug.Log(ability);
            changedAbility();
        }


    }

    public void changedAbility(){
        if(initialPOS == 0) {
            initialPOS = 1;
            if(ability != 0) {
                abilityPOS = PlayField.myabilities[ability - 1];
            } else {
                abilityPOS = 0;
            }
            
            current = Instantiate(groups[abilityPOS], transform.position, Quaternion.identity);
            FindObjectOfType<holdBlock>().abilitychanged(abilityPOS);
        } else {
            
            Destroy(current);
            if(ability != 0) {
                abilityPOS = PlayField.myabilities[ability - 1];
            } else {
                abilityPOS = 0;
            }
            Debug.Log(abilityPOS);
            current = Instantiate(groups[abilityPOS], transform.position, Quaternion.identity);
            FindObjectOfType<holdBlock>().abilitychanged(abilityPOS);
        }
        // -3.38 8.47 0.01083
    }

    public void changedheld(){
        abilityPOS = 0;
        changedAbility();
    }
}
