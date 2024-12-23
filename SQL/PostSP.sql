create procedure SPCreatePost
	@UserId INT,
	@Title varchar(50),
	@Content varchar(500),
	@NewPostId INT OUTPUT
AS
	-- Validar que los parámetros no sean nulos ni vacíos
    IF (@Title IS NULL OR LTRIM(RTRIM(@Title)) = '')
    BEGIN
        SET @NewPostId = 0;
        RETURN -1; -- Salir del procedimiento
    END

	IF (@Content IS NULL OR LTRIM(RTRIM(@Content)) = '')
    BEGIN
        SET @NewPostId = 0;
        RETURN -2; -- Salir del procedimiento
    END

	IF (@UserId IS NULL OR @UserId = 0)
    BEGIN
        SET @NewPostId = 0;
        RETURN -3; -- Salir del procedimiento
    END

	IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        SET @NewPostId = 0;
        RETURN -4; -- No existe el usuario
    END

	BEGIN TRY
        -- Insertar el nuevo usuario en la tabla
        INSERT INTO Posts (UserId, Title,Content,IsClosed)
        VALUES (@UserId, @Title,@Content,0);

        -- Obtener el ID del nuevo post
        SET @NewPostId = SCOPE_IDENTITY();

        RETURN 1; -- Código de éxito
    END TRY

	BEGIN CATCH
        -- Manejar errores inesperados
        SET @NewPostId = 0; -- Código de error genérico
        RETURN -99; -- Salir del procedimiento
    END CATCH
GO

create procedure SPGetPostById
	@Id INT
AS
	BEGIN
		SELECT * FROM Posts WHERE Id = @Id
	END
GO

create procedure SPGetAllPosts
AS
BEGIN
	SELECT * FROM Posts ORDER BY CreatedAt DESC;
END
GO

CREATE PROCEDURE SPDeletePost
	@Id INT
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = @Id)
	BEGIN
		RETURN -1; 
	END
	BEGIN TRY
		DELETE FROM Comments WHERE PostId = @Id
		DELETE FROM Posts WHERE Id = @Id
	END TRY
	BEGIN CATCH
		RETURN -99; 
	END CATCH
END
GO

CREATE PROCEDURE SPUpdatePost
	@Id INT,
	@Title varchar(50),
	@Content varchar(255),
	@IsClosed bit
AS
	IF NOT EXISTS(SELECT * FROM Posts WHERE Id = @Id)
	BEGIN
		RETURN -1;
	END

	IF (@Title IS NULL OR LTRIM(RTRIM(@Title)) = '')
	BEGIN
		RETURN -2
	END

	IF (@Content IS NULL OR LTRIM(RTRIM(@Content)) = '')
	BEGIN
		RETURN -3
	END

	BEGIN TRY
		Update Posts SET
		Title = @Title,
		Content = @Content,
		IsClosed = @IsClosed
		WHERE Id = @Id

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99;
	END CATCH
GO

CREATE PROCEDURE SPGetUserPosts
	@UserId INT
AS
	IF NOT EXISTS(SELECT * FROM Users WHERE Id = @UserId)
	BEGIN
		RETURN -1;
	END

	BEGIN TRY
		SELECT P.ID as PostId, P.Title, P.Content, P.IsClosed, P.CreatedAt, U.Id as UserId, U.Username
		FROM Posts as P
		INNER JOIN Users as U ON P.UserId = U.Id;

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99;
	END CATCH
GO

CREATE PROCEDURE SPGetPostComments
	@Id INT
AS
	IF NOT EXISTS(SELECT * FROM Posts WHERE Id = @ID)
	BEGIN
		RETURN -1;
	END

	BEGIN TRY
		SELECT * FROM Comments WHERE PostId = @Id

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99
	END CATCH

GO

CREATE PROCEDURE SPClosePost
	@Id INT
AS
	IF NOT EXISTS(SELECT * FROM Posts WHERE Id = @ID)
	BEGIN
		RETURN -1;
	END
	
	BEGIN TRY
		UPDATE Posts SET IsClosed = 1
		WHERE Id = @Id

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99;
	END CATCH

GO

