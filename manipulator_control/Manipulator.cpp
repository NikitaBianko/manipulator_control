#include "Manipulator.h"

template <typename T>
std::string to_string_with_precision(const T a_value, const int n = 6) {
	std::ostringstream out;
	out.precision(n); out << a_value;
	return out.str();
}

bool Manipulator::checkCoordinates(Point coordinates) {

	double len = pow(coordinates.X, 2) + pow(coordinates.X, 2) + pow(coordinates.X, 2);

	if (len > pow(shoulder.len + elbow.len + wrist.len, 2)) {
		return false;
	}
	if (coordinates.Y < 0) {
		return false;
	}

	return true;
}

bool Manipulator::checkAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {
	if (manipulatorAngle < 0 || manipulatorAngle > 180) {
		return false;
	}
	if (shoulderAngle < 0 || shoulderAngle > 180) {
		return false;
	}
	if (elbowAngle < 0 || elbowAngle > 180) {
		return false;
	}
	if (wristAngle < 0 || wristAngle > 180) {
		return false;
	}

	return true;
}

Manipulator::Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition) {
	this->shoulder.len = lenShoulder;
	this->elbow.len = lenElbow;
	this->wrist.len = lenWrist;


	if (setToInitialPosition) {
		this->position = { 0, lenElbow + lenWrist, lenShoulder };
		moveInPoint(position);
	}
}

bool Manipulator::moveInPoint(Point coordinates) {
	if (!checkCoordinates(coordinates)) {
		return false;
	}

	this->position = coordinates;

	this->angle = atan(coordinates.Y / coordinates.X) * 180 / M_PI;
	if (coordinates.X < 0) {
		this->angle *= -1;
	}
	else if (coordinates.X > 0) {
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

void Manipulator::wristFixed(bool isFixed) {
	this->wrist.isFixed = isFixed;
}

void Manipulator::wristFixed(double angle) {
	this->wrist.isFixed = true;
	this->wrist.angle = angle;
}

void Manipulator::setAngleToHorizon(double angle) {
	this->wrist.angleToHorizon = angle + 90;
}

bool Manipulator::setAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {

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

std::string Manipulator::Request() {
	return to_string_with_precision(this->angle) + ' ' +
		to_string_with_precision(this->shoulder.angle) + ' ' +
		to_string_with_precision(this->elbow.angle) + ' ' +
		to_string_with_precision(this->wrist.angle) + ' ' +
		to_string_with_precision(this->wrist.compression);
}

Point Manipulator::getPosition() {
	return this->position;
}