#version 400
layout (location = 0) in vec3 position;
uniform mat4 MVP;
uniform mat3 NormalMatrix;			//�@���s��
uniform mat4 ModelMatrix;
out vec4 v_position;			//���_

void main(void)
{
	gl_Position = MVP * vec4(position, 1.0);
	v_position = vec4(position,1.0);
}

