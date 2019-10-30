using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// EventSystem利用

using Model;

namespace Ctrl {

    public class TileData_Maneger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private FieldDataProxy _Field;

        public int Player;
        public int RankID;
        public GameObject obj;

        private Color default_color;  // 初期化カラー
        private Color select_color;    // 選択時カラー
        protected Material _material;

        public Vector2 planePos;
        public GameObject childObj;
        public GameObject topObj;
        public GameObject downObj;
        public GameObject leftObj;
        public GameObject rightObj;

        public bool isBuilding = false;
        public int myNumber = 0;              //建っている建物のコスト　　同一のコストであればランクアップ可能
        public bool isLinkBuilding = false; //繋がってるかどうか
        #region 探索処理(rankup)
        public int SearchRankUp()
        {
            //左右優先
            //左を探す
            if (leftObj != null && leftObj.GetComponent<TileData_Maneger>().childObj != null)
            {
                if (!leftObj.GetComponent<TileData_Maneger>().Field.IsRankUp && 
                    myNumber == leftObj.GetComponent<TileData_Maneger>().myNumber &&
                    _Field.PossessionPlayer == leftObj.GetComponent<TileData_Maneger>().Field.PossessionPlayer)
                {
                    leftObj.GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                    _Field.IsRankUp = true;

                    leftObj.GetComponent<TileData_Maneger>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                    this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                    //BldgData_Maneger.Instance.makeColider(0, this.childObj.transform.position, this.childObj);
                    return 1;
                }
            }
            //右を探す
            if (rightObj != null && rightObj.GetComponent<TileData_Maneger>().childObj != null)
            {
                if (!rightObj.GetComponent<TileData_Maneger>().Field.IsRankUp &&
                    myNumber == rightObj.GetComponent<TileData_Maneger>().myNumber &&
                    _Field.PossessionPlayer == rightObj.GetComponent<TileData_Maneger>().Field.PossessionPlayer)
                {
                    rightObj.GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                    _Field.IsRankUp = true;

                    rightObj.GetComponent<TileData_Maneger>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                    this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                    //BldgData_Maneger.Instance.makeColider(1, this.childObj.transform.position, this.childObj);
                    return 2;
                }
            }
            //上を探す
            if (topObj != null && topObj.GetComponent<TileData_Maneger>().childObj != null)
            {
                if (!topObj.GetComponent<TileData_Maneger>().Field.IsRankUp && 
                    myNumber == topObj.GetComponent<TileData_Maneger>().myNumber &&
                    _Field.PossessionPlayer == topObj.GetComponent<TileData_Maneger>().Field.PossessionPlayer)
                {
                    topObj.GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                    _Field.IsRankUp = true;

                    topObj.GetComponent<TileData_Maneger>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                    this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                    //BldgData_Maneger.Instance.makeColider(2, this.childObj.transform.position, this.childObj);
                    return 3;
                }
            }
            //下を探す
            if (downObj != null && downObj.GetComponent<TileData_Maneger>().childObj != null)
            {
                if (!downObj.GetComponent<TileData_Maneger>().Field.IsRankUp && 
                    myNumber == downObj.GetComponent<TileData_Maneger>().myNumber &&
                    _Field.PossessionPlayer == downObj.GetComponent<TileData_Maneger>().Field.PossessionPlayer)
                {
                    downObj.GetComponent<TileData_Maneger>().Field.IsRankUp = true;
                    _Field.IsRankUp = true;

                    downObj.GetComponent<TileData_Maneger>().childObj.GetComponent<Renderer>().material.color = Color.magenta;
                    this.childObj.GetComponent<Renderer>().material.color = Color.magenta;

                    //BldgData_Maneger.Instance.makeColider(3, this.childObj.transform.position, this.childObj);
                    return 4;
                }
            }

            return 0;
        }
        #endregion
        void Awake()
        {
            // このクラスが付属しているマテリアルを取得 
            _material = this.gameObject.GetComponent<Renderer>().material;

            // 選択時と非選択時のカラーを保持 
            default_color = _material.color;
            select_color = Color.cyan;

            //フィールドデータ初期化
            Field = new FieldDataProxy(null, -1, -1, false, -1);
        }


        public void FixedUpdate()
        {
            RankID = Field.RankID;
        }

        //ポインターがオブジェクト上に入った時
        public void OnPointerEnter(PointerEventData eventData)
        {
            _material.color = select_color;
            if (!isBuilding)
            {
                Debug.Log("建ってないよ");
            }
        }

        //ポインターがオブジェクトから出た時

        public void OnPointerExit(PointerEventData ped)
        {
            _material.color = default_color;
        }

        //クリックされた時

        public void OnPointerDown(PointerEventData _data)
        {
            ////Debug.Log("clicked");
            //if (isBuilding == false)
            //{
            //    if (BldgData_Maneger.Instance.Cobj != null)
            //    {
            //        BldgData_Maneger.Instance.coliderOff();
            //    }
            //    BldgData_Maneger.Instance.SetPosition(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            //    isBuilding = true;
            //}
        }

        public void CreateNewObj(FieldDataProxy newField)
        {
            if (isBuilding == false)
            {
                if (BldgData_Maneger.Instance.Cobj != null)
                {
                    BldgData_Maneger.Instance.coliderOff();
                }
                //オブジェ生成
                BldgData_Maneger.Instance.SetPosition(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
                BldgData_Maneger.Instance.MakeBildings(newField.Data.Number,this.gameObject);

                //フィールドに情報をセット
                _Field = newField;
                myNumber = _Field.Data.Number;

                isBuilding = true;
            }
        }

        //フィールドデータのゲッター、セッター
        public FieldDataProxy Field
        {
            get
            {
                return _Field;
            }
            set
            {
                _Field = value;
            }
        }
    }

}

