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
        public static DrawManager instance;

        public GameObject[] MiniObject;     // ミニオブジェクトのプレハブ
        public GameObject[] Object;         // オブジェクトのプレハブ
        public GameObject[] Card;           // カードのプレハブ

        public GameObject BattingEffect;    // バッティングエフェクトのプレハブ
        public GameObject BuildingEffect;   // ビルディングエフェクトのプレハブ
        public GameObject RankUpEffect;     // ランクアップエフェクトのプレハブ
        public GameObject ClosedEffect;     // 廃業のエフェクトのプレハブ

        public List<GameObject> CardObj;

        private GlobalParamenter _prm = new GlobalParamenter(6,6);
        public GameObject[][] _AllFieldObj;
        public List<List<int>> _DrawDataList = new List<List<int>>();
        public bool _IsDrawFaze;
        private int _NowDrowFaze;

        public float AllSetTime;    // 全体描画処理のセットする時間
        public List<float> SetTime; // オブジェクト描画処理のセットする時間
        public float WaitTime;      // 待ち時間
        public bool isCardMove;     // カードの移動終了判定

        enum ActionFaze
        {
            Mine,// 地雷
            Typhoon,// 台風
            Shuffle,// シャッフル
            Meteo,// メテオ
            ActionEnd
        };

        enum Faze
        {
            CardCreate,
            CardMove,
            CreateEffect,
            CreateObject,
            End
        };

        List<int> FazeList = new List<int>();
        List<int> ActionFazeList = new List<int>();

        public List<int> NowFase = new List<int>();
        public List<int> NowActionFase = new List<int>();

        public List<int> Count = new List<int>();
        public bool isDrawInit;
        public Vector3[] CardPos = new Vector3[4];
        public Vector3[] View = new Vector3[4];
        public int[] SubScript = new int[] { 1, 2, 3, 0, 1, 2, 3 };
        public int pNum;            // プレイヤーナンバー

        //
        private GlobalParamenter global = new GlobalParamenter(6,6);

        public const int mineNum = 3;

        //地雷の描画に必要なデータ
        public GameObject actMinePrefab;             //アクションカード「地雷」のプレハブ
        public const float mineMoveSecond1 = 50.0f;  //垂直に落ちる速度
        public const float mineMoveSecond2 = 61.0f;  //ランダムに移動している間の時間
        private int mineMoveEnd = 0;
        struct ActMine
        {
            public int count;
            public Vector3 startPos;     //開始位置
            public Vector3 endPos;      //着地点
            public float distance;      //移動距離
            public float speed;         //移動速度
            public GameObject moveObj;  //地雷の実態
        }

        ActMine[] actMine = new ActMine[mineNum];
        //メテオの描画に必要なデータ
        public GameObject actMeteorPreafab;       //アクションカード「メテオ」のプレハブ
        public GameObject actMeteorEffect;        //範囲を決めるエフェクトのプレハブ
        public const float meteorMoveSecond = 30.0f;    //メテオの動く速さ
        struct ActMeteor
        {
            public Vector3 startPos;      //開始位置
            public Vector3 endPos;        //着地点
            public float distance;        //移動距離
            public float speed;           //移動速度
            public int count;             //尺(移動間隔)
            public GameObject moveObj;    //メテオ実態
            public GameObject effectObj;  //エフェクト実態  
        }

        ActMeteor actMeteor;

        //地雷爆発に必要なデータ
        public GameObject mineBoomEffectPrefab;
        public List<GameObject> boomEffectList = new List<GameObject>();
        public bool isBoomEffect;

        enum ActFaze
        {
            set,
            move,
            effect,
            end
        }

        enum EffectFaze
        {
            set,
            effect,
            end
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            // テスト
            List<int> testData = new List<int>();

            //--------------
            //mineTest
            //--------------

            //testData = new List<int>() { 4 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 4 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 4 };
            //_DrawDataList.Add(testData);

            //-------------
            //meteorTest
            //-------------

            //testData = new List<int>() { 9, 2, 2, 1 };
            //_DrawDataList.Add(testData);

            //--------------------
            //mineBomdEffectTest
            //--------------------

            //testData = new List<int>() { 201, 0, 0, 1, 1, 2, 2, 3, 3 };
            //_DrawDataList.Add(testData);

            //testData = new List<int>() { 202 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 5 };
            //_DrawDataList.Add(testData);



            //1
            //testData = new List<int>() { 99, 4, 4, 1, 2 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 99, 2, 2, 3, 4 };
            //_DrawDataList.Add(testData);

            //2
            //testData = new List<int>() { 1, 0, 0, 1 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 5, 5, 3 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 7, 4, 4 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 0, 5, 2 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 7, 0, 2 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 1, 1, 3 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 3, 4, 4 };
            //_DrawDataList.Add(testData);

            //3
            //testData = new List<int>() { 1, 4, 4, 4, 5, 1 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 1, 2, 2, 3, 2, 1 };
            //_DrawDataList.Add(testData);

            //4
            //testData = new List<int>() { 4, 4 };
            //_DrawDataList.Add(testData);
            //testData = new List<int>() { 3, 3 };
            //_DrawDataList.Add(testData);

            // テスト
            _NowDrowFaze = 0;
            _IsDrawFaze = false;

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

            isBoomEffect = true;

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
                    // サーバからデータを受け取る
                    _DrawDataList = Model.RecvCliantDataProxy._Instance.DrawDataList; // カード描画での必要なデータ類を受け取る

                    // pNum = 1;   // 自分のプレイヤーナンバー


                    // 時間関連初期化
                    AllSetTime = 0.0f;
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        SetTime.Add(0.0f);
                    }
                    WaitTime = 1.0f;

                    // 
                    isCardMove = false;
                    isDrawInit = true;


                    // フェーズ関連初期化
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        NowFase.Add(0);
                    }
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        NowActionFase.Add(0);
                    }
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        FazeList.Add((int)Faze.CardCreate);
                    }
                    for (int i = 0; i < _DrawDataList.Count; i++)
                    {
                        ActionFazeList.Add((int)ActionFaze.Mine);
                    }
                }
                DrawMain();
            }
            
        }

        #region 描画メイン

        /// <summary>
        /// 描画メイン
        /// </summary>
        private void DrawMain()
        {
            if (AllSetTime + WaitTime < Time.time)
            {
                //各描画フェーズに合わせた描画
                switch (_NowDrowFaze)
                {
                    case (int)DrawFaze.DrawAct:
                        DrawAct(_DrawDataList[0]);                   //アクションカードの描画
                        break;

                    case (int)DrawFaze.DrawBatting: //バッティング描画
                        DrawBatting();
                        break;

                    case (int)DrawFaze.DrawObj:     //オブジェ描画                     
                        DrawObject();
                        break;

                    case (int)DrawFaze.DrawBoomMine:                 //地雷描画データ    
                        if (isBoomEffect)
                        {
                            _DrawDataList[0].RemoveAt(0);
                            isBoomEffect = false;
                        }
                        DrawBoomMine(_DrawDataList[0]);
                        break;

                    case (int)DrawFaze.DrawRankUp:  //ランクアップ描画
                        DrawRankUp();                       
                        break;

                    case (int)DrawFaze.DrawClosed:  // 廃業描画
                        DrawClosed();
                         break;

                    case (int)DrawFaze.DrawEnd:     //描画終了
                        DrawEnd();
                        break;

                    default:                                        //描画不明
                        Debug.Log("存在しない描画フェイズに入ろうとしてますよー");
                        break;
                }             
                
            }

        }


        #endregion

        #region アクション描画
        /// <summary>
        /// アクション描画のメイン部分
        /// </summary>
        void DrawAct(List<int> data)
        {
            switch (data[0])            //どのアクションカードか比較
            {
                case (int)CardName.Mine:
                    DrawActMine();
                    break;
                case (int)CardName.Typhoon:
                    DrawActTyphoon();
                    break;
                case (int)CardName.Shuffle:
                    DrawActShuffle();
                    break;
                case (int)CardName.Meteo:
                    DrawActMeteor();
                    break;
            }

            //アクション描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawActionEnd)
            {
                _NowDrowFaze++;
                AllSetTime = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }
        #endregion

        #region アクションカード「地雷」
        /// <summary>
        /// アクションカード「地雷」のメイン関数
        /// </summary>
        void DrawActMine()
        {
            if (SetTime[0] + WaitTime < Time.time)
            {
                switch (NowFase[0])
                {
                    case (int)ActFaze.set:
                        //移動する三つの地雷objectを生成
                        MakeActMine();
                        NowFase[0]++;
                        SetTime[0] = Time.time;
                        break;
                    case (int)ActFaze.move:
                        // 地雷オブジェクト移動
                        if (MoveActMine())
                        {
                            Debug.Log("地雷の移動完了");
                            NowFase[0]++;
                            SetTime[0] = Time.time;
                        }
                        break;
                    case (int)ActFaze.effect:
                        if (JumbledMoveActMine())
                        {
                            for (int j = 0; j < mineNum; j++)
                            {
                                Destroy(actMine[j].moveObj);
                            }
                            NowFase[0]++;
                        }
                        //SetTime = Time.time;
                        break;
                    case (int)ActFaze.end:
                        //データリストの中からアクションカード「地雷」のものを削除
                        _DrawDataList.RemoveAt(0);
                        NowFase.RemoveAt(0);
                        SetTime.RemoveAt(0);
                        break;
                }
            }
        }

        //地雷生成
        void MakeActMine()
        {
            //Mineオブジェクト生成
            for (int i = 0; i < mineNum; i++)
            {
                actMine[i].startPos.y = 10;
                actMine[i].startPos.z = ((global.FieldSize.x / (mineNum + 0.5f)) * (i - 1)) - 0.5f;
                actMine[i].moveObj = Instantiate(actMinePrefab, actMine[i].startPos, actMinePrefab.transform.rotation);
            }
        }
        //地雷の移動(縦に移動)
        bool MoveActMine()
        {
            for (int i = 0; i < mineNum; i++)
            {
                actMine[i].endPos = new Vector3(actMine[i].moveObj.transform.position.x, 0.0f, actMine[i].moveObj.transform.position.z);
                actMine[i].distance = Vector3.Distance(actMine[i].startPos, actMine[i].endPos);
                actMine[i].speed = actMine[i].distance / mineMoveSecond1;
            }
            for (int i = 0; i < mineNum; i++)
            {
                // 地雷移動
                actMine[i].moveObj.transform.position = Vector3.MoveTowards(actMine[i].moveObj.transform.position,
                                                                new Vector3(actMine[i].endPos.x, actMine[i].endPos.y + 0.5f, actMine[i].endPos.z),
                                                                            actMine[i].speed);
                //移動完了かどうか
                if (actMine[i].count >= mineMoveSecond1 + 1)
                {
                    mineMoveEnd++;
                }
                actMine[i].count++;
            }
            if (mineMoveEnd >= mineNum)
            {
                for (int i = 0; i < mineNum; i++)
                {
                    actMine[i].count = 0;
                }
                mineMoveEnd = 0;
                return true;
            }
            //移動完了していない
            return false;
        }
        //ランダム移動
        bool JumbledMoveActMine()
        {
            for (int i = 0; i < mineNum; i++)
            {
                actMine[i].startPos = actMine[i].moveObj.transform.position;
                actMine[i].endPos = new Vector3(Random.Range(-global.FieldSize.x / 2 * 5, global.FieldSize.x / 2 * 5),
                                        0.0f,
                                        Random.Range(-global.FieldSize.y / 2 * 5, global.FieldSize.y / 2 * 5));

                actMine[i].distance = Vector3.Distance(actMine[i].startPos, actMine[i].endPos);
                actMine[i].speed = actMine[i].distance / 20.0f;
            }
            for (int i = 0; i < mineNum; i++)
            {
                // 地雷移動
                actMine[i].moveObj.transform.position = Vector3.MoveTowards(actMine[i].moveObj.transform.position,
                                                                new Vector3(actMine[i].endPos.x, 0.0f, actMine[i].endPos.z),
                                                                            actMine[i].speed);
                //移動完了かどうか
                if (actMine[i].count >= mineMoveSecond2)
                {
                    mineMoveEnd++;
                }
                actMine[i].count++;
            }
            if (mineMoveEnd >= mineNum)
            {
                for (int i = 0; i < mineNum; i++)
                {
                    actMine[i].count = 0;
                }
                mineMoveEnd = 0;
                return true;
            }
            //移動完了していない
            return false;
        }
        #endregion

        #region　アクションカード「台風」
        //台風の描画
        void DrawActTyphoon()
        {

        }
        #endregion

        #region アクションカード「シャッフル」
        //シャッフルの描画
        void DrawActShuffle()
        {

        }
        #endregion

        #region アクションカード「メテオ」
        //メテオの描画
        void DrawActMeteor()
        {
            if (SetTime[0] + WaitTime < Time.time)
            {
                switch (NowFase[0])
                {
                    case (int)ActFaze.set:
                        //メテオオブジェクトの生成＆範囲指定
                        MakeActMeteor(_DrawDataList[0]);
                        NowFase[0]++;
                        SetTime[0] = Time.time;
                        break;

                    case (int)ActFaze.move:
                        // メテオオブジェクト移動
                        if (MoveActMeteor(_DrawDataList[0]))
                        {
                            Debug.Log("メテオの移動完了");
                            NowFase[0]++;
                            SetTime[0] = Time.time;
                        }
                        //同時にエフェクトを生成

                        break;
                    case (int)ActFaze.effect:
                        Destroy(actMeteor.moveObj);
                        Destroy(actMeteor.effectObj);
                        //メテオ建物作成
                        CreateMeteorBldg(_DrawDataList[0]);
                        NowFase[0]++;

                        //SetTime = Time.time;
                        break;
                    case (int)ActFaze.end:
                        //データリストの中からアクションカードナンバー「メテオ」のものを削除
                        _DrawDataList.RemoveAt(0);
                        NowFase.RemoveAt(0);
                        SetTime.RemoveAt(0);
                        break;
                }
            }
        }

        /// <summary>
        /// メテオオブジェクト生成
        /// </summary>
        void MakeActMeteor(List<int> date)
        {
            actMeteor.startPos = new Vector3(-global.FieldSize.x, 10.0f, global.FieldSize.y);
            //メテオオブジェクト生成
            actMeteor.moveObj = Instantiate(actMeteorPreafab,
                                            actMeteor.startPos,
                                            actMeteorPreafab.transform.rotation);
            Vector3 effecrPos = ConvertArrayToWorldPos(date[1], date[2]);

            //範囲指定エフェクトを同時生成
            actMeteor.effectObj = Instantiate(actMeteorEffect,
                        new Vector3(effecrPos.x, effecrPos.y + 0.25f, effecrPos.z),
                        actMeteorEffect.transform.rotation);
        }

        /// <summary>
        /// メテオオブジェクトの移動
        /// </summary>
        bool MoveActMeteor(List<int> data)
        {
            actMeteor.distance = Vector3.Distance(actMeteor.startPos, ConvertArrayToWorldPos(data[1], data[2]));
            actMeteor.speed = actMeteor.distance / meteorMoveSecond;

            actMeteor.endPos = ConvertArrayToWorldPos(data[1], data[2]);
            // メテオ移動
            actMeteor.moveObj.transform.position = Vector3.MoveTowards(actMeteor.moveObj.transform.position, new Vector3(actMeteor.endPos.x, actMeteor.endPos.y + 0.5f, actMeteor.endPos.z), actMeteor.speed);

            //移動完了かどうか
            if (actMeteor.count >= meteorMoveSecond + 1)
            {
                return true;
            }

            actMeteor.count++;

            //移動完了していない
            return false;
        }
        /// <summary>
        /// メテオ建物生成
        /// </summary>
        /// <param name="data"></param>
        void CreateMeteorBldg(List<int> data)
        {
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(MiniObject[4], pos, transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
        }
        #endregion

        #region バッティング描画

        /// <summary>
        /// バッティング描画メイン
        /// </summary>
        private void DrawBatting()
        {
            for (int i = 0; _DrawDataList[0][0] == (int)CardName.DrawObjBatting; i++)
            {
                CreateBattingEffect(_DrawDataList[i]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(i);
                i--;
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawBattingEnd)
            {
                _NowDrowFaze++;
                AllSetTime = Time.time;
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
        /// オブジェクト描画のメイン部分
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
                AllSetTime = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
            }
        }

        #region カード描画
        /// <summary>
        /// カード生成
        /// </summary>
        private void CreateCard(List<int> data, int num)
        {
            // カードを生成
            CardObj.Add(null);
            CardObj[num] = Instantiate(Card[data[0]], View[SubScript[data[3] - pNum + 3]], transform.rotation, _AllFieldObj[data[1]][data[2]].transform);
            Debug.Log("カードの生成完了");
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
            if(Count[num] >= 16)
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

        #region 地雷爆破描画
        void DrawBoomMine(List<int> data)
        {
            if (SetTime[0] + WaitTime < Time.time)
            {
                switch (NowFase[0])
                {
                    case (int)EffectFaze.set:
                        for (int i = 0; i < data.Count; i += 2)
                        {
                            boomEffectList.Add(Instantiate(mineBoomEffectPrefab,
                                                ConvertArrayToWorldPos(data[i], data[i + 1]),
                                                mineBoomEffectPrefab.transform.rotation));
                        }
                        NowFase[0]++;
                        SetTime[0] = Time.time;
                        break;
                    case (int)EffectFaze.effect:
                        NowFase[0]++;
                        SetTime[0] = Time.time;
                        break;
                    case (int)EffectFaze.end:
                        //爆発処理終了
                        for (int i = 0; i < boomEffectList.Count; i++)
                        {
                            Destroy(boomEffectList[i]);
                        }
                        _DrawDataList.RemoveAt(0);
                        break;
                }
            }
            //爆発描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawMineBoomEnd)
            {
                _NowDrowFaze++;
                AllSetTime = Time.time;
                //コマンドの削除
                _DrawDataList.RemoveAt(0);
                isBoomEffect = false;
            }
        }
        #endregion

        #region ランクアップ描画

        /// <summary>
        /// ランクアップ描画メイン
        /// </summary>
        private void DrawRankUp()
        {
            //while (_DrawDataList[0][0] != (int)CardName.DrawRankUpEnd)
            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                // ランクアップエフェクトの生成
                CreateRankUpEffect(_DrawDataList[i]);

                // ランクアップしたオブジェクト描画
                CreateRankUp(_DrawDataList[i]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(i);
            }

            //バッティング描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawRankUpEnd)
            {
                _NowDrowFaze++;
                AllSetTime = Time.time;
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
            //_AllFieldObj[DeleteMeshObj.y][DeleteMeshObj.x].GetComponent<TileData_Maneger>().childObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            //_AllFieldObj[DeleteMeshObj.y][DeleteMeshObj.x].GetComponent<TileData_Maneger>().childObj.GetComponent<Renderer>().enabled = false;

            //ランクアップ後のオブジェクト生成
            Vector3 pos;
            pos = ConvertArrayToWorldPos(data[1], data[2]);
            pos.z -= ((data[1] - data[3]) / 2.0f);  // X(2D)
            pos.x -= ((data[2] - data[4]) / 2.0f);  // Y(2D)
            pos.y += 0.5f;

            Quaternion q = Quaternion.Euler(0f, 0f, 0f);
            if ((data[1] - data[3]) / 2.0f < (data[2] - data[4]) / 2.0f)
            {
                q = Quaternion.Euler(0f, 90f, 0f);
            }    
            //ランクアップ後のオブジェクトを生成
            Instantiate(Object[data[0]], pos, transform.rotation * q, _AllFieldObj[data[1]][data[2]].transform);
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
            //while (_DrawDataList[0][0] != (int)CardName.DrawClosedEnd)
            for (int i = 0; i < _DrawDataList.Count; i++)
            {
                // 廃業エフェクトの生成
                CreateClosedEffect(_DrawDataList[i]);

                // 建物を削除
                Destroy(_DrawDataList[i]);

                //最初の描画リストを削除
                _DrawDataList.RemoveAt(i);
            }

            //廃業描画終了が来たら描画フェーズを次の段階に進める
            if (_DrawDataList[0][0] == (int)CardName.DrawClosedEnd)
            {
                _NowDrowFaze++;
                AllSetTime = Time.time;
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
            pos = ConvertArrayToWorldPos(data[0], data[1]);
            pos.y += 0.5f;

            //オブジェクトを生成
            Instantiate(ClosedEffect, pos, transform.rotation, _AllFieldObj[data[0]][data[1]].transform);
            Debug.Log("廃業エフェクト生成完了");

            //カラーはdata[3]~
        }

        #endregion

        #region 描画終了系


        private void DrawEnd()
        {
            //最初の描画リストを削除
            _DrawDataList.Clear();

            FazeList.Clear();

            //描画フラグをへし折る
            _IsDrawFaze = false;

            // 操作可能フラグをTrueにする
            UIMgr.instance.isClientTurn = true;

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

    }
}