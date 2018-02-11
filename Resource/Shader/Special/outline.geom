#version 400
#extension GL_EXT_geometry_shader4 : enable
uniform mat4 uMVP;					//ƒ‚ƒfƒ‹*camera*proj
uniform float uOffset;
uniform mat3 uNormalMatrix;
layout (triangles) in;
layout( triangle_strip, max_vertices = 6) out;
in vec4 v_position[];
in vec3 v_color[];	
in vec3 v_normal[];	
in vec2 v_texcoord[];

out vec4 g_position;
out vec3 g_color;	
out vec3 g_normal;	
out vec2 g_texcoord;

void main (void)
{
	vec4 gravity = gl_in[0].gl_Position + gl_in[1].gl_Position + gl_in[2].gl_Position;
	
	gravity /= 3;
	vec4 vector;
	for(int i = 0; i < gl_in.length(); i++)
	{
		vector = gravity - gl_in[i].gl_Position;
		g_position =  uMVP * (gl_in[i].gl_Position + uOffset * vector);
		g_color = v_color[i];
		g_normal = uNormalMatrix * v_normal[i];
		g_texcoord = v_texcoord[i];
		gl_Position = g_position;
		EmitVertex();
	}
	EndPrimitive();
	/*
	for(int i = 0; i < gl_in.length(); i++)
	{
		g_position = v_position[i];
		g_color = vec3(0);
		g_normal = v_normal[i];
		g_texcoord = v_texcoord[i];
		gl_Position = uMVP * gl_PositionIn[i];
		EmitVertex();
	
	}
	EndPrimitive();
	*/
}
