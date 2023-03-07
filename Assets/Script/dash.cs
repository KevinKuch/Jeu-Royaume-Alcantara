using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerDeplacement pm;
    public GameObject playerObj;

    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    public float dashCd;
    private float dashCdTimer;

    public KeyCode dashKey = KeyCode.E;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerDeplacement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            dashF();
        }
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void dashF()
    {
        if (dashCdTimer > 0)
        {
            return;
        }
        else dashCdTimer = dashCd;
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;

        Invoke(nameof(delayedDashForce), 0.025f);

        Invoke(nameof(resetDash), dashDuration);

        pm.dashing = true;
        playerObj.GetComponent<Animator>().SetBool("dash", true);

        Invoke(nameof(cancelAnim), dashDuration);
    }
    private Vector3 delayedForceToApply;

    private void delayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }


    private void resetDash()
    {
        pm.dashing = false;
    }
    private void cancelAnim()
    {
        playerObj.GetComponent<Animator>().SetBool("dash", false);
    }
}
