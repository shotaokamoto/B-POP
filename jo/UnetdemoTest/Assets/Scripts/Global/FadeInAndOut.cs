/***
*
*       Title :"B-POP"
*
*        グローバル層：スクリーンフェードインandアウト
*
*       Description:
*
*
*       Data : 2019.10.09
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOut : MonoBehaviour
{
    public static FadeInAndOut Instance;                                                              //クラスの例

    public float FloColorChangeSpeed = 1F;                                                      //カラー変更スビート

    public GameObject goRawImage;                                                                  //RawImageオブジェクト

    private RawImage _RawImage;                                                                       //RawImageコンポーネント

    private bool _BoolScenesToClear = true;        //スクリーンフェードイン
    private bool _BoolScenesToBlack = false;      //スクリーンフェードアウト

    void Awake()
    {
        //クラスの実態を取得
        Instance = this;

        //RawImageコンポーネント取得
        if (goRawImage)
        {
            _RawImage = goRawImage.GetComponent<RawImage>();
        }
    }

    /// <summary>
    /// フェードイン設置
    /// </summary>
    public void SetScenesToClear()
    {
        _BoolScenesToClear = true;
        _BoolScenesToBlack = false;
    }

    /// <summary>
    /// フェードアウト設置
    /// </summary>
    public void SetScenesToBlack()
    {
        _BoolScenesToClear = false;
        _BoolScenesToBlack = true;
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    private void FadeToClear()
    {
        //colorのα値演算
        _RawImage.color = Color.Lerp(_RawImage.color, Color.clear, FloColorChangeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    private void FadeToBlack()
    {
        //colorのα値演算
        _RawImage.color = Color.Lerp(_RawImage.color, Color.black, FloColorChangeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// スクリーンフェードイン
    /// </summary>
    private void ScenesToClear()
    {
        FadeToClear();
        if (_RawImage.color.a <= 0.05)
        {
            _RawImage.color = Color.clear;
            _RawImage.enabled = false;
            _BoolScenesToClear = false;
        }
    }

    /// <summary>
    /// スクリーンフェードアウト
    /// </summary>
    private void ScenesToBlack()
    {
        _RawImage.enabled = true;
        FadeToBlack();
        if (_RawImage.color.a >= 0.95)
        {
            _RawImage.color = Color.black;
            _BoolScenesToBlack = false;
        }
    }

    void Update()
    {
        if (_BoolScenesToClear)
        {
            //スクリーンフェードイン
            ScenesToClear();
        }
        else if (_BoolScenesToBlack)
        {
            //スクリーンフェードアウト
            ScenesToBlack();
        }
    }




}
