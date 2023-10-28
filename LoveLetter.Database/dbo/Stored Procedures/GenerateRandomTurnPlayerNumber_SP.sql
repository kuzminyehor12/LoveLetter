CREATE PROCEDURE [dbo].[GenerateRandomTurnPlayerNumber_SP]
	@MaxValue tinyint,
	@RandomNumber tinyint
AS
    DECLARE @RandomFloat FLOAT
    SET @RandomFloat = RAND()
    
    SET @RandomNumber = CAST(@RandomFloat * @MaxValue AS INT) + 1

    SELECT @RandomNumber AS RandomNumber
RETURN 0