#include <Servo.h>
#include <Arduino.h>
#include <vector>

#include "protocoll.hpp"
#include "components.hpp"

// a list of servos
std::vector<Servo> servos;

// a list of potentiometers
std::vector<Potentiometer> potentiometers;

// a list of breaks ( 5 position switch )
std::vector<Break> breaks;

// a list of reversers ( 3 position switches )
std::vector<Reverser> reversers;

void setup() {

    // setup serial
    Serial.begin( TSP_START_BAUD );

}

void handleMessage()
{
    // read a message
    Message msg = readMSG( TSP_NOWAIT );

    // check if a message was received
    if ( msg.intent == TSP_INTENT_NONE )
        return;

    switch ( msg.intent ) {
        // info intent handle
        case TSP_INTENT_INFO:
            if ( msg.id == 0 )
            {
                // update baud rate
                Serial.begin( msg.msg );
            }
            break;

        case TSP_INTENT_GETVAL: // ( No handler needed for this intent here )
            break;
        case TSP_INTENT_SETVAL:
            // set a servo
            servos[msg.id].write( msg.msg );
            break;

        case TSP_INTENT_CLRCOMPONENT:
            // reset all components
            servos = std::vector<Servo>();
            potentiometers = std::vector<Potentiometer>();
            breaks = std::vector<Break>();
            reversers = std::vector<Reverser>();
            break;
        case TSP_INTENT_ADDCOMPONENT:
            // check for the component type
            if ( msg.id == 0 ) // servo
            {
                // add a servo
                servos.push_back( Servo() );
                // assign the pin
                servos[servos.size()-1].attach( msg.msg );
            }
            if ( msg.id == 1 ) // potentiometer
            {
                // add a pot
                potentiometers.push_back( Potentiometer( msg.msg ) );
            }
            if ( msg.id == 2 ) // breaks
            {
                // add a pot
                breaks.push_back( Break( msg.msg ) );
            }
            if ( msg.id == 3 ) // reverser
            {
                // add a pot
                reversers.push_back( Reverser( msg.msg & 0xf, (msg.msg >> 8) & 0xf ) );
            }
            break;

        default:
            break;
    }
}

void loop() {

    // handle messages
    handleMessage();

    // send all the values
    int id = 0;

    for ( unsigned int i = 0; i < potentiometers.size(); i++ )
    {
        sendMSG( TSP_INTENT_GETVAL, id, potentiometers[i].get() );
        id++;
    }

    for ( unsigned int i = 0; i < breaks.size(); i++ )
    {
        sendMSG( TSP_INTENT_GETVAL, id, breaks[i].get() );
        id++;
    }

    for ( unsigned int i = 0; i < reversers.size(); i++ )
    {
        sendMSG( TSP_INTENT_GETVAL, id, reversers[i].get() );
        id++;
    }
}