using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMouseOver : MonoBehaviour
{
    public float defaultR, defaultG, defaultB;
    public bool isUsePos;

    public static FieldMouseOver instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        defaultR = this.gameObject.GetComponent<Renderer>().material.color.r;
        defaultG = this.gameObject.GetComponent<Renderer>().material.color.g;
        defaultB = this.gameObject.GetComponent<Renderer>().material.color.b;

        isUsePos = false;
    }

    public void OnMouseOver()
    {

        //Debug.Log(this.gameObject.name);
        if (UIMgr.instance.isClientTurn)
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        
    }

    public void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().material.color = new Color(defaultR, defaultG, defaultB, 1.0f);

    }

    private void Update()
    {
        if(isUsePos)
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        if (!UIMgr.instance.isClientTurn)
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(defaultR, defaultG, defaultB, 1.0f);
            isUsePos = false;
        }
    }

}
