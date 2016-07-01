#version 400

in int v_position;			//���_
in vec3 v_color;			//�F
in vec3 v_normal;			//�@��

uniform mat4 ModelMatrix;
uniform vec3 LightPos;				//�����̈ʒu
uniform vec3 CameraPos;

uniform mat4 Project;
uniform mat4 LightMatrix;
uniform sampler2D ShadowMap;
layout (location = 4) out vec4 FragColor;

vec3 GetADS()
{
	//�_���烉�C�g����
	vec3 LightDir = normalize(LightPos - (ModelMatrix * v_position).xyz);	
	vec3 m_Eye = normalize(CameraPos -  (ModelMatrix * v_position).xyz);//�_���王������
	float diffuse = max(dot(LightDir,v_normal),0.0) * 0.5 + 0.5;
	vec3 spec_half = normalize(LightDir + m_Eye);//�n�[�t�x�N�g��
	float specular = pow(dot(v_normal,spec_half),50.0);

	return vec3(diffuse * 0.8 + specular * 0.2);
}
float getShadow(vec4 world)
{
	vec4 mapCoord = Project * LightMatrix * world;
	mapCoord /= mapCoord.w;
	mapCoord = (mapCoord + 1.0) * 0.5; 
	float depth = mapCoord.z;
	if(mapCoord.x <= 0 || mapCoord.y <= 0 || mapCoord.z <= 0 ||
	   mapCoord.x >= 1 || mapCoord.y >= 1 || mapCoord.z >= 1)
	{
		return 1;
	}
	float mapDepth = texture2DProj(ShadowMap,mapCoord).z;
	if(mapDepth < depth-0.001)
	{
		return 0.5;
	}
	return 1;
}
void main(void)
{	
	vec3 light_color = GetADS();	
	gl_FragColor = vec4(vec3(0,0,1) * light_color,1.0);
}
