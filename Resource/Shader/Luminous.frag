#version 400
in vec4 v_position;			//’¸“_

uniform vec3 LightPos;				//ŒõŒ¹‚ÌˆÊ’u
uniform vec3 LightColor;
uniform mat4 UnProject;
uniform sampler2D posit_2D;
uniform sampler2D normal_2D;
uniform int ScreenW;
uniform int ScreenH;
layout (location = 4) out vec4 FragColor;
out vec4 OutputColor;

void main(void)
{	
	vec2 coord = (gl_FragCoord).xy;
	coord.x = coord.x / ScreenW;
	coord.y = coord.y / ScreenH;

	vec4 normal = texture2D(normal_2D,vec2(coord.x,coord.y));
	normal = normal * 2 - 1;
	
	vec4 depth  = texture2D(posit_2D ,vec2(coord.x,coord.y));
	depth = depth * 2 - 1;
	depth.w = 1;
	vec4 world = (UnProject * depth);
	world /= world.w;
	
	vec3 lightDir = LightPos - world.xyz;
	float diffuse = max(dot(normal.xyz,normalize(lightDir)),0.0);

	vec4 pixelColor = vec4(LightColor * diffuse,1.0);

	OutputColor = pixelColor;
	//OutputColor = vec4(LightColor,1);

}
