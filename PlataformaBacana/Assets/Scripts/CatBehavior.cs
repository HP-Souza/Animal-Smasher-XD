using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{

    public float enemySpeed;
    public float actualSpeed;
    public bool verifyDirection;
    public Transform rightLimit, leftLimit;
    private AudioManager audioManager;
    public GameObject death;
 
    

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = actualSpeed;
        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.position.x <=leftLimit.position.x && verifyDirection==true)
        {
            enemySpeed = actualSpeed;
            direction();
        }
        else if(transform.position.x >= rightLimit.position.x && verifyDirection==false)
        {
            enemySpeed = -actualSpeed;
            direction();
        }

        transform.Translate(new Vector2(enemySpeed,0) * Time.deltaTime);

    }

    public void direction()
    {
        verifyDirection = !verifyDirection;

        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Instantiate(death, collision.gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            audioManager.SFXmanager(audioManager.enemyDeathSFX, 0.5f);          
            Destroy(collision.gameObject);
            Destroy(gameObject);          
        }

        else if (collision.gameObject.CompareTag("sword"))
        {
            Instantiate(death, collision.gameObject.transform.position+new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
            audioManager.SFXmanager(audioManager.enemyDeathSFX, 0.5f);
        }

    }


}
