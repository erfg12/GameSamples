using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_op : MonoBehaviour
{
    public GameObject tp;
    public Player script;
    public AudioClip TheAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        script = collision.collider.GetComponent<Player>();
        if (script == null)
            return;
        AudioSource.PlayClipAtPoint(TheAudioClip, transform.position);
        PlayerStats.coins++;
        Destroy(gameObject);
        //Debug.Log("colliding with coin");
    }
}
