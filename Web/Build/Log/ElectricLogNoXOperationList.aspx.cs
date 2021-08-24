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

public partial class Log_ElectricLogNoXOperationList : PhoenixBasePage
{
    String ReportShortCode = string.Empty;
    int usercode = 0;
    int vesselId = 0;
    int recordsPerPage = 12;
    bool isChiefEnggLocked = false;
    string isicon = string.Empty;
    int isChiefEnggLockCount = 0;
    string url = "ElectricLogNoXOperationList.aspx";
    string logbook = "Annex-VI Nox Tier";

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            gvElogTransaction.PageSize = recordsPerPage;
            BindLogStatus();
            SetLatestPage();
            Userguide();
        }
        string txids = GetLogIDFromGrid();
        GetChiefEnggSignedName(txids);
        ShowToolBar();
    }
    private void Userguide()
    {
        string location = string.Empty;
        location = HttpContext.Current.Request.MapPath("~\\Template\\Log") + @"\AnnexureNox.pdf";
        lnkfilename.NavigateUrl = "../Reports/ReportsDownload.aspx?filename=" + location + "&type=pdf";
    }

    private void SetLatestPage()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        PhoenixMarpolLogNOX.LogNOXBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                 , General.GetNullableDateTime(txtFromDate.Text)
                                                 , General.GetNullableDateTime(txtToDate.Text)
                                                 , gvElogTransaction.CurrentPageIndex + 1
                                                 , gvElogTransaction.PageSize
                                                 , ref iRowCount
                                                 , ref iTotalPageCount);
        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        if (iTotalPageCount == 0)
        {
            return;
        }


        gvElogTransaction.CurrentPageIndex = iTotalPageCount - 1;
        gvElogTransaction.Rebind();
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
        toolbar.AddFontAwesomeButton("", "Export PDF", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "Audit Log", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        if (isicon == "Green")
        {
            toolbar.AddFontAwesomeButton("", "ChiefEngg Signature Done", "<img src=\"../css/Theme1/images/lock_3.png\" />", "CHIEFENGGUNLOCK");
        }
        else
        {
            toolbar.AddFontAwesomeButton("", "ChiefEngg Signature Pending", "<img src=\"../css/Theme1/images/unlock_2.png\" />", "CHIEFENGGSIGN");
        }
        toolbar.AddFontAwesomeButton("", "Event Log", "<i class=\"fas fa-administration\"></i>", "EVENTLOG");
        MenugvCounterUpdate.MenuList = toolbar.Show();
        MenugvCounterUpdate.AccessRights = this.ViewState;
    }

    public void GetChiefEnggSignedName(string txtIds)
    {
        if (string.IsNullOrWhiteSpace(txtIds))
        {
            txtChiefEnggSign.Text = "No Signature";
            return;
        }

        DataSet ds = PhoenixMarpolLogNOX.ChiefEnggSignatureNOXSelect(usercode, txtIds);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            string ChiefEnggName = row["FLDCHIEFENGGNAME"] != DBNull.Value ? row["FLDCHIEFENGGNAME"].ToString() : string.Empty;
            txtChiefEnggSign.Text = string.IsNullOrWhiteSpace(ChiefEnggName) ? "No Signature" : ChiefEnggName;
            isChiefEnggLocked = Convert.ToBoolean(row["FLDISCHIEFENGGLOCKED"].ToString());
            isChiefEnggLockCount = int.Parse(row["FLDCHIEFENGGUNLOCKCOUNT"].ToString());
            isicon = row["FLDICON"].ToString();
            return;
        }
        txtChiefEnggSign.Text = "No Signature";
    }

    public void BindLogStatus()
    {
        DataSet ds = PhoenixMarpolLogNOX.LogNOXStatusList();
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
            SetChiefEnggSignature();
            if (Filter.LogBookPdfExportCriteria != null) // from nvc check the condition
            {
                NameValueCollection nvc = Filter.LogBookPdfExportCriteria;
                DateTime? startDate = General.GetNullableDateTime(txtFromDate.Text);
                DateTime? endDate = General.GetNullableDateTime(txtToDate.Text);
                int? pageNo = gvElogTransaction.CurrentPageIndex + 1;

                if (nvc["isDateRange"] != null && nvc["isDateRange"].ToLower() == "true")
                {
                    startDate = General.GetNullableDateTime(nvc["startDate"]);
                    endDate = General.GetNullableDateTime(nvc["endDate"]);
                    pageNo = 1;

                }
                Filter.LogBookPdfExportCriteria = null;
                ExportPDF(pageNo, startDate, endDate);
            }
            else
            {
                gvElogTransaction.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixMarpolLogNOX.LogNOXBookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ddlStatus.SelectedValue)
                                         , General.GetNullableDateTime(txtFromDate.Text)
                                         , General.GetNullableDateTime(txtToDate.Text)
                                         , gvElogTransaction.CurrentPageIndex + 1
                                         , gvElogTransaction.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);



        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        gvElogTransaction.DataSource = ds;
        gvElogTransaction.VirtualItemCount = iRowCount;

    }


    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {
        int pageNumber = pageNo.HasValue ? pageNo.Value : gvElogTransaction.CurrentPageIndex + 1;


        return PhoenixMarpolLogNOX.LogNOXBookEntryReportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                 , General.GetNullableDateTime(startDate.ToString())
                                                 , General.GetNullableDateTime(endDate.ToString())
                                                 , pageNo
                                                 , gvElogTransaction.PageSize
                                                 , ref iRowCount
                                                 , ref iTotalPageCount);

    }

    private Dictionary<string, bool> GetAccessRights()
    {
        // get it from db and form a dictionary
        Dictionary<string, bool> accessrights = new Dictionary<string, bool>();
        string rankShortCode = PhoenixElog.GetRankName(usercode);
        string designaionShortCode = null;
        bool isVessel = vesselId > 0 ? true : false;
        bool isChiefEngg = PhoenixElog.validCheifEngineer(rankShortCode);
        DataSet ds = PhoenixMarpolLogNOX.GetMarpolLogNOXAccessRights(usercode, vesselId, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("CHIEFENGGUNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("TRANSCATIONHISTORY", Convert.ToBoolean(row["FLDHISTORY"]));

            if (isChiefEngg)
            {
                accessrights.Add("CHIEFENGGSIGN", Convert.ToBoolean(row["FLDSIGN"]));
            }
            else
            {
                accessrights.Add("CHIEFENGGSIGN", Convert.ToBoolean(false));
            }
        }
        else
        {
            accessrights.Add("VIEW", false);
            accessrights.Add("ADD", false);
            accessrights.Add("SIGN", false);
            accessrights.Add("CHIEFENGGUNLOCK", false);
            accessrights.Add("CHIEFENGGSIGN", false);
            accessrights.Add("AMEND", false);
            accessrights.Add("TRANSCATIONHISTORY", false);
        }
        accessrights.Add("PDF", true);
        accessrights.Add("FIND", true);
        accessrights.Add("CLEAR", true);
        accessrights.Add("EVENTLOG", true);

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
                gvElogTransaction.Rebind();
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
            }

            if (CommandName.ToUpper().Equals("PDF"))
            {
                EventLog(url, caption, CommandName, logbook);
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx?refreshWindowName=oilLog', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("CHIEFENGGSIGN"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                if (string.IsNullOrWhiteSpace(txids))
                {
                    throw new ArgumentException("No Records found to ChiefEngg Sign");
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','{0}/Log/ElectricLogCommonMasterSignature.aspx?IsAnnex=1&TxnId={1}',null, 400,280,null,null,{{model:true}});", Session["sitepath"], txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("ADD"))
            {   
                EventLog(url, caption, CommandName, logbook);
                int mainEngineCount = EngineConfigCount("MainEngine");
                if(mainEngineCount == 0)
                {
                    throw new ArgumentException("Before putting the Record Books into use, you must do the configuration for Tanks and Diesel Engines");
                }
                int pageNumber = gvElogTransaction.CurrentPageIndex + 1;
                if (ViewState["iTotalCount"].ToString() != "0" && pageNumber.ToString() != ViewState["iTotalCount"].ToString())
                {
                    throw new ArgumentException("Page is already full. Select the empty pages and try again");
                }

                string script = String.Format("javascript:parent.openNewWindow('Log','','" + Session["sitepath"] + "/Log/ElectricLogNOX.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogNOXTranscationHistory.aspx?txid={0}',null, null,null,null,null,{{model:true}});", txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("CHIEFENGGUNLOCK"))
            {
                EventLog(url, caption, CommandName, logbook);
                string currentUserRank = PhoenixElog.GetRankName(usercode);
                if (PhoenixElog.validCheifEngineer(currentUserRank) == false)
                {
                    throw new ArgumentException("ChiefEngg only unlock the page");
                }
                string logids = GetLogIDFromGrid();
                PhoenixMarpolLogNOX.ChiefEnggUnlockTheLogNOX(usercode, logids);
                gvElogTransaction.Rebind();
            }
            if (CommandName.ToUpper().Equals("EVENTLOG"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogEventLogHistory.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
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
    private void SetChiefEnggSignature()
    {
        if (Filter.MasterSignatureCriteria != null)
        {
            NameValueCollection nvc = Filter.MasterSignatureCriteria;
            int ChiefEnggUserCode = Convert.ToInt32(nvc["usercode"]);
            string ChiefEnggSignature = nvc["signature"];
            string ChiefEnggName = nvc["mastername"];
            DateTime ChiefEnggSignedDate = Convert.ToDateTime(nvc["signeddate"]);
            string txids = nvc["txids"];
            PhoenixMarpolLogNOX.ChiefEnggSignatureNOX(ChiefEnggUserCode, txids, ChiefEnggName, ChiefEnggSignedDate);
            Filter.MasterSignatureCriteria = null;
        }
    }

    private bool IsValidDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (DateTime.TryParse(txtToDate.Text, out resultDate)
            && DateTime.Compare(resultDate, DateTime.Now) > 0)
            ucError.ErrorMessage = "To Date should not be greater than Current Date.";
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
            nvc.Add("reportcode", "OPERATIONREPORTNOX");
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
            RadLabel lblTxnId = (RadLabel)gvrow.FindControl("lblLogBookId");
             txtids += lblTxnId.Text + ",";
        }

        return txtids;
    }



    protected void gvElogTransaction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvElogTransaction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
            RadLabel lblDtKey = (RadLabel)e.Item.FindControl("lblDtKey");
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            HtmlGenericControl attachmentIcon = (HtmlGenericControl)e.Item.FindControl("attachmentIcon");
            if (cmdAttachment != null)
            {
                //cmdAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDtKey.Text + "&MOD=LOG&RefreshWindowName=oilLog" + "');return false;");
                string status = (string)drv["FLDSTATUS"];
                string script = string.Format("openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey={0}&MOD=LOG&RefreshWindowName=oilLog&Status={1}');return false;", lblDtKey.Text, status);
                cmdAttachment.Attributes.Add("onclick", script);
            }

            //DisplayAttachment(e);
            AssignNoSignature(e);
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
            if (e.CommandName == "OFFICERINCHARGE" || e.CommandName == "OFFICERSIGN")
            {
                RadLabel TxnId = (RadLabel)e.Item.FindControl("lblTxnId");
                String scriptpopup = string.Empty;
                scriptpopup = String.Format("javascript:parent.openNewWindow('Location','','" + Session["sitepath"] + "/Log/ElectronicLogOperationApproval.aspx?TxnId=" + TxnId.Text + "&ReportCode=" + ReportShortCode + " ');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName == "EDIT")
            {
                RadLabel TxnId = (RadLabel)e.Item.FindControl("lblTxnId");
            }
            if (e.CommandName == "DELETE")
            {
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }
                
                RadLabel lblBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                string scriptpopup = "javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogNOXBookEntryDelete.aspx?TxnId=" + lblBookId.Text + "', 'false', '400', '250', null, null, { 'disableMinMax': true });";
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
                if (lblLogBookId != null)
                {
                    string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?View=true&LogBookId={2}');", Session["sitepath"], "ElectricLogNOX.aspx", lblLogBookId.Text);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                gvElogTransaction.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                gvElogTransaction.Rebind();
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
        bool isChiefEnggSigned;
        bool isAmend;
        Boolean.TryParse(drv["FLDISCHIEFENGGLOCKED"].ToString(), out isChiefEnggSigned);
        Boolean.TryParse(drv["FLDISAMEND"].ToString(), out isAmend);
        bool isDeleted = Convert.ToBoolean(drv["FLDISDELETED"] == DBNull.Value ? false : drv["FLDISDELETED"]);

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("cmdDelete");
        LinkButton btnView = (LinkButton)e.Item.FindControl("CmdView");
        RadLabel lblCurrentStatus = (RadLabel)e.Item.FindControl("lblCurrentStatus");
        RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
        LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgFlag");
        RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");


        if (IsBackDatedEntry.Text == "1")
        {
            BackDatedEntry.Visible = true;
        }

        if (isChiefEnggSigned) return;

        if ((isDeleted == false && (statusShortCode == "PVER" || statusShortCode == "PRVE")) || (isDeleted == false && statusShortCode == "VRFD" && isAmend == false))
        {
            btnView.Visible = true;
            btnDelete.Visible = true;

        }
        else
        {
            btnView.Visible = false;
            btnDelete.Visible = false;
        }
    }
    private void AssignNoSignature(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        GridDataItem dataItem = (GridDataItem)e.Item;     
        bool isDeleted = Convert.ToBoolean(drv["FLDISDELETED"] == DBNull.Value ? false : drv["FLDISDELETED"]);

        LinkButton btnCheifEngineerSignature = (LinkButton)e.Item.FindControl("btnCheifEngineerSignature");
    }

    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        gvElogTransaction.CurrentPageIndex = 0;
        gvElogTransaction.Rebind();
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

    private int EngineConfigCount(string engineDetail)
    {
        int engineCount = 0;
        DataTable dt = PhoenixMarpolLogNOX.AnnexureEngineCount(usercode, vesselId, engineDetail);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            engineCount = Convert.ToInt32(row["FLDENGINECOUNT"]);
        }
        return engineCount;
    }

    protected void gvElogTransaction_PreRender(object sender, EventArgs e)
    {
        string txids = GetLogIDFromGrid();
        GetChiefEnggSignedName(txids);
        ShowToolBar();
        showHideColumns();
    }

    private void showHideColumns()
    {
        int mainEngineCount = EngineConfigCount("MainEngine");
        int auxEngineCount = EngineConfigCount("AuxEngine");
        int harbourGenCount = EngineConfigCount("HarbourGenerator");

        ShowHideEngines(mainEngineCount, "meTier", "meStatus");
        ShowHideEngines(auxEngineCount, "aeTier", "aeStatus");
        ShowHideEngines(harbourGenCount, "harbourgenTier", "harbourgenStatus");

    }

    private void ShowHideEngines(int engineCount, string tier, string status)
    {
        for (int i = 1; i <= engineCount; i++)
        {
            GridTemplateColumn meTier = (GridTemplateColumn)gvElogTransaction.Columns.FindByUniqueName(tier + i);
            GridTemplateColumn meStatus = (GridTemplateColumn)gvElogTransaction.Columns.FindByUniqueName(status + i);

            meTier.Display = true;
            meStatus.Display = true;
        }
    }

}