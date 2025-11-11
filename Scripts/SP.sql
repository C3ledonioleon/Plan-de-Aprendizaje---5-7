USE 5to_SVE;
DELIMITER $$

DROP PROCEDURE IF EXISTS PagarOrden $$
CREATE PROCEDURE PagarOrden(IN p_IdOrden INT)
proc_pagar: BEGIN
    DECLARE v_IdCliente INT;
    DECLARE v_IdTarifa INT;
    DECLARE v_IdFuncion INT;
    DECLARE v_Precio DECIMAL(10,2);

    START TRANSACTION;

    -- Obtener cliente y tarifa
    SELECT IdCliente, IdTarifa
    INTO v_IdCliente, v_IdTarifa
    FROM Orden
    WHERE IdOrden = p_IdOrden
    FOR UPDATE;

    -- Verificar si existe la orden
    IF ROW_COUNT() = 0 THEN
        ROLLBACK;
        SELECT 'La orden no existe.' AS Mensaje;
        LEAVE proc_pagar;
    END IF;

    -- Obtener precio e IdFuncion de la tarifa
    SELECT IdFuncion, Precio
    INTO v_IdFuncion, v_Precio
    FROM Tarifa
    WHERE IdTarifa = v_IdTarifa
    FOR UPDATE;

    -- Marcar orden como pagada
    UPDATE Orden
    SET Estado = 1
    WHERE IdOrden = p_IdOrden;

    -- Crear entrada
    INSERT INTO Entrada (IdOrden, IdTarifa, IdCliente, Estado, IdFuncion, Precio)
    VALUES (p_IdOrden, v_IdTarifa, v_IdCliente, 0, v_IdFuncion, v_Precio);

    -- Descontar stock
    UPDATE Tarifa
    SET Stock = Stock - 1
    WHERE IdTarifa = v_IdTarifa AND Stock > 0;

    COMMIT;
    SELECT 'Orden pagada y entrada creada.' AS Mensaje;
END $$

DELIMITER ;
