# SqlServerAggregates

This project contains code to create .NET CLR for the purpose of creating two SQL Server aggregate functions, the median function and the Median Absolute Deviation function, a sort of small sample version of Mean and Standard Deviation. If the project is compiled into a StatsPackage.dll file, you can use it to create assembly on the desired database, using SSMS's GUI menus or using a "CREATE ASSEMBLY.." command (if you have access to the file system where the server is running). Once the assembly exists on the database, then run

CREATE AGGREGATE dbo.MEDIAN(@input FLOAT )
       RETURNS FLOAT
       EXTERNAL NAME [StatsPackage].[Median]
      
CREATE AGGREGATE dbo.MAD(@input FLOAT)
       RETURNS FLOAT
       EXTERNAL NAME [StatsPackage].[MedianAbsoluteDeviation]
       
If StatsPackage is the name of the assembly created there. Also, the command "sp_configure 'clr enabled', 1" needs to be run, if you have permissions to do so.
