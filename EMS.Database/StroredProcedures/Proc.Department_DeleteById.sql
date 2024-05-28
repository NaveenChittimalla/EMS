CREATE PROCEDURE [dbo].[Department_DeleteById] @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Department
    WHERE Id = @Id
END
GO