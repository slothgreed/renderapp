#version 400
in vec4 v_position;				
in vec3 v_normal;
in vec2 v_texcoord;
uniform int uGeometryID;
uniform sampler2D uAlbedoMap;
uniform sampler2D uNormalMap;
uniform sampler2D uSpecularMap;		
out vec4 OutputColor0;
out vec4 OutputColor1;
out vec4 OutputColor2;
out vec4 OutputColor3;
void main(void)
{	
	vec4 pos = v_position / v_position.w;
	OutputColor0 = (pos + 1)* 0.5;
	OutputColor1 = vec4((normalize(v_normal) + 1.0)*0.5,1);
	OutputColor2 = texture2D(uAlbedoMap,v_texcoord);
	OutputColor3 = vec4(texture2D(uSpecularMap,v_texcoord).xyz,1);
}
