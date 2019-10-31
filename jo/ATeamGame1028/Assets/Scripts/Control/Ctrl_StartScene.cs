/***
*
*       Title :"B-POP"
*
*        コントローラー：開始スクリーン
*
*       Description:
*
*
*       Data : 2019.10.9
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using kernal;

namespace Ctrl
{
    public class Ctrl_StartScene : BaseControl
    {
        public static Ctrl_StartScene Instance;                             //クラスの実体
        public AudioClip AucBackground;                                     //BGM  audio cut

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            AudioManger.SetAudioBackgroundVolumns(0.5F);
            AudioManger.SetAudioEffectVolumns(1F);
            AudioManger.PlayBackground(AucBackground);
        }

        internal void ClickNewGame()
        {
            NetworkMangerCustom.NetworkGame();

            //次のスクリーンに入る
            StartCoroutine("EnterNextScenes");
        }

        /// <summary>
        /// 次のシーンに入る
        /// </summary>
        /// <returns></returns>
        IEnumerator EnterNextScenes()
        {
            //スクリーンフェードアウト
            //FadeInAndOut.Instance.SetScenesToBlack();
            yield return new WaitForSeconds(1.5F);
            //次のシーンに入る
            base.EnterNextScenes(Global.ScenesEnum.GameScenes);
        }
    }
}

