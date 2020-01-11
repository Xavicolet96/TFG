using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1f, 0), 2);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1f, 0), -2);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(1f, 0, 0), 2);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(1f, 0, 0), -2);
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            Debug.Log("a");
            this.transform.position = this.transform.position - this.transform.position * 0.008f;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            Debug.Log("a");
            this.transform.position = this.transform.position + this.transform.position * 0.008f;
        }

        if (Input.GetKeyDown("c"))
        {
            this.transform.position = new Vector3(0, 0, -3f);
            this.transform.rotation = Quaternion.identity;
        }

    }
}
