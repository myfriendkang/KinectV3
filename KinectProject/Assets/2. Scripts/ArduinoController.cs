using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ArduinoController : MonoBehaviour {
    
    SerialPort sp = new SerialPort("COM6", 9600);
    public int count = 0;
    bool flag;

    void Start()
    {
        OpenConnection();
        flag = false;
    }
    public void OpenConnection()
    {
        if(sp!= null)
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            else{
                sp.Open();
                sp.ReadTimeout = 1;
            }
        }
        else
        {
            if (sp.IsOpen)
            {
                print("port is already open");
            }
        }
    }
    void Update()
    {
        try
        {
            if (sp.ReadByte() != 0)
            {
                byte rcv;
                char tmp;
                rcv = (byte)sp.ReadByte();
                Debug.Log(rcv);
                //string temp;
                //temp = sp.Readbyte();
                //count = int.Parse(temp);
                //Debug.Log(sp.ReadByte());
            }
        }
        catch (System.Exception e)
        {
            //Debug.Log(e);
            //throw;
        }
    }
    public void SetUserDetected(bool check)
    {
        if(check == true && flag == false)
        {
           // sp.Write("c");
            Debug.Log("run arduino");
            flag = true;
        }
       /* else if (check == false)
        {
            flag = false;
        }
        */
    }
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        flag = false;
         if (sp.IsOpen)
        {
            sp.Close();
        }
        sp.Close();
    }

}
