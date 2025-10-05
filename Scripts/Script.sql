-- --------------------------------------------------
-- Tablas para el sistema Sve
-- --------------------------------------------------

-- Usuario
CREATE TABLE Usuario (
    IdUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Apodo VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Contrasenia VARCHAR(255) NOT NULL,
    Rol ENUM('Administrador','Empleado','Cliente') NOT NULL
);

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
    Estado ENUM('Inactivo','Publicado','Cancelado') NOT NULL
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
    Estado ENUM('Pendiente','Cancelada','Finalizada') NOT NULL,
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
    Estado ENUM('Activa','Inactiva') NOT NULL,
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
    Estado ENUM('Creada','Pagada','Cancelada') NOT NULL,
    FOREIGN KEY (IdTarifa) REFERENCES Tarifa(IdTarifa),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
);

-- Entrada
CREATE TABLE Entrada (
    IdEntrada INT AUTO_INCREMENT PRIMARY KEY,
    Precio DECIMAL(10,2) NOT NULL,
    Estado ENUM('Activa','Anulada') NOT NULL,
    IdOrden INT NOT NULL,
    IdTarifa INT NOT NULL,
    IdCliente INT NOT NULL,
    IdFuncion INT NOT NULL,
    FOREIGN KEY (IdOrden) REFERENCES Orden(IdOrden),
    FOREIGN KEY (IdTarifa) REFERENCES Tarifa(IdTarifa),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente),
    FOREIGN KEY (IdFuncion) REFERENCES Funcion(IdFuncion)
);

-- RefreshToken
CREATE TABLE RefreshToken (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Token VARCHAR(255) NOT NULL,
    Expira DATETIME NOT NULL,
    EstaRevocado BIT DEFAULT 0,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(IdUsuario)
);
