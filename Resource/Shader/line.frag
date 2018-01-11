#version 400
in vec3 v_color;			
out vec4 OutputColor;
void main(void)
{
    OutputColor = vec4(v_color, 1.0);
}
