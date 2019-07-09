#version 400
in vec4 v_position;			//’¸“_
in vec2 v_texcoord;
uniform sampler2D uTarget;
out vec4 OutputColor;

float luma(vec4 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

void main(void)
{	
	vec4 pixelColor = texture2D(uTarget,v_texcoord.xy);
	
	float glay = luma(pixelColor);
	OutputColor = vec4(glay,glay,glay,1);
}

