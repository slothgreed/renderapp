#version 400
layout (location = 0) in vec3 position;
out vec3 v_position;

void main(void)
{
	gl_Position = vec4(position, 1.0);
	v_position = position;
}
