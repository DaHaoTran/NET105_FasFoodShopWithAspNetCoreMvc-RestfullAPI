USE FastFoodShopNET105;

CREATE OR ALTER TRIGGER AUTO_UPDATE_ISONL
ON admins
FOR INSERT
AS
	BEGIN
		UPDATE admins
		SET IsOnl = 0 
		WHERE AdminCode = (SELECT AdminCode FROM inserted)

		UPDATE admins
		SET CreatedDate = SYSDATETIME()
		WHERE AdminCode = (SELECT AdminCode FROM inserted)
	END
GO

CREATE OR ALTER TRIGGER AUTO_SET_SOLD
ON foods
FOR INSERT
AS
	BEGIN
		UPDATE foods
		SET Sold = 0
		WHERE FoodCode = (SELECT FoodCode FROM inserted)
	END
GO

CREATE OR ALTER TRIGGER AUTO_Create_Cart
ON customer
AFTER INSERT
AS
	BEGIN
		DECLARE @EMAIL VARCHAR(MAX);
		SET @EMAIL = (SELECT Email FROM inserted);
		INSERT INTO cart values (@EMAIL);
	END
GO

CREATE OR ALTER TRIGGER AUTO_DELETE_CART
ON customer
INSTEAD OF DELETE
AS
	BEGIN
		DECLARE @CARTID INT;
		SET @CARTID = (SELECT CartId FROM cart WHERE CustomerEmail = (SELECT Email FROM deleted));
		IF (SELECT COUNT(*) FROM cartItem AS item WHERE item.CartId = @CARTID) = 0
			BEGIN
				DELETE FROM cart WHERE CartId = @CARTID;
				DELETE FROM customer WHERE Email = (SELECT Email FROM deleted);
			END
	END
GO

CREATE OR ALTER TRIGGER DECREASE_QUANTITY_FOOD
ON orderitem
AFTER INSERT
AS
	BEGIN
		DECLARE @FOODCODE CHAR(5), @QUANTITY INT;
		DECLARE INSERTCUSOR CURSOR FOR
		SELECT FoodCode, Quantity
		FROM inserted;

		OPEN INSERTCUSOR;

		FETCH NEXT FROM INSERTCUSOR INTO @FOODCODE, @QUANTITY;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE foods
			SET Sold = @QUANTITY, [Left] -= @QUANTITY
			WHERE FoodCode = @FOODCODE;

			FETCH NEXT FROM INSERTCUSOR INTO @FOODCODE, @QUANTITY;
		END

		CLOSE INSERTCUSOR;
		DEALLOCATE INSERTCUSOR;
	END	
GO

CREATE OR ALTER TRIGGER PAYMENT
ON [dbo].[order]
AFTER INSERT
AS
	BEGIN
		DECLARE @QUANTITY INT, @FOODCODE CHAR(5), @UNITPRICE INT;

		DECLARE @ORDERID INT;
		SET @ORDERID = (SELECT OrderId FROM inserted);

		DECLARE INSERTCUSOR1 CURSOR FOR
		SELECT FoodCode, Quantity, (SELECT CurrentPrice FROM foods WHERE FoodCode = C.FoodCode)
		FROM cartItem AS C;

		DECLARE @CARTID INT;
		SET @CARTID = (SELECT CartId FROM cart WHERE CustomerEmail = (SELECT CustomerEmail FROM inserted));

		OPEN INSERTCUSOR1;

		FETCH NEXT FROM INSERTCUSOR1 INTO @FOODCODE, @QUANTITY, @UNITPRICE;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO orderItem VALUES (@UNITPRICE, @QUANTITY, @ORDERID, @FOODCODE);
			DELETE FROM cartItem WHERE FoodCode = @FOODCODE;

			FETCH NEXT FROM INSERTCUSOR1 INTO @FOODCODE, @QUANTITY, @UNITPRICE;
		END

		CLOSE INSERTCUSOR1;
		DEALLOCATE INSERTCUSOR1;
	END
GO