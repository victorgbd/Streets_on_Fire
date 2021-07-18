using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxVelocidad=4.0f;
    public float jumpforce = 400;
    public float minHeight ;
    public float maxHeight ;
    public int maxhealth = 10;
    public string playername;
    public Sprite playerimage;
    public AudioClip collisionSound, jumpSound, healtitemSong;

    private int currentHealt;
    private float currentvel;
    private Rigidbody _rb;

    private Animator _anim;

    private Transform groudcheck;

    private bool onGround;

    private bool isdead=false;

    private bool facingRight = true;

    private bool jump = false;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        groudcheck = gameObject.transform.Find("GroundCheck");
        currentvel = maxVelocidad;
        currentHealt = maxhealth;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Linecast(transform.position,groudcheck.position,1<<LayerMask.NameToLayer("Ground"));
        _anim.SetBool("OnGround",onGround);
        _anim.SetBool("Dead",isdead);
        
        if (Input.GetButtonDown("Jump")&&onGround)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if (!isdead)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (!onGround) v = 0;
            _rb.velocity = new Vector3(h * currentvel, _rb.velocity.y, v * currentvel);

            if (onGround)
            {
                _anim.SetFloat("Speed",Mathf.Abs(_rb.velocity.magnitude));
            }
            if (h > 0 && !facingRight)
            {
                flip();
            }
            else if (h<0 && facingRight)
            {
                flip();
            }

            if (jump)
            {
                jump = false;
                _rb.AddForce(Vector3.up * jumpforce);
                PlaySong(jumpSound);
            }

            float minWith = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWith = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            
            _rb.position = new Vector3(Mathf.Clamp(_rb.position.x, minWith + 1, maxWith - 1),
                _rb.position.y,Mathf.Clamp(_rb.position.z, minHeight , maxHeight));
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }

    void ZeroSpeed()
    {
        currentvel = 0;
    }

    void ResetSpeed()
    {
        currentvel = maxVelocidad;
    }
    public void TookDamage(int damage)
    {
        if (!isdead)
        {
            
            currentHealt -= damage;
            _anim.SetTrigger("HitDamage");
            FindObjectOfType<UIManager>().UpdateHealth(currentHealt);
            PlaySong(collisionSound);
            if (currentHealt <= 0)
            {
                isdead = true;
                FindObjectOfType<GameManger>().numvidas--;
                FindObjectOfType<UIManager>().Updatelives();
                if (facingRight)
                {
                    _rb.AddForce(new Vector3(-3,5,0),ForceMode.Impulse);
                }
                else
                {
                    _rb.AddForce(new Vector3(3,5,0),ForceMode.Impulse);
                }
            }
        }
    }
    public void PlaySong(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Health Item"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Destroy(other.gameObject);
                _anim.SetTrigger("Catching");
                PlaySong(healtitemSong);
                currentHealt = maxhealth;
                FindObjectOfType<UIManager>().UpdateHealth(currentHealt);
            }
        }
    }

    void PlayerRespawn()
    {
        isdead = false;
        currentHealt = maxhealth;
        FindObjectOfType<UIManager>().UpdateHealth(currentHealt);
        _anim.Rebind();
        float minWith = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        transform.position = new Vector3(minWith, 10, -4);
    }
}
