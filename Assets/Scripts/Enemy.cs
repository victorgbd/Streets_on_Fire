using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float maxVelocidad=4.0f;
    //public float jumpforce = 400;
    public float minHeight ;
    public float maxHeight ;
    public float damageTime = 0.5f;
    public int maxHealth;
    public float attackrate=1f;
    public string enemyname;
    public Sprite enemyImage;
    public AudioClip collisionSound, deadSound;

    private int currenthealth;
    private float currentvel;
    private Rigidbody _rb;
    private Transform groundcheck;
    protected Animator _anim;
    private bool onGround;
    protected bool facingRight = false;
    private Transform target;
    protected bool isDead = false;
    private float zforce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer;
    private float nextAttack;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        groundcheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        currenthealth = maxHealth;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _anim.SetBool("Grounded",onGround);
        _anim.SetBool("Dead",isDead);
        if(!isDead){
            facingRight = (target.position.x < transform.position.x) ? false : true;
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            
        }
        

        if (damaged && !isDead)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damaged = true;
                damageTimer = 0;
            }
        }

        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDistance = target.position - transform.position;
            float hforce = targetDistance.x / Mathf.Abs(targetDistance.x);
            if (walkTimer >= Random.Range(1f, 2f))
            {
                zforce = Random.Range(-1, 2);
                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 1.5f)
            {
                hforce = 0;
            }

            if (!damaged)
                _rb.velocity = new Vector3(hforce * currentvel, 0, zforce * currentvel);
            _anim.SetFloat("Speed", Mathf.Abs( currentvel));
            if (Mathf.Abs(targetDistance.x) < 1.5f && Mathf.Abs(targetDistance.z) < 1.5f && Time.time > nextAttack)
            {
                _anim.SetTrigger("Attack");
                currentvel = 0;
                nextAttack = Time.time + attackrate;
            }
        }

        _rb.position = new Vector3(
            _rb.position.x,
            _rb.position.y,
            Mathf.Clamp(_rb.position.z, minHeight , maxHeight));
    }

    public void TookDamage(int damage)
    {
        if (!isDead)
        {
            damaged = true;
            currenthealth -= damage;
            _anim.SetTrigger("HitDamage");
            PlaySong(collisionSound);
            FindObjectOfType<UIManager>().UpdateEnemyUI(maxHealth,currenthealth,enemyname,enemyImage);
            if (currenthealth<=0)
            {
                isDead = true;
                PlaySong(deadSound);
                _rb.AddRelativeForce(new Vector3(3,5,0),ForceMode.Impulse);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }
    void ResetSpeed()
    {
        currentvel = maxVelocidad;
    }
    public void PlaySong(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
