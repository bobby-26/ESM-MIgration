﻿using SouthNests.Phoenix.Elog;
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

public partial class Log_ElectricLogORB2OperationList : PhoenixBasePage
{
    String ReportShortCode = string.Empty;
    int usercode = 0;
    int vesselId = 0;
    int recordsPerPage = 25;
    bool isMasterLocked = false;
    string isicon = string.Empty;
    int isMasterLockCount = 0;
    string url = "ElectricLogORB2OperationList.aspx";
    string logbook = "ORB-2";

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            ViewState["isMasterLoggedIn"] = false;
            gvElogTransaction.PageSize = recordsPerPage;
            BindLogStatus();
            SetLatestPage();
            //Userguide();
            CheckMasterLoggedIn();
        }
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }

    private void CheckMasterLoggedIn()
    {
        DataSet userdetail = PhoenixElog.GetSeaFarerRankName(usercode);
        if (userdetail != null && userdetail.Tables.Count > 0 && userdetail.Tables[0].Rows.Count > 0)
        {
            DataRow userdetailRow = userdetail.Tables[0].Rows[0];
            DataTable dt = userdetail.Tables[0];
            DataRow row = dt.Rows[0];
            lblUsername.Text = string.Format("{0} - {1}", row["FLDFIRSTNAME"].ToString() + "   " + row["FLDLASTNAME"].ToString(), row["FLDRANKCODE"]);
            if (userdetailRow["FLDRANKCODE"].ToString() == "MST")
            {
                ViewState["isMasterLoggedIn"] = true;
            }
        }
    }

    //private void Userguide()
    //{
    //    string location = string.Empty;
    //    location = HttpContext.Current.Request.MapPath("~\\Template\\Log") + @"\OilRecordBookPartII_ESM.pdf";
    //    lnkfilename.NavigateUrl = "../Reports/ReportsDownload.aspx?filename=" + location + "&type=pdf";        
    //}

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
        toolbar.AddFontAwesomeButton("", "Export PDF", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "Tank List", "<i class=\"fa fa-clipboard-list\"></i>", "TANKLIST");
        toolbar.AddFontAwesomeButton("", "Audit Log", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        //toolbar.AddImageButton("","", "", "", );
        if (isicon == "Green")
        {
            //toolbar.AddFontAwesomeButton("", "Master Signature Done", "<i class=\"fa fa-lock\"></i>", "MASTERUNLOCK");
            toolbar.AddFontAwesomeButton("", "Master Signature Done", "<img src=\"../css/Theme1/images/lock_3.png\" />", "MASTERUNLOCK");
        }
        else
        {
            toolbar.AddFontAwesomeButton("", "Master Signature Pending", "<img src=\"../css/Theme1/images/unlock_2.png\" />", "MASTERSIGN");
        }

        if (Convert.ToBoolean(ViewState["isMasterLoggedIn"]))
        {
            toolbar.AddFontAwesomeButton("", "Miscellaneous", "<i class=\"fa fa-clipboard\"></i>", "MISC");
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
        ddlStatus.DataTextField = "FLDSTATUSDESC";
        ddlStatus.DataValueField = "FLDLOGSTATUSID";
        ddlStatus.DataBind();
        DropDownListItem item = new DropDownListItem("All", string.Empty);
        item.Selected = true;
        ddlStatus.Items.Insert(0, item);
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
            PhoenixMarbolLogORB2.MasterSignatureORB2(masterUserCode, txids, masterName, masterSignedDate);
            Filter.MasterSignatureCriteria = null;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SetMasterSignature();
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

        gvElogTransaction.DataSource = ds;
        gvElogTransaction.VirtualItemCount = iRowCount;

    }


    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {
        int pageNumber = pageNo.HasValue ? pageNo.Value : gvElogTransaction.CurrentPageIndex + 1;


        DataSet ds = PhoenixMarbolLogORB2.LogORB2BookEntryReportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
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
            //accessrights.Add("PDF", Convert.ToBoolean(row["FLDPRINT"]));
            accessrights.Add("TANKLIST", Convert.ToBoolean(row["FLDTANKLIST"]));
            accessrights.Add("TRANSCATIONHISTORY", Convert.ToBoolean(row["FLDHISTORY"]));            

            if (isMaster)
            {
                accessrights.Add("MASTERSIGN", Convert.ToBoolean(row["FLDSIGN"]));
                accessrights.Add("MISC", Convert.ToBoolean(row["FLDSIGN"]));
            }
            else
            {
                accessrights.Add("MASTERSIGN", Convert.ToBoolean(false));
                accessrights.Add("MISC", false);
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
            //accessrights.Add("PDF", false);

            accessrights.Add("TANKLIST", false);
            accessrights.Add("TRANSCATIONHISTORY", false);
            accessrights.Add("MISC", false);
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
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
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
            }


            if (CommandName.ToUpper().Equals("ADD"))
            {
                EventLog(url, caption, CommandName, logbook);
                int pageNumber = gvElogTransaction.CurrentPageIndex + 1;
                if (ViewState["iTotalCount"].ToString() != "0" && pageNumber.ToString() != ViewState["iTotalCount"].ToString())
                {
                    throw new ArgumentException("Page is already full. Select the empty pages and try again");
                }
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogORB2Operation.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("TANKLIST"))
            {
                EventLog(url, caption, CommandName, logbook);
                string script = String.Format("javascript:parent.openNewWindow('ifMoreInfo','','" + Session["sitepath"] + "/Log/ElectricLogORB2TankList.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                EventLog(url, caption, CommandName, logbook);
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogORB2TranscationHistory.aspx?txid={0}',null, null,null,null,null,{{model:true}});", txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
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
                PhoenixMarbolLogORB2.MasterUnlockTheLogORB2(usercode, logids);
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

            if (CommandName.ToUpper().Equals("MISC"))
            {
                string script = String.Format("javascript:parent.openNewWindow('Log','','" + Session["sitepath"] + "/Log/ElectricLogORB2AdditionalProcedure.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("HELP"))
            {
                EventLog(url, caption, CommandName, logbook);
                string location = string.Empty;
                location = HttpContext.Current.Request.MapPath("~\\Template\\Log") + @"\OilRecordBookPartII_ESM.pdf";
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
        DateTime fromdate;
        DateTime todate;
        if (DateTime.TryParse(txtToDate.Text, out resultDate)
            && DateTime.Compare(resultDate, DateTime.Now) > 0)
        { 
            ucError.ErrorMessage = "To Date should not be greater than Current Date.";            
        }
        if (DateTime.TryParse(txtFromDate.Text, out fromdate) && DateTime.TryParse(txtToDate.Text, out todate)
            && DateTime.Compare(fromdate, todate) > 0)
        {
            ucError.ErrorMessage = "To Date cannot be earlier than from Date.";
        }
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
            nvc.Add("reportcode", "OPERATIONREPORTORB2");
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
                //cmdAttachment.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDtKey.Text + "&MOD=LOG&RefreshWindowName=oilLog" + "');return false;");
                string status = (string)drv["FLDSTATUS"];
                string script = string.Format("openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey={0}&MOD=LOG&RefreshWindowName=oilLog&Status={1}');return false;", lblDtKey.Text, status);
                cmdAttachment.Attributes.Add("onclick", script);
            }

            DisplayAttachment(e);
            AssignNoSignature(e);
            DisplayDeletedUser(e);
            DisplayButton(e);
            DisplayStrikeThroughIfDeleted(e);
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
        if (orderno == "1" || (drv["FLDISMISSEDOPERATION"].ToString() == "1" && orderno == "2"))
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
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                RadLabel lblTxnId = (RadLabel)e.Item.FindControl("lblTxnId");
                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogORB2BookEntryDelete.aspx?TxnId=" + lblTxnId.Text + "', 'false', '400', '250', null, null, {{ 'disableMinMax': true }});");
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
                if (lblTxnId != null && lblLogName != null && lblurl != null)
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

        if (orderno == "1")
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
        }
    }

    private void DisplayStrikeThroughIfDeleted(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView rowData = (DataRowView)e.Item.DataItem;
        GridDataItem dataItem = (GridDataItem)e.Item;
        bool isDeleted = Convert.ToBoolean(rowData["FLDISDELETED"] == DBNull.Value ? false : rowData["FLDISDELETED"]);
        bool isAmend = Convert.ToBoolean(rowData["FLDISAMEND"] == DBNull.Value ? false : rowData["FLDISAMEND"]);
        int orderno = Convert.ToInt32(rowData["FLDORDERNO"].ToString());
        bool isMissedOperation = Convert.ToBoolean(rowData["FLDISMISSEDOPERATION"] == DBNull.Value ? false : rowData["FLDISMISSEDOPERATION"]);

        if (isAmend && !isDeleted)
        {
            RadLabel lblRecord = (RadLabel)e.Item.FindControl("lblRecord");

            string amendRecord = string.Format("<span style='text-decoration:line-through;'>{0}</span> &nbsp;&nbsp; <span style='color: none;'>{1}</span>", rowData["FLDOLDRECORD"], rowData["FLDRECORD"]);
            lblRecord.Text = amendRecord;
        }
        if (isDeleted)
        {
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            RadLabel lblItemno = (RadLabel)e.Item.FindControl("lblItemNo");
            RadLabel lblRecord = (RadLabel)e.Item.FindControl("lblRecord");
            RadLabel lblCheifSign = (RadLabel)e.Item.FindControl("lblCheifOfficerSign");
            RadLabel lblInchargeSign = (RadLabel)e.Item.FindControl("lblInchargeSign");

            if (lblStatus != null)
            {
                lblStatus.Attributes.Add("style", "text-decoration:line-through;");
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
        }

        RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
        if (isMissedOperation && orderno == 2)
        {
            lblDate.Visible = true;
        }
        else if (orderno >= 2)
        {
            lblDate.Visible = false;
        }
    }

    private void AssignNoSignature(GridItemEventArgs e)
    {
        if (e == null) return;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        GridDataItem dataItem = (GridDataItem)e.Item;
        string orderno = drv["FLDORDERNO"].ToString();
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

    protected void gvElogTransaction_PreRender(object sender, EventArgs e)
    {
        string txids = GetLogIDFromGrid();
        GetMasterSignedName(txids);
        ShowToolBar();
    }
}