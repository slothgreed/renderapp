#version 400
in vec4 g_position;			//���_
in vec3 g_color;			//�F
in vec3 g_normal;			//�@��
in vec2 g_texcoord;			//�e�N�X�`��

uniform mat4 ModelMatrix;
uniform vec3 LightPos;
uniform vec3 CameraPos;
uniform mat4 CameraMatrix;
vec3 GetADS(vec3 normal)
{
	//�_���烉�C�g����
	
	vec3 lightDir = normalize(LightPos - (ModelMatrix * g_position).xyz);	
	vec3 eye = normalize(CameraPos -  ( ModelMatrix * g_position).xyz);//�_���王������
	
	float diffuse = max(dot(lightDir,normal),0.0)* 0.5 + 0.5;
	vec3 spec_half = normalize(lightDir + eye);//�n�[�t�x�N�g��
	float specular = pow(dot(normal,spec_half),50.0);
	return vec3(specular,specular,specular);
}


void main(void)
{	
	vec4 pos = g_position/g_position.w;
	pos = (pos + 1)* 0.5;
	gl_FragData[0] = pos;
	gl_FragData[1] = vec4((normalize(g_normal) + 1.0)*0.5,1.0);
	gl_FragData[2] = vec4(g_color,1.0);
	gl_FragData[3] = vec4(GetADS(g_normal),1.0);
}
	
