CREATE PROCEDURE SPCreateComment
	@UserId INT,
	@PostId INT,
	@Content varchar(500),
	@NewCommentId INT OUTPUT
AS
	IF (@Content IS NULL OR LTRIM(RTRIM(@Content)) = '')
    BEGIN
        SET @NewCommentId = 0;
        RETURN -1; -- Salir del procedimiento
    END

	IF (@UserId IS NULL OR @UserId = 0)
    BEGIN
        SET @NewCommentId = 0;
        RETURN -2; -- Salir del procedimiento
    END

	IF (@PostId IS NULL OR @PostId = 0)
    BEGIN
        SET @NewCommentId = 0;
        RETURN -3; -- Salir del procedimiento
    END

	IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        SET @NewCommentId = 0;
        RETURN -4; -- No existe el usuario
    END

	IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = @PostId)
    BEGIN
        SET @NewCommentId = 0;
        RETURN -5; -- No existe el post
    END

	BEGIN TRY
		INSERT INTO Comments
		(UserId, PostId, Content) values
		(@UserId, @PostId, @Content)

		SET @NewCommentId = SCOPE_IDENTITY();

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99;
	END CATCH

GO

CREATE PROCEDURE SPGetCommentById
	@Id INT
AS
	IF NOT EXISTS (SELECT 1 FROM Comments WHERE Id = @Id)
    BEGIN
        RETURN -1; -- No existe el post
    END

	BEGIN
		SELECT * FROM Comments WHERE Id = @Id
		RETURN 1;
	END
GO

CREATE PROCEDURE SPGetAllComments
AS
	BEGIN
		SELECT * from Comments
	END

GO

CREATE PROCEDURE SPDeleteComment
	@Id INT
AS
	IF (@Id IS NULL OR @Id = 0)
    BEGIN
        RETURN -1; -- Salir del procedimiento
    END

	IF NOT EXISTS (SELECT 1 FROM Comments WHERE Id = @Id)
    BEGIN
        RETURN -2; -- No existe el comment
    END

	BEGIN TRY
		DELETE FROM Comments WHERE Id = @Id

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -99;
	END CATCH

GO

CREATE PROCEDURE SPUpdateComment
	@Id INT,
	@Content varchar(500)
AS
	IF (@Id IS NULL OR @Id = 0)
    BEGIN
        RETURN -1; -- Salir del procedimiento
    END

	IF NOT EXISTS (SELECT 1 FROM Comments WHERE Id = @Id)
    BEGIN
        RETURN -2; -- No existe el comment
    END

	IF (@Content IS NULL OR LTRIM(RTRIM(@Content)) = '')
    BEGIN
        RETURN -3; -- Salir del procedimiento
    END

	BEGIN TRY
		UPDATE Comments
		SET Content = @Content
		WHERE Id = @Id

		RETURN 1;
	END TRY

	BEGIN CATCH
		RETURN -1;
	END CATCH

GO