#version 400

layout (location = 0) in vec4 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec3 color;
layout (location = 3) in vec2 texcoord;
uniform mat4 uMVP;
uniform mat3 uNormalMatrix;			//�@���s��
uniform mat4 uModelMatrix;
out vec4 v_position;			//���_
out vec3 v_color;				//�F
out vec3 v_normal;				//�@��
out vec2 v_texcoord;
void main(void)
{
	gl_Position = uMVP * position;
	v_position = position;
	v_normal = uNormalMatrix * normal;
	v_color = color;
	v_texcoord = texcoord;
}

