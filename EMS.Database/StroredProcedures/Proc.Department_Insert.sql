CREATE PROCEDURE [dbo].[Department_Insert]
    @Name NVARCHAR(250),
    @Active BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Department
    (
        Name,
        Active
    )
    values
    (
        @Name,
        @Active
    );

    DECLARE @NewPkId INT
    SET @NewPkId = @@IDENTITY
    
    UPDATE Department
    SET Code = [dbo].[FN_Department_Generate_Code](@NewPkId)
    FROM dbo.Department Department
    WHERE id = @NewPkId;
    
    SELECT @NewPkId
END
GO