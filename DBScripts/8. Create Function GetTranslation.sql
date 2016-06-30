USE BigBus

IF EXISTS 

	(SELECT 1
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[dbo].[GetTranslation]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT')
		  )
		DROP FUNCTION GetTranslation

		GO

		CREATE FUNCTION GetTranslation
		(
			@Term NVARCHAR(250),
			@Language NVARCHAR(5)
		)
		RETURNS NVARCHAR(500)
		AS
		BEGIN
			-- Declare the return variable here
			DECLARE @Translation NVARCHAR(500)

			-- Add the T-SQL statements to compute the return value here
			SELECT @Translation =Translation
			FROM dbo.tb_Phrase_Language
			where Phrase_Id = @Term AND Language_Id = @Language

			-- Return the result of the function
			RETURN @Translation

		END

GO