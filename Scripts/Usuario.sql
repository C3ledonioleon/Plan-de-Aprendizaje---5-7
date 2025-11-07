-- Usuario
CREATE USER IF NOT EXISTS 'Usuario'@'%' IDENTIFIED BY 'PassUsu@123';
GRANT SELECT, UPDATE ON 5to_SVE.Usuario TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Cliente TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Entrada TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Funcion TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Evento TO 'Usuario'@'%';

-- ORGANIZADOR
CREATE USER IF NOT EXISTS 'Organizador'@'%' IDENTIFIED BY '0rg@niZador#1';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Evento TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Funcion TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Tarifa TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Entrada TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Orden TO 'Organizador'@'%';
GRANT SELECT ON 5to_SVE.Local TO 'Organizador'@'%';
GRANT SELECT ON 5to_SVE.Sector TO 'Organizador'@'%';

-- ADMIN
CREATE USER IF NOT EXISTS 'Administrador'@'localhost' IDENTIFIED BY 'Admin!2345';
GRANT ALL ON 5to_SVE.* TO 'Administrador'@'localhost';

-- MOLINETE
CREATE USER IF NOT EXISTS 'Molinete'@'%' IDENTIFIED BY 'M0linete@987';
GRANT SELECT ON 5to_SVE.Cliente TO 'Molinete'@'%';
GRANT SELECT ON 5to_SVE.Funcion TO 'Molinete'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Entrada TO 'Molinete'@'%';	

FLUSH PRIVILEGES;

