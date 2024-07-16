using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{



    public float speed;
    private Rigidbody2D rbPlayer;
    private float horizontalMovement;
    private Animator animation;
    private bool verifyDirection;
    public float playerJump;
    public Transform positionSensor;
    public bool sensor;
    public int HP;
    public TextMeshProUGUI textHP;
    public int ammo;
    public TextMeshProUGUI textAmmo;
    public GameObject gameOverPanel;
    public float bulletSpeed;
    public Transform bulletPosition;
    public GameObject bullet;
    public GameObject bulletDirection;
    public GameObject sword;
    public GameObject swordPosition;
    private AudioManager audioManager;
    public bool jumpAttack;
    public int points;
    public TextMeshProUGUI textPoints;
    public GameObject pauseMenu;
    public GameObject winScreen;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        HP = 1;
        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
    }

    
    void Update()
    
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        rbPlayer.velocity = new Vector2(horizontalMovement * speed, rbPlayer.velocity.y);

        animation.SetInteger("Run", (int)horizontalMovement);

        jump();

        detectGround();

        textHP.text = HP.ToString();
        textAmmo.text = ammo.ToString();
        textPoints.text = points.ToString();

        if (HP>0)
        {
            if (horizontalMovement > 0 && verifyDirection == true)
            {
                direction();
            }
            else if (horizontalMovement < 0 && verifyDirection == false)
            {
                direction();
            }

            if(Input.GetMouseButtonDown(0) && ammo>0)
            {
                shoot();
                ammo--;
                animation.SetTrigger("IdleShoot");
                Destroy(bullet.gameObject, 3f);
;            }

            if (jumpAttack == true)
            {
                animation.SetTrigger("JumpMelee");
                animation.SetBool("Jump?", true);
            }

            if (Input.GetMouseButtonDown(1))
            {
                melee();
                animation.SetTrigger("Melee"); 
            }

        }
        else
        {
            if (ammo == 9)
            {
                animation.SetTrigger("Death");
                audioManager.SFXmanager(audioManager.secretDeathSFX, 0.2f);
                speed = 0;
                gameOverPanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                animation.SetTrigger("Death");
                audioManager.SFXmanager(audioManager.deathSFX, 0.2f);
                speed = 0;
                gameOverPanel.SetActive(true);
                Time.timeScale = 0;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }


    }

    public void direction()
    {
        verifyDirection = !verifyDirection;

        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        bulletSpeed *=-1;

        bulletDirection.GetComponent<SpriteRenderer>().flipX = verifyDirection;
    }

    public void jump()
    {
        if (Input.GetButtonDown("Jump") && sensor == true && jumpAttack==false)
        {
            rbPlayer.AddForce(new Vector2 (0, playerJump));
        }

        if (Input.GetMouseButtonDown(1) && sensor==false)
        {
            jumpAttack = true;
        }
        else if(Input.GetMouseButtonDown(1) && sensor==true)
        {
            jumpAttack=false;
        }
        animation.SetBool("Jump?", false);
        animation.SetBool("Sensor", sensor);
    }

    public void detectGround()
    {
        sensor = Physics2D.OverlapCircle(positionSensor.position, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HP"))
        {
            HP++;
            Destroy(collision.gameObject);
            audioManager.SFXmanager(audioManager.colectableHPSFX, 0.3f);
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            HP--;
            audioManager.SFXmanager(audioManager.damagedSFX, 0.75f);
        }

        if (collision.gameObject.CompareTag("ammo"))
        {
            ammo++;
            Destroy(collision.gameObject);
            audioManager.SFXmanager(audioManager.colectableAmmoSFX, 0.4f);
        }

        if (collision.gameObject.CompareTag("laser"))
        {
            HP--;
            Destroy(collision.gameObject);
            audioManager.SFXmanager(audioManager.damagedSFX, 0.75f);
        }

        if (collision.gameObject.CompareTag("instakill"))
        {
            HP=0;
        }

        if (collision.gameObject.CompareTag("coin"))
        {
            points += 5;
            Destroy(collision.gameObject);
            audioManager.SFXmanager(audioManager.coinSFX, 0.75f);
        }

        if (collision.gameObject.CompareTag("gameEnd"))
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
            audioManager.SFXmanager(audioManager.winSFX, 0.75f);
        }

    }

    public void restart()
    {
        SceneManager.LoadScene("Gameplay", 0);
        Time.timeScale = 1.0f;
    }

    public void exit()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void backToGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void shoot()
    {
        GameObject temporary = Instantiate(bullet);

        temporary.transform.position = bulletPosition.transform.position;

        temporary.GetComponent<Rigidbody2D>().velocity = new Vector2 (bulletSpeed+5, 0);
    }

    public void melee()
    {
        GameObject temporary = Instantiate(sword);
        temporary.transform.position = swordPosition.transform.position;
        Destroy(temporary.gameObject,1);
    }

    public void jumpMelee()
    {
        jumpAttack = false;
    }



}
