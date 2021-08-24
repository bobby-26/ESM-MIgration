using SouthNests.Phoenix.Common;
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


public partial class Log_MarbolLogORB2OperationList : PhoenixBasePage
{
    //String PageTitle;
    String ReportShortCode = string.Empty;
    int usercode = 0;
    int vesselId = 0;
    int recordsPerPage = 25;
    bool isMasterLocked = false;
    string isicon = string.Empty;
    int isMasterLockCount = 0;

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
        }
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }

    private void SetLatestPage()
    {        
        int iRowCount = 0;
        int iTotalPageCount = 0;

        PhoenixMarbolLogORB2.LogORB2BookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode                                                 
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
        toolbar.AddFontAwesomeButton("", "Download PDF", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "Tank List", "<i class=\"fa fa-clipboard-list\"></i>", "TANKLIST");
        toolbar.AddFontAwesomeButton("", "History", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        //toolbar.AddImageButton("","", "", "", );
        if (isicon == "Green")
        {
            //toolbar.AddFontAwesomeButton("", "Master Signature Done", "<i class=\"fa fa-lock\"></i>", "MASTERUNLOCK");
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

        DataSet ds = PhoenixMarbolLogORB2.MasterSignatureORB2Select(usercode, txtIds);
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
        DataSet ds = PhoenixMarbolLogORB2.LogORB2StatusList();
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

      //  string[] alColumns = { "FLDDATE", "FLDCODE", "FLDITEMNO", "FLDFROMLOCATION", "FLDFROMROB", "FLDAFTTXNFROM", "FLDTOLOCATION", "FLDTOROB", "FLDAFTTXNTO", "FLDINCHARGENAME", "FLDCHIEFNAME" };
       // string[] alCaptions = { "Date", "Code", "Item No", "Transferred From", "Before From ROB", "After From ROB", "Transfer To", "Before To ROB", "After To ROB", "Officer Incharge", "Chief Officer" };


        DataSet ds = PhoenixMarbolLogORB2.LogORB2BookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode                                         
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
        
        //General.SetPrintOptions("gvElogTransaction", "Sludge Transfer Record", alCaptions, alColumns, ds);
        gvElogTransaction.DataSource = ds;
        gvElogTransaction.VirtualItemCount = iRowCount;

    }


    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {        
        int pageNumber = pageNo.HasValue ? pageNo.Value : gvElogTransaction.CurrentPageIndex + 1;


        DataSet ds = PhoenixMarbolLogORB2.LogORB2BookEntrySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode                                                 
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
        DataSet ds = PhoenixMarbolLogORB2.GetMarpolLogORB2AccessRights(usercode, vesselId, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("MASTERUNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("PDF", Convert.ToBoolean(row["FLDPRINT"]));
            accessrights.Add("TANKLIST", Convert.ToBoolean(row["FLDTANKLIST"]));
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
            accessrights.Add("PDF", false);

            accessrights.Add("TANKLIST", false);
            accessrights.Add("TRANSCATIONHISTORY", false);
        }
        accessrights.Add("FIND", true);
        accessrights.Add("CLEAR", true);


        return accessrights;
    }

    protected void gvCounterUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Dictionary<string, bool> accessrights = GetAccessRights();

            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
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
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (CommandName.ToUpper().Equals("MASTERSIGN"))
            {

                string txids = GetLogIDFromGrid();
                if (string.IsNullOrWhiteSpace(txids))
                {
                    ucError.ErrorMessage = "No Records found to Master Sign";
                    ucError.Visible = true;
                    return;
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogORB2MasterSignature.aspx?TxnId=" + txids + "',null, 400,280,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }


            if (CommandName.ToUpper().Equals("ADD"))
            {
                int pageNumber = gvElogTransaction.CurrentPageIndex + 1;
                if (ViewState["iTotalCount"].ToString() != "0" && pageNumber.ToString() != ViewState["iTotalCount"].ToString())
                {
                    throw new ArgumentException("Page is already full. Select the empty pages and try again");
                }
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogORB2Operation.aspx',null, null,null,null,null,{{model:true}});");
                // string script = "javascript:openNewWindow('oilLog-1','Oil Log','Log/ElectricLogOperation.aspx', false, 1000, 600)";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("TANKLIST"))
            {
                string script = String.Format("javascript:parent.openNewWindow('ifMoreInfo','','" + Session["sitepath"] + "/Log/ElectricLogORB2TankList.aspx',null, null,null,null,null,{{model:true}});");
                // string script = "javascript:openNewWindow('ifMoreInfo','Oil Log','Log/ElectricLogTankList.aspx', false, 1000, 600)";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogORB2TranscationHistory.aspx?txid={0}',null, null,null,null,null,{{model:true}});", txids);
                //string script = string.Format("javascript:openNewWindow('ifMoreInfo','Oil Log','Log/ElectricLogTranscationHistory.aspx?txid={0}')", txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("MASTERUNLOCK"))
            {
                string currentUserRank = PhoenixElog.GetRankName(usercode);
                if (PhoenixElog.validMaster(currentUserRank) == false)
                {
                    ucError.ErrorMessage = "Master only unlock the page";
                    ucError.Visible = true;
                    return;
                }
                string logids = GetLogIDFromGrid();
                PhoenixMarbolLogORB2.MasterUnlockTheLogORB2(usercode, logids);
                gvElogTransaction.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            //string pageNumber = (gvElogTransaction.CurrentPageIndex + 1).ToString();
            //Response.Redirect(string.Format("../Reports/ReportsView.aspx?applicationcode=9&reportcode=OPERATIONREPORT&fromDate={0}&toDate={1}&locationId=null&status={2}&pageNumber={3}&pageSize={4}&resultCount={5}&totalPageCount={6}", txtFromDate.Text, txtToDate.Text, ddlStatus.SelectedItem.Value, pageNumber, 25, null, null), false);

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("applicationcode", "24");
            nvc.Add("reportcode", "OPERATIONREPORT");
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
            RadLabel lblTxnId = (RadLabel)gvrow.FindControl("lblTxnId");
            RadLabel lblOrderNo = (RadLabel)gvrow.FindControl("lblOrderNo");
            if (lblOrderNo.Text == "1")
            {
                txtids += lblTxnId.Text + ",";
            }
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
                cmdAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDtKey.Text + "&MOD=LOG&RefreshWindowName=oilLog" + "');return false;");
            }

            //if (attachmentIcon != null && drv["FLDATTACHMENTCODE"] != DBNull.Value)
            //{
            //    attachmentIcon.Attributes.Add("class", "fas fa-paperclip");
            //}

            DisplayAttachment(e);
            //DisplayStrikeThroughIfDeleted(e);
            AssignNoSignature(e);
            DisplayDeletedUser(e);
            DisplayButton(e);
        }
    }

    private void DisplayDeletedUser(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView rowData = (DataRowView)e.Item.DataItem;
        string orderno = rowData["FLDORDERNO"].ToString();
        RadLabel lblDeleted = (RadLabel)e.Item.FindControl("lblDeleted");

        if (orderno == "1" && lblDeleted != null && rowData["FLDDELETEDDATE"] != DBNull.Value)
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
        string orderno = drv["FLDORDERNO"].ToString();
        bool isAttachmentVisible = drv["FLDISATTACHMENTREQ"] == DBNull.Value || Convert.ToBoolean(drv["FLDISATTACHMENTREQ"]) == false ? false : true;
        HtmlGenericControl attachmentIcon = (HtmlGenericControl)e.Item.FindControl("attachmentIcon");
        bool isAttachmentAttached = drv["FLDATTACHMENTCODE"].ToString() == "1" ? true : false;
        if (orderno == "1")
        {
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");
            attachmentBtn.Visible = isAttachmentVisible;
            attachmentIcon.Attributes.Add("class", isAttachmentAttached ? "fas fa-paperclip" : "fa-paperclip-na");
        }

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
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblTxnId = (RadLabel)e.Item.FindControl("lblTxnId");
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogORB2BookEntryDelete.aspx?TxnId=" + lblTxnId.Text + "');");
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

                RadLabel lblTxnId = (RadLabel)e.Item.FindControl("lblTxnId");
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                RadLabel lblLogName = (RadLabel)e.Item.FindControl("lblLogName");
                RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
                if (lblTxnId != null && lblLogName != null && lblurl!= null)
                {
                    string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?TxnId={2}&View=true&LogBookId={3}');", Session["sitepath"], lblurl.Text, lblTxnId.Text, lblLogBookId.Text);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                gvElogTransaction.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                gvElogTransaction.Rebind();
            }

            if (e.CommandName == "CHEIFENGINEERSIGNATURE")
            {
                string rankCode = PhoenixElog.GetRankName(usercode);
                bool isChiefEngineer = PhoenixElog.validCheifEngineer(rankCode);
                if (isChiefEngineer == false)
                {
                    throw new ArgumentException("You don't have access rights");
                }
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                LinkButton btnCheifEngineerSignature = (LinkButton)e.Item.FindControl("btnCheifEngineerSignature");
                //string script = "javascript:parent.openNewWindow('OfficerSign','','" + Session["sitepath"] + "/Log/ElectricLogCheifEngineerSignature.aspx?id=" + lblLogId.Text + "', 'false', '370', '170', null, null, { 'disableMinMax': true });";
                //btnCheifEngineerSignature.Attributes.Add("onclick", script);
                string script = "javascript:parent.openNewWindow('OfficerSign','','" + Session["sitepath"] + "/Log/ElectricLogCheifEngineerSignature.aspx?id=" + lblLogId.Text + "', 'false', '370', '170', null, null, { 'disableMinMax': true });";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //private void showLog(string txId, string lblLogName, string logbookId, string logId)
    //{
    //    string LogName = string.Empty;
    //    DataSet ds = PhoenixMarbolLogORB2.GetLogId(usercode, LogShortCode);
    //    if (logs.ContainsKey(lblLogName))
    //    {
    //        logs.TryGetValue(lblLogName, out LogName);
    //    }

    //    if (string.IsNullOrWhiteSpace(LogName))
    //    {
    //        return;
    //    }

    //    string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?TxnId={2}&View=true&LogBookId={3}');", Session["sitepath"], LogName, txId, logbookId);
    //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    //}

    private void DisplayButton(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        string orderno = drv["FLDORDERNO"].ToString();
        string statusShortCode = drv["FLDSTATUSSHORTCODE"].ToString();
        bool isMasterSigned;
        bool isAmend;
        Boolean.TryParse(drv["FLDISMASTERLOCKED"].ToString(), out isMasterSigned);
        Boolean.TryParse(drv["FLDISAMEND"].ToString(), out isAmend);
        LinkButton btnDelete = (LinkButton)e.Item.FindControl("cmdDelete");
        LinkButton btnView = (LinkButton)e.Item.FindControl("CmdView");
        RadLabel lblCurrentStatus = (RadLabel)e.Item.FindControl("lblCurrentStatus");
        RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
        bool isDeleted = Convert.ToBoolean(drv["FLDISDELETED"] == DBNull.Value ? false : drv["FLDISDELETED"]);
        string isCheifEnginner = drv["FLDISCHEIFENGINEERSIGN"].ToString();

        if(orderno == "1")
            lblCurrentStatus.Visible = true;

        if (isMasterSigned) return;

        if ((orderno == "1" && isDeleted == false && (statusShortCode == "PVER" || statusShortCode == "PRVE")) || (orderno == "1" && isDeleted == false && statusShortCode == "VRFD" && isAmend == false))
        {
            btnView.Visible = true;
            btnDelete.Visible = true;

        }
        else
        {
            btnView.Visible = false;
            btnDelete.Visible = false;
            //lblDate.Visible = false;
        }
    }

    private void DisplayStrikeThroughIfDeleted(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView rowData = (DataRowView)e.Item.DataItem;
        GridDataItem dataItem = (GridDataItem)e.Item;
        bool isDeleted = Convert.ToBoolean(rowData["FLDISDELETED"] == DBNull.Value ? false : rowData["FLDISDELETED"]);
        if (isDeleted)
        {
            string orderno = rowData["FLDORDERNO"].ToString();
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            RadLabel lblItemno = (RadLabel)e.Item.FindControl("lblItemNo");
            RadLabel lblRecord = (RadLabel)e.Item.FindControl("lblRecord");
            RadLabel lblCheifSign = (RadLabel)e.Item.FindControl("lblCheifOfficerSign");
            RadLabel lblInchargeSign = (RadLabel)e.Item.FindControl("lblInchargeSign");

            if (lblDate != null)
            {
                lblDate.Attributes.Add("style", "text-decoration:line-through;");
            }
            if (lblCode != null)
            {
                lblCode.Attributes.Add("style", "text-decoration:line-through;");
            }

            if (lblItemno != null)
            {
                lblItemno.Attributes.Add("style", "text-decoration:line-through;");
            }

            if (lblRecord != null)
            {
                lblRecord.Attributes.Add("style", "text-decoration:line-through;");
            }

            if (lblCheifSign != null)
            {
                lblCheifSign.Attributes.Add("style", "text-decoration:line-through;");
            }

            if (lblInchargeSign != null)
            {
                lblInchargeSign.Attributes.Add("style", "text-decoration:line-through;");
            }

            if (lblStatus != null)
            {
                lblStatus.Attributes.Add("style", "text-decoration:line-through;");
            }

            //if (orderno == "2" && lblStatus != null)
            //{
            //    DateTime deletedDate = Convert.ToDateTime(rowData["FLDDELETEDDATE"]);
            //    lblStatus.Text = string.Format("Deleted By {0} {1} {2}", rowData["FLDDELETEDRANK"], rowData["FLDDELETEDUSERNAME"], PhoenixElog.GetElogDateFormat(deletedDate));
            //    lblStatus.Visible = true;
            //}
            //else
            //{
            //    lblStatus.Visible = false;
            //}
        }
    }

    private void AssignNoSignature(GridItemEventArgs e)
    {
        if (e == null) return;
        TableCell cell;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        GridDataItem dataItem = (GridDataItem)e.Item;
        string orderno = drv["FLDORDERNO"].ToString();
        bool isDeleted = Convert.ToBoolean(drv["FLDISDELETED"] == DBNull.Value ? false : drv["FLDISDELETED"]);
        //if (orderno == "1" || isDeleted == true) return;

        // check if signed then no need to go further in this method
        string isCheifEnginner = drv["FLDISCHEIFENGINEERSIGN"].ToString();
        LinkButton btnCheifEngineerSignature = (LinkButton)e.Item.FindControl("btnCheifEngineerSignature");

        if (isCheifEnginner == "True" && string.IsNullOrWhiteSpace(drv["FLDCHIEFRANK"].ToString()))
        {
            cell = dataItem["record"];
            HtmlGenericControl recordContainer = (HtmlGenericControl)cell.FindControl("recordContainer");
            recordContainer.Attributes.Add("class", "not-signed");
            btnCheifEngineerSignature.Visible = true;
        }
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

    protected void gvElogTransaction_PreRender(object sender, EventArgs e)
    {
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }
}