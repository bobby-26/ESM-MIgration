using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogGRBOperationList : PhoenixBasePage
{
    String ReportShortCode = string.Empty;
    int usercode = 0;
    int vesselId = 0;
    int recordsPerPage = 10;
    int ExceprecordsPerPage = 2;
    bool isMasterLocked = false;
    string isicon = string.Empty;
    int isMasterLockCount = 0;
    string url = "ElectricLogGRBOperationList.aspx";
    string logbook = "GRB-1";

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            gvElogTransaction.PageSize = recordsPerPage;
            gvElogExceptional.PageSize = ExceprecordsPerPage;
            BindLogStatus();
            SetLatestPage();
            SetExcepLatestPage();
            showLoggedUser();
        }
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }
    private void showLoggedUser()
    {

        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            lblUsername.Text = string.Format("{0} - {1}", row["FLDFIRSTNAME"].ToString() + "   " + row["FLDLASTNAME"].ToString(), row["FLDRANKCODE"]);
        }
    }

    private void SetLatestPage()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        PhoenixMarpolLogGRB.LogGRBBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                 , General.GetNullableDateTime(txtFromDate.Text)
                                                 , General.GetNullableDateTime(txtToDate.Text)
                                                 , gvElogTransaction.CurrentPageIndex + 1
                                                 , gvElogTransaction.PageSize
                                                 , ref iRowCount
                                                 , ref iTotalPageCount
                                                 , 0                                //Planned Disposal
                                                 );
        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        if (iTotalPageCount == 0)
        {
            return;
        }


        gvElogTransaction.CurrentPageIndex = iTotalPageCount - 1;
        gvElogTransaction.Rebind();
    }

    private void SetExcepLatestPage()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        PhoenixMarpolLogGRB.LogGRBBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                 , General.GetNullableDateTime(txtFromDate.Text)
                                                 , General.GetNullableDateTime(txtToDate.Text)
                                                 , gvElogExceptional.CurrentPageIndex + 1
                                                 , gvElogExceptional.PageSize
                                                 , ref iRowCount
                                                 , ref iTotalPageCount,
                                                 1                          // Exceptional Discharge
                                                 );
        //ViewState["iRowCounnt"] = iRowCount;
        //ViewState["iTotalCount"] = iTotalPageCount;

        if (iTotalPageCount == 0)
        {
            return;
        }

        gvElogExceptional.CurrentPageIndex = iTotalPageCount - 1;
        gvElogExceptional.Rebind();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        ShowToolBar();
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("", "Download PDF", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "Audit Log", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        if (isicon == "Green")
        {
            toolbar.AddFontAwesomeButton("", "Master Signature Done", "<img src=\"../css/Theme1/images/lock_3.png\" />", "MASTERUNLOCK");
        }
        else if (isicon == "Yellow")
        {
            toolbar.AddFontAwesomeButton("", "Master Signature Pending", "<img src=\"../css/Theme1/images/unlock_4.png\" />", "MASTERSIGN");
        }
        else
        {
            toolbar.AddFontAwesomeButton("", "Master Signature Pending", "<img src=\"../css/Theme1/images/unlock_2.png\" />", "MASTERSIGN");
        }
        toolbar.AddFontAwesomeButton("", "Event Log", "<i class=\"fas fa-administration\"></i>", "EVENTLOG");
        toolbar.AddFontAwesomeButton("", "User Guide", "<i class=\"fas fa-question\"></i>", "HELP", ToolBarDirection.Right);
        MenugvCounterUpdate.MenuList = toolbar.Show();
        MenugvCounterUpdate.AccessRights = this.ViewState;
    }

    public void GetMasterSignedName(string txtIds)
    {
        if (string.IsNullOrWhiteSpace(txtIds))
        {
            txtMasterSign.Text = "No Signature";
            return;
        }

        DataSet ds = PhoenixMarpolLogGRB.MasterSignatureGRBSelect(usercode, txtIds);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            string masterName = row["FLDMASTERNAME"] != DBNull.Value ? row["FLDMASTERNAME"].ToString() : string.Empty;
            txtMasterSign.Text = string.IsNullOrWhiteSpace(masterName) ? "No Signature" : masterName;
            isMasterLocked = Convert.ToBoolean(row["FLDISMASTERLOCKED"].ToString());
            isMasterLockCount = int.Parse(row["FLDMASTERUNLOCKCOUNT"].ToString());
            isicon = row["FLDICON"].ToString();
            return;
        }
        txtMasterSign.Text = "No Signature";
    }

    public void BindLogStatus()
    {
        DataSet ds = PhoenixMarpolLogGRB.LogGRBStatusList();
        ddlStatus.DataSource = ds;
        ddlStatus.DataTextField = "FLDSTATUS";
        ddlStatus.DataValueField = "FLDLOGSTATUSID";
        ddlStatus.DataBind();
        DropDownListItem item = new DropDownListItem("All", string.Empty);
        item.Selected = true;
        ddlStatus.Items.Insert(0, item);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SetMasterSignature();
            On2ndOfficerSign();
            if (Filter.LogBookPdfExportCriteria != null) // from nvc check the condition
            {
                NameValueCollection nvc = Filter.LogBookPdfExportCriteria;
                DateTime? startDate = General.GetNullableDateTime(txtFromDate.Text);
                DateTime? endDate = General.GetNullableDateTime(txtToDate.Text);
                int? pageNo = gvElogTransaction.CurrentPageIndex + 1;
                int? exceppageno = gvElogExceptional.CurrentPageIndex + 1;

                if (nvc["isDateRange"] != null && nvc["isDateRange"].ToLower() == "true")
                {
                    startDate = General.GetNullableDateTime(nvc["startDate"]);
                    endDate = General.GetNullableDateTime(nvc["endDate"]);
                    pageNo = 1;
                    exceppageno = 1;

                }
                Filter.LogBookPdfExportCriteria = null;
                ExportPDF(pageNo, startDate, endDate);
            }
            else
            {
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetMasterSignature()
    {
        if (Filter.MasterSignatureCriteria != null)
        {
            NameValueCollection nvc = Filter.MasterSignatureCriteria;
            int masterUserCode = Convert.ToInt32(nvc["usercode"]);
            string masterSignature = nvc["signature"];
            string masterName = nvc["mastername"];
            DateTime masterSignedDate = Convert.ToDateTime(nvc["signeddate"]);
            string txids = nvc["txids"];
            PhoenixMarpolLogGRB.MasterSignatureGRB(masterUserCode, txids, masterName, masterSignedDate);
            Filter.MasterSignatureCriteria = null;
        }
    }

    private void On2ndOfficerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null && Filter.DutyEngineerSignatureFilterCriteria["rank"].ToLower() == "2e")
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            PhoenixMarpolLogGRB.LogGRB2ndEnggSignature(usercode,
                                    General.GetNullableGuid(nvc.Get("LogBookId")),
                                    General.GetNullableInteger(nvc.Get("id")),
                                    General.GetNullableInteger(null),
                                    nvc.Get("name"),
                                    nvc.Get("rank"),
                                    DateTime.Now);
            Filter.DutyEngineerSignatureFilterCriteria = null;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixMarpolLogGRB.LogGRBBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ddlStatus.SelectedValue)
                                         , General.GetNullableDateTime(txtFromDate.Text)
                                         , General.GetNullableDateTime(txtToDate.Text)
                                         , gvElogTransaction.CurrentPageIndex + 1
                                         , gvElogTransaction.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , 0                        //Planned Disposal
                                         );

        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        gvElogTransaction.DataSource = ds;
        gvElogTransaction.VirtualItemCount = iRowCount;

    }

    private void BindExcepData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixMarpolLogGRB.LogGRBBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ddlStatus.SelectedValue)
                                         , General.GetNullableDateTime(txtFromDate.Text)
                                         , General.GetNullableDateTime(txtToDate.Text)
                                         , gvElogExceptional.CurrentPageIndex + 1
                                         , gvElogExceptional.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , 1                        //Exceptional Discharge
                                         );

        //ViewState["iRowCounnt"] = iRowCount;
        //ViewState["iTotalCount"] = iTotalPageCount;

        gvElogExceptional.DataSource = ds;
        gvElogExceptional.VirtualItemCount = iRowCount;

    }

    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {
        int pageNumber = pageNo.HasValue ? pageNo.Value : gvElogTransaction.CurrentPageIndex + 1;


        DataSet ds = PhoenixMarpolLogGRB.LogGRBBookEntryReportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                 , General.GetNullableDateTime(startDate.ToString())
                                                 , General.GetNullableDateTime(endDate.ToString())
                                                 , pageNo
                                                 , gvElogTransaction.PageSize
                                                 , ref iRowCount
                                                 , ref iTotalPageCount);

        return ds;
    }

    private Dictionary<string, bool> GetAccessRights()
    {
        // get it from db and form a dictionary
        Dictionary<string, bool> accessrights = new Dictionary<string, bool>();
        string rankShortCode = PhoenixElog.GetRankName(usercode);
        string designaionShortCode = null;
        bool isVessel = vesselId > 0 ? true : false;
        bool isMaster = PhoenixElog.validMaster(rankShortCode);
        DataSet ds = PhoenixMarpolLogGRB.GetMarpolLogGRBAccessRights(usercode, vesselId, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("MASTERUNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            //accessrights.Add("PDF", Convert.ToBoolean(row["FLDPRINT"]));
            accessrights.Add("TRANSCATIONHISTORY", Convert.ToBoolean(row["FLDHISTORY"]));

            if (isMaster)
            {
                accessrights.Add("MASTERSIGN", Convert.ToBoolean(row["FLDSIGN"]));
            }
            else
            {
                accessrights.Add("MASTERSIGN", Convert.ToBoolean(false));
            }
        }
        else
        {
            accessrights.Add("VIEW", false);
            accessrights.Add("ADD", false);
            accessrights.Add("SIGN", false);
            accessrights.Add("MASTERUNLOCK", false);
            accessrights.Add("MASTERSIGN", false);
            accessrights.Add("AMEND", false);

            accessrights.Add("TRANSCATIONHISTORY", false);
        }
        accessrights.Add("FIND", true);
        accessrights.Add("CLEAR", true);
        accessrights.Add("PDF", true);
        accessrights.Add("EVENTLOG", true);
        accessrights.Add("HELP", true);

        return accessrights;
    }

    protected void gvCounterUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string caption = dce.Item.ToolTip;

            Dictionary<string, bool> accessrights = GetAccessRights();

            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                EventLog(url, caption, CommandName, logbook);
                if (!IsValidDate())
                {
                    ucError.Visible = true;
                    return;
                }
                gvElogTransaction.CurrentPageIndex = 0;
                gvElogExceptional.CurrentPageIndex = 0;
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                EventLog(url, caption, CommandName, logbook);
                txtFromDate.Text = null;
                txtToDate.Text = null;
                ddlStatus.ClearSelection();
                DropDownListItem item = ddlStatus.Items[0];
                item.Selected = true;
                gvElogTransaction.CurrentPageIndex = 0;
                gvElogTransaction.Rebind();
                gvElogExceptional.CurrentPageIndex = 0;
                gvElogExceptional.Rebind();
            }

            if (CommandName.ToUpper().Equals("PDF"))
            {
                EventLog(url, caption, CommandName, logbook);
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }

            if (CommandName.ToUpper().Equals("MASTERSIGN"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                if (string.IsNullOrWhiteSpace(txids))
                {
                    ucError.ErrorMessage = "No Records found to Master Sign";
                    ucError.Visible = true;
                    return;
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','{0}/Log/ElectricLogCommonMasterSignature.aspx?TxnId={1}',null, 400,280,null,null,{{model:true}});", Session["sitepath"], txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }


            if (CommandName.ToUpper().Equals("ADD"))
            {
                EventLog(url, caption, CommandName, logbook);
                int pageNumber = gvElogTransaction.CurrentPageIndex + 1;
                if (ViewState["iTotalCount"].ToString() != "0" && pageNumber.ToString() != ViewState["iTotalCount"].ToString())
                {
                    throw new ArgumentException("Page is already full. Select the empty pages and try again");
                }
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogGRBOperation.aspx',false, 420,250,null,null,{{model:true}});");
                // string script = "javascript:openNewWindow('oilLog-1','Oil Log','Log/ElectricLogOperation.aspx', false, 1000, 600)";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }


            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogGRBTranscationHistory.aspx?txid={0}',null, null,null,null,null,{{model:true}});", txids);
                //string script = string.Format("javascript:openNewWindow('ifMoreInfo','Oil Log','Log/ElectricLogTranscationHistory.aspx?txid={0}')", txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }

            if (CommandName.ToUpper().Equals("MASTERUNLOCK"))
            {
                EventLog(url, caption, CommandName, logbook);
                string currentUserRank = PhoenixElog.GetRankName(usercode);
                if (PhoenixElog.validMaster(currentUserRank) == false)
                {
                    ucError.ErrorMessage = "Master only unlock the page";
                    ucError.Visible = true;
                    return;
                }
                string logids = GetLogIDFromGrid();
                PhoenixMarpolLogGRB.MasterUnlockTheLogGRB(usercode, logids);
                gvElogTransaction.Rebind();
                gvElogExceptional.Rebind();
            }
            if (CommandName.ToUpper().Equals("EVENTLOG"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogEventLogHistory.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }
            if (CommandName.ToUpper().Equals("HELP"))
            {
                EventLog(url, caption, CommandName, logbook);
                string location = string.Empty;
                location = HttpContext.Current.Request.MapPath("~\\Template\\Log") + @"\GRBPartI_ESM.pdf";
                string navigateUrl = "../Reports/ReportsDownload.aspx?filename=" + location + "&type=pdf";
                Response.Redirect(navigateUrl);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EventLog(string url, string Caption, string CommandName, string logbook)
    {
        PhoenixElogCommon.MarpolEventLogInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                url,
                                                CommandName,
                                                Caption,
                                                logbook,
                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                General.GetNullableDateTime(null), General.GetNullableDateTime(null));
    }

    private bool IsValidDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (DateTime.TryParse(txtToDate.Text, out resultDate)
            && DateTime.Compare(resultDate, DateTime.Now) > 0)
            ucError.ErrorMessage = "To Date should not be greater than Current Date.";
        // txtToDate.Text = DateTime.Now.ToString();
        return (!ucError.IsError);

    }
    private void ExportPDF(int? pageNo, DateTime? startDate, DateTime? endDate)
    {
        string Tmpfilelocation = string.Empty;
        string[] reportfile = new string[0];
        int iTotalPageCount = 0;
        int iRowCount = 0;
        DataSet ds = GetSSRSData(ref iRowCount, ref iTotalPageCount, startDate, endDate, pageNo);

        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("applicationcode", "24");
            nvc.Add("reportcode", "OPERATIONREPORTGRB");
            nvc.Add("CRITERIA", "");
            Session["PHOENIXREPORTPARAMETERS"] = nvc;

            Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
            string filename = string.Format("{0}.pdf", "LogBook");
            Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);
            PhoenixReportsCommon.LoadLogo(ds);
            PhoenixSsrsReportsCommon.getLogo();
            PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
            Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
        }
    }

    private string GetLogIDFromGrid()
    {
        string txtids = string.Empty;
        foreach (GridDataItem gvrow in gvElogTransaction.Items)
        {
            RadLabel lblLogBookId = (RadLabel)gvrow.FindControl("lblLogBookId");
            txtids += lblLogBookId.Text + ",";
        }

        foreach (GridDataItem gvrow in gvElogExceptional.Items)
        {
            RadLabel lblLogBookId = (RadLabel)gvrow.FindControl("lblLogBookId");
            txtids += lblLogBookId.Text + ",";

        }

        return txtids;
    }



    protected void gvElogTransaction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvElogExceptional_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindExcepData();
    }

    protected void gvElogTransaction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
            RadLabel lblDtKey = (RadLabel)e.Item.FindControl("lblDtKey");
            RadLabel lblIsMissedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");
            LinkButton  MissedEntry = (LinkButton)e.Item.FindControl("imgFlag");
            if (lblIsMissedEntry != null && MissedEntry != null)
            {
                if (lblIsMissedEntry.Text == "1")
                {
                    MissedEntry.Visible = true;

                }
            }
            RadLabel lblPosition = (RadLabel)e.Item.FindControl("lblPosition");
            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            RadLabel lblintosea = (RadLabel)e.Item.FindControl("lblintosea");
            RadLabel lblfacility = (RadLabel)e.Item.FindControl("lblfacility");
            RadLabel lblincineration = (RadLabel)e.Item.FindControl("lblincineration");
            RadLabel lblremarks = (RadLabel)e.Item.FindControl("lblremarks");

            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");

            string flddelete = drv["FLDISDELETED"].ToString();
            string fldisamend = drv["FLDISAMEND"].ToString();

            if (drv["FLDISDELETED"].ToString() == "1")
            {
                lblPosition.Text = "<S>" + drv["FLDPORTPOSITION"].ToString() + "</S>";
                lblCategory.Text = "<S>" + drv["FLDCATEGORY"].ToString() + "</S>";
                lblintosea.Text = "<S>" + drv["FLDDISCHARGEINTOSEA"].ToString() + "</S>";
                lblfacility.Text = "<S>" + drv["FLDDISCHARGEINTORF"].ToString() + "</S>";
                lblincineration.Text = "<S>" + drv["FLDINCINERATED"].ToString() + "</S>";
                lblremarks.Text = "<S>" + drv["FLDREMARKS"].ToString() + "</S>";
            }
            else if (drv["FLDISAMEND"].ToString() == "True" && (drv["FLDISDELETED"] == DBNull.Value || drv["FLDISDELETED"].ToString() == ""))
            {
                lblPosition.Text = "<S>" + drv["FLDOLDPORTPOSITION"].ToString() + "</S></br>" + drv["FLDPORTPOSITION"].ToString();
                lblCategory.Text = "<S>" + drv["FLDCATEGORY"].ToString() + "</S></br>" + drv["FLDCATEGORY"].ToString();
                lblintosea.Text = "<S>" + drv["FLDOLDDISCHARGEINTOSEA"].ToString() + "</S></br>" + drv["FLDDISCHARGEINTOSEA"].ToString();
                lblfacility.Text = "<S>" + drv["FLDOLDDISCHARGEINTORF"].ToString() + "</S></br>" + drv["FLDDISCHARGEINTORF"].ToString();
                lblincineration.Text = "<S>" + drv["FLDOLDINCINERATED"].ToString() + "</S></br>" + drv["FLDINCINERATED"].ToString();
                lblremarks.Text = "<S>" + drv["FLDOLDREMARKS"].ToString() + "</S></br>" + drv["FLDREMARKS"].ToString();
            }
            else
            {
                lblPosition.Text = drv["FLDPORTPOSITION"].ToString();
                lblCategory.Text = drv["FLDCATEGORY"].ToString();
                lblintosea.Text = drv["FLDDISCHARGEINTOSEA"].ToString();
                lblfacility.Text = drv["FLDDISCHARGEINTORF"].ToString();
                lblincineration.Text = drv["FLDINCINERATED"].ToString()=="0"?"-": drv["FLDINCINERATED"].ToString();
                lblremarks.Text = drv["FLDREMARKS"].ToString();
            }

            HtmlGenericControl attachmentIcon = (HtmlGenericControl)e.Item.FindControl("attachmentIcon");
            if (cmdAttachment != null)
            {
                //cmdAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDtKey.Text + "&MOD=LOG&RefreshWindowName=oilLog" + "');return false;");
                string status = (string)drv["FLDSTATUS"];
                string script = string.Format("openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey={0}&MOD=LOG&RefreshWindowName=oilLog&Status={1}');return false;", lblDtKey.Text, status);
                cmdAttachment.Attributes.Add("onclick", script);
            }

            DisplayAttachment(e);
            DisplayDeletedUser(e);
            DisplayButton(e);
        }
        if (e.Item is GridFooterItem)
        {
            e.Item.Cells[2].ColumnSpan = ((RadGrid)sender).Columns.Count + 2;
            e.Item.Cells[0].Visible = false;
            e.Item.Cells[1].Visible = false;
            for (int i = 3; i < ((RadGrid)sender).Columns.Count + 2; i++)
            {
                e.Item.Cells[i].Visible = false;
            }
        }
    }

    protected void gvElogExceptional_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
            RadLabel lblDtKey = (RadLabel)e.Item.FindControl("lblDtKey");
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            HtmlGenericControl attachmentIcon = (HtmlGenericControl)e.Item.FindControl("attachmentIcon");

            RadLabel lblPosition = (RadLabel)e.Item.FindControl("lblPosition");
            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            RadLabel lblincineration = (RadLabel)e.Item.FindControl("lblincineration");
            RadLabel lblremarks = (RadLabel)e.Item.FindControl("lblremarks");

            RadLabel lblIsMissedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");
            LinkButton MissedEntry = (LinkButton)e.Item.FindControl("imgFlag");
            if (lblIsMissedEntry != null && MissedEntry != null)
            {
                if (lblIsMissedEntry.Text == "1")
                {
                    MissedEntry.Visible = true;

                }
            }

            if (drv["FLDISDELETED"].ToString() == "1")
            {
                lblPosition.Text = "<S>" + drv["FLDPORTPOSITION"].ToString() + "</S>";
                lblCategory.Text = "<S>" + drv["FLDCATEGORY"].ToString() + "</S>";
                lblincineration.Text = "<S>" + drv["FLDDISCHARGEACCIDENT"].ToString() + "</S>";
                lblremarks.Text = "<S>" + drv["FLDREMARKS"].ToString() + "</S>";
            }
            else if (drv["FLDISAMEND"].ToString() == "True" && (drv["FLDISDELETED"] == DBNull.Value || drv["FLDISDELETED"].ToString() == ""))
            {
                lblPosition.Text = "<S>" + drv["FLDOLDPORTPOSITION"].ToString() + "</S></br>" + drv["FLDPORTPOSITION"].ToString();
                lblCategory.Text = "<S>" + drv["FLDCATEGORY"].ToString() + "</S></br>" + drv["FLDCATEGORY"].ToString();
                lblincineration.Text = "<S>" + drv["FLDOLDDISCHARGEACCIDENT"].ToString() + "</S></br>" + drv["FLDDISCHARGEACCIDENT"].ToString();
                lblremarks.Text = "<S>" + drv["FLDOLDREMARKS"].ToString() + "</S></br>" + drv["FLDREMARKS"].ToString();
            }
            else
            {
                lblPosition.Text = drv["FLDPORTPOSITION"].ToString();
                lblCategory.Text = drv["FLDCATEGORY"].ToString();
                lblincineration.Text = drv["FLDDISCHARGEACCIDENT"].ToString() == "0" ? "-" : drv["FLDDISCHARGEACCIDENT"].ToString();
                lblremarks.Text = drv["FLDREMARKS"].ToString();
            }

            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDtKey.Text + "&MOD=LOG&RefreshWindowName=oilLog" + "');return false;");
            }

            DisplayAttachment(e);
            DisplayDeletedUser(e);
            DisplayButton(e);
        }
        if (e.Item is GridFooterItem)
        {
            e.Item.Cells[2].ColumnSpan = ((RadGrid)sender).Columns.Count + 2;
            e.Item.Cells[0].Visible = false;
            e.Item.Cells[1].Visible = false;
            for (int i = 3; i < ((RadGrid)sender).Columns.Count + 2; i++)
            {
                e.Item.Cells[i].Visible = false;
            }
        }
    }

    private void DisplayDeletedUser(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView rowData = (DataRowView)e.Item.DataItem;
        RadLabel lblDeleted = (RadLabel)e.Item.FindControl("lblDeleted");

        if (lblDeleted != null && rowData["FLDDELETEDDATE"] != DBNull.Value)
        {
            DateTime deletedDate = Convert.ToDateTime(rowData["FLDDELETEDDATE"]);
            lblDeleted.Text = string.Format("Deleted By {0} {1} {2}", rowData["FLDDELETEDRANK"], rowData["FLDDELETEDUSERNAME"], PhoenixElog.GetElogDateFormat(deletedDate));
            lblDeleted.Visible = true;
        }
    }

    private void DisplayAttachment(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        bool isAttachmentVisible = drv["FLDISATTACHMENTREQ"] == DBNull.Value || Convert.ToBoolean(drv["FLDISATTACHMENTREQ"]) == false ? false : true;
        HtmlGenericControl attachmentIcon = (HtmlGenericControl)e.Item.FindControl("attachmentIcon");
        bool isAttachmentAttached = drv["FLDATTACHMENTCODE"].ToString() == "1" ? true : false;
        LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");
        attachmentBtn.Visible = isAttachmentVisible;
        attachmentIcon.Attributes.Add("class", isAttachmentAttached ? "fas fa-paperclip" : "fa-paperclip-na");

    }

    protected void gvElogTransaction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EDIT")
            {
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
            }
            if (e.CommandName == "DELETE")
            {
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                string scriptpopup = "javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogGRBBookEntryDelete.aspx?lblLogBookId=" + lblLogBookId.Text + "', 'false', '400', '250', null, null, { 'disableMinMax': true });";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName == "VIEW")
            {
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblLogName = (RadLabel)e.Item.FindControl("lblLogName");
                RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
                if (lblLogBookId != null && lblLogName != null && lblurl != null)
                {
                    string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?LogBookId={2}&View=true');", Session["sitepath"], lblurl.Text, lblLogBookId.Text);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                gvElogTransaction.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                gvElogTransaction.Rebind();
            }

            if (e.CommandName == "2NDENGINEERSIGNATURE")
            {
                string popupTitle = "2E Signature";
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                string scriptpopup = "javascript:parent.openNewWindow('engineersign','','" + Session["sitepath"] + "/Log/ElectricLogOfficerSignature.aspx?rankName=2e&popupname=oilLog&popupTitle=" + popupTitle + "&LogBookId=" + lblLogBookId.Text + "', 'false', '370', '200', null, null, { 'disableMinMax': true });";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvElogExceptional_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "EDIT")
            {
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
            }
            if (e.CommandName == "DELETE")
            {
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                string scriptpopup = "javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogGRBBookEntryDelete.aspx?LogBookId=" + lblLogBookId.Text + "', 'false', '400', '250', null, null, { 'disableMinMax': true });";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName == "VIEW")
            {
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblLogName = (RadLabel)e.Item.FindControl("lblLogName");
                RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
                if (lblLogBookId != null && lblLogName != null && lblurl != null)
                {
                    string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?LogBookId={2}&View=true');", Session["sitepath"], lblurl.Text, lblLogBookId.Text);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                gvElogExceptional.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                gvElogExceptional.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void DisplayButton(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        string statusShortCode = drv["FLDSTATUSSHORTCODE"].ToString();
        bool isMasterSigned;
        bool isAmend;
        Boolean.TryParse(drv["FLDISMASTERLOCKED"].ToString(), out isMasterSigned);
        Boolean.TryParse(drv["FLDISAMEND"].ToString(), out isAmend);
        LinkButton btnDelete = (LinkButton)e.Item.FindControl("cmdDelete");
        LinkButton btnView = (LinkButton)e.Item.FindControl("CmdView");
        RadLabel lblCurrentStatus = (RadLabel)e.Item.FindControl("lblCurrentStatus");
        LinkButton cmd2esignature = (LinkButton)e.Item.FindControl("cmd2esignature");
        RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
        bool isDeleted = Convert.ToBoolean(drv["FLDISDELETED"] == DBNull.Value ? false : drv["FLDISDELETED"]);
        //lblCurrentStatus.Visible = true;

        if (isMasterSigned) return;

        if ((isDeleted == false && (statusShortCode == "PVER" || statusShortCode == "PRVE") && drv["FLDIS2ESIGN"].ToString() == "0.00") || (isDeleted == false && statusShortCode == "VRFD" && isAmend == false)
                    )
        {
            btnView.Visible = true;
            btnDelete.Visible = true;
        }
        else if ((statusShortCode == "PVAL" || statusShortCode == "PRVA") && drv["FLDIS2ESIGN"].ToString() != "0.00")
        {
            btnView.Visible = true;
            btnDelete.Visible = true;
            if (drv["FLDISEXCEPDISCHARGE"].ToString() == "0")
            {
                cmd2esignature.Visible = true;
            }
        }
        else
        {
            btnView.Visible = false;
            btnDelete.Visible = false;
            if (drv["FLDISEXCEPDISCHARGE"].ToString() == "0")
            {
                cmd2esignature.Visible = false;
            }
        }
    }

    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        gvElogTransaction.CurrentPageIndex = 0;
        gvElogExceptional.CurrentPageIndex = 0;
        gvElogTransaction.Rebind();
        gvElogExceptional.Rebind();
    }


    protected void gvElogTransaction_GridExporting(object sender, GridExportingArgs e)
    {
        using (FileStream fs = File.Create(Request.PhysicalApplicationPath + "RadGrid.pdf"))
        {
            byte[] output = Encoding.GetEncoding(1252).GetBytes(e.ExportOutput);
            fs.Write(output, 0, output.Length);
        }

        Response.Redirect(Request.Url.ToString());
    }

    protected void gvElogExceptional_GridExporting(object sender, GridExportingArgs e)
    {
        using (FileStream fs = File.Create(Request.PhysicalApplicationPath + "RadGrid.pdf"))
        {
            byte[] output = Encoding.GetEncoding(1252).GetBytes(e.ExportOutput);
            fs.Write(output, 0, output.Length);
        }

        Response.Redirect(Request.Url.ToString());
    }

    protected void gvElogTransaction_PreRender(object sender, EventArgs e)
    {
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }
    protected void gvElogExceptional_PreRender(object sender, EventArgs e)
    {
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }
}