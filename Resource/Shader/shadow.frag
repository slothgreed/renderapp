#version 400
in vec4 v_position;			//’¸“_
out vec4 OutputColor;

void main(void)
{
	float depth = (v_position.z / v_position.w + 1.0) * 0.5;
    OutputColor = vec4(vec3(depth), 1.0);
}
