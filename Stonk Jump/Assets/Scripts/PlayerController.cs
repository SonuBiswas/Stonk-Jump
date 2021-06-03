using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    // Start is called before the first frame update
   // public VariableJoystick joystick;
    public Transform player;
    public float speed;
    public float slideSpeed;
    
    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        
    }


    private Vector2 startTouch;
    private Vector2 EnddTouch;
    private Vector2 DeltaTouch;
    
    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            EnddTouch = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            EnddTouch = Input.mousePosition;
            DeltaTouch = startTouch - EnddTouch;
        }
        
        
    
        player.position = new Vector3 (Mathf.Clamp(Mathf.Lerp(player.position.x, player.position.x - (DeltaTouch.x*0.01f) , slideSpeed)
            , -4.1f, 4.1f),player.position.y, player.position.z+Time.deltaTime*speed);
        
        
        
        startTouch = EnddTouch;
        //Debug.Log("Joy stick"+joystick.Horizontal*Time.deltaTime*slideSpeed);
    }
    

}
