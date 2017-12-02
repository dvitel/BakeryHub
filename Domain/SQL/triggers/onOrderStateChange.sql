IF EXISTS(SELECT * FROM sys.triggers where name='onOrderStateChange' AND parent_id = object_id(N'dbo.Orders'))
DROP TRIGGER onOrderStateChange
GO

CREATE TRIGGER onOrderStateChange
    ON [dbo].Orders
    AFTER UPDATE
    AS
    BEGIN
    SET NOCOUNT ON

	--BEGIN TRANSACTION onOrderStateChangeCleanup
	--do not need transaction, triggers are always within transactions
	--of triggering statements
	--either if it one autocommit statement or explisit transaction
	
	DELETE sensitive 
	FROM OrderPaymentSensitiveInfo sensitive
	JOIN  		
		(SELECT newOrders.CustomerId, newOrders.OrderId 
		 FROM 
			inserted newOrders
			JOIN 
			deleted oldOrders ON newOrders.CustomerId = oldOrders.CustomerId AND newOrders.OrderId = oldOrders.OrderId
		 WHERE newOrders.Status > 0 AND oldOrders.Status = 0) as statusIsNotInitial(CustomerId, OrderId)
	ON sensitive.CustomerId = statusIsNotInitial.CustomerId AND sensitive.OrderId = statusIsNotInitial.OrderId

	SELECT inserted.CustomerId, inserted.OrderId INTO #finalizedOrders
	FROM inserted WHERE [Status] IN 
		(6, --Delivered
		 1) --Canceled, PaymentFialed goes to Canceled after some period of inactivity

	SELECT Id INTO #handshakeToDelete
	FROM Handshake h
	JOIN #finalizedOrders o ON h.CustomerId = o.CustomerId AND h.OrderId = o.OrderId

	DELETE HandshakeComments WHERE HandshakeId IN (SELECT Id FROM #handshakeToDelete)

	DELETE Handshake WHERE Id IN (SELECT Id FROM #handshakeToDelete)

	--COMMIT TRAN onOrderStateChangeCleanup

    END
GO