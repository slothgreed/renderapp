#version 400
layout (location = 0) in vec3 position;

uniform mat4 uMVP;
void main(void)
{
	gl_Position = uMVP * vec4(position, 1.0);
}
