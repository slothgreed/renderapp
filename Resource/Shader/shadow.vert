#version 400
layout (location = 0) in vec3 position;
out vec4 v_position;
uniform mat4 uSMVP;	//shadow用
void main(void)
{
	gl_Position = uSMVP * vec4(position, 1.0);	
	v_position = uSMVP * vec4(position, 1.0);
}
