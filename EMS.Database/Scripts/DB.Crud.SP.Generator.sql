DECLARE @TableName NVARCHAR(100) = 'Employee'

DECLARE @InsertQueryTemplate NVARCHAR(MAX)

SET @InsertQueryTemplate = '
CREATE PROCEDURE ###ProcedureName###
###(Parameters)###
AS
BEGIN
SET NOCOUNT ON;

INSERT INTO ###TableName### ###Columns###
VALUES ###Values###

DECLARE @NewId INT
SET @NewId = @@Identity

SELECT @NewId;

###PostExecutionCode###
END
GO
'

DECLARE @InsertProcedureText NVARCHAR(MAX)
DECLARE @InsertQueryParameters NVARCHAR(MAX)
DECLARE @Parameters NVARCHAR(MAX)
DECLARE @Columns NVARCHAR(MAX)
DECLARE @Values NVARCHAR(MAX)
DECLARE @PostExecutionCode NVARCHAR(MAX)


SELECT *
--T.name, 
--T.object_id,
--C.TABLE_SCHEMA,
--C.COLUMN_NAME,
--C.DATA_TYPE,
--C.CHARACTER_MAXIMUM_LENGTH
--C.ORDINAL_POSITION
INTO ##Columns
FROM Sys.tables T
INNER JOIN INFORMATION_SCHEMA.COLUMNS C ON T.object_id = OBJECT_ID(C.TABLE_NAME)
WHERE name = @TableName

SET @InsertProcedureText = ''
SET @Parameters = ''
SET @Columns = ''
SET @Values = ''
SET @PostExecutionCode = ''


SET @InsertProcedureText = @InsertQueryTemplate
SET @InsertProcedureText = REPLACE(@InsertProcedureText, '###ProcedureName###', QUOTENAME(CONCAT(@TableName,'_', 'Insert' )))
SET @InsertProcedureText = REPLACE(@InsertProcedureText, '###TableName###', QUOTENAME(@TableName))
SET @InsertProcedureText = REPLACE(@InsertProcedureText, '###Colums###', @Columns)
SET @InsertProcedureText = REPLACE(@InsertProcedureText, '###Values###', @Values)
SET @InsertProcedureText = REPLACE(@InsertProcedureText, '###PostExecutionCode###', @PostExecutionCode)

SELECT @InsertProcedureText AS InsertQuery

DROP TABLE ##Columns
