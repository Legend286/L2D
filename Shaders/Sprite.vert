layout(location = 0) in vec4 vertex; // <vec2 position, vec2 texCoords>

#ifdef INSTANCED
layout(location = 1) in mat4 model;
#else
uniform mat4 model;
#endif

out vec2 uv;
mat4 transform;
uniform mat4 viewprojection;

void main()
{
    uv = vertex.zw;

    transform = model;

    gl_Position = vec4(vertex.xy, 0.0f, 1.0) * transform * viewprojection;
}