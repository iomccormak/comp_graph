#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include <vector>

GLuint Program;
GLint Attrib_vertex, Attrib_color;
GLuint VBO;

struct Vertex {
    GLfloat x, y, z;
    GLfloat r, g, b; // Цвет вершины
};

// Шейдеры
const char* GradientVertexShaderSource = R"(
#version 330 core
in vec3 coord;
in vec3 color;
out vec3 vertexColor; // Передача цвета
void main() {
    gl_Position = vec4(coord, 1.0);
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

void Init();
void InitShader();
void InitBuffers();
void Draw();
void Release();
void ShaderLog(GLuint shader);

int main() {
    sf::Window window(sf::VideoMode(800, 600), "Tetrahedron", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    Init();

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
                window.close();
            else if (event.type == sf::Event::Resized)
                glViewport(0, 0, event.size.width, event.size.height);
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();
        window.display();
    }

    Release();
    return 0;
}

void Init() {
    glEnable(GL_DEPTH_TEST); // Включить тест глубины
    InitShader();
    InitBuffers();
}

void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &GradientVertexShaderSource, NULL);
    glCompileShader(vShader);
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &GradientFragShaderSource, NULL);
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
}

void InitBuffers() {
    std::vector<Vertex> vertices = {
        {  0.0f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f }, // Верхняя вершина (красный)
        { -0.5f, -0.5f,  0.5f,  0.0f,  1.0f,  0.0f }, // Левая нижняя вершина (зеленый)
        {  0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f }, // Правая нижняя вершина (синий)
        {  0.0f, -0.5f, -0.5f,  1.0f,  1.0f,  0.0f }  // Задняя нижняя вершина (желтый)
    };

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Draw() {
    glUseProgram(Program);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glEnableVertexAttribArray(Attrib_vertex);
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, x));

    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, r));

    glDrawArrays(GL_TRIANGLES, 0, 12);

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);
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
