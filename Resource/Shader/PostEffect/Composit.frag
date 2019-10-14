#version 400
in vec4 v_position;			
in vec2 v_texcoord;
uniform sampler2D uSource;
uniform sampler2D uTarget;
out vec4 OutputColor;


void main(void)
{	
	vec4 pixelColor = texture2D(uSource,v_texcoord.xy);

#define bADD
	pixelColor += texture2D(uTarget, v_texcoord.xy);
#endif 

#define bMultiply
	pixelColor *= texture2D(uTarget, v_texcoord.xy);
#endif

#define bOverwirte
	if(uTarget.a != 0)
	{
		pixelColor = texture2D(uTarget, v_texcoord.xy);
	}
#endif
	OutputColor = pixelColor;
	
	vec4 foreGround = texture2D(uForeGround, v_texcoord.xy);
	if(foreGround.a !=0)
	{
		//OutputColor = foreGround;
	}
}

