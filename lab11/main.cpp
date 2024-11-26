#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include <vector>

GLuint Program;
GLint Attrib_vertex, Attrib_color, Uniform_color;
GLuint VBO;
int drawMode = 0; // 0 - четырехугольник, 1 - веер, 2 - пятиугольник, 3 - котлин???
int renderMode = 0; // 0 - Константный цвет, 1 - Цвет через uniform, 2 - Градиент
bool useGradient = false; // Управление градиентом

struct Vertex {
    GLfloat x, y;
    GLfloat r, g, b; // Цвет вершины
};

const float PI = 2 * acos(0.0);

// Шейдеры
const char* VertexShaderSource = R"(
 #version 330 core
 in vec2 coord;
 void main() {
    gl_Position = vec4(coord, 0.0, 1.0);
 }
)";

const char* GradientVertexShaderSource = R"(
#version 330 core
in vec2 coord;
in vec3 color;
out vec3 vertexColor; // Передача цвета
void main() {
    gl_Position = vec4(coord, 0.0, 1.0);
    vertexColor = color;
}
)";

const char* GradientFragShaderSource = R"(
#version 330 core
in vec3 vertexColor;
out vec4 color;
void main() {
    color = vec4(vertexColor, 1.0);
}
)";

const char* UniformFragShaderSource = R"(
#version 330 core
uniform vec4 uColor;
out vec4 color;
void main() {
    color = uColor;
}
)";

const char* FlatFragShaderSource = R"(
#version 330 core
out vec4 color;
void main() {
    color = vec4(0.0, 1.0, 1.0, 1.0);
}
)";

void Init();
void InitShader();
void InitBuffers();
void UpdateBuffer();
void Draw();
void Release();
void ShaderLog(GLuint shader);

int main() {
    sf::Window window(sf::VideoMode(800, 600), "Lab 11", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    Init();
    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
            {
                window.close();
            }
            else if (event.type == sf::Event::KeyPressed) {
                if (event.key.code == sf::Keyboard::Num1) drawMode = 0; 
                else if (event.key.code == sf::Keyboard::Num2) drawMode = 1; 
                else if (event.key.code == sf::Keyboard::Num3) drawMode = 2; 
                else if (event.key.code == sf::Keyboard::Num4) drawMode = 3;
                else if (event.key.code == sf::Keyboard::F) {
                    renderMode = 0; 
                    useGradient = false;
                    InitShader();
                }
                else if (event.key.code == sf::Keyboard::H) {
                    renderMode = 1;
                    useGradient = false;
                    InitShader();
                }
                else if (event.key.code == sf::Keyboard::G) {
                    renderMode = 2;
                    useGradient = true;
                    InitShader();
                }
                UpdateBuffer();
            }
            else if (event.type == sf::Event::Resized)
            {
                glViewport(0, 0, event.size.width, event.size.height);
            }
        }
        glClear(GL_COLOR_BUFFER_BIT);
        Draw();
        window.display();
    }
    Release();
    return 0;
}

void Init() {
    InitShader();
    InitBuffers();
    UpdateBuffer();
}

void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    const char* vertexSource = useGradient ? GradientVertexShaderSource : VertexShaderSource;
    glShaderSource(vShader, 1, &vertexSource, NULL);
    glCompileShader(vShader);
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    const char* fragSource;
    if (renderMode == 0) {
        fragSource = FlatFragShaderSource;
    }
    else if (renderMode == 1) {
        fragSource = UniformFragShaderSource;
    }
    else {
        fragSource = GradientFragShaderSource;
    }
    glShaderSource(fShader, 1, &fragSource, NULL);
    glCompileShader(fShader);
    ShaderLog(fShader);

    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    glLinkProgram(Program);

    glDeleteShader(vShader);
    glDeleteShader(fShader);

    Attrib_vertex = glGetAttribLocation(Program, "coord");
    Attrib_color = glGetAttribLocation(Program, "color");
    Uniform_color = glGetUniformLocation(Program, "uColor");
}

void InitBuffers() {
    glGenBuffers(1, &VBO);
}

