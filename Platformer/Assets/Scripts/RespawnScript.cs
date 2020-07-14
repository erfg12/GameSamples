using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        PlayerStats.respawn = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        Debug.Log("Saving position for respawn: " + PlayerStats.respawn);
    }
}
