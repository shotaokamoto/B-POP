using UnityEngine;
using System.Collections;

public class ReceiveEvent : MonoBehaviour
{
    public int whidth = 100;
    public int hight = 100;
    private Vector3 _oldPosition;
    /// <summary>
    /// Cleckしたときの処理
    /// </summary>
    public void MyPointerDownUI()
    {
        Debug.Log("押された");
        _oldPosition = transform.position;
    }
    /// <summary>
    ///  Dragした時の処理
    /// </summary>
    public void MyDragUI()
    {
        transform.position = Input.mousePosition;
    }
    /// <summary>
    /// Dropした時の処理
    /// </summary>
    public void MyDropUI()
    {
        Debug.Log("離した");
  
    }
}

