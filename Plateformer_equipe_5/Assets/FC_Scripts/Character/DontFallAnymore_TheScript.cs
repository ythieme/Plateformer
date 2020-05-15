﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontFallAnymore_TheScript : MonoBehaviour
{
    RaycastHit2D uponTheGroundHit;
    Controler_YT controler;
    public BoxCollider2D boxCollider;

    bool playerIsInGround;

    [SerializeField] float underRayLenght = 0.09f;
    [SerializeField] float underRayPositionY = 0.13f;
    [SerializeField] float underRayPositionX = 0.09f;

    float crouchDecalageY = 0f;
    public LayerMask collisionMask;

    private void Start()
    {
        controler = GetComponent<Controler_YT>();
    }

    private void Update()
    {
        SizeCheck();
        InGroundCheck();

        if (playerIsInGround)
        {
            transform.localPosition = transform.localPosition + (new Vector3(0f, 0.02f, 0f));
        }
    }
    
    void SizeCheck()
    {
        if (controler.isCrouching || controler.isSliding) crouchDecalageY = 0.22f;
        else crouchDecalageY = 0f;
    }

    private void InGroundCheck()
    {
        Vector3 startPositionRaycastUponTheGround = new Vector3(boxCollider.bounds.center.x - underRayPositionX,
            boxCollider.bounds.center.y - underRayPositionY + crouchDecalageY, transform.position.z) ; 

        uponTheGroundHit = Physics2D.Raycast(startPositionRaycastUponTheGround, transform.TransformDirection(new Vector2(1f, 0f)), underRayLenght, collisionMask);

        Debug.DrawRay(startPositionRaycastUponTheGround, transform.TransformDirection(new Vector2(underRayLenght, 0)), Color.magenta);

        if (uponTheGroundHit.collider != null)
        {
            playerIsInGround = true;
        }
        else
        {
            playerIsInGround = false;
        }
    }    
}
