#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec3 color;
layout (location = 3) in vec2 texcoord;
uniform mat4 MVP;
uniform mat3 NormalMatrix;			//法線行列
uniform mat4 ModelMatrix;
out vec4 v_position;			//頂点
out vec3 v_color;				//色
out vec3 v_normal;				//法線
out vec4 m_position;
out vec2 v_texcoord;			
void main(void)
{
	gl_Position = MVP * vec4(position, 1.0);
	v_position = gl_Position;
	m_position = vec4(position,1.0);
	v_normal = NormalMatrix * normal;
	v_color = color;
	v_texcoord = texcoord;
}

