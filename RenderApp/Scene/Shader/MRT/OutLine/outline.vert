#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec3 color;
layout (location = 3) in vec2 texcoord;

out vec4 v_position;			//���_
out vec3 v_normal;				//�@��
out vec3 v_color;				//�F
out vec2 v_texcoord;			//�e�N�X�`�����W

void main(void)
{
	gl_Position = vec4(position, 1.0);
	
	v_position = gl_Position;
	v_normal = normal;
	v_color = color;
	v_texcoord = texcoord;
}

