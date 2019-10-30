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
        public GameObject[] MiniObject;     // ミニオブジェクトのプレハブ
        public GameObject[] Object;         // オブジェクトのプレハブ
        public GameObject[] Card;           // カードのプレハブ
        public GameObject BattingEffect;    // バッティングエフェクトのプレハブ
        public GameObject BuildingEffect;   // ビルディングエフェクトのプレハブ
        public GameObject RankUpEffect;     // ランクアップエフェクトのプレハブ
        public GameObject ClosedEffect;     // 廃業のエフェクトのプレハブ
        public List<GameObject> CardObj;
        public GameObject MainCamera;
        private GlobalParamenter _prm = new GlobalParamenter();
        public GameObject[][] _AllFieldObj;
        private List<List<int>> _DrawDataList = new List<List<int>>();
        private bool _IsDrawFaze;
        private int _NowDrowFaze;

        public List<float> SetTime;
        public float WaitTime;
        public bool isCardMove;

        enum Faze
        {
            CardCreate,
            CardMove,
            CreateEffect,
            CreateObject,
            End
        };

        List<int> FazeList = new List<int>();
        //public int NowFaze;
        public List<int> NowFase = new List<int>();
        public List<int> Count = new List<int>();
        public bool isDrawInit;
        public Vector3[] CardPos = new Vector3[4];

        public Vector3[] View = new Vector3[4];
        public int[] SubScript = new int[] { 1, 2, 3, 0, 1, 2, 3 };
        public int pNum;    // プレイヤーナンバー

        void Start()
        {
            // テスト
            List<int> testData = new List<int>();
            //testData = new List<int>() { 99, 4, 4, 1, 2, 3 ,4};   //1
            testData = new List<int>() { 1, 0, 0, 1 };              //2
            //testData = new List<int>() { 1, 4, 4, 5, 4 ,1};       //3
            //testData = new List<int>() { 4,4 };                   //4
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 5, 5, 3 };
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 7, 4, 4 };
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 0, 5, 2 };
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 7, 0, 2 };
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 1, 1, 3 };
            _DrawDataList.Add(testData);
            testData = new List<int>() { 1, 3, 4, 4 };
            _DrawDataList.Add(testData);

            _NowDrowFaze = 2;
            _IsDrawFaze = true;

            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                SetTime.Add(0.0f);
            }
            WaitTime = 1.0f;
            //NowFaze = 0;
            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                NowFase.Add(0);
            }

            isCardMove = false;
            isDrawInit = true;

            pNum = 4;

            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                FazeList.Add((int)Faze.CardCreate);
            }

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
            if (_IsDrawFaze)
            {
                // 初期処理
                if (isDrawInit)
                {
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        Count.Add(i);
                    }
                }
                DrawMain();
            }
            else
            {
                FazeList.Clear();
            }
        }

        #region 描画メイン

        /// <summary>
        /// 描画メイン
        /// </summary>
        private void DrawMain()
        {
            //Debug.Log(Time.time);
            //if (SetTime + WaitTime < Time.time)
            //{
                //各描画フェーズに合わせた描画
                switch (_NowDrowFaze)
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

                    case (int)DrawFaze.DrawClosed:      // 廃業描画
                        DrawClosed();
                        break;

                    case (int)DrawFaze.DrawEnd:                     //描画終了
                        DrawEnd();
                        break;

                    default:                                        //描画不明
                        Debug.Log("存在しない描画フェイズに入ろうとしてますよー");
                        break;
                }             
                
            //}

        }


        #endregion

        #region アクション描画

        #endregion

        #region バッティング描画

        /// <summary>
        /// バッティング描画メイン
        /// </summary>
        private void DrawBatting()
        {
            while (_DrawDataList[0][0] != (int)CardName.DrawBattingEnd)
            {
                CreateBattingEffect(_DrawDataList[0]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawBattingEnd)
            {
                _NowDrowFaze++;
                SetTime[0] = Time.time;
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
            Debug.Log("バッティングエフェクト生成完了");
            //カラーはdata[3]~
        }

        #endregion

        #region オブジェクト描画

        /// <summary>
        /// オブジェクト描画のメイン部分です
        /// </summary>
        private void DrawObject()
        {
            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                if (SetTime[i] + WaitTime < Time.time)
                {
                    switch (NowFase[i])
                    {
                        case (int)Faze.CardCreate:
                            CreateCard(_DrawDataList[i], i);               // カード生成
                            NowFase[i]++;
                            SetTime[i] = Time.time;
                            break;
                        case (int)Faze.CardMove:
                            if (MoveCard(_DrawDataList[i], i))          // カード移動
                            {
                                Debug.Log("カードの移動完了");
                                NowFase[i]++;
                                SetTime[i] = Time.time;
                            }
                            break;
                        case (int)Faze.CreateEffect:
                            Destroy(CardObj[i]);
                            CreateBuildingEffect(_DrawDataList[i]);     // ビルディングエフェクト描画      
                            NowFase[i]++;
                            //SetTime = Time.time;
                            break;

                        case (int)Faze.CreateObject:
                            //描画開始
                            switch (_DrawDataList[i][0])
                            {
                                case (int)CardName.Restaurants:
                                    CreateRestaurants(_DrawDataList[i]);
                                    Debug.Log("レストランのオブジェクト生成完了");
                                    break;

                                case (int)CardName.TapiocaShop:
                                    CreateTapiocaShop(_DrawDataList[i]);
                                    Debug.Log("タピオカ屋のオブジェクト生成完了");
                                    break;

                                case (int)CardName.Supermarket:
                                    CreateSupermarket(_DrawDataList[i]);
                                    Debug.Log("スーパーマーケットのオブジェクト生成完了");
                                    break;

                                case (int)CardName.AmusementPark:
                                    CreateAmusementPark(_DrawDataList[i]);
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
                            NowFase[i]++;
                            SetTime[i] = Time.time;
                            break;
                    }
                    if (NowFase[i] == (int)Faze.End)
                    {
                        _DrawDataList.RemoveAt(i);
                        i--;
                    }
                }
            }

            //オブジェクト描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawObjEnd)
            {
                _NowDrowFaze++;
                SetTime[0] = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        #region カード描画
        /// <summary>
        /// カード生成
        /// </summary>
        private void CreateCard(List<int> data, int i)
        {
            //Vector3 pos;
            //pos = ConvertArrayToWorldPos(data[1], data[2]);
            //pos.y += 5.0f;

            // カードを生成
            CardObj.Add(null);
            CardObj[i] = Instantiate(Card[data[0]], View[SubScript[data[3] - pNum + 3]], transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("カードの生成完了");

            //CardPos[data[3]] = CardObj[i].transform.position;
        }
        #endregion

        #region カード移動描画
        // <summary>
        /// カード移動
        /// </summary>
        private bool MoveCard(List<int> data, int num) 
        {

            float Distance = Vector3.Distance(View[SubScript[data[3] - pNum + 3]], ConvertArrayToWorldPos(data[1], data[2]));
            float Speed = Distance / 10.0f;

            Vector3 EndPos = ConvertArrayToWorldPos(data[1], data[2]);

            // カード移動
            CardObj[num].transform.position = Vector3.MoveTowards(CardObj[num].transform.position, new Vector3(EndPos.x, EndPos.y + 0.5f, EndPos.z), Speed);

            //移動完了かどうか
            if(Count[num] >= 11)
            {        
                return true;
            }

            Count[num]++;

            //移動完了していない
            return false;
        }

        #endregion

        #region レストラン描画

        private void CreateRestaurants(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(MiniObject[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

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
            Instantiate(MiniObject[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

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
            Instantiate(MiniObject[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

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
            Instantiate(MiniObject[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);

            //カラーはdata[3]
        }

        #endregion

        #region ビルディングエフェクト描画
        /// <summary>
        /// ビルディングエフェクトの生成
        /// </summary>
        /// <param name="data"></param>
        private void CreateBuildingEffect(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(BuildingEffect, pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("ビルディングエフェクト生成完了");

            //カラーはdata[3]~

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
                // ランクアップエフェクトの生成
                CreateRankUpEffect(_DrawDataList[0]);

                CreateRankUp(_DrawDataList[0]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawRankUpEnd)
            {
                _NowDrowFaze++;
                SetTime[0] = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        private void CreateRankUp(List<int> data)
        {
            //1つ目のオブジェを消す
            Vector2Int DestroyObj = new Vector2Int(data[1], data[2]);
            Destroy(_AllFieldObj[DestroyObj.y][DestroyObj.x].GetComponent<TileData_Maneger>().childObj);

            //２つ目のオブジェのメッシュだけを消す
            Vector2Int DeleteMeshObj = new Vector2Int(data[3], data[4]);
            _AllFieldObj[DeleteMeshObj.y][DeleteMeshObj.x].GetComponent<Renderer>().enabled = false;

            //おっきいのひょうじ
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.z -= ((data[1] - data[3]) / 2.0f);  // X(2D)
            pos.x -= ((data[2] - data[4]) / 2.0f);  // Y(2D)
            pos.y += 0.5f;

            //ランクアップ後のオブジェクトを生成
            Instantiate(Object[data[0]], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("ランクアップ後の建物生成完了");
        }

        /// <summary>
        /// ランクアップエフェクトの生成
        /// </summary>
        /// <param name="data"></param>
        private void CreateRankUpEffect(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.z -= ((data[1] - data[3]) / 2.0f);
            pos.x -= ((data[2] - data[4]) / 2.0f);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(RankUpEffect, pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("ランクアップエフェクト生成完了");

            //カラーはdata[3]~
        }


        #endregion

        #region 廃業描画
        /// <summary>
        /// 廃業描画メイン
        /// </summary>
        private void DrawClosed()
        {
            while (_DrawDataList[0][0] != (int)CardName.DrawClosedEnd)
            {
                // 廃業エフェクトの生成
                CreateClosedEffect(_DrawDataList[0]);

                // 建物を削除
                Destroy(_DrawDataList[0]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(0);
            }

            //廃業描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawClosedEnd)
            {
                _NowDrowFaze++;
                SetTime[0] = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        // 建物を削除
        private void Destroy(List<int> data)
        {
            Vector2Int DestroyObj = new Vector2Int(data[0], data[1]);
            Destroy(_AllFieldObj[DestroyObj.y][DestroyObj.x].GetComponent<TileData_Maneger>().childObj);
            Debug.Log("廃業完了");
        }

        /// <summary>
        /// 廃業エフェクトの生成
        /// </summary>
        /// <param name="data"></param>
        private void CreateClosedEffect(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(ClosedEffect, pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("廃業エフェクト生成完了");

            //カラーはdata[3]~
        }

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
        public Vector3 ConvertArrayToWorldPos(int x, int y)
        {
            Vector3 outpos = new Vector3();

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

        // プレイヤーナンバーとカードナンバーを比較
        public Vector3 InitPnumToCnum(int pNum, int data)
        {
            return View[SubScript[data - pNum + 3]];         
        }

    }
}