#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;
out vec4 v_position;
out vec3 v_color;
void main(void)
{
	gl_Position = vec4(position, 1.0);	
	v_color = color;
}
