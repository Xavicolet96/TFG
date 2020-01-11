using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class ElementObject : MonoBehaviour
{
    
    public Controller c;
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        deselect();


    }

    // Update is called once per frame
    void Update()
    {
     
    }

    virtual public void deselect()
    {
        rend = GetComponent<Renderer>();

        //rend.material = Resources.Load("Assets / Mat.mat", typeof(Material)) as Material;
        //rend.material.SetColor("_Color", new Color(0 ,1,1,0.5f));

        Material m = Resources.Load<Material>("Mat");

        rend.material = m;
    }

    public void select()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", new Color(1, 0.2f, 0.2f, 1));
    }


}
