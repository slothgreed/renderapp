#version 400
layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform mat3 uNormalMatrix;
in vec3 tePatchDistance[3];

out vec3 gFacetNormal;
out vec3 gPatchDistance;
out vec3 gTriDistance;
out vec4 g_position;
void main()
{
    vec3 A = (gl_in[1].gl_Position - gl_in[0].gl_Position).xyz;
    vec3 B = (gl_in[2].gl_Position - gl_in[0].gl_Position).xyz;
    
    gFacetNormal = uNormalMatrix * normalize(cross(A,B));
    
    gPatchDistance = tePatchDistance[0];
    gTriDistance = vec3(1, 0, 0);
	g_position = gl_in[0].gl_Position;
    gl_Position = gl_in[0].gl_Position; EmitVertex();

    gPatchDistance = tePatchDistance[1];
    gTriDistance = vec3(0, 1, 0);
	g_position = gl_in[1].gl_Position;
	gl_Position = gl_in[1].gl_Position; EmitVertex();

    gPatchDistance = tePatchDistance[2];
    gTriDistance = vec3(0, 0, 1);
	g_position = gl_in[2].gl_Position;
	gl_Position = gl_in[2].gl_Position; EmitVertex();

    EndPrimitive();
}
