#version 400
#extension GL_NV_shadow_samplers_cube : enable

in vec4 v_position;			//頂点
in vec2 v_texcoord;			//テクスチャ
uniform sampler2D uAlbedoMap;
uniform sampler2D uNormalMap;
uniform sampler2D uWorldMap;
uniform samplerCube uCubeMap;
uniform vec3 uCameraPosition;
uniform mat4 uUnProjectMatrix;
out vec4 OutputColor;

vec4 getWorldPos(vec4 texturePos)
{
	vec4 worldPos =  uUnProjectMatrix * texturePos;
	worldPos /= worldPos.w;
	return worldPos;
}

vec3 schlick(vec3 f0, float product)
{
    return f0 + (1.0 - f0) * pow((1.0 - product), 5.0);
}

void main(void)
{	
	vec4 pixelColor = vec4(1.0,0,0,1.0);
	vec2 coord = v_texcoord;
	
	vec3 normal = vec3(texture2D(uNormalMap,coord.xy) * 2 - 1).xyz;
	vec4 pos	= texture2D(uWorldMap,coord.xy) * 2 - 1;
	vec4 world	= getWorldPos(pos);
	vec4 albedo	= texture2D(uAlbedoMap,coord.xy);
	
	vec3 cameraDir = normalize(world.xyz - uCameraPosition);
	vec3 vref	= reflect(cameraDir, normal.xyz);
	OutputColor = textureCube(uCubeMap,vref);
}

