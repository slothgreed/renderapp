#version 400
in vec4 v_position;			
in vec3 v_color;			
in vec3 v_normal;			
in vec4 m_position;
in vec2 v_texcoord;
uniform int textureNum;
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
vec3 GetColor()
{
	return texture2D(uAlbedoMap,v_texcoord).xyz;
	return vec4(v_color,1.0);
	// check
	//return vec4(vec3(mod(floor(s) + floor(t),2.0)),1);
}
void main(void)
{	
	vec4 pos = v_position / v_position.w;
	pos = (pos + 1)* 0.5;
	OutputColor0 = vec4(pos.x,pos.y,pos.z,1.0);
	OutputColor1 = vec4(((normalize(v_normal) + 1.0)*0.5,1.0);
	OutputColor2 = vec4(GetColor(),1.0);
	OutputColor3 = vec4(1.0);
}
