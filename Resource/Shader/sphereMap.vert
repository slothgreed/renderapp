#version 400

layout (location = 0) in vec4 position;
layout (location = 3) in vec2 texcoord;
uniform mat4 uMVP;
out vec2 v_texcoord;
void main(void)
{
	gl_Position = uMVP * position;
	v_texcoord = texcoord;
}

