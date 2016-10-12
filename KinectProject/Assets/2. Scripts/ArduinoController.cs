using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour {
    
    SerialPort sp = new SerialPort("COM7", 9600);
    public int count = 0;


    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        try
        {
            string temp;
            temp = sp.ReadLine();
            count = int.Parse(temp);
            Debug.Log(temp);

        }
        catch (System.Exception)
        {

        }
    }
    
   
}
