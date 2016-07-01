#version 400
in vec4 v_position;			//���_
in vec2 v_texcoord;
uniform sampler2D render_2D;
uniform int width;
uniform int height;
uniform float threshold;
layout (location = 4) out vec4 FragColor;

float luma(vec4 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

void main(void)
{	
	float dw = 1.0 / width;
	float dh = 1.0 / height;
	vec4 pixelColor = texture2D(render_2D,v_texcoord.xy);
	
	float color00 = luma(texture2D(render_2D,vec2(v_texcoord.x - dw,v_texcoord.y - dh)));
	float color01 = luma(texture2D(render_2D,vec2(v_texcoord.x - dw,v_texcoord.y)));
	float color02 = luma(texture2D(render_2D,vec2(v_texcoord.x - dw,v_texcoord.y + dh)));

	float color10 = luma(texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - dh)));
	float color11 = luma(texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y)));
	float color12 = luma(texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + dh)));
	
	float color20 = luma(texture2D(render_2D,vec2(v_texcoord.x + dw,v_texcoord.y - dh)));
	float color21 = luma(texture2D(render_2D,vec2(v_texcoord.x + dw,v_texcoord.y)));
	float color22 = luma(texture2D(render_2D,vec2(v_texcoord.x + dw,v_texcoord.y + dh)));


	float sx = -color00 +color02 -(2*color10) +(2*color12) -color20 +color22;
	float sy = -color00 -(2*color01) -color02 +color20 +(2*color21) +color22; 
	
	float dist = sx*sx + sy*sy;
	if(dist < threshold)
	{
		pixelColor = vec4(0,0,0,1);
	}else{
		pixelColor = vec4(1);
	}

	gl_FragColor = pixelColor;
}

