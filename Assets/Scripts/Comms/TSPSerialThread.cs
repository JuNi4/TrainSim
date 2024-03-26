/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;

using System.IO.Ports;
using System;

/**
 * This class contains methods that must be run from inside a thread and others
 * that must be invoked from Unity. Both types of methods are clearly marked in
 * the code, although you, the final user of this library, don't need to even
 * open this file unless you are introducing incompatibilities for upcoming
 * versions.
 * 
 * For method comments, refer to the base class.
 */
public class TSPSerialThread : AbstractSerialThread
{
    // Messages to/from the serial port should be delimited using this separator.
    // private byte separator;
    // Buffer where a single message must fit
    // private int[] buffer = new int[6];
    // private int bufferUsed = 0;
    // private int bufferRead = 0;
    
    public TSPSerialThread(string portName,
                                       int baudRate,
                                       int delayBeforeReconnecting,
                                       int maxUnreadMessages)
        : base(portName, baudRate, delayBeforeReconnecting, maxUnreadMessages, false)
    {
        // this.separator = separator;
    }

    // ------------------------------------------------------------------------
    // Must include the separator already (as it shold have been passed to
    // the SendMessage method).
    // ------------------------------------------------------------------------
    protected override void SendToWire(object message, SerialPort serialPort)
    {
        // convert the input to a string
        string msg = message.ToString();

        // convert the input to a list of bytes
        byte[] binaryMessage = new byte[msg.Length];

        for ( int i = 0; i < msg.Length; i++ )
        {
            binaryMessage[i] = (byte)msg[i];
        }

        serialPort.Write(binaryMessage, 0, binaryMessage.Length);
    }

    protected override object ReadFromWire(SerialPort serialPort)
    {
        int msgSize = 6;

        // if neccesary, reset buffer index
        // if ( bufferUsed >= buffer.Length - msgSize || bufferRead >= buffer.Length - msgSize )
        // {
        //     bufferUsed = 0;
        //     bufferRead = 0;
        // }

        // // read all the data
        // if ( serialPort.BytesToRead >= msgSize )
        // {
        //     for ( int i = 0; i < msgSize; i++ )
        //     {
        //         this.buffer[ i ] = serialPort.ReadByte();
        //         // if neccesary, reset buffer index
        //         // if ( bufferUsed >= buffer.Length - msgSize)
        //         //     bufferUsed = 0;
        //     }
        // }

        // // check if the read process cought up to the write index
        // if ( this.bufferRead >= this.bufferUsed )
        // {
        //     return null;
        // }

        String returnBuffer = "";

        // // return 6 bytes of the buffer
        // for ( int i = 0; i < msgSize; i++ )
        // {
        //     returnBuffer += (char)this.buffer[ i ];
        //     // if neccesary, reset buffer read index
        //     // if ( bufferRead >= buffer.Length )
        //     //     bufferRead = 0;
        // }

        if ( serialPort.BytesToRead >= msgSize )
        {
            for ( int i = 0; i < msgSize; i++ )
            {
                returnBuffer += (char) serialPort.ReadByte();
            }
        }
        else
        {
            return null;
        }

        return returnBuffer;
    }
}
