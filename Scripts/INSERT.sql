-- Inserts para Usuario
INSERT INTO Usuario ( Username, password ,Email,Rol) VALUES
('juan123', 'pass1', 'juan123@gmail.com',2),
('maria88', 'pass2', 'maria88@gmail.com',2),
('carlos22', 'pass3', 'carlos22@gmail.com',3),
('sofia07', 'pass4', 'sofia07@gmail.com',3),
('luisito', 'pass5', 'luisito@gmail.com',3),
('pele', 'adminpass', 'pele@gmail.com',2);


-- Inserts para Cliente
INSERT INTO Cliente (DNI, Nombre, Telefono, IdUsuario) VALUES
('40111222', 'Juan Pérez', '1122334455', 2),
('38999888', 'María Gómez', '1133445566', 3),
('37222444', 'Carlos Fernández', '1144556677', 4),
('36555111', 'Sofía López', '1155667788', 5),
('40222333', 'Luis Ramírez', '1166778899', 6),
('35444777', 'Pelé Santos', '1177889900', 7);


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
('Concierto Rock', 'Banda nacional de rock en vivo', '2025-10-01 20:00:00', '2025-10-01 23:00:00', '1'),
('Opera Clásica', 'Obra de Verdi en 3 actos', '2025-10-05 19:00:00', '2025-10-05 22:00:00', '1'),
('Festival Pop', 'Festival con varios artistas pop', '2025-10-10 18:00:00', '2025-10-10 23:59:00', '1'),
('Obra de Teatro', 'Comedia romántica', '2025-10-15 21:00:00', '2025-10-15 23:30:00', '1'),
('Concierto Internacional', 'Artista internacional en vivo', '2025-10-20 20:00:00', '2025-10-20 23:30:00', '1'),
('Festival de Jazz', 'Encuentro de jazz nacional e internacional', '2025-10-25 19:00:00', '2025-10-25 23:59:00', '1');


-- Inserts para Funcion
INSERT INTO Funcion (IdEvento, IdLocal, FechaHora, Estado) VALUES
(1, 1, '2025-10-01 20:00:00', '0'),
(2, 1, '2025-10-05 19:00:00', '0'),
(3, 2, '2025-10-10 18:00:00', '0'),
(4, 3, '2025-10-15 21:00:00', '0'),
(5, 4, '2025-10-20 20:00:00', '0'),
(6, 5, '2025-10-25 19:00:00', '0');


-- Inserts para Tarifa
INSERT INTO Tarifa (IdFuncion, IdSector, Precio, Stock, Estado) VALUES
(1, 1, 5000.00, 200, '0'),
(2, 2, 3500.00, 300, '0'),
(3, 3, 2500.00, 500, '0'),
(4, 4, 4000.00, 250, '0'),
(5, 5, 10000.00, 100, '0'),
(6, 6, 1500.00, 50, '0');


-- Inserts para Orden
INSERT INTO Orden (IdTarifa, IdCliente, Total, Fecha, Estado) VALUES
(1, 1, 10000.00, '2025-09-20 12:00:00', '0'),
(2, 2, 7000.00, '2025-09-21 14:00:00', '1'),
(3, 3, 5000.00, '2025-09-22 16:00:00', '0'),
(4, 4, 4000.00, '2025-09-23 18:00:00', '2'),
(5, 5, 20000.00, '2025-09-24 20:00:00', '1'),
(6, 6, 1500.00, '2025-09-25 22:00:00', '0');


-- Inserts para Entrada
INSERT INTO Entrada (Precio, Estado, IdOrden, IdTarifa, IdCliente, IdFuncion) VALUES
(5000.00, 1, 1, 1, 1, 1),
(3500.00, 1, 2, 2, 2, 1),
(2500.00, 1, 3, 3, 3, 2),
(4000.00, 2, 4, 4, 4, 2),
(10000.00, 1, 5, 5, 5, 3),
(1500.00, 1, 6, 6, 6, 3);
