#version 400
layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;
uniform mat3 NormalMatrix;

in vec3 te_position[3];
in vec3 te_normal[3];
in vec2 te_texcoord[3];
in vec3 tCameraDir[3];
in vec3 tLightDir[3];

in vec3 tePatchDistance[3];

out vec3 gFacetNormal;
out vec3 gPatchDistance;
out vec3 gTriDistance;
out vec3 g_normal;
out vec2 g_texcoord;

out vec3 g_tCameraDir;
out vec3 g_tLightDir;
void main()
{
    vec3 A = (te_position[1] - te_position[0]).xyz;
    vec3 B = (te_position[2] - te_position[0]).xyz;
    
    gFacetNormal = vec3(tePatchDistance[1].z);
    
    gPatchDistance = tePatchDistance[0];
	g_normal = te_normal[0];
	g_texcoord = te_texcoord[0];
    gTriDistance = vec3(1, 0, 0);
	g_tCameraDir = tCameraDir[0];
	g_tLightDir = tLightDir[0];
    gl_Position = gl_in[0].gl_Position; EmitVertex();

    gPatchDistance = tePatchDistance[1];
    gTriDistance = vec3(0, 1, 0);
	g_normal = te_normal[1];
	g_texcoord = te_texcoord[1];
	g_tCameraDir = tCameraDir[1];
	g_tLightDir = tLightDir[1];
	gl_Position = gl_in[1].gl_Position; EmitVertex();

    gPatchDistance = tePatchDistance[2];
    gTriDistance = vec3(0, 0, 1);
	g_normal = te_normal[2];
	g_texcoord = te_texcoord[2];
	g_tCameraDir = tCameraDir[2];
	g_tLightDir = tLightDir[2];
	gl_Position = gl_in[2].gl_Position; EmitVertex();

    EndPrimitive();
}
