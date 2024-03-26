/**
 * @file protocoll.hpp
 * @author JuNi
 * @brief Communicates between the arduiono and the game engine
 * @version 0.1
 * @date 2024-03-16
 * 
 * @copyright Copyright (c) 2024
 * 
 */

#include "protocoll.hpp"

// Message Class Functions //

Message::Message( int intent, int id, int msg )
{
    this->intent = intent;
    this->id = id;
    this->msg = msg;
}
Message::Message()
{
    this->intent = TSP_INTENT_NONE;
    this->id = 0;
    this->msg = 0;
}

void Message::decodeMessage( std::vector<uint8_t> bytes )
{
    // extract header
    this->intent = (int) bytes[0];
    this->id = (int) bytes[1];

    this->msg = 0;
    // read the bytes
    for ( int i = 0; i < 4; i++ )
    {
        this->msg |= bytes[bytes.size() -1 -i] << 8*i ;
    }
}

// Main functions //

/**
 * @brief reads a message from the serail bus
 * 
 * @return Message the received message
 */
Message readMSG()
{
    // create output message object
    Message out;

    // receive the message
    std::vector<uint8_t> bytes;

    while ( bytes.size() < TSP_MESSAGE_SIZE )
    {
        // check if a message is available
        if ( Serial.available() > 0 )
        {
            // read a message
            bytes.push_back( Serial.read() );
        }
    }

    // decode message
    out.decodeMessage( bytes );

    // reuturn the recieved message
    return out;
}

/**
 * @brief reads a message from the serail bus
 * 
 * @param dont_wait make the receiver not wait for incomming bits
 */
Message readMSG( bool dont_wait )
{
    // create output message object
    Message out;

    // receive the message
    std::vector<uint8_t> bytes;

    // check if a message is available
    if ( Serial.available() >= TSP_MESSAGE_SIZE )
    {
        for ( int i = 0; i < TSP_MESSAGE_SIZE; i++ )
        {
            // read a message
            bytes.push_back( Serial.read() );
        }

        out.decodeMessage( bytes );
    }

    // reuturn the recieved message
    return out;
}

/**
 * @brief Sends a message over the serial bus
 * 
 * @param intent The intent of the message
 * @param id Extra info / context for the intent
 * @param msg The message to be send
 */
void sendMSG( int intent, int id, int msg )
{
    // make sure the delimeter does not get send
    // if ( (char)intent == TSP_DELIMITER ) { intent -= 1; }
    // if ( (char)id == TSP_DELIMITER ) { id -= 1; }

    // send header
    Serial.print( (char) intent );
    Serial.print( (char) id );

    // send message
    for ( int i = 0; i < 4; i++ )
    {
        // send all bytes of the message
        int x = ( msg >> (8 * (3-i)) ) & 0xff;
        // make sure the delimeter does not get send
        // if ( (char) x == TSP_DELIMITER ) { x -= 1; }
        Serial.print( (char) x );
    }

    // Serial.print( String("") +(char) intent + (char) id + (char) ( ( msg >> (8 * 3) ) & 0xf ) + (char) ( ( msg >> (8 * 2) ) & 0xf ) + (char) ( ( msg >> (8 * 1) ) & 0xf ) + (char) ( msg  & 0xf ) );

    // print line break
    // Serial.print( TSP_DELIMITER );
}

/**
 * @brief Sends a message over the serial bus
 * 
 * @param msg The message to be send
 */
void sendMSG( Message msg )
{
    sendMSG( msg.intent, msg.id, msg.msg );
}