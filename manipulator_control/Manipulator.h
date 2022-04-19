#pragma once
#define _USE_MATH_DEFINES
#include <string>
#include <sstream>
#include <math.h>
#include "Point.h"
#include "Wrist.h"
#include "Elbow.h"
#include "Shoulder.h"

class Manipulator {
private:
	double angle; // angle of rotation of the entire manipulator
	Shoulder shoulder;
	Elbow elbow;
	Wrist wrist;
	Point position;

public:
	Manipulator();
	Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition = false);
	
	bool CheckCoordinates(Point coordinates);
	bool CheckAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle);

	bool MoveInPoint(Point coordinates);

	void SetFixedWrist(bool isFixed = true);
	void SetFixedWrist(double angle);

	void SetAngleToHorizon(double angle);

	bool SetAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle);

	std::string GetAnglesByString();

	Point GetPosition();

	double GetAngle();

	double GetShoulderAngle();

	double GetElbowAngle();

	double GetWristAngle();
};