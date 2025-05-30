DECLARE @NewID INT;

EXEC sp_ManageChartOfAccounts
    @Action = 'Insert',
    @AccountID = @NewID OUTPUT,
    @AccountName = 'Test Account',
    @AccountCode = 'T123',
    @AccountTypeID = 3, 
    @ParentAccountID = NULL,
    @Description = 'Test Description',
    @IsActive = 1;

SELECT @NewID;
--for testing create procedure