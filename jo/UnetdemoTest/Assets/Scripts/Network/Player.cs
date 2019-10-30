﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private Rigidbody rb;
    public float FloSpeed = 5F;

    //public Transform CardRotion;                                                  //方向（姿勢）
    //public GameObject CardPrefab;                                             //カード


    public MeshRenderer[] Cuberender;                                       //Player

    [HideInInspector]
    [SyncVar]
    public int teamIndex = 0;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    public override void OnStartLocalPlayer()
    {
        
    }

    public override void OnStartClient()
    {
        Team team = GameMgr.GetInstance().teams[teamIndex];
        for (int i = 0; i < Cuberender.Length; i++)
        {
            Cuberender[i].material = team.material;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;                                                 //自分クライアントのオブジェクトを操作する

        Vector2 moveDir;
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            moveDir.x = 0;
            moveDir.y = 0;
        }
        else
        {
            //移動できる
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.y = Input.GetAxis("Vertical");
            Move11(moveDir);
        }
    }

    void Move11(Vector2 direction = default(Vector2))
    {
        if (direction != Vector2.zero)
        {
            //回転方向
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            //前の距離を計算
            Vector3 movementDir = transform.forward * FloSpeed * Time.deltaTime;
            //移動
            rb.MovePosition(rb.position + movementDir);
        }
    }

}