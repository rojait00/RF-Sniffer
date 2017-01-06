/*
 Name:		RF_Sniffer.ino
 Created:	1/4/2017 8:52:16 PM
 Author:	Rojait00
*/

#include <RCSwitch.h>

RCSwitch mySwitch = RCSwitch();

void setup() {
	Serial.begin(9600);
	mySwitch.enableReceive(0);  // Receiver on interrupt 0 => that is pin #2
}

void loop() {
	if (mySwitch.available()) {

		int value = mySwitch.getReceivedValue();

		if (value == 0) {
			Serial.print("Unknown encoding");
		}
		else {
			Serial.print(mySwitch.getReceivedValue() + "/");
			Serial.println(mySwitch.getReceivedProtocol() + ";");
		}

		mySwitch.resetAvailable();
	}
}
