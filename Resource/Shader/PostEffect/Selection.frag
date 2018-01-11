#version 400

#define MAXSAMPLE 1200
in vec2 v_texcoord;
uniform sampler2D uAlbedoMap;
uniform int uID;
uniform int uWidth;
uniform int uHeight; 
out vec4 OutputColor;
void main(void)
{
	float dw = 1.0 / uWidth;
	float dh = 1.0 / uHeight;
	int select = 1;
	/*���͂ɑI���������̂������select��0�ɂȂ�B����ȊO��0�ȊO*/

	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x - dw,v_texcoord.y)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x + dw,v_texcoord.y)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x - dw,v_texcoord.y + dh)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x - dw,v_texcoord.y - dh)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x + dw,v_texcoord.y + dh)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x + dw,v_texcoord.y - dh)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x,v_texcoord.y + dh)).a);
	select *= uID - int(255 * texture2D(uAlbedoMap,vec2(v_texcoord.x,v_texcoord.y - dh)).a);
	
	int current = uID - int(255 * texture2D(uAlbedoMap,v_texcoord).a);
	
	if(select == 0 && current != 0)
	{
		//�I��
		OutputColor = vec4(1,0,0,1);
	}else{
		//��I��
		OutputColor = vec4(0,0,0,1);
	}
}