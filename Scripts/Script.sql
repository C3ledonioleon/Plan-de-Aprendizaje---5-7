DROP DATABASE IF EXISTS  5to_SVE;
CREATE DATABASE  5to_SVE;
USE 5to_SVE;

-- Usuario
CREATE TABLE Usuario (
    IdUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Password VARCHAR(256) NOT NULL,
    Rol INT NULL,
    RefreshToken VARCHAR(255) NULL,
    RefreshTokenExpiracion DATETIME NULL
);

-- INSERTAR USUARIO ADMINISTRADOR POR DEFECTO
INSERT INTO Usuario (Username, Email, Password, Rol)
VALUES ('admin', 'admin@gmail.com', '3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121', 1);

-- Cliente
CREATE TABLE Cliente (
    IdCliente INT AUTO_INCREMENT PRIMARY KEY,
    DNI VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(50),
    IdUsuario INT NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

-- Evento
CREATE TABLE Evento (
    IdEvento INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion TEXT,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Estado INT NOT NULL
);

-- Local
CREATE TABLE Local (
    IdLocal INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Direccion VARCHAR(255),
    CapacidadTotal INT NOT NULL
);

-- Sector
CREATE TABLE Sector (
    IdSector INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Capacidad INT NOT NULL,
    IdLocal INT NOT NULL,
    FOREIGN KEY (IdLocal) REFERENCES Local(IdLocal)
);

-- Funcion
CREATE TABLE Funcion (
    IdFuncion INT AUTO_INCREMENT PRIMARY KEY,
    IdEvento INT NOT NULL,
    IdLocal INT NOT NULL,
    FechaHora DATETIME NOT NULL,
    Estado INT NOT NULL,
    FOREIGN KEY (IdEvento) REFERENCES Evento(IdEvento),
    FOREIGN KEY (IdLocal) REFERENCES Local(IdLocal)
);

-- Tarifa
CREATE TABLE Tarifa (
    IdTarifa INT AUTO_INCREMENT PRIMARY KEY,
    IdFuncion INT NOT NULL,
    IdSector INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Estado INT NOT NULL,
    FOREIGN KEY (IdFuncion) REFERENCES Funcion(IdFuncion),
    FOREIGN KEY (IdSector) REFERENCES Sector(IdSector)
);

-- Orden
CREATE TABLE Orden (
    IdOrden INT AUTO_INCREMENT PRIMARY KEY,
    IdTarifa INT NOT NULL,
    IdCliente INT NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    Fecha DATETIME NOT NULL,
    Estado INT NOT NULL,
    FOREIGN KEY (IdTarifa) REFERENCES Tarifa(IdTarifa),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
);

-- Entrada
CREATE TABLE Entrada (
    IdEntrada INT AUTO_INCREMENT PRIMARY KEY,
    Precio DECIMAL(10,2) NOT NULL,
    Estado INT NOT NULL,
    IdOrden INT NOT NULL,
    IdTarifa INT NOT NULL,
    IdCliente INT NOT NULL,
    IdFuncion INT NOT NULL,
    FOREIGN KEY (IdOrden) REFERENCES Orden(IdOrden),
    FOREIGN KEY (IdTarifa) REFERENCES Tarifa(IdTarifa),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente),
    FOREIGN KEY (IdFuncion) REFERENCES Funcion(IdFuncion)
);




