
#version 400
in vec4 v_position;			//’¸“_

void main(void)
{
	float depth = (v_position.z / v_position.w + 1.0) * 0.5;
    gl_FragColor = vec4(vec3(depth), 1.0);
}
