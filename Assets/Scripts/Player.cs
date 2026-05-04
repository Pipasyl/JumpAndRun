using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Fields

    public float moveSpeed;
    public float poweredUpTime = 10.0f;

    public float poweredUpScaleX = 2.0f;

    public Vector2 movementRangeSmall;
    public Vector2 movementRangeLarge;

    #endregion

    #region Private Fields

    private bool isPoweredUp = false;

    private float powerTimer = 0.0f;

    private Rigidbody rb;

    #endregion

    void Start()
    {
        this.rb = this.GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {

    }

    public void DeactivtePowerup()
    {

    }

    public void ActivatePowerup()
    {

    }
}