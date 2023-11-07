CREATE PROCEDURE [dbo].[EnumeratePlayers_SP]
    @InputNVarChar NVARCHAR(MAX),
    @Delimiter NVARCHAR(1),
    @OutputNVarChar NVARCHAR(MAX) OUTPUT,
	@ElementCount INT OUTPUT
AS
BEGIN
    DECLARE @ResultXML XML = '<Players></Players>';

    DECLARE @Index INT = 1;
    DECLARE @Token NVARCHAR(MAX);

	SET @ElementCount = 0

    WHILE LEN(@InputNVarChar) > 0
    BEGIN
        IF CHARINDEX(@Delimiter, @InputNVarChar) = 0
        BEGIN
            SET @Token = @InputNVarChar;
            SET @InputNVarChar = N'';
        END
        ELSE
        BEGIN
            SET @Token = LEFT(@InputNVarChar, CHARINDEX(@Delimiter, @InputNVarChar) - 1);
            SET @InputNVarChar = SUBSTRING(@InputNVarChar, CHARINDEX(@Delimiter, @InputNVarChar) + 1, LEN(@InputNVarChar));
        END

        -- Append a Player element to the XML
        SET @ResultXML.modify('
            insert
            <Player>
                <PlayerNickname>{sql:variable("@Token")}</PlayerNickname>
                <PlayerNumber>{sql:variable("@Index")}</PlayerNumber>
            </Player>
            into (/Players)[1]');

        SET @Index = @Index + 1;
		SET @ElementCount = @ElementCount + 1
    END

    SET @OutputNVarChar = CAST(@ResultXML AS NVARCHAR(MAX));
END





