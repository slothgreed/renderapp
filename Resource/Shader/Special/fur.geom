#version 400
#extension GL_EXT_geometry_shader4 : enable
#define OUTNUM 20 
uniform mat4 MVP;					//ƒ‚ƒfƒ‹*camera*proj
uniform float Offset;
uniform vec3 CameraPos;
uniform mat3 NormalMatrix;
layout (triangles) in;
layout( triangle_strip, max_vertices = 60) out;
in vec4 v_position[];
in vec3 v_color[];	
in vec3 v_normal[];	
in vec2 v_texcoord[];
out float FUR_OFFSET;
out vec4 g_position;
out vec3 g_color;	
out vec3 g_normal;	
out vec2 g_texcoord;
out float g_FUROFFSET;
void main (void)
{
	float OffSet = 0.01 / OUTNUM;
	float sum = 0;
	int index;
	for(int i = 0; i < OUTNUM; i++)
	{
		g_FUROFFSET = sum;
		g_position = v_position[0] + vec4(v_normal[0] * sum,0.0);
		g_color = vec3(0);
		g_normal = NormalMatrix * v_normal[0];
		g_texcoord = g_position.xy;
		gl_Position = MVP * g_position;
		g_position = gl_Position;
		EmitVertex();

		index = 3 * i + 1;
		g_position = v_position[1] + vec4(v_normal[1] * sum,0.0);
		g_color = vec3(0);
		g_normal = NormalMatrix * v_normal[1];
		g_texcoord = g_position.xy;
		gl_Position = MVP * g_position;
		g_position = gl_Position;
		EmitVertex();

		index = 3 * i + 2;
		g_position = v_position[2] + vec4(v_normal[2] * sum,0.0);
		g_color = vec3(0);
		g_normal = NormalMatrix * v_normal[2];
		g_texcoord = g_position.xy;
		gl_Position = MVP * g_position;
		g_position = gl_Position;
		EmitVertex();

		EndPrimitive();
		
		sum += OffSet;
	}
}
