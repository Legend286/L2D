in vec2 uv;
uniform sampler2D texture0;

out vec4 outputColor;

void main()
{
    outputColor = vec4(texture(texture0,uv.xy).rgba);
    if ( outputColor.a < 1 ){ discard; }
}