#version 400
in vec4 v_position;		
in vec2 v_texcoord;
uniform int horizon;
uniform int scale;
uniform sampler2D render_2D;
layout (location = 4) out vec4 FragColor;
uniform float Weight[5];
void main(void)
{	
	vec4 sum = vec4(0,0,0,1);
	double offset = 1.0 / scale;
	sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y)) * Weight[0];
	if(horizon == 1)
	{
		sum += texture2D(render_2D,vec2(v_texcoord.x + 1*offset,v_texcoord.y)) * Weight[1];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 1*offset,v_texcoord.y)) * Weight[1];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 2*offset,v_texcoord.y)) * Weight[2];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 2*offset,v_texcoord.y)) * Weight[2];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 3*offset,v_texcoord.y)) * Weight[3];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 3*offset,v_texcoord.y)) * Weight[3];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 4*offset,v_texcoord.y)) * Weight[4];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 4*offset,v_texcoord.y)) * Weight[4];
		/*
		sum += texture2D(render_2D,vec2(v_texcoord.x + 5*offset,v_texcoord.y)) * Weight[5];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 5*offset,v_texcoord.y)) * Weight[5];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 6*offset,v_texcoord.y)) * Weight[6];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 6*offset,v_texcoord.y)) * Weight[6];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 7*offset,v_texcoord.y)) * Weight[7];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 7*offset,v_texcoord.y)) * Weight[7];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 8*offset,v_texcoord.y)) * Weight[8];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 8*offset,v_texcoord.y)) * Weight[8];
		sum += texture2D(render_2D,vec2(v_texcoord.x + 9*offset,v_texcoord.y)) * Weight[9];
		sum += texture2D(render_2D,vec2(v_texcoord.x - 9*offset,v_texcoord.y)) * Weight[9];
		*/
	}else{
		
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 1*offset)) * Weight[1];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 1*offset)) * Weight[1];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 2*offset)) * Weight[2];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 2*offset)) * Weight[2];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 3*offset)) * Weight[3];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 3*offset)) * Weight[3];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 4*offset)) * Weight[4];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 4*offset)) * Weight[4];
		/*
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 5*offset)) * Weight[5];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 5*offset)) * Weight[5];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 6*offset)) * Weight[6];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 6*offset)) * Weight[6];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 7*offset)) * Weight[7];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 7*offset)) * Weight[7];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 8*offset)) * Weight[8];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 8*offset)) * Weight[8];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y + 9*offset)) * Weight[9];
		sum += texture2D(render_2D,vec2(v_texcoord.x,v_texcoord.y - 9*offset)) * Weight[9];
		*/
	}
	gl_FragColor = vec4(vec3(sum.xyz),1);
	//gl_FragColor = vec4(sum.x*0.5,sum.y*0.5,sum.z*0.5,1);
	
}

