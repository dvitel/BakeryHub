
IF EXISTS(SELECT * FROM sys.procedures WHERE name='createPartitionsFromNow')
DROP PROCEDURE createPartitionsFromNow
GO

CREATE PROCEDURE createPartitionsFromNow (
	@nameOfPartitionFunction nvarchar(max),
	@numOfDays int)
AS
BEGIN
	DECLARE @txt nvarchar(max) = 
		N'
CREATE PARTITION FUNCTION ' + @nameOfPartitionFunction + '(datetime2(0)) 
AS RANGE LEFT 
FOR VALUES 
	('
	DECLARE @now date = GETUTCDATE();
	WITH Gen(n) As
	(
		SELECT @numOfDays
		UNION ALL
		SELECT n-1 FROM Gen WHERE n > 0
	)
	SELECT @txt = @txt + '''' + CAST(DATEADD(day, -n, @now) as nvarchar(max)) + (CASE WHEN n = 0 THEN '''' ELSE ''',' END) FROM Gen

	SET @txt = @txt + ')'

	SET @txt = @txt + N'

CREATE PARTITION SCHEME ' + @nameOfPartitionFunction + 'Scheme
AS PARTITION ' + @nameOfPartitionFunction + '
ALL TO ([PRIMARY]);'	
	EXEC (@txt) 
	PRINT ('DONE: ' + @txt)

END

IF EXISTS(SELECT * FROM sys.partition_functions WHERE name='bucketsPerDay')
	DROP PARTITION FUNCTION bucketsPerDay
GO

IF EXISTS(SELECT * FROM sys.partition_schemes WHERE name='bucketsPerDayScheme')
	DROP PARTITION SCHEME bucketsPerDayScheme
GO

EXEC createPartitionsFromNow @nameOfPartitionFunction = 'bucketsPerDay', @numOfDays = 10
GO