#version 400

smooth in vec3 fragViewPos;
flat in vec3 g_position;
flat in vec3 g_otherPosition;


uniform sampler2D texture_2D;

const float BigPFloat = 9999.0;
const float BigMFloat =-9999.0;
const float radius = 5.0;
vec2 LineLineIntersect(vec3 p1,vec3 p2,vec3 p3,vec3 p4)
{
	vec3 p13,p43,p21;
	float d1343,d4321,d1321,d4343,d2121;
	float numer,denom;
	float mua;

	p13 = p1-p3;
	p43 = p4-p3;
	p21 = p2-p1;

	d1343 = dot(p13,p43);
	d4321 = dot(p43,p21);
	d1321 = dot(p13,p21);
	d4343 = dot(p43,p43);
	d2121 = dot(p21,p21);

	denom = d2121 * d4343 - d4321 * d4321;
	//if (abs(denom) < 0.0000001)//almost impossible: view ray perfectly aligned with line direction (and it would only affect one pixel)
	//	return vec2(0.0,0.0);
	numer = d1343 * d4321 - d1321 * d4343;

	mua = numer/denom;
	mua = clamp(mua,0.0,1.0);
	return vec2(mua, (d1343 + d4321*mua)/d4343); //return (mua,mub)
}

void main()
{
	vec3 P1 = g_position;
	vec3 P2 = g_otherPosition;
	vec3 P3 = fragViewPos;
	vec3 P4 = fragViewPos+fragViewPos;
	//compute the two closest points on the volumetric line and current view ray
	vec2 muab = LineLineIntersect(P1,P2,P3,P4);

	//pa and pb, the two closest points
	vec3 pa = P1 + muab.x*(P2-P1);
	vec3 pb = P3 + muab.y*(P4-P3);

	//texture sample coordinate
	float linePos = length(pa-pb)/radius;
	//final color
	vec4 lineColor = texture(texture_2D,vec2(linePos,0.5));
	if(lineColor.x < 0.6)
	{
		discard;
	}
	gl_FragColor = lineColor;
	
}
