#version 400

in vec3 g_tCameraDir;
in vec3 g_tLightDir;
in vec3 gFacetNormal;
in vec3 gTriDistance;
in vec3 gPatchDistance;
in float gPrimitive;

in vec3 g_normal;
in vec2 g_texcoord;
uniform vec3 LightPos;
uniform vec3 CameraPos;
uniform sampler2D t_2D;
uniform sampler2D t_Normal;
uniform sampler2D t_Height;
out vec4 OutputColor;

float amplify(float d, float scale, float offset)
{
    d = scale * d + offset;
    d = clamp(d, 0, 1);
    d = 1 - exp2(-2*d*d);
    return d;
}

void main()
{
    vec3 N = normalize(vec3(0,0,1));
    vec3 L = vec3(0,0,1);
    float df = max(dot(N, L),0.0);
    vec3 color = vec3(df);

    float d1 = min(min(gTriDistance.x, gTriDistance.y), gTriDistance.z);
    float d2 = min(min(gPatchDistance.x, gPatchDistance.y), gPatchDistance.z);
    color = amplify(d1, 40, -0.5) * amplify(d2, 60, -0.5) * color;

	float height = texture2D(t_Height,g_texcoord).x * 10;
	vec2 h_texcoord = g_texcoord - height * CameraPos.xy;


	vec3 t_2DColor = texture2D(t_Height,g_texcoord).xyz;
	vec3 t_NormalColor = (texture2D(t_Normal,h_texcoord) * 2.0 - 1.0).xyz;
	vec3 halfEye = normalize(g_tLightDir + g_tCameraDir);



	
	float diffuse  = clamp(dot(t_NormalColor,g_tLightDir),0.1,1.0);
	float specular = pow(clamp(dot(t_NormalColor, halfEye), 0.0, 1.0), 1.0);
	color = vec3((t_2DColor * diffuse ));
	
	OutputColor = vec4(color, 1.0);
	
}
