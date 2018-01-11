#version 400

in vec2 v_texcoord;			//テクスチャ

uniform sampler2D uAlbedoMap;
out vec4 OutputColor1;
out vec4 OutputColor2;
out vec4 OutputColor3;
out vec4 OutputColor4;

void main(void)
{	
	//gl_FragColor = texture2D(uAlbedoMap,v_texcoord);
	OutputColor0 = pos;
	OutputColor1 = vec4((normalize(v_normal) + 1.0)*0.5,1.0);
	OutputColor2 = texture2D(uAlbedoMap,v_texcoord);
	OutputColor3 = vec4(GetADS(v_normal),1);
}
