#version 400
layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;
uniform mat3 NormalMatrix;

in vec3 te_position[3];

out vec4 v_color;
void main()
{
    vec3 A = (te_position[1] - te_position[0]).xyz;
    vec3 B = (te_position[2] - te_position[0]).xyz;
    
    v_color.xyz = vec3(normalize(cross(A,B)));
    v_color.w = 1;
    gl_Position = gl_in[0].gl_Position; EmitVertex();
	gl_Position = gl_in[1].gl_Position; EmitVertex();
	gl_Position = gl_in[2].gl_Position; EmitVertex();

    EndPrimitive();
}
