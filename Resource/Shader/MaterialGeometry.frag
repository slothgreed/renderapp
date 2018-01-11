#version 400
in vec4 v_position;			
in vec3 v_color;			
in vec3 v_normal;			
in vec4 m_position;
in vec2 v_texcoord;
uniform int textureNum;
uniform int uGeometryID;
uniform sampler2D uAlbedoMap;
uniform sampler2D uNormalMap;
uniform sampler2D uHeightMap;
uniform mat4 uModelMatrix;
uniform vec3 uLightPosition;
uniform vec3 uCameraPosition;
uniform mat4 uCameraMatrix;
out vec4 OutputColor0;
out vec4 OutputColor1;
out vec4 OutputColor2;
out vec4 OutputColor3;
vec3 GetADS(vec3 normal)
{
	//点からライト方向
	
	vec3 lightDir = normalize(uLightPosition - (uModelMatrix * m_position).xyz);	
	vec3 eye = normalize(uCameraPosition -  ( uModelMatrix * m_position).xyz);//点から視線方向
	
	float diffuse = max(dot(lightDir,normal),0.0)* 0.5 + 0.5;
	vec3 spec_half = normalize(lightDir + eye);//ハーフベクトル
	float specular = pow(dot(normal,spec_half),50.0);
	return vec3(specular,specular,specular);
}

void main(void)
{	
	vec4 pos = v_position / v_position.w;
	pos = (pos + 1)* 0.5;
	OutputColor0 = vec4(pos.xyz,1.0);
	OutputColor1 = vec4((normalize(v_normal) + 1.0)*0.5,1.0);
	OutputColor2 = vec4(texture2D(uAlbedoMap,v_texcoord),1);
	OutputColor3 = vec4(1);
}
