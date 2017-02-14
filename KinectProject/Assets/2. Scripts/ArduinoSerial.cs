using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ArduinoSerial : MonoBehaviour {

    private const string SERIAL_PORT = "COM5";
    private const int SERIAL_BAUD_RATE = 115200;
    private const int SERIAL_TIMEOUT = 100;
    private Thread _ReadThread;
    private static SerialPort _serialPort;
    private static bool _CONTINUE;
    public static bool _flag_1;
    public static bool _flag_2;
    //public static bool _flag_3;
    bool flag;
    public static int count;
    //public static bool phoneChecked = false;
    //public bool ActualTesting;

    // Use this for initialization
    void Start () {
        Debug.Log("Serial Start");
        _ReadThread = new Thread(Read);
        _serialPort = new SerialPort(SERIAL_PORT, SERIAL_BAUD_RATE);
        _serialPort.ReadTimeout = SERIAL_TIMEOUT;
        _serialPort.Open();
        _CONTINUE = true;
        _ReadThread.Start();
        _flag_1 = false;
        _flag_2 = false;
        //_flag_3 = false;
        //phoneChecked = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    void OnApplicationQuit()
    {
        _CONTINUE = false;
        _ReadThread.Join();
        _serialPort.Close();
    }
    private static void Read()
    {
        //Debug.Log("Start Thread");
        while (_CONTINUE)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    //ARDUINO -> UNITY
                    string value = _serialPort.ReadLine();
                    Debug.Log("connecting...." + value);
                    int temp = int.Parse(value);
                    if (temp == 1 && _flag_1 == false)
                    {
                        ReturnIndex(temp);
                        Debug.Log("cloth was choosen #1");
                        _serialPort.Write("d");
                        Debug.Log("reset all the flags");
                        _flag_1 = true;
                    }
                    else if(temp ==2 && _flag_2 == false)
                    {
                        ReturnIndex(temp);
                        Debug.Log("cloth was choosen #2");
                        _serialPort.Write("d");
                        Debug.Log("reset all the flags");
                        _flag_2 = true;
                    }
                    /*
                    if(temp == 6 && _flag_3 == false)
                    {
                        Debug.Log("Phone Ring!");
                        //phoneChecked = true;
                        //ReturnPhone();
                        _flag_3 = true;
                        // phoneChecked = false;
                    }
                    */
                }
                catch (TimeoutException)
                {
                }
            }

            Thread.Sleep(1);
            
        }
    }

    //UNITY -> ARDUINO
    /*
    public void SetUserDetected(bool check)
    {
        if (check == true && flag == false)
        {
            if (ActualTesting == true)
            {
                Debug.Log("run arduino!!");
              //  _serialPort.Write("c");
              //_serialPort.Write("d");
            }
            Debug.Log("only Kinect Testing!!");
            flag = true;
        }
    }
    */

    static void ReturnIndex(int num)
    {
        count = num;
    }

    /*
    public static void ReturnPhone()
    {
        phoneChecked = true;

    }

    public Boolean GetPhoneCheck()
    {
        return phoneChecked;
    }
    */
}
