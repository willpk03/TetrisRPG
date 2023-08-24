using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    float lastFall = 0;
    bool faker = false;
    public static int sameRound = 0;
    public static int cProcess = 0;
    string deletedObject;
    float lastenemyattempt = 0;
    int rotPos = 0;
    
    
    
    // public static int attack = 0;
    // public static health = 9;
    // public static enemyhealth;
    
    // Start is called before the first frame update
    void Start()

    
    {
        PlayField.updateText();
        
            if(!isValidGridPos() || PlayField.health <= 0) {
           
                Debug.Log("GAME OVER");
                Destroy(gameObject);
            } 
        
        
    }

    

    public void updateSpeed(int Rows) {
        int Level = (int) Rows / 10;
        Debug.Log(Level);
        FindObjectOfType<Spawner>().Dif = 1 - (Level *.5) / 100;
        //Debug.Log(FindObjectOfType<Spawner>().Dif);
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<Spawner>().enemyattackcheck();
        if (PlayField.attack.Count > 0) {
            //Debug.Log(PlayField.attack.Count);
            int num = 0;
            while (num <= PlayField.attack.Count - 1) {
                //Debug.Log(PlayField.attack[num]);
                if (Time.time - PlayField.lastattack[num] >= 20) {
                    PlayField.enemyHealth = PlayField.enemyHealth - PlayField.attack[num];
                    PlayField.attack.RemoveAt(num);
                    PlayField.lastattack.RemoveAt(num);
                    //Debug.Log("You have: " + PlayField.enemyHealth + "health");
                    if(PlayField.enemyHealth <= 0) {
                        Debug.Log("YOU WIN");
                        Destroy(gameObject);
                    }
                } else {
                    //Debug.Log("Not calculated");
                }

                
                num++;
            }
            
            
        } 
        if (PlayField.lastenemyattack.Count > 0) {
            int num = 0;
            while (num <= PlayField.enemyattack.Count - 1) {
                //Debug.Log(PlayField.lastenemyattack[num]);
                if (Time.time - PlayField.lastenemyattack[num] >= 20) {
                    PlayField.health = PlayField.health - PlayField.enemyattack[num];
                    PlayField.enemyattack.RemoveAt(num);
                    PlayField.lastenemyattack.RemoveAt(num);
                    Debug.Log("You have: " + PlayField.enemyHealth + "health");
                    if(PlayField.health <= 0) {
                        Debug.Log("Gameover");
                        Destroy(gameObject);
                    }
                } else {
                    //Debug.Log("Not calculated");
                }
                num++;
            }
            PlayField.updateText();
        }

        
        

        
        double Dif = FindObjectOfType<Spawner>().Dif;
    
        
        //Debug.Log(Time.time);
        //Debug.Log(Dif);
        if(transform.name.Contains("fake") || transform.name.Contains("hold")) {
            faker = true;
            
        } else {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                // Modify position
                transform.position += new Vector3(-1, 0, 0);
            
                // See if it's valid
                if (isValidGridPos())
                    // It's valid. Update grid.
                    updateGrid();
                else
                    // Its not valid. revert.
                    transform.position += new Vector3(1, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                // Modify position
                transform.position += new Vector3(1, 0, 0);
            
                // See if valid
                if (isValidGridPos())
                    // It's valid. Update grid.
                    updateGrid();
                else
                    // It's not valid. revert.
                    transform.position += new Vector3(-1, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                Vector2 pos = PlayField.roundVec2(transform.position);
                if (!((int)pos.x == 11 && (int)pos.y == 11)) {
                    transform.Rotate(0, 0, -90);
                
                    // See if valid
                    if (isValidGridPos())
                        // It's valid. Update grid.
                        updateGrid();
                    else
                        // It's not valid. revert.
                        
                        transform.Rotate(0, 0, 90);
                }
            
                
            }else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - lastFall >= Dif )) {
                    Vector2 pos = PlayField.roundVec2(transform.position);
                    if(!((int)pos.x == 11 && (int)pos.y == 11)){// Modify position
                        transform.position += new Vector3(0, -1, 0);

                        // See if valid
                        if (isValidGridPos()) {
                            // It's valid. Update grid.
                            updateGrid();
                        } else {
                            // It's not valid. revert.
                            transform.position += new Vector3(0, 1, 0);

                            //Abilities 
                                if (Spawner.abilityinUse > 0) {
                                    Spawner.abilityinUse = 0;
                                    Debug.Log("Ability was placed");
                                    PlayField.abilityCheck();
                                }
                            // Clear filled horizontal lines
                            PlayField.deleteFullRows();

                            // Spawn next Group
                            
                            FindObjectOfType<NextBlock>().spawnFunc();

                            // Disable script
                            enabled = false;
                            
                        }

                        lastFall = Time.time;
                        
                    }
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                int check = 2;
                do 
                {
                    Vector2 pos = PlayField.roundVec2(transform.position);
                    if(!((int)pos.x == 11 && (int)pos.y == 11)){
                        transform.position += new Vector3(0, -1, 0);
                        if (isValidGridPos()) {
                            updateGrid();
                        } else {
                            transform.position += new Vector3(0, 1, 0);

                            //Abilities 
                                if (Spawner.abilityinUse > 0) {
                                    
                                    Debug.Log("Ability was placed");
                                    PlayField.abilityCheck();
                                }


                            // Clear filled horizontal lines
                            PlayField.deleteFullRows();

                            // Spawn next Group
                            
                            FindObjectOfType<NextBlock>().spawnFunc();

                            // Disable script
                            enabled = false;

                            check = 1;
                    
                        }
                    }
                }
                while (check == 2);
                
            } else if (Input.GetKeyDown(KeyCode.C)) {
                transform.name = "ghost";
                Debug.Log(sameRound);
                
                if(sameRound == 0) {
                    Destroy(gameObject);
                    FindObjectOfType<Spawner>().cFunc(gameObject);
                }
            }
        }    
    }

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = PlayField.roundVec2(child.position);

            // Not inside Border?
            if(child.gameObject.tag != "ignore") {
                if (!PlayField.insideBorder(v, transform)){
                    //Debug.Log("B");
                    //Debug.Log(PlayField.insideBorder(v, transform));
                    return false;
                }
                    
            } else {
                return true;
            }
            //Ignores check if transform isn't in grid.
            
            // Block in grid cell (and not part of same group)?
            //Checks to see if it should be checked
            if (!(transform.name.Contains("fake") || transform.name.Contains("hold") || transform.name.Contains("ghost") )) {
                //Checks if a c press is in process
                
                    //Checks to see if the position of a block isn't null and that the position doesn't already have a block in it that isn't the same group.
                    if (PlayField.grid[(int)v.x, (int)v.y] != null && PlayField.grid[(int)v.x, (int)v.y].parent != transform && PlayField.grid[(int)v.x, (int)v.y].parent.name != "ghost"){ 
                        //Debug.Log("A");
                        //Debug.Log(PlayField.grid[(int)v.x, (int)v.y] != null);
                        //Debug.Log(PlayField.grid[(int)v.x, (int)v.y].parent.name);
                        //Debug.Log(transform.name);
                        // Debug.Log(PlayField.grid[(int)v.x, (int)v.y].parent.name != deletedObject);
                        // Debug.Log(deletedObject);
                        // Debug.Log(v.x + v.y);
                    return false;
                    }
                
                
                    
            }
        }
        return true;
    }

    
    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < PlayField.h; ++y)
            for (int x = 0; x < PlayField.w; ++x)
                if (PlayField.grid[x, y] != null)
                    if (PlayField.grid[x, y].parent == transform)
                        PlayField.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            
                Vector2 v = PlayField.roundVec2(child.position);
                PlayField.grid[(int)v.x, (int)v.y] = child;
                
            
            
        }
    }

    
}
