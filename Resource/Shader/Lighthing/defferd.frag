#version 400
in vec4 v_position;			//頂点
in vec2 v_texcoord;			//テクスチャ
uniform sampler2D uShadowMap;			//フレームバッファ番号
uniform sampler2D uAlbedoMap;
uniform sampler2D uNormalMap;
uniform sampler2D uWorldMap;
uniform sampler2D uLightingMap;

uniform mat4 uLightMatrix;
uniform mat4 uProjectMatrix;
uniform vec3 uLightPosition;
uniform vec3 uCameraPosition;
uniform mat4 uUnProjectMatrix;

uniform vec4 fogColor;
uniform int fogStart;
uniform int fogEnd;

out vec4 OutputColor;

vec4 getWorldPos(vec4 texturePos)
{
	vec4 worldPos =  uUnProjectMatrix * texturePos;
	worldPos /= worldPos.w;
	return worldPos;
}
//フォグ適用後の色値
vec4 getFogColor(vec4 color,vec4 worldPos)
{
	float length = length(worldPos.xyz-uCameraPosition.xyz);
	float f = (fogEnd - length) / (fogEnd - fogStart);
	return mix(fogColor,color,f);
}
vec4 getADS(vec4 color, vec4 normal, vec4 world)
{
	//点からライト方向
	vec3 lightDir 	= normalize(uLightPosition - world.xyz);	
	vec3 cameraDir 	= normalize(uCameraPosition -  world.xyz);	//点から視線方向
	float diffuse 	= max(dot(lightDir,normal.xyz),0.0);
	vec3 spec_half 	= normalize(lightDir + cameraDir);			//ハーフベクトル
	float specular 	= pow(dot(normal.xyz,spec_half),1.0);
	float value 	= diffuse * 0.7 + specular * 0.3;
	return value * color;
	
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
	float mapDepth = texture2DProj(uShadowMap,mapCoord).z;
	if(mapDepth < depth-0.001)
	{
		return 0.2;
	}
	return 1;
}

void main(void)
{	
	vec4 normal = texture2D(uNormalMap,v_texcoord.xy) * 2 - 1;
	normal = vec4(normal.xyz,1);
	
	vec4 position  = texture2D(uWorldMap,v_texcoord.xy) * 2 - 1;
	vec4 world = getWorldPos(position);

	//float shadow = getShadow(world);
	vec4 albedo  = texture2D(uAlbedoMap,v_texcoord.xy);
	vec4 pixelColor = getADS(albedo, normal, world);

	//pixelColor = getFogColor(albedo,world);
	pixelColor.a = 1;
	OutputColor = pixelColor;
	
}