static void createFanVertices(std::vector<Vertex>& vertices) {
    float angle = 30.0f * PI / 180.0f;
    float currentAngle = angle / 2.0f ;
    for (int i = 0; i < 5; ++i) {
        vertices.push_back({ 0.0f, -0.5f, 1.0f, 0.27f, 0.21f });
        vertices.push_back({ cos(currentAngle), sin(currentAngle) - 0.5f, 1.0f + 0.1f * i, 0.27f + 0.1f * i, 0.21f + 0.1f * i });
        vertices.push_back({ cos(currentAngle + angle), sin(currentAngle + angle) - 0.5f, 1.0f + 0.1f * (i + 1), 0.27f + 0.1f * (i + 1), 0.21f + 0.1f * (i + 1) });
        currentAngle += angle;
    }
}

void UpdateBuffer() {
    std::vector<Vertex> vertices;

    if (drawMode == 0) {
        vertices = {
            { -0.5f, -0.5f, 1.0f, 0.0f, 0.0f },
            {  0.5f, -0.5f, 0.0f, 1.0f, 0.0f },
            {  0.5f,  0.5f, 0.0f, 0.0f, 1.0f },
            {  0.5f,  0.5f, 0.0f, 0.0f, 1.0f }, 
            { -0.5f,  0.5f, 1.0f, 1.0f, 0.0f },
            { -0.5f, -0.5f, 1.0f, 0.0f, 0.0f }
        };
    }
    else if (drawMode == 1) {
        createFanVertices(vertices);
    }
    else if (drawMode == 2) {
        vertices = {
            {  0.0f,  0.0f, 1.0f, 1.0f, 1.0f }, 
            {  0.0f,  0.5f, 1.0f, 0.0f, 0.0f }, 
            {  0.47f,  0.15f, 0.0f, 1.0f, 0.0f },

            {  0.0f,  0.0f, 1.0f, 1.0f, 1.0f },
            {  0.47f,  0.15f, 0.0f, 1.0f, 0.0f },
            {  0.29f, -0.4f, 0.0f, 0.0f, 1.0f },

            {  0.0f,  0.0f, 1.0f, 1.0f, 1.0f }, 
            {  0.29f, -0.4f, 0.0f, 0.0f, 1.0f },
            { -0.29f, -0.4f, 1.0f, 1.0f, 0.0f },

            {  0.0f,  0.0f, 1.0f, 1.0f, 1.0f }, 
            { -0.29f, -0.4f, 1.0f, 1.0f, 0.0f },
            { -0.47f,  0.15f, 0.0f, 1.0f, 1.0f },

            {  0.0f,  0.0f, 1.0f, 1.0f, 1.0f }, 
            { -0.47f, 0.15f, 0.0f, 1.0f, 1.0f },
            {  0.0f,  0.5f, 1.0f, 0.0f, 0.0f }
        };
    }
    else if (drawMode == 3) {
        vertices = {
            {  0.0f,  0.0f, 1.0f, 0.27f, 0.21f },
            { -0.5f, -0.5f, 1.0f, 0.27f, 0.21f },
            {  0.5f, -0.5f, 0.25f, 0.32f, 0.71f },
            {  0.5f,  0.5f, 1.0f, 0.27f, 0.21f },
            { -0.5f,  0.5f, 0.25f, 0.32f, 0.71f },
            { -0.5f, -0.5f, 0.25f, 0.32f, 0.71f }
        };
    }

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Draw() {
    glUseProgram(Program);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glEnableVertexAttribArray(Attrib_vertex);
    glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, x));

    if (renderMode == 1) 
    {
        glUniform4f(Uniform_color, 0.0f, 1.0f, 0.0f, 1.0f);
    } 
    else if (renderMode == 2) 
    {
        glEnableVertexAttribArray(Attrib_color);
        glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, r));
    }

    glDrawArrays(GL_TRIANGLES, 0, 15);

    if (renderMode == 2) {
        glDisableVertexAttribArray(Attrib_color);
    }

    glDisableVertexAttribArray(Attrib_vertex);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Release() {
    glDeleteBuffers(1, &VBO);
    glDeleteProgram(Program);
}

void ShaderLog(GLuint shader) {
    GLint length;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &length);
    if (length > 0) {
        char* log = new char[length];
        glGetShaderInfoLog(shader, length, &length, log);
        std::cout << log << std::endl;
        delete[] log;
    }
}
