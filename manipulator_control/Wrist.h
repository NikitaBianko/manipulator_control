#pragma once
class Wrist {
public:
	double angle = 90;
	double angle_to_horizon = 90;
	double compression = 0;
	double len;
	bool is_fixed = false; // fixed relative to the elbow
};

