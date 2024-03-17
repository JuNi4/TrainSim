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

// Messages are made off:
//  Intent | ID     | Body
//  4 Bits | 4 Bits | 8 Bits


///////////////////////////
// a list of all intents //
///////////////////////////

/**
 * @brief info intent
 * 
 * Send by game to get info about the connected components
 * 
 */ 
#define TSP_INTENT_INFO 0x00

/**
 * @brief Intent for setting a value for a compenent
 * 
 */
#define TSP_INTENT_SETVAL 0x01

/**
 * @brief Intent for reading a value
 * 
 */
#define TSP_INTENT_GETVAL 0x02


///////////////
// Functions //
///////////////

class Message
{

    int intent;
    int id;
    int msg;

    Message();
    Message( int intent, int id, int msg );
};

/**
 * @brief reads a message from the serail bus
 * 
 * @return Message the received message
 */
Message readMSG();

/**
 * @brief Sends a message over the serial bus
 * 
 * @param intent The intent of the message
 * @param id Extra info / context for the intent
 * @param msg 
 */
void sendMSG( int intent, int id, int msg );