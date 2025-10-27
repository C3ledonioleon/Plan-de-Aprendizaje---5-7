CREATE USER IF NOT EXISTS 'Cliente'@'%' IDENTIFIED BY 'PassCl1et@123';
CREATE USER IF NOT EXISTS 'Administrador'@'localhost' IDENTIFIED BY 'Admin!2345';
CREATE USER IF NOT EXISTS 'Molinete'@'10.160.2%' IDENTIFIED BY 'M0linete@987';

-- CLIENTE 
GRANT SELECT, UPDATE ON SVE.Usuario TO 'Cliente'@'%';
GRANT SELECT ON SVE.Cliente TO 'Cliente'@'%';
GRANT SELECT ON SVE.Entrada TO 'Cliente'@'%';
GRANT SELECT ON SVE.Funcion TO 'Cliente'@'%';
GRANT SELECT ON SVE.Evento TO 'Cliente'@'%';

-- ADMIN
GRANT ALL ON SVE.* TO 'Administrador'@'localhost';

-- MOLINETE
GRANT SELECT ON SVE.Cliente TO 'Molinete'@'10.3.45.%';
GRANT SELECT ON SVE.Funcion TO 'Molinete'@'10.3.45.%';
GRANT SELECT, INSERT, UPDATE ON SVE.Entrada TO 'Molinete'@'10.3.45.%';	


