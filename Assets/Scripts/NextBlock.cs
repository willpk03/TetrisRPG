using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlock : MonoBehaviour
{
    public int groupID = 0;
    public GameObject[] ignores;
    GameObject Next;
    // public int[] groups;
    // public int[] setgroups;
    int id = 0;
    public List<int> groups  = new List<int>();
    public List<int> setgroups =  new List<int>();
    float lastenemyattempt = 0;

    public void spawnNext() {
        Destroy(Next);
        groupID = checkBag();
        //Debug.Log(groupID);
        Next = Instantiate(ignores[groupID], transform.position, Quaternion.identity);
    }

    public void spawnFunc() {
        FindObjectOfType<Spawner>().spawn(groupID);
        spawnNext();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        groupID = checkBag();
        //Debug.Log(groupID);
        FindObjectOfType<Spawner>().spawn(groupID);
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int thebag() {
        
        //Picks from the index
        int id = Random.Range(0, groups.Count);
        //Debug.Log(id);
        
        //grabs the id group
        //bid is the group, id is the index
        
        int bid = groups[id]; 
        
        groups.Remove(bid);
        
        return bid;
    }

    int checkBag() {
        if(groups.Count == 0) {
            groups.Add(0);
            groups.Add(1);
            groups.Add(2);
            groups.Add(3);
            groups.Add(4);
            groups.Add(5);
            groups.Add(6);
            return thebag();

        } else {

            return thebag();

        }
    }

}
 