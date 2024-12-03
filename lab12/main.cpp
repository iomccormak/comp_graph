﻿#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include <vector>

GLuint Program;
GLint Attrib_vertex, Attrib_color;
GLuint VBO, VAO, EBO;

float offsetX = 0.0f, offsetY = 0.0f, offsetZ = 0.0f;
float angleX = 0.0f, angleY = 0.0f, angleZ = 0.0f;
const float offsetStep = 0.1f;
const float angleStep = 5.0f;
const float M_PI = 2 * acos(0.0);
float scaleX = 1.0f, scaleY = 1.0f;
const float scaleStep = 0.1f;

struct Vertex {
    GLfloat x, y, z;
    GLfloat r, g, b;
};


const char* VertexShaderSource = R"(
#version 330 core
in vec3 coord;
in vec3 color;
out vec3 vertexColor;
uniform mat4 translationMatrix;
uniform mat4 rotationMatrix;
uniform mat4 scaleMatrix;
void main() {
    gl_Position = translationMatrix * rotationMatrix * scaleMatrix * vec4(coord, 1.0);
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

void Init();
void InitShader();
void InitTetrahedron();
void InitGradientCircle();
void Draw();
void Release();
void ShaderLog(GLuint shader);
void createTranslationMatrix(float offsetX, float offsetY, float offsetZ, float* matrix);
void createScaleMatrix(float scaleX, float scaleY, float* matrix);
void HSVtoRGB(float h, float s, float v, float& r, float& g, float& b);
void rotateY(float angle, float* matrix);
void rotateX(float angle, float* matrix);
void rotateZ(float angle, float* matrix);
void multiplyMatrices(const float* a, const float* b, float* result);


int main() {
    sf::Window window(sf::VideoMode(800, 600), "Lab12", sf::Style::Default, sf::ContextSettings(24));
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
            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case sf::Keyboard::Num1:
                    InitTetrahedron();
                    break;
                case sf::Keyboard::Num4:
                    InitGradientCircle();
                    break;
                case sf::Keyboard::W: offsetY += offsetStep; break;
                case sf::Keyboard::S: offsetY -= offsetStep; break;
                case sf::Keyboard::A: offsetX -= offsetStep; break;
                case sf::Keyboard::D: offsetX += offsetStep; break;
                case sf::Keyboard::Q: offsetZ += offsetStep; break;
                case sf::Keyboard::E: offsetZ -= offsetStep; break;
                case sf::Keyboard::Up: angleY -= angleStep; break;
                case sf::Keyboard::Down: angleY += angleStep; break;
                case sf::Keyboard::Left: angleX -= angleStep; break;
                case sf::Keyboard::Right: angleX += angleStep; break;
                case sf::Keyboard::Numpad2: angleZ -= angleStep; break;
                case sf::Keyboard::Numpad8: angleZ += angleStep; break;
                case sf::Keyboard::I: scaleY += scaleStep; break;
                case sf::Keyboard::K: scaleY -= scaleStep; break;
                case sf::Keyboard::J: scaleX -= scaleStep; break;
                case sf::Keyboard::L: scaleX += scaleStep; break;
                default: break;
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();
        window.display();
    }

    Release();
    return 0;
}


void Init() {
    glEnable(GL_DEPTH_TEST);
    InitShader();
    InitTetrahedron();
}

void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
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
}

void InitTetrahedron() {
    std::vector<Vertex> vertices = {
        {  0.0f,   0.5f,  0.0f,  0.0f,  0.0f,  1.0f }, // Верхняя вершина
        { -0.5f,  -0.5f,  0.0f,  1.0f,  0.0f,  0.0f }, // Левая нижняя вершина 
        {  0.5f,  -0.5f,  0.0f,  0.0f,  1.0f,  0.0f }, // Правая нижняя вершина
        {  0.0f,  -0.5f,  -0.5f, 1.0f,  1.0f,  1.0f }  // Задняя нижняя вершина 
    };

    GLuint indices[] = {
        0, 1, 2,
        0, 2, 3,
        0, 3, 1,
        1, 2, 3
    };

    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);
    glGenVertexArrays(1, &VAO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, x));
    glEnableVertexAttribArray(Attrib_vertex);

    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, r));
    glEnableVertexAttribArray(Attrib_color);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

void InitGradientCircle() {
    const int segments = 100;
    const float radius = 0.5f;
    const float PI = 3.14159265359f;

    std::vector<Vertex> vertices;
    vertices.push_back({ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f });

    for (int i = 0; i <= segments; ++i) {
        float angle = 2.0f * PI * i / segments;
        float x = radius * cos(angle);
        float y = radius * sin(angle);

        float hue = static_cast<float>(i) / segments;
        float r, g, b;
        HSVtoRGB(hue, 1.0f, 1.0f, r, g, b);

        vertices.push_back({ x, y, 0.0f, r, g, b });
    }

    std::vector<GLuint> indices;
    for (int i = 1; i < segments; ++i) {
        indices.push_back(0);
        indices.push_back(i);
        indices.push_back(i + 1);
    }
    indices.push_back(0);
    indices.push_back(segments);
    indices.push_back(1);

    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);
    glGenVertexArrays(1, &VAO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(GLuint), indices.data(), GL_STATIC_DRAW);

    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, x));
    glEnableVertexAttribArray(Attrib_vertex);

    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, r));
    glEnableVertexAttribArray(Attrib_color);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

