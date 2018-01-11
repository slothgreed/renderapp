#version 400
in vec4 v_position;			
in vec3 v_color;			
in vec3 v_normal;
in vec4 m_position;
in vec2 v_texcoord;
uniform sampler2D uFrameBuffer;
out vec4 OutputColor0;
out vec4 OutputColor1;
out vec4 OutputColor2;
out vec4 OutputColor3;

void main(void)
{	
	vec4 pos = v_position / v_position.w;
	pos = (pos + 1)* 0.5;
	OutputColor0 = vec4(pos.x,pos.y,pos.z,1.0);
	OutputColor1 = vec4((normalize(v_color) + 1.0)*0.5,1.0);
	OutputColor2 = texture2D(uFrameBuffer,v_texcoord.xy);
	OutputColor3 = vec4(1);
}
