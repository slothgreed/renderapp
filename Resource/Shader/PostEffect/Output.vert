#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texcoord;
out vec4 v_position;			//
out vec2 v_texcoord;
void main(void)
{
	gl_Position = vec4(position, 1.0);
	v_position = vec4(position,1.0);
	v_texcoord = texcoord;
}

