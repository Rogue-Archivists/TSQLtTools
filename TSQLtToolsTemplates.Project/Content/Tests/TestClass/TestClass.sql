/* Test Class [$itemname$] for tSQLt  */

CREATE SCHEMA [$itemname$]
	AUTHORIZATION [dbo];
GO

--EXEC tSQLt.NewTestClass '$itemname$';  -- can't be used with SSDT so do it manually
EXECUTE sp_addextendedproperty 
	@level0type = N'SCHEMA', @level0name = N'$itemname$',
	@name = N'tSQLt.TestClass', @value = 1;
GO
