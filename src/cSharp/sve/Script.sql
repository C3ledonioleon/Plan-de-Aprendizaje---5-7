Build started...
Build succeeded.
The Entity Framework tools version '8.0.2' is older than that of the runtime '8.0.20'. Update the tools for the latest features and bug fixes. See https://aka.ms/AAc1fbw for more information.
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Evento` (
    `IdEvento` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Descripcion` longtext CHARACTER SET utf8mb4 NOT NULL,
    `FechaInicio` datetime(6) NOT NULL,
    `FechaFin` datetime(6) NOT NULL,
    `Estado` int NOT NULL,
    CONSTRAINT `PK_Evento` PRIMARY KEY (`IdEvento`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Local` (
    `IdLocal` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Direccion` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CapacidadTotal` int NOT NULL,
    CONSTRAINT `PK_Local` PRIMARY KEY (`IdLocal`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Usuario` (
    `IdUsuario` int NOT NULL AUTO_INCREMENT,
    `Apodo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `contrasenia` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Rol` int NOT NULL,
    CONSTRAINT `PK_Usuario` PRIMARY KEY (`IdUsuario`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Funcion` (
    `IdFuncion` int NOT NULL AUTO_INCREMENT,
    `IdEvento` int NOT NULL,
    `IdLocal` int NOT NULL,
    `FechaHora` datetime(6) NOT NULL,
    `Estado` int NOT NULL,
    `EventoIdEvento` int NULL,
    `LocalIdLocal` int NULL,
    CONSTRAINT `PK_Funcion` PRIMARY KEY (`IdFuncion`),
    CONSTRAINT `FK_Funcion_Evento_EventoIdEvento` FOREIGN KEY (`EventoIdEvento`) REFERENCES `Evento` (`IdEvento`),
    CONSTRAINT `FK_Funcion_Evento_IdEvento` FOREIGN KEY (`IdEvento`) REFERENCES `Evento` (`IdEvento`) ON DELETE CASCADE,
    CONSTRAINT `FK_Funcion_Local_LocalIdLocal` FOREIGN KEY (`LocalIdLocal`) REFERENCES `Local` (`IdLocal`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Sector` (
    `IdSector` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Capacidad` int NOT NULL,
    `IdLocal` int NOT NULL,
    `LocalIdLocal` int NOT NULL,
    CONSTRAINT `PK_Sector` PRIMARY KEY (`IdSector`),
    CONSTRAINT `FK_Sector_Local_IdLocal` FOREIGN KEY (`IdLocal`) REFERENCES `Local` (`IdLocal`) ON DELETE CASCADE,
    CONSTRAINT `FK_Sector_Local_LocalIdLocal` FOREIGN KEY (`LocalIdLocal`) REFERENCES `Local` (`IdLocal`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Cliente` (
    `IdCliente` int NOT NULL AUTO_INCREMENT,
    `DNI` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Telefono` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IdUsuario` int NOT NULL,
    CONSTRAINT `PK_Cliente` PRIMARY KEY (`IdCliente`),
    CONSTRAINT `FK_Cliente_Usuario_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuario` (`IdUsuario`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `Tarifa` (
    `IdTarifa` int NOT NULL AUTO_INCREMENT,
    `IdFuncion` int NOT NULL,
    `IdSector` int NOT NULL,
    `Precio` decimal(65,30) NOT NULL,
    `Stock` int NOT NULL,
    `FuncionIdFuncion` int NULL,
    `SectorIdSector` int NULL,
    `Estado` int NOT NULL,
    CONSTRAINT `PK_Tarifa` PRIMARY KEY (`IdTarifa`),
    CONSTRAINT `FK_Tarifa_Funcion_FuncionIdFuncion` FOREIGN KEY (`FuncionIdFuncion`) REFERENCES `Funcion` (`IdFuncion`),
    CONSTRAINT `FK_Tarifa_Sector_SectorIdSector` FOREIGN KEY (`SectorIdSector`) REFERENCES `Sector` (`IdSector`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Orden` (
    `IdOrden` int NOT NULL AUTO_INCREMENT,
    `IdTarifa` int NOT NULL,
    `IdCliente` int NOT NULL,
    `Total` decimal(65,30) NOT NULL,
    `Fecha` datetime(6) NOT NULL,
    `Estado` int NOT NULL,
    `ClienteIdCliente` int NOT NULL,
    `TarifaIdTarifa` int NOT NULL,
    CONSTRAINT `PK_Orden` PRIMARY KEY (`IdOrden`),
    CONSTRAINT `FK_Orden_Cliente_ClienteIdCliente` FOREIGN KEY (`ClienteIdCliente`) REFERENCES `Cliente` (`IdCliente`) ON DELETE CASCADE,
    CONSTRAINT `FK_Orden_Cliente_IdCliente` FOREIGN KEY (`IdCliente`) REFERENCES `Cliente` (`IdCliente`) ON DELETE CASCADE,
    CONSTRAINT `FK_Orden_Tarifa_IdTarifa` FOREIGN KEY (`IdTarifa`) REFERENCES `Tarifa` (`IdTarifa`) ON DELETE CASCADE,
    CONSTRAINT `FK_Orden_Tarifa_TarifaIdTarifa` FOREIGN KEY (`TarifaIdTarifa`) REFERENCES `Tarifa` (`IdTarifa`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Entrada` (
    `IdEntrada` int NOT NULL AUTO_INCREMENT,
    `Precio` decimal(65,30) NOT NULL,
    `Estado` int NOT NULL,
    `IdOrden` int NOT NULL,
    `IdTarifa` int NOT NULL,
    `IdCliente` int NOT NULL,
    `IdFuncion` int NOT NULL,
    `OrdenIdOrden` int NOT NULL,
    `TarifaIdTarifa` int NOT NULL,
    `FuncionIdFuncion` int NOT NULL,
    `ClienteIdCliente` int NOT NULL,
    CONSTRAINT `PK_Entrada` PRIMARY KEY (`IdEntrada`),
    CONSTRAINT `FK_Entrada_Cliente_ClienteIdCliente` FOREIGN KEY (`ClienteIdCliente`) REFERENCES `Cliente` (`IdCliente`) ON DELETE CASCADE,
    CONSTRAINT `FK_Entrada_Funcion_FuncionIdFuncion` FOREIGN KEY (`FuncionIdFuncion`) REFERENCES `Funcion` (`IdFuncion`) ON DELETE CASCADE,
    CONSTRAINT `FK_Entrada_Orden_IdOrden` FOREIGN KEY (`IdOrden`) REFERENCES `Orden` (`IdOrden`) ON DELETE CASCADE,
    CONSTRAINT `FK_Entrada_Orden_OrdenIdOrden` FOREIGN KEY (`OrdenIdOrden`) REFERENCES `Orden` (`IdOrden`) ON DELETE CASCADE,
    CONSTRAINT `FK_Entrada_Tarifa_IdTarifa` FOREIGN KEY (`IdTarifa`) REFERENCES `Tarifa` (`IdTarifa`) ON DELETE CASCADE,
    CONSTRAINT `FK_Entrada_Tarifa_TarifaIdTarifa` FOREIGN KEY (`TarifaIdTarifa`) REFERENCES `Tarifa` (`IdTarifa`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_Cliente_IdUsuario` ON `Cliente` (`IdUsuario`);

CREATE INDEX `IX_Entrada_ClienteIdCliente` ON `Entrada` (`ClienteIdCliente`);

CREATE INDEX `IX_Entrada_FuncionIdFuncion` ON `Entrada` (`FuncionIdFuncion`);

CREATE INDEX `IX_Entrada_IdOrden` ON `Entrada` (`IdOrden`);

CREATE INDEX `IX_Entrada_IdTarifa` ON `Entrada` (`IdTarifa`);

CREATE INDEX `IX_Entrada_OrdenIdOrden` ON `Entrada` (`OrdenIdOrden`);

CREATE INDEX `IX_Entrada_TarifaIdTarifa` ON `Entrada` (`TarifaIdTarifa`);

CREATE INDEX `IX_Funcion_EventoIdEvento` ON `Funcion` (`EventoIdEvento`);

CREATE INDEX `IX_Funcion_IdEvento` ON `Funcion` (`IdEvento`);

CREATE INDEX `IX_Funcion_LocalIdLocal` ON `Funcion` (`LocalIdLocal`);

CREATE INDEX `IX_Orden_ClienteIdCliente` ON `Orden` (`ClienteIdCliente`);

CREATE INDEX `IX_Orden_IdCliente` ON `Orden` (`IdCliente`);

CREATE INDEX `IX_Orden_IdTarifa` ON `Orden` (`IdTarifa`);

CREATE INDEX `IX_Orden_TarifaIdTarifa` ON `Orden` (`TarifaIdTarifa`);

CREATE INDEX `IX_Sector_IdLocal` ON `Sector` (`IdLocal`);

CREATE INDEX `IX_Sector_LocalIdLocal` ON `Sector` (`LocalIdLocal`);

CREATE INDEX `IX_Tarifa_FuncionIdFuncion` ON `Tarifa` (`FuncionIdFuncion`);

CREATE INDEX `IX_Tarifa_SectorIdSector` ON `Tarifa` (`SectorIdSector`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251002155510_MigracionInicial', '8.0.20');

COMMIT;


