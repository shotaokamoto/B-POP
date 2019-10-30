/***
 * 　　　Title : "B-POP"
 * 
 *              機能：
 * 
 *                  MP4の素材でloading画面作り
 * 
 * 
 * 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Global
{
    public class PlayVideoUI : MonoBehaviour
    {
        //videoPlayerとImageのコンポーネント定義
        private VideoPlayer videoPlayer;
        private RawImage rawImage;

        //スクリプト初期化
        private void Awake()
        {
            //シーンのコンポーネント取得
            videoPlayer = this.GetComponent<VideoPlayer>();
            rawImage = this.GetComponent<RawImage>();
        }


        private void FixedUpdate()
        {
            //もしvideoPlayerの中に何も入れてない、return;
            if(videoPlayer.texture==null)
            {
                return;
            }

            //videoPlayerのビデオをRawImageで描画する。
            rawImage.texture = videoPlayer.texture;
        }
    }
}

