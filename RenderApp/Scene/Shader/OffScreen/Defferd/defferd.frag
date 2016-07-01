#version 400
in vec4 v_position;			//頂点
in vec2 v_texcoord;			//テクスチャ
uniform sampler2D shadow_2D;			//フレームバッファ番号
uniform sampler2D normal_2D;
uniform sampler2D color_2D;
uniform sampler2D posit_2D;
uniform sampler2D light_2D;

uniform mat4 LightMatrix;
uniform mat4 Project;
uniform vec3 LightPos;
uniform vec3 CameraPos;
uniform mat4 UnProject;
uniform mat4 CameraMatrix;

uniform vec4 fogColor;
uniform int fogStart;
uniform int fogEnd;
vec4 getWorldPos(vec4 texturePos)
{
	vec4 worldPos =  UnProject * texturePos;
	worldPos /= worldPos.w;
	return worldPos;
}
//フォグ適用後の色値
vec4 getFogColor(vec4 color,vec4 worldPos)
{
	float length = length(worldPos.xyz-CameraPos.xyz);
	float f = (fogEnd - length) / (fogEnd - fogStart);
	return mix(fogColor,color,f);
}
vec4 GetADS(vec4 normal,vec4 world)
{
	//点からライト方向
	vec3 lightDir = normalize(LightPos - world.xyz);	
	vec3 eye = normalize(CameraPos -  world.xyz);//点から視線方向
	float diffuse = max(dot(lightDir,normal.xyz),0.0);
	vec3 spec_half = normalize(lightDir + eye);//ハーフベクトル
	float specular = pow(dot(normal.xyz,spec_half),1.0);
	float color = diffuse * 0.7 + specular * 0.3;
	return vec4(color,color,color,1);
	
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
	float mapDepth = texture2DProj(shadow_2D,mapCoord).z;
	if(mapDepth < depth-0.001)
	{
		return 0.2;
	}
	return 1;
}
void main(void)
{	
	vec4 pixelColor = vec4(1.0,0,0,1.0);
	
	vec2 coord = v_texcoord;
	vec4 normal = texture2D(normal_2D,coord.xy) * 2 - 1;
	vec4 position  = texture2D(posit_2D,coord.xy) * 2 - 1;
	vec4 world = getWorldPos(position);
	float shadow = getShadow(world);
	vec4 color  = texture2D(color_2D,coord.xy);
	pixelColor = color * GetADS(normal,world) * shadow;
	//pixelColor = getFogColor(color,world);
	pixelColor.a = 1;
	gl_FragColor = pixelColor;
	
}

