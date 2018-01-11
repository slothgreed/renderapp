#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 3) in vec2 texcoord;
uniform mat4 uMVP;
uniform mat3 uNormalMatrix;		//法線行列
out vec4 v_position;			//頂点
out vec3 v_normal;				//法線
out vec2 v_texcoord;			
void main(void)
{
	gl_Position = uMVP * vec4(position, 1.0);
	v_position = gl_Position;
	v_normal = uNormalMatrix * normal;
	v_texcoord = texcoord;
}

