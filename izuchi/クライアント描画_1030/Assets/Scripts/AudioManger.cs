/***
 * 
 *  核心層：サウンドマネージャー 
 * 
 *  機能：　プロジェクトサウンドクリップ   
 * 
 *  Data :       2019.10.09
 * 
 */
using System.Collections;
using System.Collections.Generic;                                                  //OOPコレクション
using UnityEngine;

namespace kernal
{
    public class AudioManger : MonoBehaviour
    {
        public AudioClip[] AudioClipArray;                                            //クリップ配列
        public static float AudioBackgroundVolumns = 1F;                 //BGM音量
        public static float AudioEffectVolumns = 1F;                            //サウンドエフェクト音量

        private static Dictionary<string, AudioClip> _DicAudioClipLib;     //サウンドライブラリ
        private static AudioSource[] _AudioSourceArray;                            //サウンドリソース配列
        private static AudioSource _AudioSource_BackgroundAudio;      //BGM
        private static AudioSource _AudioSource_AudioEffectA;               //sound effectA
        private static AudioSource _AudioSource_AudioEffectB;               //sound effectB

        /// <summary>
        /// サウンドロード
        /// </summary>
        void Awake()
        {
            //サウンドライブラリロード
            _DicAudioClipLib = new Dictionary<string, AudioClip>();
            foreach (AudioClip audioClip in AudioClipArray)
            {
                _DicAudioClipLib.Add(audioClip.name, audioClip);
            }
            //サウンド処理
            _AudioSourceArray = this.GetComponents<AudioSource>();
            _AudioSource_BackgroundAudio = _AudioSourceArray[0];
            _AudioSource_AudioEffectA = _AudioSourceArray[1];
            _AudioSource_AudioEffectB = _AudioSourceArray[2];

            //データ永続性から音量データ　get
            if (PlayerPrefs.GetFloat("AudioBackgroundVolumns") >= 0)
            {
                AudioBackgroundVolumns = PlayerPrefs.GetFloat("AudioBackgroundVolumns");
                _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
            }
            if (PlayerPrefs.GetFloat("AudioEffectVolumns") >= 0)
            {
                AudioEffectVolumns = PlayerPrefs.GetFloat("AudioEffectVolumns");
                _AudioSource_AudioEffectA.volume = AudioEffectVolumns;
                _AudioSource_AudioEffectB.volume = AudioEffectVolumns;
            }
        }//Start_end

        /// <summary>
        /// display BGM
        /// </summary>
        /// <param name="audioClip">サウンドクリップ</param>
        public static void PlayBackground(AudioClip audioClip)
        {
            //BGM循環ディスプレイを防ぐ
            if (_AudioSource_BackgroundAudio.clip == audioClip)
            {
                return;
            }
            //グローバル音量管理処理
            _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
            if (audioClip)
            {
                _AudioSource_BackgroundAudio.loop = true;
                _AudioSource_BackgroundAudio.clip = audioClip;
                _AudioSource_BackgroundAudio.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] audioClip==null !");
            }
        }

        //BGM　display
        public static void PlayBackground(string strAudioName)
        {
            if (!string.IsNullOrEmpty(strAudioName))
            {
                PlayBackground(_DicAudioClipLib[strAudioName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] strAudioName==null !");
            }
        }

        /// <summary>
        /// sound effect_A display
        /// </summary>
        /// <param name="audioClip">サウンドクリップ</param>
        public static void PlayAudioEffectA(AudioClip audioClip)
        {
            //グローバル音量管理処理
            _AudioSource_AudioEffectA.volume = AudioEffectVolumns;

            if (audioClip)
            {
                _AudioSource_AudioEffectA.clip = audioClip;
                _AudioSource_AudioEffectA.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectA()] audioClip==null ! Please Check! ");
            }
        }

        /// <summary>
        /// sound effect_B display
        /// </summary>
        /// <param name="audioClip">サウンドクリップ</param>
        public static void PlayAudioEffectB(AudioClip audioClip)
        {
            //グローバル音量管理処理
            _AudioSource_AudioEffectB.volume = AudioEffectVolumns;

            if (audioClip)
            {
                _AudioSource_AudioEffectB.clip = audioClip;
                _AudioSource_AudioEffectB.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectB()] audioClip==null ! Please Check! ");
            }
        }

        /// <summary>
        /// 播放音效_音频源A
        /// </summary>
        /// <param name="strAudioEffctName">音效名称</param>
        public static void PlayAudioEffectA(string strAudioEffctName)
        {
            if (!string.IsNullOrEmpty(strAudioEffctName))
            {
                PlayAudioEffectA(_DicAudioClipLib[strAudioEffctName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectA()] strAudioEffctName==null ! Please Check! ");
            }
        }

        /// <summary>
        /// 播放音效_音频源B
        /// </summary>
        /// <param name="strAudioEffctName">音效名称</param>
        public static void PlayAudioEffectB(string strAudioEffctName)
        {
            if (!string.IsNullOrEmpty(strAudioEffctName))
            {
                PlayAudioEffectB(_DicAudioClipLib[strAudioEffctName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectB()] strAudioEffctName==null ! Please Check! ");
            }
        }

        /// <summary>
        /// BGM　音量　セット
        /// </summary>
        /// <param name="floAudioBGVolumns"></param>
        public static void SetAudioBackgroundVolumns(float floAudioBGVolumns)
        {
            _AudioSource_BackgroundAudio.volume = floAudioBGVolumns;
            AudioBackgroundVolumns = floAudioBGVolumns;
            //データ永続化
            PlayerPrefs.SetFloat("AudioBackgroundVolumns", floAudioBGVolumns);
        }

        /// <summary>
        /// サウンドクリップの音量　セット
        /// </summary>
        /// <param name="floAudioEffectVolumns"></param>
        public static void SetAudioEffectVolumns(float floAudioEffectVolumns)
        {
            _AudioSource_AudioEffectA.volume = floAudioEffectVolumns;
            _AudioSource_AudioEffectB.volume = floAudioEffectVolumns;
            AudioEffectVolumns = floAudioEffectVolumns;
            //データ永続化
            PlayerPrefs.SetFloat("AudioEffectVolumns", floAudioEffectVolumns);
        }
    }
}


