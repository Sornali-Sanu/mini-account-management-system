CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10),
    @AccountID INT = NULL OUTPUT,
    @AccountName NVARCHAR(100) = NULL,
    @AccountCode NVARCHAR(20) = NULL,
    @AccountTypeID INT = NULL,
    @ParentAccountID INT = NULL,
    @Description NVARCHAR(500) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO Accounts (AccountName, AccountCode, AccountTypeID, ParentAccountID, Description, IsActive, CreatedDate, UpdatedDate)
        VALUES (@AccountName, @AccountCode, @AccountTypeID, @ParentAccountID, @Description, @IsActive, GETDATE(), GETDATE());

        SET @AccountID = SCOPE_IDENTITY();
    END
    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE Accounts
        SET AccountName = @AccountName,
            AccountCode = @AccountCode,
            AccountTypeID = @AccountTypeID,
            ParentAccountID = @ParentAccountID,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedDate = GETDATE()
        WHERE AccountID = @AccountID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        DELETE FROM Accounts WHERE AccountID = @AccountID;
    END
END



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

GO
CREATE PROCEDURE GrantModuleAccess
    @RoleName NVARCHAR(100),
    @ModuleName NVARCHAR(100)
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM RoleModuleAccess WHERE RoleName = @RoleName AND ModuleName = @ModuleName
    )
    BEGIN
        UPDATE RoleModuleAccess
        SET HasAccess = 1
        WHERE RoleName = @RoleName AND ModuleName = @ModuleName;
    END
    ELSE
    BEGIN
        INSERT INTO RoleModuleAccess (RoleName, ModuleName, HasAccess)
        VALUES (@RoleName, @ModuleName, 1);
    END
END

GO

CREATE PROCEDURE RevokeModuleAccess
    @RoleName NVARCHAR(100),
    @ModuleName NVARCHAR(100)
AS
BEGIN
    UPDATE RoleModuleAccess
    SET HasAccess = 0
    WHERE RoleName = @RoleName AND ModuleName = @ModuleName;
END
GO
Create PROCEDURE CheckModuleAccess
    @RoleName NVARCHAR(100),
    @ModuleName NVARCHAR(100)
AS
BEGIN
    SELECT Id, RoleName, ModuleName, HasAccess
    FROM RoleModuleAccess
    WHERE RoleName = @RoleName AND ModuleName = @ModuleName;
END