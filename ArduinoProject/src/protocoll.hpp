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

#ifndef _TS_PROTOCOLL_
#define _TS_PROTOCOLL_

#include <vector>
#include <Arduino.h>

// Messages are made off:
//  Intent | ID     | Body
//  8 Bits | 8 Bits | 4 Bytes
// Total 6 Bytes

#define TSP_MESSAGE_SIZE 6

#define TSP_START_BAUD 9600

#define TSP_DELIMITER (char)0xff

///////////////////////////
// a list of all intents //
///////////////////////////

/**
 * @brief When an invalid message was recieved
 * 
 */
#define TSP_INTENT_NONE 0x00

/**
 * @brief info intent
 * 
 * Send by game to get info about the connected components
 * 
 */ 
#define TSP_INTENT_INFO 0x01

/**
 * @brief Intent for setting a value for a compenent
 * 
 */
#define TSP_INTENT_SETVAL 0x02

/**
 * @brief Intent for reading a value
 * 
 */
#define TSP_INTENT_GETVAL 0x03

/**
 * @brief Intent for clearing all registered components
 * 
 */
#define TSP_INTENT_CLRCOMPONENT 0x04

/**
 * @brief Intent for adding a component to the arduino
 * 
 */
#define TSP_INTENT_ADDCOMPONENT 0x05

#define TSP_NOWAIT true

///////////////
// Functions //
///////////////

/**
 * @brief A class for storing information about a message
 * 
 */
class Message
{

    public:

    int intent;
    int id;
    int msg;

    Message();
    Message( int intent, int id, int msg );

    void decodeMessage( std::vector<uint8_t> bytes );
};

/**
 * @brief reads a message from the serail bus
 * 
 * @return Message the received message
 */
Message readMSG();

/**
 * @brief reads a message from the serail bus
 * 
 * @param dont_wait make the receiver not wait for incomming bits
 */
Message readMSG( bool dont_wait );

/**
 * @brief Sends a message over the serial bus
 * 
 * @param intent The intent of the message
 * @param id Extra info / context for the intent
 * @param msg The message to be send
 */
void sendMSG( int intent, int id, int msg );

/**
 * @brief Sends a message over the serial bus
 * 
 * @param msg The message to be send
 */
void sendMSG( Message msg );

#endif