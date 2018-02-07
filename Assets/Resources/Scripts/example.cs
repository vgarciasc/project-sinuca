using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RawMouseDriver;
using RawInputSharp;
 
public class example : MonoBehaviour
{
    private RawMouseDriver.RawMouseDriver mousedriver;
    private RawMouse mouse1;
    private RawMouse mouse2;
 
    private float moveY = 0.0f;
    private float moveX = 0.0f;
 
    void Start ()
    {
        mousedriver = new RawMouseDriver.RawMouseDriver ();
    }
    void Update()
    {
		print("moveX: " + moveX + ", moveY: " + moveY);
        mousedriver.GetMouse (0, ref mouse1);
        mousedriver.GetMouse (1, ref mouse2);
     
        moveY += mouse1.YDelta;
        transform.Translate(Vector3.forward * moveY);
     
        moveX += mouse2.XDelta;
        transform.Translate(Vector3.forward * moveX);
    }
    void OnApplicationQuit()
    {
        mousedriver.Dispose ();
    }
}
 