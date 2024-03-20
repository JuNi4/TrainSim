#include "components.hpp"

// Default component //
// default constructor
Component::Component() {}

Component::Component( int pin )
{
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
int Break::formatBreak(int data)
{
    // format the value
    if      ( data > 500 ) { return 4; }
    else if ( data > 50  ) { return 3; }
    else if ( data > 25  ) { return 2; }
    else if ( data > 10  ) { return 1; }
    else                   { return 0; }

}

int Break::get()
{
    // read the value
    int val = analogRead( this->pin );
    // return formatted value
    return this->formatBreak( val );
}

// Reverser //
Reverser::Reverser() {}
Reverser::Reverser( int pinA, int pinB )
{
    // set pin
    this->pinA = pinA;
    this->pinB = pinB;
    pinMode( this->pinA, INPUT );
    pinMode( this->pinB, INPUT );

    // pull the pin low for easy reading / less noise
    analogWrite( this->pinA, LOW );
    analogWrite( this->pinB, LOW );
}

int Reverser::get()
{
    // read the first pin
    bool pa = digitalRead( this->pinA );
    if ( pa )
        return 1;

    // return 2 if the second pin is high, otherwise return 0
    return digitalRead( this->pinB )? 2 : 0;
}