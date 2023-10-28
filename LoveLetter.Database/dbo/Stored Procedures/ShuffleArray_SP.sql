CREATE PROCEDURE [dbo].[ShuffleArray_SP] (@inputArray nvarchar(max), @shuffledArray nvarchar(max) OUTPUT)
AS
BEGIN
    DECLARE @arrayTable TABLE (Value nvarchar(max));
    DECLARE @shuffledValue nvarchar(max);
    -- Split the input string into rows
    INSERT INTO @arrayTable (Value)
    SELECT value
    FROM STRING_SPLIT(@inputArray, ' ');
    
    CREATE TABLE #ShuffledTable (SeqID INT IDENTITY(1,1), ShuffledValue nvarchar(max));

    INSERT INTO #ShuffledTable (ShuffledValue)
    SELECT Value
    FROM @arrayTable
    ORDER BY NEWID();
    SET @shuffledArray = '[';

    DECLARE @rowNum INT = 1;
    DECLARE @rowCount INT = ((SELECT COUNT(*) FROM #ShuffledTable) - 1);

    WHILE @rowNum <= @rowCount
    BEGIN
        SELECT @shuffledValue = ShuffledValue
        FROM #ShuffledTable
        WHERE SeqID = @rowNum;

		IF @rowNum = 1
		BEGIN
			SET @shuffledArray = @shuffledArray + @shuffledValue;
		END
		ELSE
		BEGIN
			SET @shuffledArray = @shuffledArray + ',' + @shuffledValue;
		END
        SET @rowNum = @rowNum + 1;
    END

	SET @shuffledArray = @shuffledArray + ']';

    DROP TABLE #ShuffledTable;
END
