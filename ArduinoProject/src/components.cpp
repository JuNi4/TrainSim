#include "components.hpp"

// Default component //
// default constructor
Component::Component() {}

Component::Component( int pin, int id )
{
    // set id
    this->id = id;

    // set pin
    this->pin = pin;
    pinMode( this->pin, INPUT );

    // pull the pin low for easy reading / less noise
    analogWrite( this->pin, LOW );
}

// get the value of the component
int Component::get()
{
    // read the value
    return analogRead( this->pin );
}

// Break //
int Switch5::formatBreak(int data)
{
    // format the value
    if      ( data > 500 ) { return 4; }
    else if ( data > 50  ) { return 3; }
    else if ( data > 15  ) { return 2; }
    else if ( data > 5  ) { return 1; }
    else                   { return 0; }

}

int Switch5::get()
{
    // read the value
    int val = analogRead( this->pin );
    // return formatted value
    return this->formatBreak( val );
}

// Reverser //
Switch3::Switch3() {}
Switch3::Switch3( int pinA, int pinB, int id )
{
    // set the id of the component
    this->id = id;

    // set pin
    this->pinA = pinA;
    this->pinB = pinB;
    pinMode( this->pinA, INPUT );
    pinMode( this->pinB, INPUT );

    // pull the pin low for easy reading / less noise
    analogWrite( this->pinA, LOW );
    analogWrite( this->pinB, LOW );
}

int Switch3::get()
{
    // read the first pin
    bool pa = digitalRead( this->pinA );
    if ( pa )
        return 1;

    // return 2 if the second pin is high, otherwise return 0
    return digitalRead( this->pinB )? 2 : 0;
}

int Switch2::get()
{
    // read the digital pin
    return digitalRead( this->pin );
}