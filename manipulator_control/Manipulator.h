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

	bool checkCoordinates(Point coordinates);
	bool checkAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle);

public:
	Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition = false);
	bool moveInPoint(Point coordinates);

	void wristFixed(bool isFixed = true);
	void wristFixed(double angle);
	void setAngleToHorizon(double angle);

	bool setAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle);

	std::string Request();

	Point getPosition();
};