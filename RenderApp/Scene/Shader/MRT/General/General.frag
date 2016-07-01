#version 400
in vec4 v_position;			
in vec3 v_color;			
in vec3 v_normal;			
in vec4 m_position;
in vec2 v_texcoord;
uniform int textureNum;
uniform sampler2D texture1;
uniform sampler2D texture2;
uniform sampler2D texture3;		
uniform mat4 ModelMatrix;
uniform vec3 LightPos;
uniform vec3 CameraPos;
uniform mat4 CameraMatrix;
vec3 GetADS(vec3 normal)
{
	//点からライト方向
	
	vec3 lightDir = normalize(LightPos - (ModelMatrix * m_position).xyz);	
	vec3 eye = normalize(CameraPos -  ( ModelMatrix * m_position).xyz);//点から視線方向
	
	float diffuse = max(dot(lightDir,normal),0.0)* 0.5 + 0.5;
	vec3 spec_half = normalize(lightDir + eye);//ハーフベクトル
	float specular = pow(dot(normal,spec_half),50.0);
	return vec3(specular,specular,specular);
}
vec4 GetColor()
{
	if(textureNum == 1)
	{
		return texture2D(texture1,v_texcoord);
	}
	return vec4(v_color,1.0);
	// check
	//return vec4(vec3(mod(floor(s) + floor(t),2.0)),1);
}
void main(void)
{	
	vec4 pos = v_position / v_position.w;
	pos = (pos + 1)* 0.5;
	gl_FragData[0] = pos;
	gl_FragData[1] = vec4((normalize(v_normal) + 1.0)*0.5,1.0);
	gl_FragData[2] = GetColor();
	gl_FragData[3] = vec4(GetADS(v_normal),1);
}
