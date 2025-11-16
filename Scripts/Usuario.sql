-- Usuario
CREATE USER IF NOT EXISTS 'Usuario'@'%' IDENTIFIED BY 'PassUsu@123';
GRANT SELECT, UPDATE ON 5to_SVE.Usuario TO 'Usuario'@'%';
GRANT SELECT, INSERT ON 5to_SVE.Cliente TO 'Usuario'@'%';
GRANT INSERT, UPDATE ON 5to_SVE.Orden TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Entrada TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Funcion TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Evento TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Sector TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Tarifa TO 'Usuario'@'%';
GRANT SELECT ON 5to_SVE.Local TO 'Usuario'@'%';
GRANT EXECUTE ON PROCEDURE 5to_SVE.PagarOrden TO 'Usuario'@'%';
GRANT EXECUTE ON PROCEDURE 5to_SVE.CancelarOrden TO 'Usuario'@'%';

-- ORGANIZADOR
CREATE USER IF NOT EXISTS 'Organizador'@'%' IDENTIFIED BY '0rg@niZador#1';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Evento TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Funcion TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Tarifa TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Entrada TO 'Organizador'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Orden TO 'Organizador'@'%';
GRANT SELECT, UPDATE ON 5to_SVE.Usuario TO 'Organizador'@'%';

GRANT SELECT ON 5to_SVE.Local TO 'Organizador'@'%';
GRANT SELECT ON 5to_SVE.Sector TO 'Organizador'@'%';
FLUSH PRIVILEGES;

-- Administrador 
CREATE USER IF NOT EXISTS 'Administrador'@'%' IDENTIFIED BY 'Admin!2345';
GRANT ALL ON 5to_SVE.* TO 'Administrador'@'%';

-- MOLINETE
CREATE USER IF NOT EXISTS 'Molinete'@'%' IDENTIFIED BY 'M0linete@987';
GRANT SELECT ON 5to_SVE.Cliente TO 'Molinete'@'%';
GRANT SELECT ON 5to_SVE.Funcion TO 'Molinete'@'%';
GRANT SELECT, INSERT, UPDATE ON 5to_SVE.Entrada TO 'Molinete'@'%';	
GRANT SELECT ON 5to_SVE.Sector TO 'Molinete'@'%';	
GRANT SELECT ON 5to_SVE.Evento TO 'Molinete'@'%';
GRANT SELECT, UPDATE ON 5to_SVE.Usuario TO 'Molinete'@'%';

FLUSH PRIVILEGES;
