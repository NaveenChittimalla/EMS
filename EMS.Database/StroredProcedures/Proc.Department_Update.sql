CREATE PROCEDURE [dbo].[Department_Update_Details] 
    @Id INT,
    @Name NVARCHAR(250),
    @Active BIT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE Department
  SET Name = @Name,
      Active = @Active
  FROM dbo.Department Department
  WHERE Id = @Id

END
GO
