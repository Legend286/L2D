layout(location = 0) in vec4 vertex; // <vec2 position, vec2 texCoords>

out vec2 uv;

uniform mat4 model;
uniform mat4 viewprojection;

void main()
{
    uv = vertex.zw;
    gl_Position = vec4(vertex.xy, 0.0f, 1.0) * model * viewprojection;
}