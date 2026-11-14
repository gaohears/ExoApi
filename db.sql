CREATE DATABASE ExoApi
GO

USE ExoAPi
GO

CREATE TABLE Projetos (
    Id INT PRIMARY KEY IDENTITY,
    Nome VARCHAR(150) NOT NULL,
    Descricao VARCHAR(500),
    DataInicio DATE NOT NULL,
    Status VARCHAR(30) NOT NULL
)
GO

INSERT INTO Projetos (Nome, Descricao, DataInicio, Status)
VALUES ('Projeto A', 'Primeiro projeto de exemplo', '2026-01-10', 'Planejado')
GO

INSERT INTO Projetos (Nome, Descricao, DataInicio, Status)
VALUES ('Projeto B', 'Segundo projeto de exemplo', '2026-02-15', 'EmAndamento')
GO

SELECT Id, Nome, Descricao, DataInicio, Status FROM Projetos
GO
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Senha VARCHAR(120) NOT NULL
)
GO

INSERT INTO Usuarios VALUES ('email@sp.br', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
GO

SELECT * FROM Usuarios WHERE email = 'email@sp.br' AND senha = '1234'
GO