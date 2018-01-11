#version 400
in vec4 v_position;			
in vec2 v_texcoord;
uniform sampler2D uTarget;
uniform sampler2D uSelectMap;
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
	vec4 pixelColor = texture2D(uTarget,v_texcoord.xy);
	//pixelColor += texture2D(uSelectMap,v_texcoord.xy);
	OutputColor = pixelColor;
}

