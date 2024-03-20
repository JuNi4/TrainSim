#include <Arduino.h>

#ifndef _TS_COMPONENTS_
#define _TS_COMPONENTS_

class Component
{
    public:

    // pin of the component
    int pin;

    // default constructor
    Component();

    Component( int pin );

    // get the value of the component
    int get();
};

class Potentiometer : public Component
{
    public:
    Potentiometer() : Component{} {};
    Potentiometer( int pin ) : Component{ pin } {};
};

class Break : public Component
{
    protected:

    // formats the break values
    int formatBreak(int data);

    public:

    Break() : Component{} {};
    Break( int pin ) : Component{ pin } {};

    // get the value of the break
    int get();
};

class Reverser
{
    public:

    // pin for forward
    int pinA;
    // pin for backwards
    int pinB;

    Reverser();
    Reverser( int pinA, int pinB );

    // get the value of the reverser
    int get();
};

#endif