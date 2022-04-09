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
	double axis = 90;
	double len;
};

struct Elbow
{
	double axis = 90;
	double len;
};

struct Wrist {
	double axis = 90;
	double compression = 0;
	double len;
	bool isFixed = false;
};

class Manipulator {
private:
	double axis; // axis of rotation of the entire manipulator
	Shoulder shoulder; 
	Elbow elbow; 
	Wrist wrist;

public:
	Manipulator(double lenShoulder, double lenElbow, double lenWrist) {
		this->shoulder.len = lenShoulder;
		this->elbow.len = lenElbow;
		this->wrist.len = lenWrist;
	}

	void moveInPoint(Point coordinates) {
		this->axis = atan(coordinates.Y / coordinates.X) * 180 / M_PI;
		if (coordinates.X < 0) {
			this->axis *= -1;
		}
		else if(coordinates.X > 0) {
			this->axis = -this->axis + 180;
		}

		double rr = pow(coordinates.X, 2) + pow(coordinates.Y, 2);
		double cc = rr + pow(coordinates.Z, 2);

		this->shoulder.axis = (atan(coordinates.Z / sqrt(rr)) + acos((pow(shoulder.len, 2) + cc - pow(elbow.len, 2)) / (2 * shoulder.len * sqrt(cc)))) * 180 / M_PI;

		this->elbow.axis = acos((pow(shoulder.len, 2) + pow(elbow.len, 2) - cc) / (2 * shoulder.len * elbow.len)) * 180 / M_PI;

		if (this->wrist.isFixed) {
			this->wrist.axis = 0.0;
		}
	}

	std::string Request() {
		return	to_string_with_precision(this->axis) + ' ' + 
				to_string_with_precision(this->shoulder.axis) + ' ' + 
				to_string_with_precision(this->elbow.axis) + ' ' + 
				to_string_with_precision(this->wrist.axis) + ' ' + 
				to_string_with_precision(this->wrist.compression);
	}
};

int main() {

	Manipulator manipulator(3, 3, 3);

	Point newPoint = { 3, 3, 3};

	manipulator.moveInPoint(newPoint);

	std::cout << manipulator.Request();

	return 0;
}