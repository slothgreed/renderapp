#version 400
in vec4 g_position;			//���_
in vec3 g_color;			//�F
in vec3 g_normal;			//�@��
in vec2 g_texcoord;			//�e�N�X�`��
in float g_FUROFFSET;
uniform mat4 ModelMatrix;
uniform vec3 LightPos;
uniform vec3 CameraPos;
uniform mat4 CameraMatrix;
uniform sampler2D FurTexture;
uniform sampler2D FurBumpTexture;
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
	vec4 bump = texture2D(FurBumpTexture,g_texcoord);
	if(bump.b <= g_FUROFFSET || bump.a <= 0.0)
	{
		discard;
	}
	gl_FragData[0] = pos;
	gl_FragData[1] = vec4((normalize(g_normal) + 1.0)*0.5,1.0);
	gl_FragData[2] = texture2D(FurTexture,g_texcoord);
	gl_FragData[3] = vec4(GetADS(g_normal),1.0);
}
	
