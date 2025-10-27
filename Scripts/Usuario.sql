-- Usuario Cliente: puede ver su información y sus entradas, y actualizar su email
CREATE USER IF NOT EXISTS 'Cliente'@'%' IDENTIFIED BY 'passcliet123';
-- Usuario Administrador: acceso total desde localhost
CREATE USER IF NOT EXISTS 'Administrador'@'localhost' IDENTIFIED BY 'admin12345';
-- Usuario Molinete: solo registra ingresos, restringido a la LAN
CREATE USER IF NOT EXISTS 'Molinete'@'10.3.45.%' IDENTIFIED BY 'moli987';


-- CLIENTE 
-- Cliente solo puede ver y actualizar su email
GRANT SELECT, UPDATE(Email) ON SVE.Usuario TO 'Cliente'@'%';
-- Cliente puede ver su información
GRANT SELECT ON SVE.Cliente TO 'Cliente'@'%';
-- Cliente puede ver sus entradas
GRANT SELECT ON SVE.Entrada TO 'Cliente'@'%';
-- Cliente puede ver las funciones
GRANT SELECT ON SVE.Funcion TO 'Cliente'@'%';
-- Cliente puede ver los eventos
GRANT SELECT ON SVE.Evento TO 'Cliente'@'%';


-- Admin
-- Otorgar todos los privilegios sobre la base de datos SVE
GRANT ALL PRIVILEGES ON SVE.* TO 'Administrador'@'localhost';


-- Molinete
-- Lectura de clientes
GRANT SELECT(IdCliente) ON SVE.Cliente TO 'Molinete'@'10.3.45.%';
-- Lectura de funciones
GRANT SELECT(IdFuncion) ON SVE.Funcion TO 'Molinete'@'10.3.45.%';
-- Insertar y actualizar entradas
GRANT SELECT, INSERT, UPDATE(Estado) ON SVE.Entrada TO 'Molinete'@'10.3.45.%';