#version 400
in vec4 v_position;			//’¸“_
in vec2 v_texcoord;
uniform sampler2D uVector;
uniform sampler2D uNoize;
out vec4 OutputColor;
void main(void)
{	
	vec4 vector = texture2D(uVector,v_texcoord.xy);
	vec4 noize = texture2D(uNoize,v_texcoord.xy);
	OutputColor = vector * noize;
}

