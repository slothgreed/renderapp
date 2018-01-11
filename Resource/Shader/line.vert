#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;
out vec3 v_color;
uniform mat4 MVP;
void main(void)
{
	gl_Position = MVP * vec4(position, 1.0);	
	v_color = color;
}
