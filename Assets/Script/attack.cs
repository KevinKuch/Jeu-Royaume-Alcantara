using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject playerObj;
    public GameObject weapon;
    private PlayerDeplacement pm;
    public float attackColdown;
    public float coldownPAA;
    public float attackColdownOne;
    public float attackColdownDeux;
    public bool attacking = false;
    public bool performingAnAttack = false;
    public float countUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myInput();
        
    }

    public void myInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerObj.GetComponent<Animator>().SetBool("attacking", true);
            playerObj.GetComponent<Animator>().SetTrigger("attackT");
            Invoke(nameof(resetAttackColdown), attackColdown);
            Invoke(nameof(resetFirstAttack), 2f);
            Invoke(nameof(startAnimation), coldownPAA);
        }
    }

    public void resetAttackColdown()
    {
        attacking = false;
        playerObj.GetComponent<Animator>().SetBool("attacking", false);
        playerObj.GetComponent<Animator>().SetBool("performingAnAttack", false);
        performingAnAttack = false;
       
    }
    public void startAnimation()
    {
        performingAnAttack = true;
        playerObj.GetComponent<Animator>().SetBool("performingAnAttack", true);
        playerObj.GetComponent<Animator>().SetBool("resetFirstAttack", true);
        
    }
    public void resetFirstAttack()
    {
        playerObj.GetComponent<Animator>().SetBool("resetFirstAttack", false);
    }
    
}
