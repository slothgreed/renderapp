#version 400

in vec4 v_color;
out vec4 OutputColor;

vec4 degamma(vec4 color)
{
	vec4 gamma;
	gamma.r = pow(color.r,1/2.2);
	gamma.g = pow(color.g,1/2.2);
	gamma.b = pow(color.b,1/2.2);
	gamma.a = 1;
	return gamma;
}


void main(void)
{	
	vec4 color = v_color;
	if(color.x < 0){ color.x = -color.x; }
	if(color.y < 0){ color.y = -color.y; }
	if(color.z < 0){ color.z = -color.z; }
	if(color.w < 0){ color.w = -color.w; }
	OutputColor = color;
	//OutputColor = degamma(v_color);
}
