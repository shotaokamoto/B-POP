using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDraw : MonoBehaviour
{
    List<int> DrawData = new List<int>();

    public enum DataType
    {
        ORDER_1,
        ORDER_2,
        ORDER_3
    }

    public DataType dataType;

    void Start()
    {
        for (int i = 0; i < DrawData.Count; i++)
        {
            switch (dataType)
            {
                case DataType.ORDER_1:

                    break;
                case DataType.ORDER_2:

                    break;
                case DataType.ORDER_3:

                    break;
            }
        }
    }
    


}
