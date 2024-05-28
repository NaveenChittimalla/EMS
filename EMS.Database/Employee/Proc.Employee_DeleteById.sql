CREATE PROCEDURE [dbo].[Employee_DeleteById] @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Employee
    WHERE Id = @Id
END
GO