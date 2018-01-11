#version 400

#define MAXSAMPLE 512
in vec2 v_texcoord;
uniform sampler2D uPosition;
uniform float uSample[512];
uniform sampler2D uTarget;
out vec4 OutputColor;
void main(void)
{
	vec4 posit = texture2D(uPosition,v_texcoord);
	float count = 0;
	vec4 sphere;
	for(int i = 0; i < 512; i+=3)
	{
		sphere = vec4(uSample[i],uSample[i + 1],uSample[i + 2],1);
		sphere += posit;
		if(posit.z < texture2D(uPosition,sphere.xy).z)
		{
			count++;
		}
	}
	OutputColor = texture2D(uTarget,v_texcoord) * vec4(vec3(float(count) * 2/ float(512/3)),1.0);
}