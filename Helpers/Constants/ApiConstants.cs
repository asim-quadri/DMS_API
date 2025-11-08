namespace ComplianceAPI.Helpers.Constants
{
    public static class ApiConstants
    {
        // HTML Templates (bold formatted)
        public const string NewStateBySuperAdminNotificationTitleTemplate = "<b>New State</b> <b>{0}</b> added and approved successfully.";
        public const string NewStateBySuperAdminNotificationMessageTemplate = "<b>{0}</b> has added a <b>New State</b> named <b>{1}</b> and it requires your approval.";

        public const string NewStateNotificationTitleTemplate = "<b>{0}</b> added <b>New State</b> <b>{1}</b>";
        public const string NewStateNotificationMessageTemplate = "<b>New State</b> named <b>{0}</b> has been added and approved successfully.";

        public const string RoleUpdationBySuperAdminNotificationTitleTemplate = "<b>New Role</b> <b>{0}</b> updated and approved successfully.";
        public const string RoleUpdationBySuperAdminNotificationMessageTemplate = "<b>New Role</b> <b>{0}</b> updated and approved successfully.";

        public const string RoleUpdationNotificationTitleTemplate = "<b>{0}</b> updated <b>New Role</b> <b>{1}</b>";
        public const string RoleUpdationNotificationMessageTemplate = "<b>{0}</b> has updated a <b>New Role</b> named <b>{1}</b> and it requires your approval.";

        public const string NewCountryBySuperAdminNotificationTitleTemplate = "<b><b>New Country</b> <b>{0}</b> added and approved successfully.";
        public const string NewCountryBySuperAdminNotificationMessageTemplate = "<b>New Country</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewCountryNotificationTitleTemplate = "<b>{0}</b> added <b>New Country</b> <b>{1}</b>";
        public const string NewCountryNotificationMessageTemplate = "<b>{0}</b> has added a <b>New Country</b> named <b>{1}</b> and it requires your approval.";

        public const string NewOrganizationNotificationTitleTemplate = "<b>{0}</b> added <b>New Oranization</b> <b>{1}</b>";
        public const string NewOrganizationNotificationMessageTemplate = "<b>{0}</b> has added a <b>New Organization</b> named <b>{1}</b> and it requires your approval.";

        public const string NewOrganizationBySuperAdminNotificationTitleTemplate = "<b>New Oranization</b> <b>{0}</b> added and approved successfully.";
        public const string NewOrganizationBySuperAdminNotificationMessageTemplate = "<b>New Organization</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewEntityNotificationTitleTemplate = "<b>{0}</b> added <b>New Entity</b> <b>{1}</b>";
        public const string NewEntityNotificationMessageTemplate = "<b>{0}</b> has added a <b>New Entity</b> named <b>{1}</b> and it requires your approval.";

        public const string NewEntityBySuperAdminNotificationTitleTemplate = "<b>New Entity</b> <b>{0}</b> added and approved successfully.";
        public const string NewEntityBySuperAdminNotificationMessageTemplate = "<b>New Entity</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewUserNotificationTitleTemplate = "<b>{0}</b> added <b>New User</b> <b>{1}</b>";
        public const string NewUserNotificationMessageTemplate = "<b>{0}</b> has added a <b>New User</b> named <b>{1}</b> and it requires your approval.";

        public const string NewUserBySuperAdminrNotificationTitleTemplate = "<b>New User</b> <b>{0}</b> added and approved successfully.";
        public const string NewUserBySuperAdminNotificationMessageTemplate = "<b>New User</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewCountryStateMappingBySuperAdminNotificationTitleTemplate = "State <b>{0}</b> mapped and approved successfully.";
        public const string NewCountryStateMappingBySuperAdminNotificationMessageTemplate = "Mapped the state <b>{0}</b> approved successfully.";

        public const string NewCountryStateMappingNotificationTitleTemplate = "<b>{0}</b> requested to map <b>{1}</b> to <b>{2}</b>";
        public const string NewCountryStateMappingNotificationMessageTemplate = "A new request has been submitted by <b>{0}</b> to map the state <b>{1}</b> to the country <b>{2}</b>. Please review and approve the mapping.";

        public const string NewMajorIndustryBySuperAdminrNotificationTitleTemplate = "<b>New Major Industry</b> <b>{0}</b> added and approved successfully.";
        public const string NewMajorIndustryBySuperAdminNotificationMessageTemplate = "<b>New Major Industry</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewMajorIndustryNotificationTitleTemplate = "<b>{0}</b> added <b>New Major Industry</b> <b>{1}</b>";
        public const string NewMajorIndustryNotificationMessageTemplate = "<b>{0}</b> has added a <b>New Major Industry</b> named <b>{1}</b> and it requires your approval.";

        public const string NewMinorIndustryBySuperAdminrNotificationTitleTemplate = "<b>New Minor Industry</b> <b>{0}</b> added and approved successfully.";
        public const string NewMinorIndustryBySuperAdminNotificationMessageTemplate = "<b>New Minor Industry</b> named <b>{0}</b> has been added and approved successfully.";

        public const string NewMinorIndustryNotificationTitleTemplate = "<b>{0}</b> added <b>New Minor Industry</b> <b>{1}</b>";
        public const string NewMinorIndustryNotificationMessageTemplate = "<b>{0}</b> has added a <b>New Minor Industry</b> named <b>{1}</b> and it requires your approval.";

        public const string CountryMajorMappingBySuperAdminNotificationTitleTemplate = "Minor industry <b>{0}</b> mapped and approved successfully.</b>";
        public const string CountryMajorMappingBySuperAdminNotificationMessageTemplate = "Mapped the minor industry <b>{0}</b> and approved successfully.";

        public const string CountryMajorMappingNotificationTitleTemplate = "<b>{0}</b> requested to map the minor industry <b>{1}</b>";
        public const string CountryMajorMappingNotificationMessageTemplate = "A new request has been submitted by <b>{0}</b> to map the minor industry <b>{1}</b>. Please review and approve the mapping.";

        public const string NewTOBBySuperAdminTitleTemplate = "<b>Branch Type</b> <b>{0}</b> added and approved successfully.";
        public const string NewTOBBySuperAdminMessageTemplate = "<b>Branch Type</b> {0} added and approved successfully.";

        public const string NewTOBTitleTemplate = "<b>{0}</b> added new <b>Branch Type</b> <b>{1}</b>";
        public const string NewTOBMessageTemplate = "<b>{0}</b> added new branch type <b>{1}</b> and requested for approval.";

        public const string TOBMappingBySuperAdminNotificationTitleTemplate = "<b>TOB</b> <b>{0}</b> mapped and approved successfully.";
        public const string TOBMappingBySuperAdminNotificationMessageTemplate = "<b>TOB</b> <b>{0}</b> mapped and approved successfully.";

        public const string TOBMappingNotificationTitleTemplate = "<b>{0}</b> requested to map the TOB <b>{1}</b>";
        public const string TOBMappingNotificationMessageTemplate = "A new request has been submitted by <b>{0}</b> to map the TOB <b>{1}</b>. Please review and approve the mapping.";


        public const string NewEntityTypeBySuperAdminTitleTemplate = "<b>Entity Type</b> <b>{0}</b> added and approved successfully.";
        public const string NewEntityTypeBySuperAdminMessageTemplate = "<b>Entity Type</b> {0} added and approved successfully.";

        public const string NewEntityTypeTitleTemplate = "<b>{0}</b> added new <b>Entity Type</b> <b>{1}</b>";
        public const string NewEntityTypeMessageTemplate = "<b>{0}</b> added new entity type <b>{1}</b> and requested for approval.";

        public const string NewEntityMappingBySuperAdminNotificationTitleTemplate = "<b>Entity Type</b> <b>{0}</b> mapped and approved successfully.";
        public const string NewEntityMappingBySuperAdminNotificationMessageTemplate = "<b>Entity Type</b> <b>{0}</b> mapped and approved successfully.";

        public const string NewEntityMappingNotificationTitleTemplate = "<b>{0}</b> requested to map the Entity Type <b>{1}</b>";
        public const string NewEntityMappingNotificationMessageTemplate = "A new request has been submitted by <b>{0}</b> to map the Entity Type <b>{1}</b>. Please review and approve the mapping.";

        //Parameter
        public const string AddParameterTitleTemplate = "<b>{0}</b> added <b>{1}</b>";
        public const string AddParameterTitleAdminTemplate= "<b>{0}</b> added <b> New {1}</b> and approved";

        public const string AddParameterMessageTemplate = "A new {0} parameter added by {1}";

        public const string NewRegulationGroupBySuperAdminNotificationTitleTemplate = "<b>New Regulation Group</b> <b>{0}</b> added and approved successfully.";
        public const string NewRegulationGroupBySuperAdminNotificationMessageTemplate = "<b>New Regulation Group</b> <b>{0}</b> added and approved successfully.";

        public const string NewRegulationGroupNotificationTitleTemplate = "<b>{0}</b> added <b>New Regulation Group</b>";
        public const string NewRegulationGroupNotificationMessageTemplate = "<b>{0}</b> added <b>New Regulation Group</b> and requested for approval.";

        public const string NewRegulationGroupByMappingSuperAdminNotificationTitleTemplate = "<b>New Regulation Group</b> <b>{0}</b> mapped and approved successfully.";
        public const string NewRegulationGroupByMappingSuperAdminNotificationMessageTemplate = "<b>New Regulation Group</b> <b>{0}</b> mapped and approved successfully.";

        public const string NewRegulationGroupMappingNotificationTitleTemplate = "<b>{0}</b> mapped <b>New Regulation Group</b> <b>{1}</b>";
        public const string NewRegulationGroupMappingNotificationMessageTemplate = "<b>{0}</b> mapped <b>New Regulation Group</b> <b>{1}</b> and requested for approval.";

        public const string NewRegulationSetupBySuperAdminNotificationTitleTemplate = "<b>New Regulation Setup</b> <b>{0}</b> added and approved successfully.";
        public const string NewRegulationSetupBySuperAdminNotificationMessageTemplate = "<b>New Regulation Setup</b> <b>{0}</b> added and approved successfully.";

        public const string NewRegulationSetupNotificationTitleTemplate = "<b>{0}</b> added <b>New Regulation Setup</b> <b>{1}</b>";
        public const string NewRegulationSetupNotificationMessageTemplate = "<b>{0}</b> added <b>New Regulation Setup</b> <b>{1}</b> and requested for approval.";
    
        public const string NewStateApprovedNotificationTitleTemplate = "<b>{0}</b> approved a <b>New State</b>";
        public const string NewStateApprovedNotificationMessageTemplate = "<b>{0}</b> approved the <b>New State</b>";
        public const string NewStateRejectNotificationTitleTemplate = "<b>{0}</b> rejected the <b>New State</b>";
        public const string NewStateRejectNotificationMessageTemplate = "<b>{0}</b> rejected the <b>New State</b>";
        public const string NewStateForwardNotificationTitleTemplate = "<b>{0}</b> forwarded the <b>New State</b>";
        public const string NewStateForwardNotificationMessageTemplate = "<b>{0}</b> forwarded the <b>New State</b>";

        public const string NewCountryApprovedNotificationTitleTemplate = "<b>{0}</b> approved a <b>New Country</b>";
        public const string NewCountryApprovedNotificationMessageTemplate = "<b>{0}</b> approved a <b>New Country</b>";
        public const string NewCountryRejectNotificationTitleTemplate = "<b>{0}</b> rejected the <b>New Country</b>";
        public const string NewCountryRejectNotificationMessageTemplate = "<b>{0}</b> rejected the <b>New Country</b>";
        public const string NewCountryForwardNotificationTitleTemplate = "<b>{0}</b> forwarded the <b>New Country</b>";
        public const string NewCountryForwardNotificationMessageTemplate = "<b>{0}</b> forwarded the <b>New Country</b>";

        public const string NewCountryStateMappingApprovedNotificationTitleTemplate = "<b>{0}</b> approved a <b>New Country Mapping</b>";
        public const string NewCountryStateMappingApprovedNotificationMessageTemplate = "<b>{0}</b> approved a <b>New Country Mapping</b>";
        public const string NewCountryStateMappingRejectNotificationTitleTemplate = "<b>{0}</b> rejected the <b>New Country Mapping</b>";
        public const string NewCountryStateMappingRejectNotificationMessageTemplate = "<b>{0}</b> rejected the <b>New Country Mapping</b>";
        public const string NewCountryStateMappingForwardNotificationTitleTemplate = "<b>{0}</b> forwarded the <b>New Country Mapping</b>";
        public const string NewCountryStateMappingForwardNotificationMessageTemplate = "<b>{0}</b> forwarded the <b>New Country Mapping</b>";

        public const string NewMajorIndustryApprovedNotificationTitleTemplate = "<b>{0}</b> approved a  <b>New Major Industry</b>.";
        public const string NewMajorIndustryApprovedNotificationMessageTemplate = "<b>{0}</b> approved a  <b>New Major Industry</b>.";
        public const string NewMajorIndustryRejectedNotificationTitleTemplate = "<b>{0}</b> rejected the  <b>New Major Industry</b>.";
        public const string NewMajorIndustryRejectedNotificationMessageTemplate = "<b>{0}</b> rejected the  <b>New Major Industry</b>.";
        public const string NewMajorIndustryForwardedNotificationTitleTemplate = "<b>{0}</b> forwarded the  <b>New Major Industry</b>.";
        public const string NewMajorIndustryForwardedNotificationMessageTemplate = "<b>{0}</b> forwarded the  <b>New Major Industry</b>.";

        public const string NewMinorIndustryApprovedNotificationTitleTemplate = "<b>{0}</b> approved a  <b>New Minor Industry</b>.";
        public const string NewMinorIndustryApprovedNotificationMessageTemplate = "<b>{0}</b> approved a  <b>New Minor Industry</b>.";
        public const string NewMinorIndustryRejectedNotificationTitleTemplate = "<b>{0}</b> rejected the  <b>New Minor Industry</b>.";
        public const string NewMinorIndustryRejectedNotificationMessageTemplate = "<b>{0}</b> rejected the  <b>New Minor Industry</b>.";
        public const string NewMinorIndustryForwardedNotificationTitleTemplate = "<b>{0}</b> forwarded the  <b>New Minor Industry</b>.";
        public const string NewMinorIndustryForwardedNotificationMessageTemplate = "<b>{0}</b> forwarded the  <b>New Minor Industry</b>.";

        public const string NewMinorIndustryMappingApprovedNotificationTitleTemplate = "<b>{0}</b> approved a  <b>New Minor Industry Mapping</b>.";
        public const string NewMinorIndustryMappingApprovedNotificationMessageTemplate = "<b>{0}</b> approved a  <b>New Minor Industry Mapping</b>.";
        public const string NewMinorIndustryMappingRejectedNotificationTitleTemplate = "<b>{0}</b> rejected the  <b>New Minor Industry Mapping</b>.";
        public const string NewMinorIndustryMappingRejectedNotificationMessageTemplate = "<b>{0}</b> rejected the  <b>New Minor Industry Mapping</b>.";

        public const string EntityTypeApprovedNotificationTitleTemplate = "<b>{0}</b> approved a <b>New Entity Type</b>.";
        public const string EntityTypeApprovedNotificationMessageTemplate = "<b>{0}</b> approved a <b>New Entity Type</b>.";
        public const string EntityTypeRejectedNotificationTitleTemplate = "<b>{0}</b> rejected the <b>New Entity Type</b>.";
        public const string EntityTypeRejectedNotificationMessageTemplate = "<b>{0}</b> rejected the <b>New Entity Type</b>.";
        public const string EntityTypeForwardedNotificationTitleTemplate = "<b>{0}</b> forwarded the <b>New Entity Type</b>.";
        public const string EntityTypeForwardedNotificationMessageTemplate = "<b>{0}</b> forwarded the <b>New Entity Type</b>.";

        public const string EntityTypeMappingApprovedNotificationTitleTemplate = "<b>{0}</b> approved a <b>New Entity Type Mapping</b>.";
        public const string EntityTypeMappingApprovedNotificationMessageTemplate = "<b>{0}</b> approved a <b>New Entity Type Mapping</b>.";
        public const string EntityTypeMappingRejectedNotificationTitleTemplate = "<b>{0}</b> rejected the <b>New Entity Type Mapping</b>.";
        public const string EntityTypeMappingRejectedNotificationMessageTemplate = "<b>{0}</b> rejected the <b>New Entity Type Mapping</b>.";
        public const string EntityTypeMappingForwardedNotificationTitleTemplate = "<b>{0}</b> forwarded the <b>New Entity Type Mapping</b>.";
        public const string EntityTypeMappingForwardedNotificationMessageTemplate = "<b>{0}</b> forwarded the <b>New Entity Type Mapping</b>.";












    }
}