using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayField : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    public static Transform[,] grid = new Transform[w, h];
    public static int rowsDeleted = 0;
    public static List<int> attack  = new List<int>();
    public static List<int> enemyattack =  new List<int>();
    public static List<float> lastattack =  new List<float>();
    public static List<float> lastenemyattack =  new List<float>();
    public static int health = 10;
    public static int enemyHealth = 50;
    public static int attackbefore = 0;
    public static int heldpieceid;
    public static List<int> enemyattackchance  = new List<int>();
    public Sprite bomb;
    public static int prevhealth = 10;
    public static int prevenemyhealth = 50;
    public static int prevtotalAttack = 0;
    public static int prevtotalenemyAttack = 0;
    public static int[] myabilities = {1, 2, 3};
    public static int[] availableAbilities = {1, 2, 3};
    
    
    

    public static bool grabPlayerInfo() {
        int abilitieCheck = 0;
        //Grabs the abilites that have been choosen
        for(int i = 0; i < myabilities.Length - 1; i++) {
            for(int x = 0; x < availableAbilities.Length - 1; x++) {
                if(myabilities[i] == availableAbilities[x]) {
                    abilitieCheck++;
                }
            }
        }
        if(abilitieCheck == 3) {
            return true;
        } else {
            return false;
        }
    }

    public static Vector2 roundVec2(Vector2 v) {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool insideBorder(Vector2 pos, Transform transform) {
        
        if (transform.name.Contains("fake") || transform.name.Contains("hold")){
            
            return true;
        } else {
            return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0) || ((int)pos.x == 11 && (int)pos.y == 11);
        }
        
        
    }

    public static void deleteRow(int y) {
        for (int x = 0; x < w; ++x) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void decreaseRow(int y) {
        for (int x = 0; x < w; ++x) {
            if (grid[x, y] != null) {
                // Move one towards bottom
                grid[x, y-1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void decreaseRowsAbove(int y) {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }

    public static bool isRowFull(int y) {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    public static void abilityCheck() {


        int ability = Spawner.abilityUsed;
        if (ability == 1) {
            bombAbility();
        } else if (ability == 2) {
            heathAbility();
        } else if (ability == 3) {
            shieldAbility();
        } else if (ability == 4) {
            
        } else if (ability == 5) {
            
        } else if (ability == 6) {
            
        } else {
            Debug.Log("Something has gone wrong: " + ability);
        }
        Spawner.abilityinUse = 0;
        Spawner.abilityUsed = 0;

    }

    public static void heathAbility() {
        for (int y = 0; y < h; ++y) {
            if (isRowFull(y)) {
                health = health + 1;
                updateText();

                deleteRow(y);
                decreaseRowsAbove(y+1);
            }
        }
    }

    public static void shieldAbility() {
        int shieldComplete = 0;
        for (int y = 0; y < h; ++y) {
            if (isRowFull(y)) {
                shieldComplete = shieldComplete + 1;
                deleteRow(y);
                decreaseRowsAbove(y+1);
            }
        }
        if (shieldComplete == 4) {
            int num = 0;
            if (enemyattack.Count != 0) {
                while (num <= enemyattack.Count - 1) {
                   enemyattack.RemoveAt(num);
                    // Debug.Log(enemyattack[num]);
                    num = num + 1;
                }
            }
        } else {

        }
    }

    public static void bombAbility() {
        List<int> yLevels  = new List<int>();
        int maxHeight = 0;
        int lowHeight = 20;

        for (int y = 0; y < h; ++y) {
            
            
            for (int x = 0; x < w; ++x) {
                // foreach (Transform child in grid[x,y].parent) {

                    // if (child.gameObject.GetComponent<SpriteRenderer>().sprite.name == "bombBlock") {
                    //     deleteRow(y);
                    //     decreaseRowsAbove(y+1);
                    // }
                // }
                if(grid[x,y] != null) {
                    //Debug.Log(grid[x,y].gameObject.GetComponent<SpriteRenderer>().sprite.name);
                    if (grid[x,y].gameObject.GetComponent<SpriteRenderer>().sprite.name == "bombBlock") {
                        yLevels.Add(y);

                    }
                }
                

            }
        }
        for ( int i = 0; i <= yLevels.Count -1 ; i++) {
            for (int x = 0; x < w; ++x) {
                if (grid[x,yLevels[i]] != null) {
                    Destroy(grid[x, yLevels[i]].gameObject);
                    grid[x, yLevels[i]] = null;
                }
                
            }
            if (lowHeight > yLevels[i]){
                lowHeight = yLevels[i];
            }
            if (maxHeight < yLevels[i]) {
                maxHeight = yLevels[i];
            }
        }
        maxHeight = maxHeight - lowHeight;
        //Debug.Log(lowHeight + maxHeight);
        // decreaseRowsAbove(lowHeight + maxHeight);
        for (int i = lowHeight + maxHeight; i < h; ++i) {
            for (int x = 0; x < w; ++x) {
                if (grid[x, i] != null) {
                    // Move one towards bottom
                    grid[x, i-maxHeight - 1] = grid[x, i];
                    grid[x, i] = null;

                    // Update Block position
                    grid[x, i-maxHeight - 1].position += new Vector3(0, -maxHeight - 1, 0);
                }
            }
        }
            
        

            
    }

    public static void deleteFullRows() {
        int levelnum = 0;

        for (int y = 0; y < h; ++y) {
            if (isRowFull(y)) {
                if(Spawner.currentBlock == 0) {
                    //Check the 4 rows above if full
                    if(isRowFull(y+1) && isRowFull(y+2) && isRowFull(y+3)) {
                        //Give Bonus.
                        Debug.Log("TETRIS!");
                        levelnum = levelnum + 2;
                    }
                    


                    
                }
                
                deleteRow(y);
                rowsDeleted = rowsDeleted + 1;
                FindObjectOfType<Group>().updateSpeed(rowsDeleted);

                decreaseRowsAbove(y+1);
                --y;
                levelnum = levelnum + 1;

            }
        }
        attacked(levelnum);
    }

    public static void attacked(int attacked) {

        if(enemyattack.Count != 0) {
            int num = 0;
            while (num <= enemyattack.Count - 1 && attacked > 0) {
                //Starting by the earliest (0)
                int enemycurrentattack = enemyattack[num];
                //Grab the value
                //Check if the attacked value is greater then the enemy attack 
                if (attacked >= enemycurrentattack) {
                    //Remove the value at enemyattack(num)
                    enemyattack.RemoveAt(num);
                    //Minise the attacked value by the enemyattack value
                    attacked = attacked - enemycurrentattack;
                    //check the next one and repeat.
                } else {
                    //Grab the attacked value and minise the value above if it doesn't equal 0
                    enemycurrentattack = enemycurrentattack - attacked;
                    enemyattack.RemoveAt(num);
                    enemyattack.Insert(num, enemycurrentattack);

                    //set the value at enemyattack to the updated one.
                }
                
                //if it is 
                    
                //If not
                    
                num++;
            }

            if (attacked != 0) {
                attack.Add(attacked);
                lastattack.Add(Time.time);
                attackbefore = 1;
                // Debug.Log("Happening");
                // Debug.Log("Number of attacks" + attack.Count);
                // Debug.Log("The players health" + health);
            }
            
        } else {
            attack.Add(attacked);
            lastattack.Add(Time.time);
            attackbefore = 1;
            // Debug.Log("Happening");
            //     Debug.Log("Number of attacks" + attack.Count);
            //     Debug.Log("The players health" + health);
        }

        updateText();
        
    }

    public static void enemyattacked(int attacked) {
        if( attack.Count != 0) {
            int num = 0;
            while (num <= attack.Count -1 && attacked > 0) {
                //Starting by the earliest (0)
                int currentattack = attack[num];
                //Grab the value
                //Check if the attacked value is greater then the enemy attack 
                if (attacked >= currentattack) {
                    //Remove the value at enemyattack(num)
                    attack.RemoveAt(num);
                    //Minise the attacked value by the enemyattack value
                    attacked = attacked - currentattack;
                    //check the next one and repeat.
                } else {
                    //Grab the attacked value and minise the value above if it doesn't equal 0
                    currentattack = currentattack - attacked;
                    attack.RemoveAt(num);
                    attack.Insert(num, currentattack);

                    //set the value at enemyattack to the updated one.
                }
                
                //if it is 
                    
                //If not
                    
                num++;
            }

            if (attacked != 0) {
                enemyattack.Add(attacked);
                lastenemyattack.Add(Time.time);
                attackbefore = 1;
                Debug.Log("EHappening");
                //Debug.Log("Number of Enemy attacks" +  enemyattack.Count);
                //Debug.Log("The enemies health" + enemyHealth);
            }
        }else {
            enemyattack.Add(attacked);
            lastenemyattack.Add(Time.time);
            attackbefore = 1;
            // Debug.Log("Happening");
            // Debug.Log("Number of attacks" +  enemyattack.Count);
            // Debug.Log("The enemies health" + enemyHealth);
        }
        updateText();
    }

    public static void updateText() {
        int totalAttack = 0;
        int num = 0;
        int totalenemyattack = 0;
        if (attack.Count != 0) {
            while (num <= attack.Count - 1) {
                totalAttack = totalAttack + attack[num];
                num++;
                // Debug.Log(num);
                // Debug.Log(attack[num]);
            }
        } else {
            totalAttack = 0;
        }

        
        
        num = 0;
        if (enemyattack.Count != 0) {
            while (num <= enemyattack.Count - 1) {
                totalenemyattack = totalenemyattack + enemyattack[num];
                num++;
                // Debug.Log(enemyattack[num]);
            }
        } else {
            totalenemyattack = 0;
        }
        //The changing effects
            //Current - previous
            //49 - 50
            int enemyChange = enemyHealth - prevenemyhealth;
            int playerChange = health - prevhealth;
            int changetotalattack = totalAttack - prevtotalAttack;   
            int changeenemytotalattack = totalenemyattack - prevtotalenemyAttack;
            prevenemyhealth = enemyHealth;
            prevhealth = health;
            prevtotalAttack = totalAttack;
            prevtotalenemyAttack = totalenemyattack;

            //Checks if positive
            //enemyHealth changes
            if (enemyChange == 0) {
                FindObjectOfType<Spawner>().enemyChangetxt.text= "";
            } else if (enemyChange < 0){
                enemyChange = enemyChange * -1;
                FindObjectOfType<Spawner>().enemyChangetxt.text = "-" + enemyChange.ToString();
            }else if(enemyChange > 0){
                FindObjectOfType<Spawner>().enemyChangetxt.text = "+" + enemyChange.ToString();
            }

            //playerHealth changes
            if (playerChange == 0) {
                FindObjectOfType<Spawner>().playerChangetxt.text = "";
            } else if (playerChange < 0){
                playerChange = playerChange * -1;
                FindObjectOfType<Spawner>().playerChangetxt.text = "-" + playerChange.ToString();
            }else if(playerChange > 0){
                FindObjectOfType<Spawner>().playerChangetxt.text = "+" + playerChange.ToString();
            }

            //changetotalattack changes
            if (totalAttack == 0) {
                 FindObjectOfType<Spawner>().playerAttackChangetxt.text = "";
            } else if (changetotalattack < 0){
                changetotalattack = changetotalattack * -1;
                FindObjectOfType<Spawner>().playerAttackChangetxt.text = "-" + changetotalattack.ToString();
            }else if(changetotalattack > 0){
                FindObjectOfType<Spawner>().playerAttackChangetxt.text = "+" + changetotalattack.ToString();
            }

            //changeenemytotalattack changes
            if (totalenemyattack == 0) {
                FindObjectOfType<Spawner>().enemyattackChangetxt.text = "";
            } else if (changeenemytotalattack < 0){
                changeenemytotalattack = changeenemytotalattack * -1;
                FindObjectOfType<Spawner>().enemyattackChangetxt.text = "-" + changeenemytotalattack.ToString();
            }else if(changetotalattack > 0){
                FindObjectOfType<Spawner>().enemyattackChangetxt.text = "+" + changeenemytotalattack.ToString();
            }

        //UI Info
        float speed = rowsDeleted/100;
        string healthtext = health.ToString();
        string enemytext = enemyHealth.ToString();
        FindObjectOfType<Spawner>().yourHealthtxt.text = healthtext;
        FindObjectOfType<Spawner>().bossHealthtxt.text = enemytext;
        FindObjectOfType<Spawner>().attacktxt.text = totalAttack.ToString();
        FindObjectOfType<Spawner>().enemyattacktxt.text = totalenemyattack.ToString();
        FindObjectOfType<Spawner>().speedtxt.text = (FindObjectOfType<Spawner>().Dif).ToString();
        
        
        // Debug.Log(totalAttack + ":" + totalenemyattack);
        // // Debug.Log(health);
        // // Debug.Log(enemyHealth);
        // Debug.Log(FindObjectOfType<Spawner>().bossHealthtxt.text);
    }

    public static void checkenemybag() {
        //Debug.Log("running checkbag");
        if (enemyattackchance.Count > 0) {
            enemyBag();
        } else {
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(0);
            enemyattackchance.Add(1);
            enemyBag();
        }
    }

    public static void enemyBag() {
        int id = Random.Range(0, enemyattackchance.Count);
        //Debug.Log("Running enemybag");
        if (enemyattackchance[id] == 1) {
            //run attack
            //Debug.Log("Attacked");
            if (enemyattackchance.Count == 0) {
                enemyattackchance.RemoveAt(id);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(1);
            } else if (enemyattackchance.Count == 1) {
                enemyattackchance.RemoveAt(id);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(0);
                enemyattackchance.Add(1);
            } else {
                enemyattackchance.RemoveAt(id);
                enemyattackchance.Add(1);

            }
            enemyattacked(1);
        } else {
            enemyattackchance.RemoveAt(id);
            Debug.Log("attempted attack but missed");
        }

        
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
