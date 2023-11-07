CREATE PROCEDURE [dbo].[GenerateRandomTurnPlayerNumber_SP]
	@MaxValue smallint,
	@RandomNumber smallint OUTPUT
AS
    DECLARE @RandomFloat FLOAT
    SET @RandomFloat = RAND()
    
    SET @RandomNumber = CAST(@RandomFloat * @MaxValue AS INT) + 1
RETURN 0