void HSVtoRGB(float h, float s, float v, float& r, float& g, float& b) {
    float p, q, t, ff;
    int i;
    if (s <= 0.0f) {
        r = g = b = v;
        return;
    }
    h *= 6.0f;
    i = static_cast<int>(h);
    ff = h - i;
    p = v * (1.0f - s);
    q = v * (1.0f - (s * ff));
    t = v * (1.0f - (s * (1.0f - ff)));

    switch (i) {
    case 0: r = v; g = t; b = p; break;
    case 1: r = q; g = v; b = p; break;
    case 2: r = p; g = v; b = t; break;
    case 3: r = p; g = q; b = v; break;
    case 4: r = t; g = p; b = v; break;
    default: r = v; g = p; b = q; break;
    }
}

void Draw() {
    glUseProgram(Program);
    glBindVertexArray(VAO);

    float translationMatrix[16];
    createTranslationMatrix(offsetX, offsetY, offsetZ, translationMatrix);
    GLint translationMatrixLocation = glGetUniformLocation(Program, "translationMatrix");
    glUniformMatrix4fv(translationMatrixLocation, 1, GL_FALSE, translationMatrix);

    float scaleMatrix[16];
    createScaleMatrix(scaleX, scaleY, scaleMatrix);
    GLint scaleMatrixLocation = glGetUniformLocation(Program, "scaleMatrix");
    glUniformMatrix4fv(scaleMatrixLocation, 1, GL_FALSE, scaleMatrix);

    float rotationX[16], rotationY[16], rotationZ[16], combinedRotation[16], tempMatrix[16];
    rotateX(angleX * M_PI / 180.0f, rotationX);
    rotateY(angleY * M_PI / 180.0f, rotationY);
    rotateZ(angleZ * M_PI / 180.0f, rotationZ);
    multiplyMatrices(rotationY, rotationX, tempMatrix); // Y * X
    multiplyMatrices(rotationZ, tempMatrix, combinedRotation); // Z * (Y * X)
    GLint rotMatrixLoc = glGetUniformLocation(Program, "rotationMatrix");
    glUniformMatrix4fv(rotMatrixLoc, 1, GL_TRUE, combinedRotation);

    glDrawElements(GL_TRIANGLES, 300, GL_UNSIGNED_INT, 0);

    glBindVertexArray(0);
    glUseProgram(0);
}

void Release() {
    glDeleteBuffers(1, &VBO);
    glDeleteBuffers(1, &EBO);
    glDeleteVertexArrays(1, &VAO);
    glDeleteProgram(Program);
}

void ShaderLog(GLuint shader) {
    int success;
    char infoLog[512];
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        glGetShaderInfoLog(shader, 512, NULL, infoLog);
        std::cerr << "Shader compilation error:\n" << infoLog << std::endl;
    }
}

// ========= Matrices =========

void createTranslationMatrix(float offsetX, float offsetY, float offsetZ, float* matrix) {
    float temp[16] = {
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        offsetX, offsetY, offsetZ, 1.0f
    };
    std::copy(temp, temp + 16, matrix);
}

void createScaleMatrix(float scaleX, float scaleY, float* matrix) {
    float scaleMatrix[16] = {
        scaleX, 0.0f,   0.0f, 0.0f,
        0.0f,   scaleY, 0.0f, 0.0f,
        0.0f,   0.0f,   1.0f, 0.0f,
        0.0f,   0.0f,   0.0f, 1.0f
    };

    for (int i = 0; i < 16; ++i)
        matrix[i] = scaleMatrix[i];
}

void rotateX(float angle, float* matrix) {
    float cosA = cos(angle), sinA = sin(angle);
    float temp[16] = {
        cosA, 0, -sinA, 0,
        0,    1, 0,     0,
        sinA, 0, cosA,  0,
        0,    0, 0,     1
    };
    std::copy(temp, temp + 16, matrix);
}

void rotateY(float angle, float* matrix) {
    float cosA = cos(angle), sinA = sin(angle);
    float temp[16] = {
        1, 0,     0,     0,
        0, cosA,  sinA,  0,
        0, -sinA, cosA,  0,
        0, 0,     0,     1
    };
    std::copy(temp, temp + 16, matrix);
}

void rotateZ(float angle, float* matrix) {
    float cosA = cos(angle), sinA = sin(angle);
    float temp[16] = {
        cosA,  sinA,  0, 0,
        -sinA, cosA,  0, 0,
        0,     0,     1, 0,
        0,     0,     0, 1
    };
    std::copy(temp, temp + 16, matrix);
}

void multiplyMatrices(const float* a, const float* b, float* result) {
    for (int i = 0; i < 4; ++i) {
        for (int j = 0; j < 4; ++j) {
            result[i * 4 + j] = 0;
            for (int k = 0; k < 4; ++k)
                result[i * 4 + j] += a[i * 4 + k] * b[k * 4 + j];
        }
    }
}