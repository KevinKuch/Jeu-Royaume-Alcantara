using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemieScript : MonoBehaviour
{
    [SerializeField] float health = 30;
    GameObject player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damageAmmount)
    {
        health -= damageAmmount;
        animator.SetTrigger("damage");

        if(health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(this.gameObject);
    }
}
