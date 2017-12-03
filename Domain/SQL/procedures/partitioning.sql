
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

	SET @txt = @txt + ');'

	EXEC (@txt) 
	PRINT ('DONE: ' + @txt)
	SET @txt = N'
CREATE PARTITION SCHEME ' + @nameOfPartitionFunction + 'Scheme
AS PARTITION ' + @nameOfPartitionFunction + '
ALL TO ([PRIMARY]);'

	EXEC (@txt)
	
	PRINT ('DONE: ' + @txt)

END

IF EXISTS(SELECT * FROM sys.partition_functions WHERE name='bucketsPerDay')
	DROP PARTITION SCHEME bucketsPerDayScheme
	DROP PARTITION FUNCTION bucketsPerDay
GO

EXEC createPartitionsFromNow @nameOfPartitionFunction = 'bucketsPerDay', @numOfDays = 10
GO

(*
CREATE PARTITION FUNCTION bucketsPerDay(datetime2(0)) 
AS RANGE LEFT 
FOR VALUES 
	('2017-11-23','2017-11-24','2017-11-25','2017-11-26','2017-11-27','2017-11-28','2017-11-29','2017-11-30','2017-12-01','2017-12-02','2017-12-03');


CREATE PARTITION SCHEME bucketsPerDayScheme
AS PARTITION bucketsPerDay
ALL TO ([PRIMARY]);
*)
