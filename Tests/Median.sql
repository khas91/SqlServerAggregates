CREATE AGGREGATE [dbo].[Aggregate1]
	(@param1 nvarchar(4000))
	RETURNS nvarchar(4000)
	EXTERNAL NAME SomeAssembly.SomeType
