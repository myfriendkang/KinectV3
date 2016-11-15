using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ArduinoController : MonoBehaviour {
    
    SerialPort sp = new SerialPort("COM5", 115200);
    public int count = 0;
    bool flag;
    public bool ActualTesting;
    void Start()
    {
        OpenConnection();
        flag = false;
        ActualTesting = false;
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
    string temp;
    void Update()
    {
        try
        {
            if (sp.IsOpen)
            {
                temp = sp.ReadLine();
                Debug.Log(sp.ReadLine());
                if(temp == "1")
                {
                    count = 1;
                    Debug.Log("First Cloth");
                }
                else if(temp == "2")
                {
                    count = 2;
                    Debug.Log("2nd Cloth");
                }
               
                
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
            if(ActualTesting == true)
            {
                Debug.Log("Actual Testing");
            }
            //sp.Write("c");
            Debug.Log("run arduino");
            flag = true;
        }
      else if (check == false)
        {
            flag = false;
        }
        
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
