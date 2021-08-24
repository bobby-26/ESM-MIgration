using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Net.Mail;

public partial class CommonApproval : PhoenixBasePage
{
    string strDocumentId = string.Empty;
    string strApprovalType = string.Empty;
    string csvUserCode = string.Empty;
    string strVesselId = string.Empty;
    string empid = string.Empty;
    string offsignerid = string.Empty;
    string ifr = string.Empty;
    string pdtype = string.Empty;
    string subtype = string.Empty;
    public PhoenixModule mod;
    NameValueCollection nvc;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["reqid"] != null)
            {
                nvc = MakeNameValueCollection(new Guid(Request.QueryString["reqid"]));
            }
            strDocumentId = Request.QueryString["docid"];
            strApprovalType = Request.QueryString["type"];
            if (Request.QueryString["user"] != null)
                csvUserCode = Request.QueryString["user"];
            if (Request.QueryString["vslid"] != null)
                strVesselId = Request.QueryString["vslid"];
            if (Request.QueryString["empid"] != null)
                empid = Request.QueryString["empid"];
            if (Request.QueryString["offsignerid"] != null)
                offsignerid = Request.QueryString["offsignerid"];
            if (Request.QueryString["ifr"] != null)
                ifr = "0";
            if (Request.QueryString["pdtype"] != null)
                pdtype = Request.QueryString["pdtype"];
            if (Request.QueryString["subtype"] != null)
                subtype = Request.QueryString["subtype"].ToString();
            mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
            EmployeeInfo.Visible = false;
            if (Request.QueryString["type"] == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA") || Request.QueryString["type"] == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA"))
                lblOnbehalfProceedRemarks.Visible = false;
            if (!IsPostBack)
            {
                if (mod == PhoenixModule.PURCHASE)
                {
                    ddlStatus.ShortNameFilter = "APP";
                    ddlStatus.SelectedHard = "187"; //approved
                }
                if (mod == PhoenixModule.ACCOUNTS)
                {
                    ddlStatus.ShortNameFilter = "APP,APY";
                    if (Request.QueryString["vouchertype"] != null && Request.QueryString["vouchertype"] != "")
                    {
                        ViewState["vouchertype"] = Request.QueryString["vouchertype"];
                    }
                    if (Request.QueryString["suppliercode"] != null && Request.QueryString["suppliercode"] != "")
                    {
                        ViewState["suppliercode"] = Request.QueryString["suppliercode"];
                    }
                    if (Request.QueryString["directpoapproval"] != null && Request.QueryString["directpoapproval"] != "")
                    {
                        ViewState["directpoapproval"] = Request.QueryString["directpoapproval"];
                        ddlStatus.ShortNameFilter = "APP";
                    }
                    if (subtype != null && subtype.ToUpper().ToString().Equals("DIRECTPO"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                    }
                    ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
                }

                tdProceed.Visible = false;
                tblComments.Visible = false;
                lblNote.Visible = false;
                RadComboBox status = (RadComboBox)ddlStatus.FindControl("ddlHard");
                status.Focus();
                ViewState["ATTACHMENTCODE"] = "";
                ViewState["ISRATING"] = "";
                ViewState["ALLAPPROVED"] = 0;
                ddlOnbehalf.DataSource = PhoenixCommonApproval.ListApprovalOnbehalf(int.Parse(strApprovalType), csvUserCode, General.GetNullableInteger(strVesselId));
                ddlOnbehalf.DataBind();
                if (mod == PhoenixModule.CREW)
                {
                    if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRS") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRJ"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                        ViewState["CDTKEY"] = strDocumentId;
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRS") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRJ"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                        ViewState["CDTKEY"] = strDocumentId;
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDS") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDJ"))
                    {
                        ddlStatus.ShortNameFilter = "APP";
                    }
                    else
                    {
                        ViewState["CDTKEY"] = PhoenixCrewPlanning.EditCrewPlan(new Guid(strDocumentId)).Rows[0]["FLDDTKEY"].ToString();
                        ViewState["ATTACHMENTCODE"] = PhoenixCrewPlanning.EditCrewPlan(new Guid(strDocumentId)).Rows[0]["FLDATTACHMENTCODE"].ToString();
                    }
                }

                if (mod == PhoenixModule.QUALITY)
                {
                    if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OMA"))
                    {
                        ucTitle.Text = "Office Manual Task Approval";
                        ddlStatus.ShortNameFilter = "APP";
                        txtRemarks.Text = "";
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "TPA"))
                    {
                        ucTitle.Text = "Reschedule Task Primary Approval";
                        ddlStatus.ShortNameFilter = "APP";
                        txtRemarks.Text = "";
                    }
                    else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "TSA"))
                    {
                        ucTitle.Text = "Reschedule Task Secondary Approval";
                        ddlStatus.ShortNameFilter = "APP";
                        txtRemarks.Text = "";
                    }
                    else
                    {
                        ucTitle.Text = "Review & Close";
                        ddlStatus.ShortNameFilter = "APP";
                        txtRemarks.Text = "";
                    }
                }
                if (mod == PhoenixModule.OFFSHORE)
                {
                    ddlStatus.ShortNameFilter = "APP";
                    txtRemarks.Text = "";
                }
            }
            if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP"))
            {
                txtRemarks.CssClass = "input";
                RadComboBox status = (RadComboBox)ddlStatus.FindControl("ddlHard");
                status.Focus();
            }
            else
            {
                txtRemarks.CssClass = "input_mandatory";
                RadComboBox status = (RadComboBox)ddlStatus.FindControl("ddlHard");
                status.Focus();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (mod == PhoenixModule.CREW)
            {
                if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA"))
                {
                    ddlStatus.ShortNameFilter = "APP";
                }
                else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA"))
                {
                    ddlStatus.ShortNameFilter = "APP";
                }
                else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRS") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRJ"))
                {
                    ddlStatus.ShortNameFilter = "APP";
                }
                else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDS") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDJ"))
                {
                    ddlStatus.ShortNameFilter = "APP";
                }
                else
                {
                    EmployeeInfo.Visible = true;
                    SetEmployeePrimaryDetails();

                    if (pdtype != "2")
                        toolbar.AddImageLink("javascript:Openpopup('codehelp2','','../Crew/CrewApprovedPDAttachment.aspx?dtkey=" + strDocumentId + "&proceed=1&mod=" + mod + "&docid=" + strDocumentId.ToString() + "&type=" + strApprovalType.ToString() + "&user=" + csvUserCode.ToString() + "&vslid=" + strVesselId.ToString() + "&newapp=" + Request.QueryString["newapp"] + "'); return false;", "Approved PD", "", "APPROVEDPD");

                    if (csvUserCode.Trim() == string.Empty)
                    {
                        ucError.ErrorMessage = "Technical Director is not configured for the vessel.";
                        ucError.Visible = true;
                    }

                }
            }
            toolbar.AddButton("Approve", "SAVE", ToolBarDirection.Right);
            if (mod == PhoenixModule.CREW)
            {
                if (!(strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRS")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "PRJ")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDS")
                        || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "BDJ")))
                    toolbar.AddButton("Proceed", "PROCEED", ToolBarDirection.Right);
            }
            MenuApproval.AccessRights = this.ViewState;
            MenuApproval.MenuList = toolbar.Show();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        byte bAllApproved = 0;
        DataTable dt = PhoenixCommonApproval.ListApprovalRecord(strDocumentId, int.Parse(strApprovalType), csvUserCode, General.GetNullableInteger(strVesselId), 1, ref bAllApproved);
        ViewState["ALLAPPROVED"] = bAllApproved;

        gvApproval.DataSource = dt;

        if (mod == PhoenixModule.CREW)
        {
            if (!(strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA")) || !(strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA")))
            {
                tdProceed.Visible = true;
                tblComments.Visible = true;
                lblNote.Visible = SessionUtil.CanAccess(this.ViewState, "PROCEED");
                DataTable dtcp = PhoenixCrewPlanning.EditCrewPlan(new Guid(strDocumentId));
                if (dtcp.Rows.Count > 0)
                {
                    lblProceedRemarks.Text = dtcp.Rows[0]["FLDPLANNEDREMARKS"].ToString();
                    lblProceedRemarks.ToolTip = dtcp.Rows[0]["FLDPLANNEDREMARKS"].ToString();
                }
            }
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            string employeeid = "";
            if (empid != null && empid != "")
                employeeid = empid;
            else if (offsignerid != null && offsignerid != "")
                employeeid = offsignerid;

            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(employeeid));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeNo.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                if (dt.Rows[0]["FLDISOFFICER"].ToString() == "1")
                {
                    ViewState["ISRATING"] = "0";
                }
                else
                {
                    ViewState["ISRATING"] = "1";

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewApproval_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string status = ddlStatus.SelectedHard;
                string remarks = txtRemarks.Text;
                string onbehaalf = ddlOnbehalf.SelectedValue;

                if (!IsValidateApproval(status, remarks, onbehaalf))
                {
                    ucError.Visible = true;
                    return;
                }
                byte bAllApproved = 0;

                if (mod == PhoenixModule.ACCOUNTS)
                {
                    int iApprovalStatusAccounts;
                    if (ViewState["directpoapproval"] != null && ViewState["directpoapproval"].ToString() != string.Empty)
                    {
                        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, csvUserCode, General.GetNullableInteger(strVesselId));
                        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

                        BindData();
                        PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                        PhoenixCommonAcount.DirectPOInvoiceLineItemInsert(new Guid(strDocumentId));
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                    }
                    else if (subtype != null && subtype.ToUpper().ToString().Equals("DIRECTPO"))
                    {
                        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, csvUserCode, General.GetNullableInteger(strVesselId));

                        PhoenixAccountsInvoice.DirectPOApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(strDocumentId),
                           csvUserCode,
                           int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()));
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        BindData();
                    }
                    else if (subtype != null && subtype.ToUpper().ToString().Equals("DPOAPPROVAL"))
                    {
                        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, csvUserCode, General.GetNullableInteger(strVesselId));

                        PhoenixAccountsPOStaging.DirectPOApprovalBasedOnConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(strDocumentId),
                           csvUserCode,
                           int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()));
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        BindData();
                    }
                    else
                    {
                        if (int.Parse(strApprovalType) < 459)
                        {
                            DataTable dtAccounts = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, int.Parse(ViewState["suppliercode"].ToString()), General.GetNullableInteger(ViewState["vouchertype"].ToString()));
                            iApprovalStatusAccounts = int.Parse(dtAccounts.Rows[0][0].ToString());

                        }

                        else
                        {
                            DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, csvUserCode, General.GetNullableInteger(strVesselId));
                            iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());
                        }
                        BindData();

                        PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId, iApprovalStatusAccounts, bAllApproved == 1 ? true : false, csvUserCode);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + "," + (ddlStatus.SelectedHard == "186" ? ("null") : ("'keepopen'")) + ");", true);
                    }
                   ucStatus.Text = "Payment voucher Approved";
                    SendNotificationMail();
                }

                else if (mod == PhoenixModule.QUALITY)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status)
                    , remarks, csvUserCode, General.GetNullableInteger(strVesselId));

                    if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OMA"))
                    {
                        PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        BindData();
                    }
                    else
                    {
                        PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                    }
                }
                else if (mod == PhoenixModule.OFFSHORE)
                {

                    int n;
                    n = PhoenixCommonApproval.OffshoreAppoinmentLetterApproval(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status)
                    , remarks, csvUserCode, General.GetNullableGuid(Request.QueryString["crewplanid"].ToString()), General.GetNullableGuid(Request.QueryString["appoinmentletterid"].ToString()));
                    BindData();
                    //PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);

                }

                else
                {

                    DataTable dt1 = new DataTable();
                    if (String.IsNullOrEmpty(pdtype) || pdtype == "1" || pdtype == "3") // General Approval 
                        dt1 = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status)
                        , remarks, csvUserCode, General.GetNullableInteger(strVesselId));

                    if (!String.IsNullOrEmpty(pdtype) && pdtype == "2") // Offshore Approval
                        dt1 = PhoenixCommonApproval.OffshorePDApproval(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status)
                        , remarks, csvUserCode, General.GetNullableInteger(strVesselId));

                    int iApprovalStatus = int.Parse(dt1.Rows[0][0].ToString());
                    BindData();
                    bAllApproved = byte.Parse(ViewState["ALLAPPROVED"].ToString());
                    SendMail(dt1, onbehaalf);
                    if (mod == PhoenixModule.CREW)
                    {
                        if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA"))
                        {
                            PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        }
                        else if (strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "JRA") || strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "SRA"))
                        {
                            PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId, iApprovalStatus, bAllApproved == 1 ? true : false, csvUserCode);
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            //    "BookMarkScript", "fnReloadList('approval','null'," + (bAllApproved == 1 ? ("null") : ("'keepopen'")) + ");", true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','null',null);", true);
                        }
                        else
                        {
                            PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId, iApprovalStatus, bAllApproved == 1 ? true : false, csvUserCode);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('approval','ifMoreInfo',"
                                + (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP") ? ("null") : ("'keepopen'")) + ");", true);
                        }

                    }
                    //else if (mod == PhoenixModule.ACCOUNTS)
                    //{
                    //    DataTable dt = PhoenixCommonApproval.InsertApprovalRecord(strDocumentId, int.Parse(strApprovalType), General.GetNullableInteger(onbehaalf), int.Parse(status), remarks, int.Parse(ViewState["suppliercode"].ToString()));
                    //    iApprovalStatus = int.Parse(dt1.Rows[0][0].ToString());
                    //    PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId, iApprovalStatus, bAllApproved == 1 ? true : false, csvUserCode);
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + "," + (ddlStatus.SelectedHard == "186" ? ("null") : ("'keepopen'")) + ");", true);
                    //}
                    else if (mod == PhoenixModule.PURCHASE)
                    {
                        if (subtype != null && subtype.ToUpper().ToString().Equals("SCHEDULEPO"))
                        {
                            PhoenixPurchaseSchedulePO.SchedulePOApprovalStatusUpdate(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(strDocumentId),
                                int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()),
                                csvUserCode);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        }
                        else if (subtype != null && subtype.ToUpper().ToString().Equals("PARTPAID"))
                        {
                            PhoenixPurchaseOrderPartPaid.PartPaidApprovalStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(strDocumentId),
                                int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()),
                                csvUserCode);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','filterandsearch',null);", true);
                        }
                        else
                        {
                            PhoenixCommonApproval.Approve(mod, int.Parse(strApprovalType), strDocumentId + (Request.QueryString["v"] != null ? "~" + Request.QueryString["vslid"] : string.Empty), int.Parse(dt1.Rows[0]["FLDAPPROVALID"].ToString()), bAllApproved == 1 ? true : false, csvUserCode);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                        }
                    }
                }
                gvApproval.Rebind();
            }
            else if (CommandName.ToUpper().Equals("PROCEED"))
            {
                if (mod == PhoenixModule.CREW)
                {
                    if (!(strApprovalType == PhoenixCommonRegisters.GetHardCode(1, 98, "OTA")))
                    {
                        byte bAllApproved = 0;

                        DataTable dt = PhoenixCommonApproval.ListApprovalRecord(strDocumentId, int.Parse(strApprovalType), csvUserCode, General.GetNullableInteger(strVesselId), 0, ref bAllApproved);
                        DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["ATTACHMENTCODE"].ToString())
                            , PhoenixCrewAttachmentType.ASSESSMENT.ToString());

                        if (dt1.Rows.Count == 0 && ViewState["ISRATING"].ToString() == "1")
                        {
                            ucError.ErrorMessage = "Please attach Interview sheet/Offer letter in the proposal to proceed further";
                            ucError.Visible = true;
                            return;
                        }
                        ucConfirmCrew.Visible = true;
                        ucConfirmCrew.Text = "Please Note: <br/> <ul>" + (Request.QueryString["newapp"] != null && bAllApproved == 1 ? "<li>Officer will be moved to Personnel Master</li>" : string.Empty) + "<li>Officer will be moved to Crew Change Plan</li>" + (ViewState["ALLAPPROVED"].ToString() == "0" ? "<li>Officer should be fully approved prior generating Travel Request</li></ul>" : "</ul>");
                        ((RadTextBox)ucConfirmCrew.FindControl("txtRemarks")).Focus();
                    }
                }
            }
            gvApproval.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendNotificationMail()
    {
        try
        {

            string to, cc, bcc, mailsubject, mailbody, sessionid = string.Empty;
            bool htmlbody = true;
            MailPriority priority = MailPriority.Normal;
            int? iEmployeeId = null;
            DataTable dsPurchaser = PhoenixAccountsInvoicePaymentVoucher.Paymentvouchermaildetails(new Guid(strDocumentId));
            if (dsPurchaser.Rows.Count > 0)
            {
                int count = dsPurchaser.Rows.Count;
                while(count > 0)
                {
                    DataRow drPurchaser = dsPurchaser.Rows[count-1];
                    if (drPurchaser["FLDSOURCE"].ToString() == "APT")
                    {
                        mailbody = "Dear Purchaser," + "<br/><br/>" + "PO Number :" + drPurchaser["FLDREFERENCEDOCUMENT"].ToString() + " " + "has been" + " " + drPurchaser["FLDPAYMENTVOUCHERSTATUSNAME"].ToString() + "." + "<br/><br/>" + "Thanks & Regards" + "<br/>" + "Product Support";

                        // string comma = ",";
                        to = drPurchaser["FLDPURCHASEREMAIL"].ToString();
                        cc = "";
                        bcc = "";
                        mailsubject = "PO Number :" + drPurchaser["FLDREFERENCEDOCUMENT"].ToString() + " " + "-" + " " + drPurchaser["FLDPAYMENTVOUCHERSTATUSNAME"].ToString();
                        PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid, iEmployeeId);
                    }
                    count--;
                }
               // ucStatus.Text = "Email sent successfully";
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

       

    
    protected void gvApproval_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //GridView _gridView = (GridView)sender;


        if (e.Item is GridDataItem)
        {
            //   if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
            //     && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
                RadLabel lblEdit = (RadLabel)e.Item.FindControl("lblEdit");
                //if (lblEdit.Text == "0") db.Visible = false;
                //DataRowView drv = (DataRowView)e.Item.DataItem;
                //if (e.Item.RowIndex == drv.DataView.Count - 1 && lblEdit.Text == "1")
                //{
                //    db.Visible = true;
                //}
                //else
                //{
                //    db.Visible = false;
                //}
                UserControlHard ucstatus = (UserControlHard)e.Item.FindControl("ddlStatus");
                //if (mod == PhoenixModule.OFFSHORE)
                //{
                //    string APP = PhoenixCommonRegisters.GetHardCode(1, 49, "APP");
                //    ucstatus.SelectedHard = APP;
                //}                 
            }
        }
        else
        {
            e.Item.Attributes["onclick"] = "";
        }
    }

    protected void gvApproval_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = e.RowIndex;
        //DataTable dt = new DataTable();
        //try
        //{
        //    string approvalid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblApprovalType")).Text;
        //    string status = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlStatus")).SelectedHard;
        //    string remarks = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarks")).Text;
        //    if (!IsValidateApproval(status, remarks, approvalid))
        //    {
        //        ucError.Visible = true;
        //        return;
        //    }
        //    dt = PhoenixCommonApproval.UpdateApprovalRecord(strDocumentId, int.Parse(approvalid), int.Parse(status), remarks);
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
        //_gridView.EditIndex = -1;
        //BindData();
        //SendMail(dt, dt.Rows[0][0].ToString());
    }

    private bool IsValidateApproval(string status, string remarks, string onbehalf)
    {
        int resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(status, out resultInt))
            ucError.ErrorMessage = "Status is required.";
        if (mod == PhoenixModule.CREW && (ddlStatus.SelectedHard != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP")))
        {
            if (string.IsNullOrEmpty(remarks))
                ucError.ErrorMessage = "Remarks is required.";
        }
        else if ((string.IsNullOrEmpty(remarks)) && mod != PhoenixModule.CREW)
            ucError.ErrorMessage = "Remarks is required.";
        return (!ucError.IsError);
    }
    protected void ddlOnbehalf_DataBound(object sender, EventArgs e)
    {
        ddlOnbehalf.Items.Insert(0, new RadComboBoxItem(PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName, ""));
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
    }
    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Remarks is required for proceeding.";
        return (!ucError.IsError);
    }
    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessageCrew ucCM = sender as UserControlConfirmMessageCrew;
            if (ucCM.confirmboxvalue == 1)
            {
                if (mod == PhoenixModule.CREW)
                {
                    RadTextBox txtrem = (RadTextBox)ucConfirmCrew.FindControl("txtRemarks");
                    if (txtrem.Text == "")
                    {
                        if (!IsValidateRemarks(txtrem.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                    }
                    PhoenixCommonApproval.Proceed(mod, int.Parse(strApprovalType), strDocumentId, csvUserCode, ucCM.RequestList, txtrem.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','ifMoreInfo'," + (ViewState["ALLAPPROVED"].ToString() == "1" ? ("null") : ("'keepopen'")) + ");", true);
                    BindData();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string GridViewToHtml(RadGrid gv)
    {
        int gvControlPosition = frmApproval.Controls.IndexOf(gv);
        Page p = new Page();
        HtmlForm hf = new HtmlForm();
        p.Controls.Add(hf);
        hf.Controls.Add(gv);
        p.EnableEventValidation = false;

        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        p.DesignerInitialize();
        p.RenderControl(hw);
        hw.Flush();
        p.Dispose();
        p = null;
        frmApproval.Controls.AddAt(gvControlPosition, gv);

        string str = sb.Remove(sb.ToString().IndexOf("<form"), sb.ToString().IndexOf(">") + 1).Replace("</form>", "").ToString();
        str = str.Remove(str.IndexOf("<div>"), str.IndexOf("</div>") + 4);
        while (str.IndexOf("type=\"image\"") > -1)
        {
            str = str.Remove(str.IndexOf("<input type=\"image\""), str.Substring(0, str.IndexOf("/>")).Length - str.IndexOf("<input type=\"image\"") + 2);
        }
        return str;
    }
    private void SendMail(DataTable dt, string onbehalf)
    {
        try
        {
            if (mod == PhoenixModule.CREW)
            {
                if (dt.Rows[0][1].ToString() == "1" && General.GetNullableInteger(onbehalf).HasValue)
                {
                    DataTable dt1 = PhoenixCrewPlanning.EditCrewPlan(new Guid(strDocumentId));
                    string subject = "Approval Intimation for " + dt1.Rows[0]["FLDEMPLOYEENAME"].ToString() + " " + dt1.Rows[0]["FLDRANKNAME"].ToString();
                     PhoenixMail.SendMail(dt.Rows[0][2].ToString(), string.Empty, string.Empty, subject, "Good Day,<br/>Please find below approval status of the subject officer. <br/><br/>" + GridViewToHtml(gvApproval), true, System.Net.Mail.MailPriority.Normal, string.Empty);
                    //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString.ToString(), true);//this to refresh the grid

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private NameValueCollection MakeNameValueCollection(Guid reqid)
    {
        NameValueCollection nvc = null;
        DataTable dt = PhoenixCommonApproval.FetchApprovalRequest(reqid);
        if (dt.Rows.Count > 0)
        {
            string reqtext = dt.Rows[0]["FLDAPPROVALREQUESTTEXT"].ToString();
            nvc = new NameValueCollection();
            foreach (string str in reqtext.Split('~'))
            {
                nvc.Add(str.Split('`')[0], str.Split('`')[1]);
            }
        }
        return nvc;
    }

    protected void gvApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {

            DataTable dt = new DataTable();
            try
            {
                string approvalid = ((RadLabel)e.Item.FindControl("lblApprovalType")).Text;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatus")).SelectedHard;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
                if (!IsValidateApproval(status, remarks, approvalid))
                {
                    ucError.Visible = true;
                    return;
                }
                dt = PhoenixCommonApproval.UpdateApprovalRecord(strDocumentId, int.Parse(approvalid), int.Parse(status), remarks);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            gvApproval.Rebind();
            BindData();
            //  SendMail(dt, dt.Rows[0][0].ToString());
        }
    }
}
