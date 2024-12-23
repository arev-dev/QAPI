CREATE PROCEDURE SPCreateUser
    @Username VARCHAR(50),
    @Password VARCHAR(255),
    @NewUserId INT OUTPUT
AS
BEGIN
    -- Validar que los par�metros no sean nulos ni vac�os
    IF (@Username IS NULL OR LTRIM(RTRIM(@Username)) = '')
    BEGIN
        SET @NewUserId = 0; -- C�digo de error para nombre de usuario vac�o
        RETURN -1; -- Salir del procedimiento
    END

    IF (@Password IS NULL OR LTRIM(RTRIM(@Password)) = '')
    BEGIN
        SET @NewUserId = 0; -- C�digo de error para contrase�a vac�a
        RETURN -2; -- Salir del procedimiento
    END

    -- Verificar si el nombre de usuario ya existe
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        SET @NewUserId = 0; -- C�digo de error para nombre de usuario ya existente
        RETURN -3; -- Salir del procedimiento
    END

    BEGIN TRY
        -- Insertar el nuevo usuario en la tabla
        INSERT INTO Users (Username, Password)
        VALUES (@Username, @Password);

        -- Obtener el ID del nuevo usuario
        SET @NewUserId = SCOPE_IDENTITY(); -- SCOPE_IDENTITY() obtiene el �ltimo ID insertado en la sesi�n actual

        RETURN 1; -- C�digo de �xito
    END TRY
    BEGIN CATCH
        -- Manejar errores inesperados
        SET @NewUserId = 0; -- C�digo de error gen�rico
        RETURN -99; -- Salir del procedimiento
    END CATCH
END;
GO

CREATE PROCEDURE SPGetUserById
    @Id INT
AS
BEGIN
    SELECT * FROM Users WHERE Id = @Id
END

GO

CREATE PROCEDURE SPGetUserByUsername
    @Username VARCHAR(50)
AS
BEGIN
    SELECT * FROM Users WHERE Username = @Username
END

GO

CREATE PROCEDURE SPGetAllUsers 
AS
SELECT * FROM Users
GO;

CREATE PROCEDURE SPDeleteUser
	@Id INT
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
	BEGIN
		RETURN -1; 
	END
	BEGIN TRY
		DELETE FROM Comments WHERE UserId = @Id;
		DELETE FROM Posts WHERE UserId = @Id;
		DELETE FROM Users WHERE Id = @Id
	END TRY
	BEGIN CATCH
		RETURN -99; 
	END CATCH
END

GO

CREATE PROCEDURE SPLoginUser
	@Username varchar(50),
	@Password varchar(255)
AS
	IF (@Username IS NULL OR LTRIM(RTRIM(@Username)) = '')
		BEGIN
			RETURN -1; 
		END

    IF (@Password IS NULL OR LTRIM(RTRIM(@Password)) = '')
		BEGIN
			RETURN -2;
		END

		 -- Verificar si la contrase�a es incorrecta
		IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username AND Password = @Password)
		BEGIN
			RETURN -3; -- C�digo de error para contrase�a incorrecta
		END

		-- Credenciales v�lidas
		RETURN 1; -- �xito
GO

CREATE PROCEDURE SPUpdateUser
	@Id INT,
	@Username Varchar(50),
	@Password Varchar(255)
AS
BEGIN
	IF (@Username IS NULL OR LTRIM(RTRIM(@Username)) = '')
		BEGIN
			RETURN -1; 
		END

    IF (@Password IS NULL OR LTRIM(RTRIM(@Password)) = '')
		BEGIN
			RETURN -2;
		END

    -- Verificar si el nombre de usuario ya existe
		IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
		BEGIN
			RETURN -3; 
		END

		BEGIN TRY
			UPDATE Users
			SET Username = @Username, Password = @Password
			WHERE Id = @Id

			RETURN 1;
		END TRY
		BEGIN CATCH
			-- Manejar errores inesperados

			RETURN -99;
		END CATCH
END
