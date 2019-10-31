/***
*
*       Title :"B-POP"
*
*        ビュー層：コストテクスチャマネージャ
*
*       Description:
*            機能：
*               コストデータの値によって、テクスチャを変える
*        
*       Data : 2019.
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICostManger : MonoBehaviour
{
    public GameObject ImageCost3;
    public GameObject ImageCost2;
    public GameObject ImageCost1;

    private void Awake()
    {
        ImageCost3.SetActive(true);
        ImageCost2.SetActive(false);
        ImageCost1.SetActive(false);
    }

    private void FixedUpdate()
    {
        switch (UIMgr.instance.nowCost)
        {
            case 0:
                DisPlayerCost0();
                break;
            case 1:
                DisPlayerCost1();
                break;
            case 2:
                DisPlayerCost2();
                break;
            case 3:
                DisPlayerCost3();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// コスト３時、描画
    /// </summary>
    public void DisPlayerCost3()
    {
        ImageCost3.SetActive(true);
        ImageCost2.SetActive(false);
        ImageCost1.SetActive(false);
    }

    /// <summary>
    ///  コスト２時、描画
    /// </summary>
    public void DisPlayerCost2()
    {
        ImageCost3.SetActive(false);
        ImageCost2.SetActive(true);
        ImageCost1.SetActive(false);
    }

    /// <summary>
    ///  コスト１時、描画
    /// </summary>
    public void DisPlayerCost1()
    {
        ImageCost3.SetActive(false);
        ImageCost2.SetActive(false);
        ImageCost1.SetActive(true);
    }

    /// <summary>
    ///  コスト０時、描画
    /// </summary>
    public void DisPlayerCost0()
    {
        ImageCost3.SetActive(false);
        ImageCost2.SetActive(false);
        ImageCost1.SetActive(false);
    }
}
