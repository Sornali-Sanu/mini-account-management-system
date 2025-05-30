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
