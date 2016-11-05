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
    bool flag;

    public static int count;

    // Use this for initialization
    void Start () {
        Debug.Log("Serial Start");
        _ReadThread = new Thread(Read);
        _serialPort = new SerialPort(SERIAL_PORT, SERIAL_BAUD_RATE);
        _serialPort.ReadTimeout = SERIAL_TIMEOUT;
        _serialPort.Open();
        _CONTINUE = true;
        _ReadThread.Start();
        flag = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    void OnApplicationQuit()
    {
        _CONTINUE = false;
        _ReadThread.Join();
        _serialPort.Close();
        flag = false;
    }
    private static void Read()
    {
        //Debug.Log("Start Thread");
        while (_CONTINUE)
        {
            if (_serialPort.IsOpen)
            {
                try
                {   //ARDUINO -> UNITY
                    string value = _serialPort.ReadLine();
                    /*
                    1st cloth = 21X
                    2nd cloth = 41X
                    */
                   int temp = int.Parse(value);
                    if (temp == 1)
                    {
                        ReturnIndex(temp);
                        Debug.Log("clothe was choosen #1");
                    }
                    else if(temp ==2)
                    {
                        ReturnIndex(temp);
                        Debug.Log("clothe was choosen #2");
                    }
                }
                catch (TimeoutException)
                {
                }
            }

            Thread.Sleep(1);
        }
    }

    //UNITY -> ARDUINO
    public void SetUserDetected(bool check)
    {
        if (check == true && flag == false)
        {
            _serialPort.Write("c");
            Debug.Log("run arduino!!");
            flag = true;
        }
    }
    static void ReturnIndex(int num)
    {
        count = num;
    }
}
