DELIMITER $$

CREATE PROCEDURE PagarOrden(IN p_IdOrden INT)
BEGIN
    DECLARE v_StockInsuficiente INT DEFAULT 0;

    START TRANSACTION;

    SELECT COUNT(*) INTO v_StockInsuficiente
    FROM Entrada e
    INNER JOIN Tarifa t ON e.IdTarifa = t.IdTarifa
    WHERE e.IdOrden = p_IdOrden AND t.Stock <= 0;

    IF v_StockInsuficiente > 0 THEN
        ROLLBACK;
        SELECT 'Stock insuficiente para una o más tarifas' AS Mensaje;
    ELSE
        UPDATE Orden 
        SET Estado = 1
        WHERE IdOrden = p_IdOrden;

        UPDATE Tarifa t
        INNER JOIN Entrada e ON t.IdTarifa = e.IdTarifa
        SET t.Stock = t.Stock - 1
        WHERE e.IdOrden = p_IdOrden;

        UPDATE Entrada 
        SET Estado = 1
        WHERE IdOrden = p_IdOrden;

        COMMIT;
        SELECT 'Orden pagada exitosamente' AS Mensaje;
    END IF;
END $$

DELIMITER ;
