using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ArduinoSerial : MonoBehaviour {

    private const string SERIAL_PORT = "COM6";
    private const int SERIAL_BAUD_RATE = 9600;
    private const int SERIAL_TIMEOUT = 100;
    private Thread _ReadThread;
    private static SerialPort _serialPort;
    private static bool _CONTINUE;
   
	// Use this for initialization
	void Start () {
        Debug.Log("Serial Start");
        _ReadThread = new Thread(Read);
        _serialPort = new SerialPort(SERIAL_PORT, SERIAL_BAUD_RATE);
        _serialPort.ReadTimeout = SERIAL_TIMEOUT;
        _serialPort.Open();
        _CONTINUE = true;
        _ReadThread.Start();
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
                   string value = _serialPort.ReadLine();
                }
                catch (TimeoutException)
                {

                }
            }
            //Debug.Log("Thread Sleep");
            Thread.Sleep(1);
        }
    }
}
