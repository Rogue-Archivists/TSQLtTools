/* tSQLt Unit Test */
/* more information at https://tsqlt.org/full-user-guide/ */

CREATE PROCEDURE [$classname$].[$itemname$]
AS
BEGIN
  --Assemble: mock procedure or table
    EXEC tsqlt.FakeTable 'dbo.SomeTable';

    --Put fake data in the mock table
	INSERT INTO dbo.SomeTable (baz, meh) 
	VALUES  (1, 'Ipsum'), (2, 'Lorem');		

  --Act: Run the unit under test
	SELECT baz, meh 
	INTO #Actual 
	FROM dbo.SomeTable;
  
  --Assert: recorded into the #Actual temp table.
	    
    -- Create an empty #Expected temp table that has the same structure as the #Actual table
	SELECT TOP(0) * INTO #Expected FROM #Actual;
  
    -- Add a rows to the #Expected table with the expected values.
	INSERT INTO #Expected (baz, meh) 
	VALUES  (1, 'Ipsum'), (2, 'Lorem'), (3, 'dolor'), (4, 'amet');

    -- Compare the data in the #Expected and #Actual tables
  EXEC tSQLt.AssertEqualsTable '#Expected', '#Actual';
END;