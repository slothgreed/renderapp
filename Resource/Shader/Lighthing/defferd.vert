#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texcoord;
uniform mat4 uShadowMatrix;
out vec4 v_position;			//頂点
out vec2 v_texcoord;			//テクスチャ座標
void main(void)
{
	gl_Position = vec4(position, 1.0);
	v_position = vec4(position,1.0);
	vec4 v_shadowPosition = uShadowMatrix * vec4(0);
	v_texcoord = texcoord;
}

