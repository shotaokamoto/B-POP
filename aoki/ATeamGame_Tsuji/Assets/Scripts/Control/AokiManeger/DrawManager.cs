using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Global;
using System.Linq;

namespace Ctrl
{
    public class DrawManager : MonoBehaviour
    {
        public GameObject[] Prefub;
        public GameObject BattingEffect;
        private GlobalParamenter _prm = new GlobalParamenter();
        public GameObject[][] _AllFieldObj;
        private List<List<int>> _DrawDataList = new List<List<int>>();
        private bool _IsDrawFaze;
        private int _NowDrowFaze;

        void Start()
        {
            //リスト新規作成
            _AllFieldObj = new GameObject[_prm.FieldSize.y][];
            for (int i = 0; i < _prm.FieldSize.y; i++)
            {
                _AllFieldObj[i] = new GameObject[(int)_prm.FieldSize.y];
            }

            //タイルをソート
            GameObject[] allFieldData = GameObject.FindGameObjectsWithTag("Stage");

            for (int i = 0; i < allFieldData.Length; i++)
            {
                _AllFieldObj[(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.y]
                            [(int)allFieldData[i].GetComponent<TileData_Maneger>().planePos.x] = allFieldData[i];
            }
        }

        //描画
        private void FixedUpdate()
        {
            //描画フェーズがtrueなら描画開始
            if(_IsDrawFaze)
            {
                DrawMain();
            }
        }

        #region 描画メイン

        /// <summary>
        /// 描画メイン
        /// </summary>
        private void DrawMain()
        {
            //各描画フェーズに合わせた描画
            switch(_NowDrowFaze)
            {
                case (int)DrawFaze.DrawAct:
                    break;

                case (int)DrawFaze.DrawBatting:                 //バッティング描画
                    DrawBatting();
                    break;

                case (int)DrawFaze.DrawObj:                    //オブジェ描画
                    DrawObject();
                    break;

                case (int)DrawFaze.DrawRankUp:                  //ランクアップ描画
                    DrawRankUp();
                    break;

                case (int)DrawFaze.DrawClosed:
                    break;

                case (int)DrawFaze.DrawEnd:                     //描画終了
                    DrawEnd();
                    break;

                default:                                        //描画不明
                    Debug.Log("存在しない描画フェイズに入ろうとしてますよー");
                    break;
            }



        }


        #endregion

        #region アクション描画

        #endregion

        #region バッティング描画

        /// <summary>
        /// バッティング描画メイン
        /// </summary>
        private  void DrawBatting()
        {
            while(_DrawDataList[0][0] != (int)CardName.DrawBattingEnd)
            {
                CreateBattingEffect(_DrawDataList[0]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawBattingEnd)
            {
                _NowDrowFaze++;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        /// <summary>
        /// バッティングエフェクトの生成
        /// </summary>
        /// <param name="data"></param>
        private void CreateBattingEffect(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(BattingEffect, pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]~
        }

        #endregion

        #region オブジェクト描画

        /// <summary>
        /// オブジェクト描画のメイン部分です
        /// </summary>
        private void DrawObject()
        {
            //各オブジェクト同時描画
            while (_DrawDataList[0][0] != (int)CardName.DrawObjEnd)
            {
                //描画開始
                switch (_DrawDataList[0][0])
                {
                    case (int)CardName.Restaurants:
                        CreateRestaurants(_DrawDataList[0]);
                        Debug.Log("レストランのオブジェクト生成完了");
                        break;

                    case (int)CardName.TapiocaShop:
                        CreateTapiocaShop(_DrawDataList[0]);
                        Debug.Log("タピオカ屋のオブジェクト生成完了");
                        break;

                    case (int)CardName.Supermarket:
                        CreateSupermarket(_DrawDataList[0]);
                        Debug.Log("スーパーマーケットのオブジェクト生成完了");
                        break;

                    case (int)CardName.AmusementPark:
                        CreateAmusementPark(_DrawDataList[0]);
                        Debug.Log("遊園地のオブジェクト生成完了");
                        break;

                    //描画終了
                    case (int)CardName.DrawEnd:
                        Debug.Log("描画完了");
                        break;

                    //別コマンドはする―
                    default:
                        Debug.Log("存在しない建物を生成しようとしましたよー");
                        break;
                }
                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //オブジェクト描画終了が来たら描画フェーズを次の段階に進める
            if(_DrawDataList[0][0] == (int)CardName.DrawObjEnd)
            {
                _NowDrowFaze++;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        #region レストラン描画

        private void CreateRestaurants(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(Prefub[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]
        }


        #endregion

        #region タピオカ屋描画
        private void CreateTapiocaShop(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(Prefub[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]
        }

        #endregion

        #region スーパーマーケット描画

        private void CreateSupermarket(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(Prefub[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]
        }

        #endregion

        #region 遊園地描画
        private void CreateAmusementPark(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(Prefub[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]
        }

        #endregion

        #endregion

        #region ランクアップ描画

        /// <summary>
        /// ランクアップ描画メイン
        /// </summary>
        private void DrawRankUp()
        {
            while (_DrawDataList[0][0] != (int)CardName.DrawRankUpEnd)
            {
                CreateRankUp(_DrawDataList[0]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawRankUpEnd)
            {
                _NowDrowFaze++;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        private void CreateRankUp(List<int> data)
        {
            //1つ目のオブジェを消す
            Vector2Int DestroyObj = new Vector2Int(data[1],data[2]);
            Destroy(_AllFieldObj[DestroyObj.y][DestroyObj.x].GetComponent<TileData_Maneger>().childObj);

            //２つ目のオブジェのメッシュだけを消す
            Vector2Int DeleteMeshObj = new Vector2Int(data[3], data[4]);
            _AllFieldObj[DeleteMeshObj.y][DeleteMeshObj.x].GetComponent<Renderer>().enabled = false;

            //おっきいのひょうじ

        }

        #endregion

        #region 廃業描画

        #endregion

        #region 描画終了系


        private void DrawEnd()
        {
            //最初の描画リストを削除
            _DrawDataList.Clear();

            //描画フラグをへし折る
            _IsDrawFaze = false;
        }

        #endregion

        #region　補助関数

        /// <summary>
        /// 配列からワールド座標に変換します
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector3 ConvertArrayToWorldPos(int x,int y)
        {
            Vector3 outpos;

            outpos = _AllFieldObj[y][x].transform.position;

            return outpos;
        }


        #endregion

        /*カード属性データ*/
        public List<List<int>> DrawDataList
        {
            get
            {
                return _DrawDataList;
            }
            set
            {
                _DrawDataList = value;
            }
        }


    }
}