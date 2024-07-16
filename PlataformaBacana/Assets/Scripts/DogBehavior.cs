using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehavior : MonoBehaviour
{

    public bool verifyDirection;
    private AudioManager audioManager;
    public GameObject death;
    public float laserSpeed;
    public Transform laserPosition;
    public GameObject laser;
    public GameObject laserDirection;
    public GameObject player;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        InvokeRepeating("shoot", 0f, 3f);         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void direction()
    {
        verifyDirection = !verifyDirection;

        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        laserSpeed *= -1;

        laserDirection.GetComponent<SpriteRenderer>().flipX = verifyDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Instantiate(death, collision.gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            audioManager.SFXmanager(audioManager.enemyDeathSFX, 0.5f);          
            Destroy(gameObject);   
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("sword"))
        {
            Instantiate(death, collision.gameObject.transform.position+new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
            audioManager.SFXmanager(audioManager.enemyDeathSFX, 0.5f);
        }

    }

    public void shoot()
    {
        if(player.transform.position.x >= enemy.transform.position.x - 15)
        {
            GameObject temporary = Instantiate(laser);
            temporary.transform.position = laserPosition.transform.position;
            temporary.GetComponent<Rigidbody2D>().velocity = new Vector2(-laserSpeed, 0);
            Destroy(temporary.gameObject,3f);
        }
        
    }


}
