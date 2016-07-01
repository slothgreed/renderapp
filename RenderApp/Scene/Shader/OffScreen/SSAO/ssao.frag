#version 400

#define MAXSAMPLE 1200
in vec2 v_texcoord;
uniform sampler2D posit_2D;
uniform float[MAXSAMPLE] Sample; 
uniform sampler2D main_2D;

void main(void)
{
	vec4 posit = texture2D(posit_2D,v_texcoord);
	float count = 0;
	vec4 sphere;
	for(int i = 0; i < MAXSAMPLE; i+=3)
	{
		
		sphere = vec4(Sample[i],Sample[i + 1],Sample[i + 2],1);
		sphere += posit;
		if(posit.z < texture2D(posit_2D,sphere.xy).z)
		{
			count++;
		}
	}
	gl_FragColor = texture2D(main_2D,v_texcoord) * vec4(vec3(float(count) * 2/ float(MAXSAMPLE/3)),1.0);
}