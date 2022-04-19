#include <iostream>
#include "Manipulator.h"

int main() {

	Manipulator manipulator(10, 8, 2); // manipulator initiation 

	// just moving to a given point
	manipulator.MoveInPoint({ 5, 5, 5 });
	std::cout << manipulator.GetAnglesByString() << "\n";
	std::cout << manipulator.GetPosition().X << ' ' << manipulator.GetPosition().Y << ' ' << manipulator.GetPosition().Z << "\n\n";

	// moving to a given point with a fixed wrist
	manipulator.SetFixedWrist(90.0);
	manipulator.MoveInPoint({ 5, 5, 5 });
	std::cout << manipulator.GetAnglesByString() << "\n";
	std::cout << manipulator.GetPosition().X << ' ' << manipulator.GetPosition().Y << ' ' << manipulator.GetPosition().Z << "\n\n";

	// moving to a given point with a brush that maintains an angle to the horizon
	manipulator.SetFixedWrist(false);
	manipulator.SetAngleToHorizon(0);
	manipulator.MoveInPoint({ 5, 5, 5 });
	std::cout << manipulator.GetAnglesByString() << "\n";
	std::cout << manipulator.GetPosition().X << ' ' << manipulator.GetPosition().Y << ' ' << manipulator.GetPosition().Z << "\n\n";

	// changing the angles of inclination of the parts of the manipulator
	manipulator.SetAngles(90, 90, 90, 90);
	std::cout << manipulator.GetAnglesByString() << "\n";
	std::cout << manipulator.GetPosition().X << ' ' << manipulator.GetPosition().Y << ' ' << manipulator.GetPosition().Z << "\n\n";

	//check
	manipulator.SetFixedWrist(false);
	manipulator.MoveInPoint({-5, 10, 7});
	std::cout << manipulator.GetAnglesByString() << "\n";
	manipulator.SetAngles(manipulator.GetAngle(), manipulator.GetShoulderAngle(), manipulator.GetElbowAngle(), manipulator.GetWristAngle());
	std::cout << manipulator.GetPosition().X << ' ' << manipulator.GetPosition().Y << ' ' << manipulator.GetPosition().Z << "\n\n";

	return 0;
}