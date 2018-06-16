#version 400
in vec4 v_position;
in vec3 v_normal;
in vec3 v_color;
in vec3 v_direction;
out vec4 OutputColor0;
out vec4 OutputColor1;
out vec4 OutputColor2;
out vec4 OutputColor3;
void main(void)
{	
	vec4 pos = v_position / v_position.w;
	OutputColor0 = (pos + 1)* 0.5;
	OutputColor1 = vec4((normalize(v_normal) + 1.0)*0.5,1);
	OutputColor2 = vec4(v_color,1);
	OutputColor3 = vec4((normalize(v_direction) + 1.0)*0.5,1);
}
