#define _USE_MATH_DEFINES
#include <iostream>
#include <string>
#include <sstream>
#include <math.h>

template <typename T>
std::string to_string_with_precision(const T a_value, const int n = 6)
{
	std::ostringstream out;
	out.precision(n); out << a_value;
	return out.str();
}

struct Point {
	double X;
	double Y;
	double Z;
};

struct Shoulder
{
	double angle = 90;
	double len;
};

struct Elbow
{
	double angle = 90;
	double len;
};

struct Wrist {
	double angle = 90;
	double angleToHorizon = 90;
	double compression = 0;
	double len;
	bool isFixed = false; // fixed relative to the elbow
};

class Manipulator {
private:
	double angle; // angle of rotation of the entire manipulator
	Shoulder shoulder; 
	Elbow elbow; 
	Wrist wrist;
	Point position;

	bool checkCoordinates(Point coordinates) {
		return true;
	}
	bool checkAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {
		return true;
	}

public:
	Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition = false) {
		this->shoulder.len = lenShoulder;
		this->elbow.len = lenElbow;
		this->wrist.len = lenWrist;

		
		if (setToInitialPosition) {
			this->position = { 0, lenElbow + lenWrist, lenShoulder };
			moveInPoint(position);
		}
	}

	bool moveInPoint(Point coordinates) {
		if (!checkCoordinates(coordinates)) {
			return false;
		}

		this->position = coordinates;

		this->angle = atan(coordinates.Y / coordinates.X) * 180 / M_PI;
		if (coordinates.X < 0) {
			this->angle *= -1;
		}
		else if(coordinates.X > 0) {
			this->angle = -this->angle + 180;
		}
		
		double h = coordinates.Z;
		double r = sqrt(pow(coordinates.X, 2) + pow(coordinates.Y, 2));
		double cc = r * r + pow(h, 2);

		if (this->wrist.isFixed) {
			double dd = pow(this->wrist.len, 2) + pow(this->elbow.len, 2) - 2 * this->wrist.len * this->elbow.len * cos((90 + this->wrist.angle) * M_PI / 180);

			this->shoulder.angle = (atan(h / r) + acos((pow(this->shoulder.len, 2) + cc - dd) / (2 * this->shoulder.len * sqrt(cc)))) * 180 / M_PI;

			this->elbow.angle = (acos((pow(this->shoulder.len, 2) + dd - cc) / (2 * this->shoulder.len * sqrt(dd))) + 
				acos((pow(this->elbow.len, 2) + dd - pow(this->wrist.len, 2)) / (2 * this->elbow.len * sqrt(dd)))) * 180 / M_PI;
		}
		else {
			h += this->wrist.len * cos(this->wrist.angleToHorizon * M_PI / 180);
			r -= this->wrist.len * sin(this->wrist.angleToHorizon * M_PI / 180);

			this->shoulder.angle = (atan(h / r) + acos((pow(this->shoulder.len, 2) + cc - pow(this->elbow.len, 2)) / (2 * this->shoulder.len * sqrt(cc)))) * 180 / M_PI;

			this->elbow.angle = acos((pow(this->shoulder.len, 2) + pow(this->elbow.len, 2) - cc) / (2 * this->shoulder.len * this->elbow.len)) * 180 / M_PI;

			this->wrist.angle = 180 - this->shoulder.angle - this->elbow.angle + this->wrist.angleToHorizon;
		}

		return true;
	}

	void wristFixed(bool isFixed = true) {
		this->wrist.isFixed = isFixed;
	}
	void wristFixed(double angle) {
		this->wrist.isFixed = true;
		this->wrist.angle = angle;
	}
	void setAngleToHorizon(double angle) {
		this->wrist.angleToHorizon = angle + 90;
	}

	bool setAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {

		if (!checkAngles(manipulatorAngle, shoulderAngle, elbowAngle, wristAngle)) {
			return false;
		}

		if (this->wrist.isFixed) {
			wristAngle = this->wrist.angle;
		}
		else {
			this->wrist.angle = wristAngle;
		}
		this->angle = manipulatorAngle;
		this->shoulder.angle = shoulderAngle;
		this->elbow.angle = elbowAngle;


		this->position.X = cos(shoulderAngle * M_PI / 180) * this->shoulder.len
			+ sin((270 - shoulderAngle - elbowAngle) * M_PI / 180) * this->elbow.len
			+ cos((360 - shoulderAngle - elbowAngle - wristAngle) * M_PI / 180) * this->wrist.len;

		this->position.Y = this->position.X * sin(manipulatorAngle * M_PI / 180);
		this->position.X = this->position.X * cos(manipulatorAngle * M_PI / 180);

		this->position.Z = sin(shoulderAngle * M_PI / 180) * this->shoulder.len 
			+ cos((270 - shoulderAngle - elbowAngle) * M_PI / 180) * this->elbow.len
			- sin((360 - shoulderAngle - elbowAngle - wristAngle) * M_PI / 180) * this->wrist.len;

		return true;

	}

	std::string Request() {
		return	to_string_with_precision(this->angle) + ' ' +
				to_string_with_precision(this->shoulder.angle) + ' ' +
				to_string_with_precision(this->elbow.angle) + ' ' +
				to_string_with_precision(this->wrist.angle) + ' ' + 
				to_string_with_precision(this->wrist.compression);
	}

	Point getPosition() {
		return this->position;
	}
};

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