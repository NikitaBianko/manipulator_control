#include <iostream>
#include "Manipulator.h"

int main() {

	Manipulator manipulator(10, 8, 2); // manipulator initiation 


	// just moving to a given point

	manipulator.moveInPoint({ 5, 5, 5 });

	std::cout << manipulator.Request() << "\n";

	std::cout << manipulator.getPosition().X << ' ' << manipulator.getPosition().Y << ' ' << manipulator.getPosition().Z << "\n\n";


	// moving to a given point with a fixed wrist

	manipulator.wristFixed(90.0);

	manipulator.moveInPoint({ 5, 5, 5 });

	std::cout << manipulator.Request() << "\n";

	std::cout << manipulator.getPosition().X << ' ' << manipulator.getPosition().Y << ' ' << manipulator.getPosition().Z << "\n\n";


	// moving to a given point with a brush that maintains an angle to the horizon

	manipulator.wristFixed(false);

	manipulator.setAngleToHorizon(0);

	manipulator.moveInPoint({ 5, 5, 5 });

	std::cout << manipulator.Request() << "\n";

	std::cout << manipulator.getPosition().X << ' ' << manipulator.getPosition().Y << ' ' << manipulator.getPosition().Z << "\n\n";


	// changing the angles of inclination of the parts of the manipulator

	manipulator.setAngles(90, 90, 90, 90);

	std::cout << manipulator.Request() << "\n";

	std::cout << manipulator.getPosition().X << ' ' << manipulator.getPosition().Y << ' ' << manipulator.getPosition().Z << "\n\n";

	return 0;
}