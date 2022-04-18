#pragma once
class Wrist {
public:
	double angle = 90;
	double angleToHorizon = 90;
	double compression = 0;
	double len;
	bool isFixed = false; // fixed relative to the elbow
};

