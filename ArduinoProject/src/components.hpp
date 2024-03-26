#include <Arduino.h>

#ifndef _TS_COMPONENTS_
#define _TS_COMPONENTS_

class Component
{
    public:

    // the id of the component
    int id;

    // pin of the component
    int pin;

    // default constructor
    Component();
    Component( int pin, int id = -1 );

    // get the value of the component
    int get();
};

class Potentiometer : public Component
{
    public:
    Potentiometer() : Component{} {};
    Potentiometer( int pin, int id ) : Component{ pin, id } {};
};

class Switch5 : public Component
{
    protected:

    // formats the break values
    int formatBreak(int data);

    public:

    Switch5() : Component{} {};
    Switch5( int pin, int id ) : Component{ pin, id } {};

    // get the value of the break
    int get();
};

class Switch3
{
    public:

    // the id of the component
    int id;

    // pin for forward
    int pinA;
    // pin for backwards
    int pinB;

    Switch3();
    Switch3( int pinA, int pinB, int id );

    // get the value of the reverser
    int get();
};

class Switch2 : public Component
{
    public:
    Switch2() : Component{} {};
    Switch2( int pin, int id ) : Component{ pin, id } {};

    // get the value of the switch
    int get();
};

#endif