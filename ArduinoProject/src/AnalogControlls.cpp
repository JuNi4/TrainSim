// Serial Settings
#define BAUD 9600

// Servos Pins
#define SERVO_START 2
#define SERVO_COUNT 5

// Throttle & Brakes
#define THROTTLE A5
#define IND_BRK A1
#define TRN_BRK A2

// reverser pins
#define RVS_1 12
#define RVS_2 13

#define LB '\n'

#include <Servo.h>
#include <Arduino.h>
#include <vector>

// create servos
std::vector<Servo> servos;

void setup() {
    // initialize serial bus
    Serial.begin(BAUD);

    // instanciate all servos
    for (int i = 0; i < SERVO_COUNT; i++) {
        servos.push_back( Servo() );
        servos[i].attach( SERVO_START + i );
        servos[i].write(0);
    }

    // setup all pins
    pinMode( THROTTLE, INPUT );
    pinMode( IND_BRK, INPUT );
    pinMode( TRN_BRK, INPUT );
    pinMode( RVS_1, INPUT );
    pinMode( RVS_2, INPUT );

    // pull all the input pins to low in order to make reading easyier
    analogWrite( THROTTLE, 0 );
    analogWrite( IND_BRK, 0 );
    analogWrite( TRN_BRK, 0 );
    analogWrite( RVS_1, 0 );
    analogWrite( RVS_2, 0 );
}

// String readSerial() {
// 	String out;
// 	// read all characters from the serial bus
// 	while (Serial.available() > 0) {
// 		out += String((char)Serial.read());
// 		delay( (int) 1000/BAUD );
// 	}
// 	// return data read from serial bus
// 	return out;
// }

int fromatBreak(int data) {
    if      ( data > 500 ) { return 4; }
    else if ( data > 50  ) { return 3; }
    else if ( data > 25  ) { return 2; }
    else if ( data > 10  ) { return 1; }
    else                   { return 0; }
}

String serialData;

void loop() {

    // read serial data for the servos
    if ( Serial.available() > 0 ) {
        for ( int i = 0; i < Serial.available(); i++ ) { serialData += String((char)Serial.read()); }
    }

    // check if enough data was send
    if ( serialData[ serialData.length() -1 ] == LB ) {
        // go through all data
        for ( int i = 0; i < SERVO_COUNT; i++ ) {
            servos[i].write((int)(char)serialData[i]);
        }

        // reset serial data
        serialData = "";
    }

    // read the throttle data
    int throttle = analogRead(THROTTLE) / 1023.f * 255;

    // read the breake data
    int ind_breake = analogRead(IND_BRK);
    int trn_breake = analogRead(TRN_BRK);
    // Serial.print( String(fromatBreak( ind_breake ) ) + ", " + String(ind_breake));

    // read the reverser data
    bool reverser1 = digitalRead( RVS_1 );
    bool reverser2 = digitalRead( RVS_2 );
    int dir = reverser1? 1 : ( reverser2? 2 : 0 );

    // print all the data
    Serial.print( (char) dir );
    Serial.print( (char) throttle );
    Serial.print( (char) fromatBreak(ind_breake) );
    Serial.print( (char) fromatBreak(trn_breake) );

    // print endline char
    Serial.print(LB);
}