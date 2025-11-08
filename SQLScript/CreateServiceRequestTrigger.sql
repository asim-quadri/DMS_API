CREATE TRIGGER trg_ServiceRequestToComplianceClient
ON [client].[ServiceRequest]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE c
    SET 
        c.ExpectedDate = i.ExpectedDate,
        c.LevelMasterId = i.LevelMasterId,
        c.ModifiedBy = i.ModifiedBy,
        c.ModifiedOn = i.ModifiedOn,
        c.Comments=i.Comments
    FROM Compliance_Client.client.ServiceRequest c
    INNER JOIN inserted i ON c.Id = i.Id;
END
GO