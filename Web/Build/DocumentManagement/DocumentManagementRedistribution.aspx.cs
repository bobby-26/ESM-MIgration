using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Web.UI;

public partial class DocumentManagementRedistribution : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddButton("Enable Quality DT", "ENABLEQUALITYDT");
            toolbargrid.AddButton("Reset Seal Register", "RESETSEALAUDIT");
            //toolbargrid.AddButton("Reset Bulk PO Seal", "RESETBULKPOSEALAUDIT");
            toolbargrid.AddButton("Reset JHA Audit", "RESETJHAAUDIT");
            toolbargrid.AddButton("Reset RA Audit", "RESETRAAUDIT");
            //toolbargrid.AddButton("Redistribute JHA", "REDISTRIBUTEJHA");
            //toolbargrid.AddButton("Reset RA Distribution", "RESETRADISTRIBUTION");
            //toolbargrid.AddButton("Redistribute RA", "REDISTRIBUTERA");
            //toolbargrid.AddButton("Reset audit for module", "RESETMODULEAUDIT");
            //toolbargrid.AddButton("Send Seal mgmt data", "SENDSEALDATA");
            //toolbargrid.AddButton("Distribute Offshore Ins", "DISTRIBUTEOFFSHORE");
            //toolbargrid.AddButton("Revert Seals", "REVERTSEALISSUE");
            toolbargrid.AddButton("Send RA activity", "RAACTIVITY");
            toolbargrid.AddButton("Enable DT for WRH Lock", "ENABLEDTWRHLOCK");
            //toolbargrid.AddButton("Send LongTerm WO", "LONGTERMWO");
            //toolbargrid.AddButton("Send Preventive Action", "SENDPA");
            toolbargrid.AddButton("Update Seal Status", "UPDATESEALSTATUS");
            toolbargrid.AddButton("Recover Inspection Data", "RECOVERINSPECTIONDATA");
            toolbargrid.AddButton("Resend Registers Data", "RESENDREGISTERSDATA");
            toolbargrid.AddButton("Delete audit recovery data", "DELETEAUDITRECOVERY");
            //toolbargrid.AddButton("Update Vessel DT", "UPDATEDTVESSELS");
            //toolbargrid.AddButton("Send Seal Req", "SENDSEALREQ");
            //toolbargrid.AddButton("Delete Orphaned Tasks", "DELETEORPHANEDTASKS");
            //toolbargrid.AddButton("Update VesselList for DT", "UPDATEDTVESSELLIST");
            //toolbargrid.AddButton("Update Incident records", "UPDATEINCIDENTRECORDEDBY");
            //toolbargrid.AddButton("Delete seal duplicates", "DELETEDUPLICATES");
            //toolbargrid.AddButton("JHA/RA for offshore", "JHARA");
            //toolbargrid.AddButton("Update Process RA", "UPDATEDISTRIBUTEDRA");
            MenuRedistribution.AccessRights = this.ViewState;
            MenuRedistribution.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRedistribution_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("RESETJHAAUDIT"))
            {
                //PhoenixDocumentManagementDistribution.ResetJHADistribution();
                //PhoenixDocumentManagementDistribution.RedistributeJHA();
                string sql = "";
                sql = " EXEC PRDMSJHADISTRIBUTIONRESET";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "JHA audit reset is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("REDISTRIBUTEJHA"))
            {
                string sql = "";
                sql = " EXEC PRDMSJHAREDISTRIBUTE @ROWUSERCODE =" + PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "JHAs are redistributed successfully.";
            }
            else if (dce.CommandName.ToUpper().Equals("RESETRAAUDIT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                //PhoenixDocumentManagementDistribution.ResetRADistribution(General.GetNullableString(txtVessellist.Text));
                string sql = "";
                sql = " EXEC PRDMSRADISTRIBUTIONRESET @VESSELID=" + int.Parse(txtVessellist.Text);
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "RA audit reset is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("REDISTRIBUTERA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                //PhoenixDocumentManagementDistribution.RedistributeRA(General.GetNullableString(txtVessellist.Text),
                //    int.Parse(ucMappedVesselTypeid.Text));
                string sql = "";
                sql = " EXEC PRDMSRAREDISTRIBUTE @ROWUSERCODE =" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + ",@VESSELLIST='" + txtVessellist.Text + "'";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Redistribution of RAs is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("UPDATEDISTRIBUTEDRA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSRADISTRIBUTEDPROCESSUPDATE @ROWUSERCODE =" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + ",@VESSELLIST='" + txtVessellist.Text + "'";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "RA updates are sent successfully.";
            }
            else if (dce.CommandName.ToUpper().Equals("ENABLEQUALITYDT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=1";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Quality DT enabling is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RESETSEALAUDIT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=2";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Seal audit reset is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RESETBULKPOSEALAUDIT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=3";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Bulk PO Seal audit reset is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RESETMODULEAUDIT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=4";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Module Audit reset is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("SENDSEALDATA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=5";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Sending seal mgmt data is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("DISTRIBUTEOFFSHORE"))
            {
                string sql = "";
                sql = " EXEC PRINSPECTIONOFFSHOREDISTRIBUTE @ROWUSERCODE=1,@OFFSHORECOMPANYID=19";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Offshore inspection distribution is initiated.";
            }
            else if(dce.CommandName.ToUpper().Equals("JHARA"))
            {
                string sql = "";
                sql = " EXEC PRRISKASSESSMENTJHA @ROWUSERCODE=1";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "JHA/RA reference id updation is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("LONGTERMWO"))
            {
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + General.GetNullableString(txtVessellist.Text) + "',@TYPE=7";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Sending long term WO data is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("SENDSEALREQ"))
            {
                if (!IsValidSealReqInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + General.GetNullableString(txtVessellist.Text) + "',@TYPE=8,@SEALREQNO='" + General.GetNullableString(txtSealReqNo.Text) + "'";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Sending Seal Requisition is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("DELETEORPHANEDTASKS"))
            {
                string sql = "";
                sql = " EXEC PRINSPECTIONORPHANEDTASKSDELETE @ROWUSERCODE=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Deleting orphaned task is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("UPDATEDTVESSELLIST"))
            {
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=9";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Updating vessel list for DT for new tables is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("SENDPA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + General.GetNullableString(txtVessellist.Text) + "',@TYPE=10";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Sending Preventive actions data is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("UPDATEINCIDENTRECORDEDBY"))
            {
                string sql = "";
                sql = " EXEC PRINSPECTIONINCIDENTRECORDEDBYUPDATE @ROWUSERCODE=1";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Updating incident records is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("DELETEDUPLICATES"))
            {
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + General.GetNullableString(txtVessellist.Text) + "',@TYPE=11";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Deleting seal duplicates is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("UPDATESEALSTATUS"))
            {
                if (!IsValidSealInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string sql = "";
                sql = " EXEC PRSEALSTATUSUPDATE @ROWUSERCODE=1,@SEALNOS='" + General.GetNullableString(txtSealNo.Text) + "',@STATUS=" + General.GetNullableInteger(txtStatus.Text);
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Updating seal status is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RAACTIVITY"))
            {
                PhoenixDocumentManagementDistribution.SendInspectionData(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableString(txtVessellist.Text), 1);
                ucStatus.Text = "RA activity is sent";
            }
            else if (dce.CommandName.ToUpper().Equals("ENABLEDTWRHLOCK"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementDistribution.SendInspectionData(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableString(txtVessellist.Text), 2);
                ucStatus.Text = "DT enabled for WRH lock table";
            }
            else if (dce.CommandName.ToUpper().Equals("UPDATEDTVESSELS"))
            {
                string sql = "";
                sql = " EXEC PRDMSENABLEQUALITYDT @VESSELLIST='" + txtVessellist.Text + "',@TYPE=12";
                PhoenixDocumentManagementDistribution.DMSDistributionJobInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sql);
                ucStatus.Text = "Quality DT Vessels updation is initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RECOVERINSPECTIONDATA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementDistribution.RecoverInspectionData(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVessellist.Text));
                ucStatus.Text = "Inspection data recovery initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("RESENDREGISTERSDATA"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementDistribution.ResendInspectionRegistersData(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVessellist.Text));
                ucStatus.Text = "Inspection registers data recovery initiated.";
            }
            else if (dce.CommandName.ToUpper().Equals("DELETEAUDITRECOVERY"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementDistribution.DeleteAuditDataRecovery(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVessellist.Text));
                ucStatus.Text = "Recovery data audit delete is initiated.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtVessellist.Text) == null)
            ucError.ErrorMessage = "Vessel is Required.";
        //if (General.GetNullableInteger(ucMappedVesselTypeid.Text) == null)
        //    ucError.ErrorMessage = "Mapped Vessel type id is required.";

        return (!ucError.IsError);
    }

    private bool IsValidSealReqInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtSealReqNo.Text) == null)
            ucError.ErrorMessage = "Seal Requisition Number is Required.";

        return (!ucError.IsError);
    }

    private bool IsValidSealInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtSealNo.Text) == null)
            ucError.ErrorMessage = "Seal Number is Required.";

        if (General.GetNullableInteger(txtStatus.Text) == null)
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }
}
