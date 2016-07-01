#version 400
layout (location = 0) in vec3 position;
out vec4 v_position;
uniform mat4 MVP;
void main(void)
{
	gl_Position = MVP * vec4(position, 1.0);	
	v_position = MVP * vec4(position, 1.0);
	
	
}
