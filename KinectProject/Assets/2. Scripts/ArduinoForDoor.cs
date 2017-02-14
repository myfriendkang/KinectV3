using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ArduinoForDoor : MonoBehaviour
{

    private const string SERIAL_PORT = "COM7";
    private const int SERIAL_BAUD_RATE = 9600;
    private const int SERIAL_TIMEOUT = 100;
    private Thread _ReadThread;
    private static SerialPort _serialPort;
    private static bool _CONTINUE;
    public bool flag;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Serial Start for Door Arduino");
        _ReadThread = new Thread(Read);
        _serialPort = new SerialPort(SERIAL_PORT, SERIAL_BAUD_RATE);
        _serialPort.ReadTimeout = SERIAL_TIMEOUT;
        _serialPort.Open();
        _CONTINUE = true;
        _ReadThread.Start();
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {

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
                {
                   
                }
                catch (TimeoutException)
                {
                }
            }
            Thread.Sleep(1);
        }
    }

    //UNITY -> ARDUINO
    public void OperateDoorOpen(bool check)
    {
        if (check == true && flag == false)
        {
            _serialPort.Write("e");
            Debug.Log("open DOOR arduino!!");
            flag = true;
        }
    }
}
