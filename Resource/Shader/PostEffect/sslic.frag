#version 400
in vec4 v_position;			//’¸“_
in vec2 v_texcoord;
uniform sampler2D uFrame;
uniform sampler2D uVector;
uniform sampler2D uNoize;
uniform vec3 uScale;
out vec4 OutputColor;
void main(void)
{	
	vec4 vector = texture2D(uVector,v_texcoord.xy);
	vec4 frame = texture2D(uFrame,vector.xy);
	vec4 noize = texture2D(uNoize,v_texcoord.xy);
	//OutputColor = vector - vec4(uScale,1.0);
	//OutputColor = frame + noize ;//vec4(vector.xy,0,1);
	OutputColor = frame * frame.w + noize * (1.0 - frame.w);
}

