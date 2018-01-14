#version 400
in vec4 v_position;		
in vec2 v_texcoord;
uniform int uHorizon;
uniform int uScale;
uniform sampler2D uTarget;
out vec4 OutputColor;
uniform float uWeight[5];
void main(void)
{	
	vec4 sum = vec4(0,0,0,1);
	double offset = 1.0 / uScale;
	sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y)) * uWeight[0];
	
	if(uHorizon == 1)
	{
		sum += texture2D(uTarget,vec2(v_texcoord.x + 1*offset,v_texcoord.y)) * uWeight[1];
		sum += texture2D(uTarget,vec2(v_texcoord.x - 1*offset,v_texcoord.y)) * uWeight[1];
		sum += texture2D(uTarget,vec2(v_texcoord.x + 2*offset,v_texcoord.y)) * uWeight[2];
		sum += texture2D(uTarget,vec2(v_texcoord.x - 2*offset,v_texcoord.y)) * uWeight[2];
		sum += texture2D(uTarget,vec2(v_texcoord.x + 3*offset,v_texcoord.y)) * uWeight[3];
		sum += texture2D(uTarget,vec2(v_texcoord.x - 3*offset,v_texcoord.y)) * uWeight[3];
		sum += texture2D(uTarget,vec2(v_texcoord.x + 4*offset,v_texcoord.y)) * uWeight[4];
		sum += texture2D(uTarget,vec2(v_texcoord.x - 4*offset,v_texcoord.y)) * uWeight[4];
		
		//sum += texture2D(uTarget,vec2(v_texcoord.x + 5*offset,v_texcoord.y)) * uWeight[5];
		//sum += texture2D(uTarget,vec2(v_texcoord.x - 5*offset,v_texcoord.y)) * uWeight[5];
		//sum += texture2D(uTarget,vec2(v_texcoord.x + 6*offset,v_texcoord.y)) * uWeight[6];
		//sum += texture2D(uTarget,vec2(v_texcoord.x - 6*offset,v_texcoord.y)) * uWeight[6];
		//sum += texture2D(uTarget,vec2(v_texcoord.x + 7*offset,v_texcoord.y)) * uWeight[7];
		//sum += texture2D(uTarget,vec2(v_texcoord.x - 7*offset,v_texcoord.y)) * uWeight[7];
		//sum += texture2D(uTarget,vec2(v_texcoord.x + 8*offset,v_texcoord.y)) * uWeight[8];
		//sum += texture2D(uTarget,vec2(v_texcoord.x - 8*offset,v_texcoord.y)) * uWeight[8];
		//sum += texture2D(uTarget,vec2(v_texcoord.x + 9*offset,v_texcoord.y)) * uWeight[9];
		//sum += texture2D(uTarget,vec2(v_texcoord.x - 9*offset,v_texcoord.y)) * uWeight[9];
		
	}else{
		
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 1*offset)) * uWeight[1];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 1*offset)) * uWeight[1];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 2*offset)) * uWeight[2];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 2*offset)) * uWeight[2];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 3*offset)) * uWeight[3];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 3*offset)) * uWeight[3];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 4*offset)) * uWeight[4];
		sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 4*offset)) * uWeight[4];
		
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 5*offset)) * uWeight[5];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 5*offset)) * uWeight[5];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 6*offset)) * uWeight[6];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 6*offset)) * uWeight[6];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 7*offset)) * uWeight[7];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 7*offset)) * uWeight[7];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 8*offset)) * uWeight[8];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 8*offset)) * uWeight[8];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y + 9*offset)) * uWeight[9];
		//sum += texture2D(uTarget,vec2(v_texcoord.x,v_texcoord.y - 9*offset)) * uWeight[9];
	}

	OutputColor = vec4(vec3(sum.xyz),1);
	//OutputColor = vec4(sum.x*0.5,sum.y*0.5,sum.z*0.5,1);
	
}

