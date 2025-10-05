


-- Inserts para Usuario
INSERT INTO Usuario (Apodo, Contrasenia,Email) VALUES
('juan123', 'pass1', 'juan123@gmail.com'),
('maria88', 'pass2', 'maria88@gmail.com'),
('carlos22', 'pass3', 'carlos22@gmail.com'),
('sofia07', 'pass4', 'sofia07@gmail.com'),
('luisito', 'pass5', 'luisito@gmail.com'),
('pele', 'adminpass', 'pele@gmail.com');


-- Inserts para Cliente
INSERT INTO Cliente (DNI, Nombre, Telefono, IdUsuario) VALUES
('40111222', 'Juan Pérez', '1122334455', 1),
('38999888', 'María Gómez', '1133445566', 2),
('37222444', 'Carlos Fernández', '1144556677', 3),
('36555111', 'Sofía López', '1155667788', 4),
('40222333', 'Luis Ramírez', '1166778899', 5),
('35444777', 'Pelé Santos', '1177889900', 6);


-- Inserts para Local
INSERT INTO Local (Nombre, Direccion, CapacidadTotal) VALUES
('Teatro Colón', 'Cerrito 628', 2500),
('Luna Park', 'Av. Madero 420', 8000),
('Gran Rex', 'Av. Corrientes 857', 3200),
('Movistar Arena', 'Humboldt 450', 15000),
('Teatro Opera', 'Av. Corrientes 860', 2000),
('Estadio River Plate', 'Av. Figueroa Alcorta 7597', 70000);


-- Inserts para Sector
INSERT INTO Sector (Nombre, Capacidad, IdLocal) VALUES
('VIP', 500, 1),
('Platea Baja', 1000, 2),
('Campo', 3000, 3),
('Platea Alta', 2000, 4),
('General', 10000, 5),
('Palco', 200, 6);


-- Inserts para Evento
INSERT INTO Evento (Nombre, Descripcion, FechaInicio, FechaFin, Estado) VALUES
('Concierto Rock', 'Banda nacional de rock en vivo', '2025-10-01 20:00:00', '2025-10-01 23:00:00', 'Publicado'),
('Opera Clásica', 'Obra de Verdi en 3 actos', '2025-10-05 19:00:00', '2025-10-05 22:00:00', 'Publicado'),
('Festival Pop', 'Festival con varios artistas pop', '2025-10-10 18:00:00', '2025-10-10 23:59:00', 'Publicado'),
('Obra de Teatro', 'Comedia romántica', '2025-10-15 21:00:00', '2025-10-15 23:30:00', 'Publicado'),
('Concierto Internacional', 'Artista internacional en vivo', '2025-10-20 20:00:00', '2025-10-20 23:30:00', 'Publicado'),
('Festival de Jazz', 'Encuentro de jazz nacional e internacional', '2025-10-25 19:00:00', '2025-10-25 23:59:00', 'Publicado');


-- Inserts para Funcion
INSERT INTO Funcion (IdEvento, IdLocal, FechaHora, Estado) VALUES
(1, 1, '2025-10-01 20:00:00', 'Pendiente'),
(2, 1, '2025-10-05 19:00:00', 'Pendiente'),
(3, 2, '2025-10-10 18:00:00', 'Pendiente'),
(4, 3, '2025-10-15 21:00:00', 'Pendiente'),
(5, 4, '2025-10-20 20:00:00', 'Pendiente'),
(6, 5, '2025-10-25 19:00:00', 'Pendiente');


-- Inserts para Tarifa
INSERT INTO Tarifa (IdFuncion, IdSector, Precio, Stock, Estado) VALUES
(1, 1, 5000.00, 200, 'Activa'),
(2, 2, 3500.00, 300, 'Activa'),
(3, 3, 2500.00, 500, 'Activa'),
(4, 4, 4000.00, 250, 'Activa'),
(5, 5, 10000.00, 100, 'Activa'),
(6, 6, 1500.00, 50, 'Activa');


-- Inserts para Orden
INSERT INTO Orden (IdTarifa, IdCliente, Total, Fecha, Estado) VALUES
(1, 1, 10000.00, '2025-09-20 12:00:00', 'Creada'),
(2, 2, 7000.00, '2025-09-21 14:00:00', 'Pagada'),
(3, 3, 5000.00, '2025-09-22 16:00:00', 'Creada'),
(4, 4, 4000.00, '2025-09-23 18:00:00', 'Cancelada'),
(5, 5, 20000.00, '2025-09-24 20:00:00', 'Pagada'),
(6, 6, 1500.00, '2025-09-25 22:00:00', 'Creada');


-- Inserts para Entrada
INSERT INTO Entrada (Precio, IdOrden, IdTarifa, Estado) VALUES
(5000.00, 1, 1, 'Activa'),
(3500.00, 2, 2, 'Activa'),
(2500.00, 3, 3, 'Activa'),
(4000.00, 4, 4, 'Anulada'),
(10000.00, 5, 5, 'Activa'),
(1500.00, 6, 6, 'Activa');


INSERT INTO RefreshToken (Token, Expira, EstaRevocado, UsuarioId)
VALUES 
('abc123xyz', '2025-12-31 23:59:59', 0, 1),
('def456uvw', '2025-11-30 23:59:59', 0, 1),
('ghi789rst', '2025-10-15 23:59:59', 1, 1),
('jkl012mno', '2026-01-15 23:59:59', 0, 1),
('pqr345stu', '2026-02-28 23:59:59', 0, 1),
('vwx678yz0', '2026-03-31 23:59:59', 0, 1);
