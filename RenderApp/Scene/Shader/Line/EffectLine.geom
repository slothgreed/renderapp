#version 400
#extension GL_EXT_geometry_shader4 : enable
layout(lines) in;
layout(triangle_strip, max_vertices = 60) out;
smooth out vec3 fragViewPos;
flat out vec3 g_position;			
flat out vec3 g_otherPosition;

uniform mat4 ProjMatrix;


const vec3 Y = vec3(0,1,0);
const vec3 X = vec3(1,0,0);
const float radius = 5.0;
void main(void)
{
	vec3 v1 = gl_PositionIn[0].xyz;
	vec3 v2 = gl_PositionIn[1].xyz;

	g_position = v1;
	g_otherPosition = v2;

	//ü‚Ì’·‚³
	vec3 lineDir = v2 - v1;
	float lineLength = length(lineDir);
	//³‹K‰»
	vec3 normLineDir = lineDir / lineLength;

	vec3 oobb2 = cross(normLineDir,Y);
	vec3 oobb3 = cross(normLineDir,oobb2);
	if(abs(normLineDir.y) > 0.999)
	{
		oobb3 = cross(normLineDir,X);
	}
	
	oobb2 *= radius;
	oobb3 *= radius;

	vec3 lineDirOffsetM = radius * normLineDir;
	vec3 lineDirOffsetP = lineDir + lineDirOffsetM;

	vec4 viewPos000 = vec4( v1 -oobb2 - oobb3 - lineDirOffsetM,1.0);
	vec4 viewPos001 = vec4( v1 -oobb2 + oobb3 - lineDirOffsetM,1.0);
	vec4 viewPos010 = vec4( v1 +oobb2 - oobb3 - lineDirOffsetM,1.0);
	vec4 viewPos011 = vec4( v1 +oobb2 + oobb3 - lineDirOffsetM,1.0);
	vec4 viewPos100 = vec4( v1 -oobb2 - oobb3 + lineDirOffsetP,1.0);
	vec4 viewPos101 = vec4( v1 -oobb2 + oobb3 + lineDirOffsetP,1.0);
	vec4 viewPos110 = vec4( v1 +oobb2 - oobb3 + lineDirOffsetP,1.0);
	vec4 viewPos111 = vec4( v1 +oobb2 + oobb3 + lineDirOffsetP,1.0);

	vec4 viewPos000proj = ProjMatrix * viewPos000;
	vec4 viewPos001proj = ProjMatrix * viewPos001;
	vec4 viewPos010proj = ProjMatrix * viewPos010;
	vec4 viewPos011proj = ProjMatrix * viewPos011;
	vec4 viewPos100proj = ProjMatrix * viewPos100;
	vec4 viewPos101proj = ProjMatrix * viewPos101;
	vec4 viewPos110proj = ProjMatrix * viewPos110;
	vec4 viewPos111proj = ProjMatrix * viewPos111;
	
	fragViewPos = viewPos001.xyz;
	gl_Position = viewPos001proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos000.xyz;
	gl_Position = viewPos000proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos101.xyz;
	gl_Position = viewPos101proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos100.xyz;
	gl_Position = viewPos100proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos111.xyz;
	gl_Position = viewPos111proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos110.xyz;
	gl_Position = viewPos110proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos011.xyz;
	gl_Position = viewPos011proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos010.xyz;
	gl_Position = viewPos010proj;
	EmitVertex();/////////////////////////////////////
    EndPrimitive();//////////////////////////////////////////////////////////////////////////
	
	fragViewPos = viewPos101.xyz;
	gl_Position = viewPos101proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos111.xyz;
	gl_Position = viewPos111proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos001.xyz;
	gl_Position = viewPos001proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos011.xyz;
	gl_Position = viewPos011proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos000.xyz;
	gl_Position = viewPos000proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos010.xyz;
	gl_Position = viewPos010proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos100.xyz;
	gl_Position = viewPos100proj;
	EmitVertex();/////////////////////////////////////
	fragViewPos = viewPos110.xyz;
	gl_Position = viewPos110proj;
	EmitVertex();/////////////////////////////////////
    EndPrimitive();//////////////////////////////////////////////////////////////////////////
	
}

