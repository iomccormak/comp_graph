#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include <vector>

// Переменные
GLuint Program;
GLint Attrib_vertex, Attrib_color;
GLuint VBO, VAO, EBO;

// Глобальные переменные для смещения
float offsetX = 0.0f, offsetY = 0.0f, offsetZ = 0.0f;
const float step = 0.1f; // Шаг смещения

struct Vertex {
    GLfloat x, y, z;
    GLfloat r, g, b; // Цвет вершины
};

// Шейдеры
const char* VertexShaderSource = R"(
#version 330 core
in vec3 coord;
in vec3 color;
out vec3 vertexColor; // Передача цвета
uniform mat4 translationMatrix;
void main() {
    gl_Position = translationMatrix * vec4(coord, 1.0);
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

// Функции
void Init();
void InitShader();
void InitBuffers();
void Draw();
void Release();
void ShaderLog(GLuint shader);
void createTranslationMatrix(float offsetX, float offsetY, float offsetZ, float* matrix);

int main() {
    sf::Window window(sf::VideoMode(800, 600), "Tetrahedron Movement", sf::Style::Default, sf::ContextSettings(24));
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
                case sf::Keyboard::W: offsetY += step; break; // Вверх
                case sf::Keyboard::S: offsetY -= step; break; // Вниз
                case sf::Keyboard::A: offsetX -= step; break; // Влево
                case sf::Keyboard::D: offsetX += step; break; // Вправо
                case sf::Keyboard::Q: offsetZ += step; break; // Вперёд
                case sf::Keyboard::E: offsetZ -= step; break; // Назад
                default: break;
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw(); // Вызов без аргументов
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

void InitBuffers() {
    const float PI = 3.14159265359;
    const float angleX = 0.0f * PI / 180.0f; // угол 30 градусов в радианах
    const float angleY = 0.0f * PI / 180.0f; // угол 30 градусов в радианах

    // Математика вращения

    std::vector<Vertex> vertices = {
        {  0.0f,  0.5f,  0.0f,  0.0f,  0.0f,  1.0f }, // Верхняя вершина
        { -0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f }, // Левая нижняя вершина 
        {  0.5f, -0.5f,  0.5f,  0.0f,  1.0f,  0.0f }, // Правая нижняя вершина
        {  0.0f, 0.0f,  -0.5f,  1.0f,  1.0f,  1.0f }  // Задняя нижняя вершина 
    };

    // Применяем вращение по оси X и Y для каждой вершины

    for (auto& vertex : vertices) {
        // Вращение по оси X
        float tempY = vertex.y * cos(angleY) - vertex.z * sin(angleY);
        float tempZ = vertex.y * sin(angleY) + vertex.z * cos(angleY);
        vertex.y = tempY;
        vertex.z = tempZ;

        // Вращение по оси Y
        float tempX = vertex.x * cos(angleX) + vertex.z * sin(angleX);
        tempZ = -vertex.x * sin(angleX) + vertex.z * cos(angleX);
        vertex.x = tempX;
        vertex.z = tempZ;
    }


    GLuint indices[] = {
        0, 1, 2, // Треугольник 1
        0, 2, 3, // Треугольник 2
        0, 3, 1, // Треугольник 3
        1, 2, 3  // Треугольник 4
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

void createTranslationMatrix(float offsetX, float offsetY, float offsetZ, float* matrix) {
    float translationMatrix[16] = {
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        offsetX, offsetY, offsetZ, 1.0f
    };

    for (int i = 0; i < 16; ++i)
        matrix[i] = translationMatrix[i];
}

void Draw() {
    glUseProgram(Program);
    glBindVertexArray(VAO);

    // Создать матрицу трансляции
    float translationMatrix[16];
    createTranslationMatrix(offsetX, offsetY, offsetZ, translationMatrix);

    // Передать матрицу трансляции в шейдер
    GLint translationMatrixLocation = glGetUniformLocation(Program, "translationMatrix");
    glUniformMatrix4fv(translationMatrixLocation, 1, GL_FALSE, translationMatrix);

    // Отрисовка
    glDrawElements(GL_TRIANGLES, 12, GL_UNSIGNED_INT, 0);

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
