using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Model;

namespace Ctrl
{
    public class FieldManeger : MonoBehaviour
    {
        private FieldDataProxy _Field; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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