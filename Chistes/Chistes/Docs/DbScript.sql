-- Crear la base de datos 'FlowwwPrueba'
CREATE DATABASE FlowwwPrueba;

-- Cambiar al contexto de la base de datos 'FlowwwPrueba'
USE FlowwwPrueba;

-- Crear la tabla 'Joke'
CREATE TABLE Joke
(
    ID INT PRIMARY KEY NOT NULL,
    Text NVARCHAR(MAX) NULL
);
