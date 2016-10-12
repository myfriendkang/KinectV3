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
            if (temp != null)
            {
                count = int.Parse(sp.ReadLine());
            }
            
        }
        catch (System.Exception)
        {

        }
    }
    
   
}
