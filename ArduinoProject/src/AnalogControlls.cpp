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
std::vector<Switch5> switch5;

// a list of reversers ( 3 position switches )
std::vector<Switch3> switch3;

// a list of switches
std::vector<Switch2> switch2;

// id index
int id = 0;

void setup() {

    // setup serial
    Serial.begin( TSP_START_BAUD );

    // potentiometers.push_back(Potentiometer(19));

}

void sendValue( int id )
{
    // find the one with the id
    for ( unsigned int i = 0; i < potentiometers.size(); i++ )
    {
        if ( potentiometers[i].id == id ) {
            sendMSG( TSP_INTENT_GETVAL, potentiometers[i].id, potentiometers[i].get() );
            // Serial.println(potentiometers[i].get());
            return;
        }
    }

    for ( unsigned int i = 0; i < switch5.size(); i++ )
    {
        if ( switch5[i].id == id ) {
            sendMSG( TSP_INTENT_GETVAL, switch5[i].id, switch5[i].get() );
            // Serial.println(switch5[i].get());
            return;
        }
    }

    for ( unsigned int i = 0; i < switch3.size(); i++ )
    {
        if ( switch3[i].id == id ) {
            sendMSG( TSP_INTENT_GETVAL, switch3[i].id, switch3[i].get() );
            // Serial.println(switch3[i].get());
            return;
        }
    }

    for ( unsigned int i = 0; i < switch2.size(); i++ )
    {
        if ( switch2[i].id == id ) {
            sendMSG( TSP_INTENT_GETVAL, switch2[i].id, switch2[i].get() );
            // Serial.println(switch2[i].get());
            return;
        }
    }
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
                Serial.end();
                // update baud rate
                Serial.begin( msg.msg );
            }
            break;

        case TSP_INTENT_GETVAL:
            // send the value
            sendValue( msg.id );
            break;
        case TSP_INTENT_SETVAL:
            // set a servo
            servos[msg.id].write( msg.msg );
            break;

        case TSP_INTENT_CLRCOMPONENT:
            // reset all components
            servos = std::vector<Servo>();
            potentiometers = std::vector<Potentiometer>();
            switch5 = std::vector<Switch5>();
            switch3 = std::vector<Switch3>();
            switch2 = std::vector<Switch2>();
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
            else if ( msg.id == 1 ) // potentiometer
            {
                // add a pot
                potentiometers.push_back( Potentiometer( msg.msg, id++ ) );
            }
            else if ( msg.id == 2 ) // breaks
            {
                // add a break
                switch5.push_back( Switch5( msg.msg, id++ ) );
            }
            else if ( msg.id == 3 ) // reverser
            {
                // add a reverser
                switch3.push_back( Switch3( msg.msg & 0xf, (msg.msg >> 8) & 0xf, id++ ) );
            }
            else if ( msg.id == 4 ) // switches
            {
                // add a reverser
                switch2.push_back( Switch2( msg.msg, id++ ) );
            }
            break;

        default:
            break;
    }
}

void loop() {

    // handle messages
    handleMessage();
    
}