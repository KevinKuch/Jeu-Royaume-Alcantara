using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageDealer : MonoBehaviour
{
    public bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLenght;
    [SerializeField] float weaponDamage;

    // Start is called before the first frame update
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;

            if(Physics.Raycast(transform.position, - transform.up, out hit, weaponLenght, layerMask))
            {
                if (!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    print("damage");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }

        } 
    }

    public void StartDealDamge()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();   
    }

    public void endDealDamage()
    {
        canDealDamage = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLenght);
    }
}
