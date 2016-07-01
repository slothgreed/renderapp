#version 400
in vec4 v_position;			//’¸“_
in vec2 v_texcoord;
uniform sampler2D render_2D;
layout (location = 4) out vec4 FragColor;

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
	vec4 pixelColor = texture2D(render_2D,v_texcoord.xy);
	gl_FragColor = pixelColor;
	//gl_FragColor = vec4(1,0,0,1);
}

