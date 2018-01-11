#version 400

in vec4 v_position;			//頂点
in vec3 v_color;			//色
in vec3 v_normal;			//法線
in vec2 v_texcoord;			//テクスチャ
uniform mat4 uModelMatrix;
uniform vec3 uLightPosition;				//光源の位置
uniform vec3 uCameraPosition;

uniform mat4 uProjectMatrix;
uniform mat4 uLightMatrix;
uniform sampler2D ShadowMap;
uniform sampler2D uAlbedoMap;
out vec4 OutputColor;

vec3 GetADS()
{
	//点からライト方向
	vec3 LightDir = normalize(uLightPosition - (uModelMatrix * v_position).xyz);	
	vec3 m_Eye = normalize(uCameraPosition -  (uModelMatrix * v_position).xyz);//点から視線方向
	float diffuse = max(dot(LightDir,v_normal),0.0) * 0.5 + 0.5;
	vec3 spec_half = normalize(LightDir + m_Eye);//ハーフベクトル
	float specular = pow(dot(v_normal,spec_half),50.0);

	return vec3(diffuse * 0.8 + specular * 0.2);
}
float getShadow(vec4 world)
{
	vec4 mapCoord = uProjectMatrix * uLightMatrix * world;
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
	OutputColor = texture2D(uAlbedoMap,v_texcoord);
}
