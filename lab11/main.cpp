#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include <vector>

GLuint Program;
GLint Attrib_vertex, Attrib_color, Uniform_color;
GLuint VBO;
int drawMode = 0; // 0 - четырехугольник, 1 - веер, 2 - пятиугольник
bool useGradient = false; // Управление градиентом

struct Vertex {
    GLfloat x, y;
    GLfloat r, g, b; // Цвет вершины
};

// Шейдеры
const char* VertexShaderSource = R"(
#version 330 core
in vec2 coord;
in vec3 color;
out vec3 vertexColor; // Передача цвета
void main() {
    gl_Position = vec4(coord, 0.0, 1.0);
    vertexColor = color;
}
)";

const char* FragShaderSource = R"(
#version 330 core
in vec3 vertexColor;
out vec4 color;
void main() {
    color = vec4(vertexColor, 1.0);
}
)";

const char* FragShaderSourceFlat = R"(
#version 330 core
uniform vec4 uColor;
out vec4 color;
void main() {
    color = uColor;
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
    glewInit();
    Init();

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) window.close();
            else if (event.type == sf::Event::KeyPressed) {
                if (event.key.code == sf::Keyboard::Num1) drawMode = 0; 
                if (event.key.code == sf::Keyboard::Num2) drawMode = 1; 
                if (event.key.code == sf::Keyboard::Num3) drawMode = 2; 
                if (event.key.code == sf::Keyboard::G) useGradient = !useGradient; 
                UpdateBuffer();
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
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    ShaderLog(vShader);

    GLuint fShader;
    /*if (useGradient) {

        fShader = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fShader, 1, &FragShaderSource, NULL);
    }
    else {

        fShader = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fShader, 1, &FragShaderSourceFlat, NULL);
    }*/
    glShaderSource(fShader, 1, &FragShaderSource, NULL);
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
        vertices = {
            {  0.0f,  0.0f, 1.0f, 0.27f, 0.21f },
            { -0.5f, -0.5f, 1.0f, 0.27f, 0.21f },
            {  0.5f, -0.5f, 0.25f, 0.32f, 0.71f },
            {  0.5f,  0.5f, 1.0f, 0.27f, 0.21f },
            { -0.5f,  0.5f, 0.25f, 0.32f, 0.71f },
            { -0.5f, -0.5f, 0.25f, 0.32f, 0.71f } 
        };
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

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Draw() {
    glUseProgram(Program);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glEnableVertexAttribArray(Attrib_vertex);
    glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, x));

    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, r));

    glDrawArrays(GL_TRIANGLES, 0, 15);

    glDisableVertexAttribArray(Attrib_color);
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
