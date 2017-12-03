CREATE PROCEDURE partitionRotation
AS 
BEGIN
/* 
SELECT $PARTITION.bucketsPerDay([Date]) as partitionNo, 
COUNT(0) as countInPart FROM NotificationLog 
GROUP BY $PARTITION.bucketsPerDay([Date])
*/

DECLARE @dateToMerge datetime2(0)
SELECT @dateToMerge = CAST(MIN(v.value) as datetime2(0)) FROM 
	sys.partition_range_values v
	JOIN sys.partition_functions f ON f.function_id = v.function_id WHERE name='bucketsPerDay'

PRINT @dateToMerge

ALTER PARTITION FUNCTION bucketsPerDay()
MERGE RANGE(@dateToMerge);

DECLARE @dateToSplit datetime2(0) = CAST(GETUTCDATE() as date)
DECLARE @prevDateToSplit datetime2(0)
--SET @dateToSplit = DATEADD(day, 1, @dateToSplit)
--PRINT @dateToSplit

SELECT @prevDateToSplit = CAST(MAX(v.value) as datetime2(0)) FROM 
	sys.partition_range_values v
	JOIN sys.partition_functions f ON f.function_id = v.function_id WHERE name='bucketsPerDay'

--PRINT @prevDateToSplit

IF @dateToSplit > @prevDateToSplit 
BEGIN
	ALTER PARTITION FUNCTION bucketsPerDay()
	SPLIT RANGE(@dateToSplit)
	--PRINT 'splitted'
END
	
END
GO