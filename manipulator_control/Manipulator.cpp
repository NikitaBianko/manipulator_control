#include "Manipulator.h"

template <typename T>
std::string ToStringWithPrecision(const T a_value, const int n = 6) {
	std::ostringstream out;
	out.precision(n); out << a_value;
	return out.str();
}

bool Manipulator::CheckCoordinates(Point coordinates) {

	double len = pow(coordinates.X, 2) + pow(coordinates.X, 2) + pow(coordinates.X, 2);

	if (len > pow(shoulder.len + elbow.len + wrist.len, 2)) {
		return false;
	}
	if (coordinates.Y < 0) {
		return false;
	}

	return true;
}

bool Manipulator::CheckAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {
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

Manipulator::Manipulator()
{
}

Manipulator::Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition) {
	this->shoulder.len = lenShoulder;
	this->elbow.len = lenElbow;
	this->wrist.len = lenWrist;


	if (setToInitialPosition) {
		this->position = { 0, lenElbow + lenWrist, lenShoulder };
		MoveInPoint(position);
	}
}

bool Manipulator::MoveInPoint(Point coordinates) {
	if (!CheckCoordinates(coordinates)) {
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

	if (this->wrist.is_fixed) {
		double cc = r * r + pow(h, 2);
		double dd = pow(this->wrist.len, 2) + pow(this->elbow.len, 2) - 2 * this->wrist.len * this->elbow.len * cos((90 + this->wrist.angle) * M_PI / 180);
		this->shoulder.angle = (atan(h / r) + acos((pow(this->shoulder.len, 2) + cc - dd) / (2 * this->shoulder.len * sqrt(cc)))) * 180 / M_PI;
		this->elbow.angle = (acos((pow(this->shoulder.len, 2) + dd - cc) / (2 * this->shoulder.len * sqrt(dd))) +
			acos((pow(this->elbow.len, 2) + dd - pow(this->wrist.len, 2)) / (2 * this->elbow.len * sqrt(dd)))) * 180 / M_PI;
		//this->wrist.angleToHorizon
	}
	else {
		h += this->wrist.len * cos(this->wrist.angle_to_horizon * M_PI / 180);
		r -= this->wrist.len * sin(this->wrist.angle_to_horizon * M_PI / 180);
		double cc = r * r + pow(h, 2);
		this->shoulder.angle = (atan(h / r) + acos((pow(this->shoulder.len, 2) + cc - pow(this->elbow.len, 2)) / (2 * this->shoulder.len * sqrt(cc)))) * 180 / M_PI;
		this->elbow.angle = acos((pow(this->shoulder.len, 2) + pow(this->elbow.len, 2) - cc) / (2 * this->shoulder.len * this->elbow.len)) * 180 / M_PI;
		this->wrist.angle = 180 - this->shoulder.angle - this->elbow.angle + this->wrist.angle_to_horizon;
	}

	return true;
}

void Manipulator::SetFixedWrist(bool isFixed) {
	this->wrist.is_fixed = isFixed;
}

void Manipulator::SetFixedWrist(double angle) {
	this->wrist.is_fixed = true;
	this->wrist.angle = angle;
	MoveInPoint(this->position);
}

void Manipulator::SetAngleToHorizon(double angle) {
	this->wrist.angle_to_horizon = angle + 90;
	MoveInPoint(this->position);
}

bool Manipulator::SetAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle) {

	if (!CheckAngles(manipulatorAngle, shoulderAngle, elbowAngle, wristAngle)) {
		return false;
	}

	this->wrist.angle = wristAngle;
	//this->wrist.angle_to_horizon
	this->angle = manipulatorAngle;
	this->shoulder.angle = shoulderAngle;
	this->elbow.angle = elbowAngle;


	this->position.Y = cos(shoulderAngle * M_PI / 180) * this->shoulder.len
		+ sin((elbowAngle + shoulderAngle - 90) * M_PI / 180) * this->elbow.len
		- cos((elbowAngle + shoulderAngle + wristAngle - 90) * M_PI / 180) * this->wrist.len;

	this->position.X = this->position.Y * cos((180 - manipulatorAngle) * M_PI / 180);
	this->position.Y *= sin((180 - manipulatorAngle) * M_PI / 180);

	this->position.Z = sin(shoulderAngle * M_PI / 180) * this->shoulder.len
		- cos((elbowAngle + shoulderAngle - 90) * M_PI / 180) * this->elbow.len
		+ sin((elbowAngle + shoulderAngle + wristAngle - 90) * M_PI / 180) * this->wrist.len;

	return true;

}

std::string Manipulator::GetAnglesByString() {
	return ToStringWithPrecision(this->angle) + ' ' +
		ToStringWithPrecision(this->shoulder.angle) + ' ' +
		ToStringWithPrecision(this->elbow.angle) + ' ' +
		ToStringWithPrecision(this->wrist.angle) + ' ' +
		ToStringWithPrecision(this->wrist.compression);
}

Point Manipulator::GetPosition() {
	return this->position;
}

double Manipulator::GetAngle() {
	return angle;
}

double Manipulator::GetShoulderAngle() {
	return this->shoulder.angle;
}

double Manipulator::GetElbowAngle() {
	return this->elbow.angle;
}

double Manipulator::GetWristAngle() {
	return this->wrist.angle;
}