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


public partial class Log_ElectricLogFuelChangeOverOperationList : PhoenixBasePage
{
    String ReportShortCode = string.Empty;
    int usercode = 0;
    int vesselId = 0;
    bool isChiefEnggLocked = false;
    string isicon = string.Empty;
    int isChiefEnggLockCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            gvElogTransaction.PageSize = 12;
            Bindcolumns();
            BindLogStatus();
            SetLatestPage();
            showLoggedUser();
            
        }
        //string txids = GetLogIDFromGrid();
        //GetMasterSignedName(txids);
        //ShowToolBar();
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

        DataSet ds = PhoenixLogFuelChangeOver.FuelOilChangeOverSearch(
                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , General.GetNullableDateTime(txtFromDate.Text)
                                            , General.GetNullableDateTime(txtToDate.Text)
                                            , General.GetNullableInteger(ddlStatus.SelectedValue)
                                            , 1
                                            , gvElogTransaction.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount
            );
        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        if (iTotalPageCount == 0)
        {
            return;
        }


        gvElogTransaction.CurrentPageIndex = iTotalPageCount - 1;
        //gvElogTransaction.Rebind();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //ShowToolBar();
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
            toolbar.AddFontAwesomeButton("", "Chief Engineer Signature Done", "<img src=\"../css/Theme1/images/lock_3.png\" />", "CHIEFENGGUNLOCK");
        }
        else
        {
            toolbar.AddFontAwesomeButton("", "Chief Engineer Signature Pending", "<img src=\"../css/Theme1/images/unlock_2.png\" />", "CHIEFENGGSIGN");
        }
        //toolbar.AddFontAwesomeButton("", "User Guide", "<i class=\"fas fa-question\"></i>", "HELP", ToolBarDirection.Right);
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

        DataSet ds = PhoenixLogFuelChangeOver.ChiefEnggSignatureFuelChangeSelect(usercode, txtIds);
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
        DataSet ds = PhoenixLogFuelChangeOver.LogFuelChangeStatusList();
        ddlStatus.DataSource = ds;
        ddlStatus.DataTextField = "FLDSTATUS";
        ddlStatus.DataValueField = "FLDLOGSTATUSID";
        ddlStatus.DataBind();
        DropDownListItem item = new DropDownListItem("All", string.Empty);
        item.Selected = true;
        ddlStatus.Items.Insert(0, item);
    }

    private void SetChiefEngineerSignature()
    {
        if (Filter.MasterSignatureCriteria != null)
        {
            NameValueCollection nvc = Filter.MasterSignatureCriteria;
            int masterUserCode = Convert.ToInt32(nvc["usercode"]);
            string masterSignature = nvc["signature"];
            string masterName = nvc["mastername"];
            DateTime masterSignedDate = Convert.ToDateTime(nvc["signeddate"]);
            string txids = nvc["txids"];
            PhoenixLogFuelChangeOver.ChiefEnggSignatureFuelChange(masterUserCode, txids, masterName, masterSignedDate);
            Filter.MasterSignatureCriteria = null;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SetChiefEngineerSignature();
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


        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;

        gvElogTransaction.DataSource = new string[] { };
        gvElogTransaction.VirtualItemCount = iRowCount;

    }


    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {
        int pageNumber = pageNo.HasValue ? pageNo.Value : gvElogTransaction.CurrentPageIndex + 1;


        DataSet ds = PhoenixLogFuelChangeOver.FuelOilChangeOverReportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
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
        bool isCheifEngineer = PhoenixElog.validCheifEngineer(rankShortCode);
        DataSet ds = PhoenixLogFuelChangeOver.GetAccessRights(usercode, vesselId, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("CHIEFENGGUNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("TRANSCATIONHISTORY", Convert.ToBoolean(row["FLDHISTORY"]));

            if (isCheifEngineer)
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
        accessrights.Add("HELP", true);

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
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("CHIEFENGGSIGN"))
            {

                string txids = GetLogIDFromGrid();
                if (string.IsNullOrWhiteSpace(txids))
                {
                    ucError.ErrorMessage = "No Records found to Chief Engineer Sign";
                    ucError.Visible = true;
                    return;
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','{0}/Log/ElectricLogCommonMasterSignature.aspx?IsAnnex=1&TxnId={1}',null, 400,280,null,null,{{model:true}});", Session["sitepath"], txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                gvElogTransaction.Rebind();
            }


            if (CommandName.ToUpper().Equals("ADD"))
            {
                int pageNumber = gvElogTransaction.CurrentPageIndex + 1;
                if (ViewState["iTotalCount"].ToString() != "0" && pageNumber.ToString() != ViewState["iTotalCount"].ToString())
                {
                    throw new ArgumentException("Page is already full. Select the empty pages and try again");
                }
                string script = String.Format("javascript:parent.openNewWindow('Log','','" + Session["sitepath"] + "/Log/ElectricLogFuelChangeOver.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                string txids = GetLogIDFromGrid();
                string script = String.Format("javascript:parent.openNewWindow('History','','" + Session["sitepath"] + "/Log/ElectricLogFuelChangeTransactionHistory.aspx?txid={0}',null, null,null,null,null,{{model:true}});", txids);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                gvElogTransaction.Rebind();
            }

            if (CommandName.ToUpper().Equals("CHIEFENGGUNLOCK"))
            {
                string currentUserRank = PhoenixElog.GetRankName(usercode);
                if (PhoenixElog.validCheifEngineer(currentUserRank) == false)
                {
                    ucError.ErrorMessage = "Chief Engineer only unlock the page";
                    ucError.Visible = true;
                    return;
                }
                string logids = GetLogIDFromGrid();
                PhoenixLogFuelChangeOver.ChiefEnggUnlockTheLogFuelChange(usercode, logids);
                gvElogTransaction.Rebind();
            }
            if (CommandName.ToUpper().Equals("HELP"))
            {
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

        DataSet ssrsData = createSSRSData(ds);

        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("applicationcode", "24");
            nvc.Add("reportcode", "OPERATIONREPORTFUELCHANGE");
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

    private DataSet createSSRSData(DataSet ds)
    {
        //DataSet ssrsData = new DataSet();
        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[1];

        for (int i = 1; i < 7; i++)
        {
            dt.Columns.Add("FLDTANK" + i, typeof(string));
            dt.Columns.Add("FLDOLDTANK" + i, typeof(string));
        }

        foreach (DataRow row in dt.Rows)
        {
            for (int i = 1; i <= 6; i++)
            {
                DataRow[] dr = dt1.Select("FLDTRANSACTIONID = '" + row["FLDTRANSACTIONID"].ToString() + "' AND FLDORDERNO = '" + i + "'");
                DataRow[] dr1 = dt.Select("FLDTRANSACTIONID = '" + row["FLDTRANSACTIONID"].ToString() + "'");
                if (dr.Length > 0)
                {
                    string columnName = "FLDTANK" + i.ToString();
                    dr1[0][columnName] = dr[0]["FLDROB"].ToString();

                    columnName = "FLDOLDTANK" + i.ToString();
                    dr1[0][columnName] = dr[0]["FLDOLDROB"].ToString();
                }
            }
        }

        return ds;
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


    protected void Bindcolumns()
    {
        GridColumnGroup parentHeader1 = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(parentHeader1);
        parentHeader1.HeaderText = "FO In Use Before change Over";
        parentHeader1.Name = "BEFORE";
        parentHeader1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        parentHeader1.HeaderStyle.Width = Unit.Parse("80px");

        GridColumnGroup parentHeader2 = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(parentHeader2);
        parentHeader2.HeaderText = "FO In Use After change Over";
        parentHeader2.Name = "AFTER";
        parentHeader2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        parentHeader2.HeaderStyle.Width = Unit.Parse("80px");

        GridColumnGroup start = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(start);
        start.HeaderText = "Start change Over";
        start.Name = "START";
        start.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        start.HeaderStyle.Width = Unit.Parse("80px");

        GridColumnGroup complete = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(complete);
        complete.HeaderText = "Completed change Over";
        complete.Name = "COMPLETED";
        complete.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        complete.HeaderStyle.Width = Unit.Parse("80px");


        GridColumnGroup entry = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(entry);
        entry.HeaderText = "Entry Into / Exit From ECA";
        entry.Name = "ENTRY";
        entry.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        entry.HeaderStyle.Width = Unit.Parse("80px");


        GridColumnGroup tanks = new GridColumnGroup();
        gvElogTransaction.MasterTableView.ColumnGroups.Add(tanks);
        tanks.HeaderText = "ROB of Compliant Fuel Oil at Completion of Change Over for Entry Into ECA & At Commencement of Change Over for Exit From ECA";
        tanks.Name = "TANKS";
        tanks.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        entry.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn serialno = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(serialno);
        serialno.HeaderText = "ID";
        serialno.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        serialno.UniqueName = "FLDROWNUMBER";
        serialno.ReadOnly = true;
        serialno.DataField = "FLDROWNUMBER";
        serialno.DataType = typeof(System.Int32);
        serialno.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        serialno.ItemStyle.Width = Unit.Parse("50px");
        serialno.HeaderStyle.Width = Unit.Parse("50px");


        GridBoundColumn type = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(type);
        type.HeaderText = "Entry / Exit";
        type.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        type.UniqueName = "FLDTRANSACTIONTYPE";
        type.ReadOnly = true;
        type.DataField = "FLDTRANSACTIONTYPE";
        type.DataType = typeof(System.String);
        type.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        type.ItemStyle.Width = Unit.Parse("50px");
        type.HeaderStyle.Width = Unit.Parse("50px");

        GridBoundColumn before = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(before);
        before.HeaderText = "Type / Sulphur/ BDN Ref";
        before.UniqueName = "FLDBEFOREFUELTYPE";
        before.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        before.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        before.ColumnGroupName = "BEFORE";
        before.ReadOnly = true;
        before.DataField = "FLDBEFOREFUELTYPE";
        before.DataType = typeof(System.String);
        before.ItemStyle.Width = Unit.Parse("90px");
        before.HeaderStyle.Width = Unit.Parse("90px");

        GridBoundColumn after = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(after);
        after.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        after.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        after.HeaderText = "Type / Sulphur/ BDN Ref";
        after.UniqueName = "FLDAFTERFUELTYPE";
        after.ColumnGroupName = "AFTER";
        after.ReadOnly = true;
        after.DataField = "FLDAFTERFUELTYPE";
        after.DataType = typeof(System.String);
        after.ItemStyle.Width = Unit.Parse("90px");
        after.HeaderStyle.Width = Unit.Parse("90px");

        //FLDTRANSACTIONID
        GridBoundColumn id = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(id);
        id.HeaderText = "primaryId";
        id.UniqueName = "FLDTRANSACTIONID";
        id.ReadOnly = true;
        id.DataField = "FLDTRANSACTIONID";
        id.DataType = typeof(System.Guid);
        id.Visible = false;


        GridBoundColumn startdate = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(startdate);
        startdate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        startdate.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        startdate.HeaderText = "Date / Time";
        startdate.UniqueName = "FLDSTARTDATE";
        startdate.ColumnGroupName = "START";
        startdate.ReadOnly = true;
        startdate.DataField = "FLDSTARTDATE";
        startdate.DataType = typeof(System.DateTime);
        startdate.DataFormatString = "{0:dd-MMM-yyyy hh:mm}";
        startdate.ItemStyle.Width = Unit.Parse("80px");
        startdate.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn startlocation = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(startlocation);
        startlocation.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        startlocation.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        startlocation.HeaderText = "Position";
        startlocation.UniqueName = "FLDSTARTLOCATION";
        startlocation.ColumnGroupName = "START";
        startlocation.ReadOnly = true;
        startlocation.DataField = "FLDSTARTLOCATION";
        startlocation.DataType = typeof(System.String);
        startlocation.ItemStyle.Width = Unit.Parse("80px");
        startlocation.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn completedate = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(completedate);
        completedate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        completedate.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        completedate.HeaderText = "Date / Time";
        completedate.UniqueName = "FLDCOMPLETEDATE";
        completedate.ColumnGroupName = "COMPLETED";
        completedate.ReadOnly = true;
        completedate.DataField = "FLDCOMPLETEDATE";
        completedate.DataType = typeof(System.DateTime);
        completedate.DataFormatString = "{0:dd-MMM-yyyy / HH:mm}";
        completedate.ItemStyle.Width = Unit.Parse("80px");
        completedate.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn completelocation = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(completelocation);
        completelocation.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        completelocation.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        completelocation.HeaderText = "Position";
        completelocation.UniqueName = "FLDCOMPLETELOCATION";
        completelocation.ColumnGroupName = "COMPLETED";
        completelocation.ReadOnly = true;
        completelocation.DataField = "FLDCOMPLETELOCATION";
        completelocation.DataType = typeof(System.String);
        completelocation.ItemStyle.Width = Unit.Parse("80px");
        completelocation.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn entrydate = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(entrydate);
        entrydate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        entrydate.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        entrydate.HeaderText = "Date / Time";
        entrydate.UniqueName = "FLDENTRYDATE";
        entrydate.ColumnGroupName = "ENTRY";
        entrydate.ReadOnly = true;
        entrydate.DataField = "FLDENTRYDATE";
        entrydate.DataType = typeof(DateTime);
        entrydate.DataFormatString = "{0:dd-MMM-yyyy hh:mm}";
        entrydate.ItemStyle.Width = Unit.Parse("80px");
        entrydate.HeaderStyle.Width = Unit.Parse("80px");

        GridBoundColumn entrylocation = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(entrylocation);
        entrylocation.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        entrylocation.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

        entrylocation.HeaderText = "Position";
        entrylocation.UniqueName = "FLDENTRYLOCATION";
        entrylocation.ColumnGroupName = "ENTRY";
        entrylocation.ReadOnly = true;
        entrylocation.DataField = "FLDENTRYLOCATION";
        entrylocation.DataType = typeof(System.String);
        entrylocation.ItemStyle.Width = Unit.Parse("80px");
        entrylocation.HeaderStyle.Width = Unit.Parse("80px");


        DataTable dt = PhoenixMarpolLogNOX.AnnexureTankDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                GridNumericColumn tankname = new GridNumericColumn();
                gvElogTransaction.MasterTableView.Columns.Add(tankname);
                tankname.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                tankname.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                tankname.HeaderText = dr["FLDTANKNAME"].ToString();
                tankname.UniqueName = dr["FLDID"].ToString();
                tankname.ColumnGroupName = "TANKS";
                tankname.ReadOnly = true;
                tankname.DataType = typeof(System.String);
                tankname.ItemStyle.Width = Unit.Parse("70px");
                tankname.HeaderStyle.Width = Unit.Parse("70px");
                tankname.DataFormatString = "{0:n0}";
                tankname.DecimalDigits = 2;
            }

        }

        GridBoundColumn machinery = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(machinery);
        machinery.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        machinery.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        machinery.HeaderText = "Machinery Which Had FO Changed Over";
        machinery.UniqueName = "FLDMACHINERY";
        machinery.ReadOnly = true;
        machinery.DataField = "FLDMACHINERY";
        machinery.DataType = typeof(System.String);
        machinery.ItemStyle.Width = Unit.Parse("100px");
        machinery.HeaderStyle.Width = Unit.Parse("100px");

        GridBoundColumn signature = new GridBoundColumn();
        gvElogTransaction.MasterTableView.Columns.Add(signature);
        signature.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        signature.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        signature.HeaderText = "OIC & CE Signature";
        signature.UniqueName = "FLDSIGNATURE";
        signature.ReadOnly = true;
        signature.DataType = typeof(System.String);
        signature.ItemStyle.Width = Unit.Parse("100px");
        signature.HeaderStyle.Width = Unit.Parse("100px");
    }
    protected void gvElogTransaction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        DataSet ds = PhoenixLogFuelChangeOver.FuelOilChangeOverSearch(
                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , General.GetNullableDateTime(txtFromDate.Text)
                                            , General.GetNullableDateTime(txtToDate.Text)
                                            , General.GetNullableInteger(ddlStatus.SelectedValue)
                                            , gvElogTransaction.CurrentPageIndex + 1
                                            , gvElogTransaction.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount
            );


        ViewState["iRowCounnt"] = iRowCount;
        ViewState["iTotalCount"] = iTotalPageCount;


        gvElogTransaction.DataSource = ds;
        gvElogTransaction.VirtualItemCount = iRowCount;
    }

    protected void gvElogTransaction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DataSet ds = (DataSet)((RadGrid)sender).DataSource;
            DataTable rob = ds.Tables[1];

            GridDataItem item = (GridDataItem)e.Item;

            foreach (GridColumn c in gvElogTransaction.Columns)
            {
                if (General.GetNullableGuid(c.UniqueName) != null)
                {
                    DataRow[] dr = rob.Select("FLDTRANSACTIONID = '" + drv["FLDTRANSACTIONID"].ToString() + "' AND FLDTANKID = '" + c.UniqueName + "'");


                    if (dr.Length > 0)
                    {
                        //item[c.UniqueName].Text = dr[0]["FLDROB"].ToString();
                        if (drv["FLDISDELETED"].ToString() == "True")
                            item[c.UniqueName].Text = "<S>" + dr[0]["FLDROB"].ToString() + "</S>";
                        else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                            item[c.UniqueName].Text = "<S>" + dr[0]["FLDOLDROB"].ToString() + "</S></br>" + dr[0]["FLDROB"].ToString();
                        else
                            item[c.UniqueName].Text = dr[0]["FLDROB"].ToString();
                    }
                    else
                        item[c.UniqueName].Text = "-";
                }
                if (c.UniqueName == "FLDTRANSACTIONTYPE")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDTRANSACTIONTYPETRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDTRANSACTIONTYPEHISTORY"].ToString() + "</S></br>" + drv["FLDTRANSACTIONTYPETRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDTRANSACTIONTYPETRANSACTION"].ToString();
                }

                else if (c.UniqueName == "FLDBEFOREFUELTYPE")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDBEFOREFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELBDNTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDBEFOREFUELTYPEHISTORY"].ToString() + "/" + drv["FLDBEFOREFUELSULPHURHISTORY"].ToString() + "/" + drv["FLDBEFOREFUELBDNHISTORY"].ToString() + "</S></br>" + drv["FLDBEFOREFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELBDNTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDBEFOREFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDBEFOREFUELBDNTRANSACTION"].ToString();
                }
                else if (c.UniqueName == "FLDAFTERFUELTYPE")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDAFTERFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELBDNTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDAFTERFUELTYPEHISTORY"].ToString() + "/" + drv["FLDAFTERFUELSULPHURHISTORY"].ToString() + "/" + drv["FLDAFTERFUELBDNHISTORY"].ToString() + "</S></br>" + drv["FLDAFTERFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELBDNTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDAFTERFUELTYPETRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELSULPHURTRANSACTION"].ToString() + "/" + drv["FLDAFTERFUELBDNTRANSACTION"].ToString();
                }
                else if (c.UniqueName == "FLDSTARTDATE")
                {
                    item[c.UniqueName].Text = Convert.ToDateTime(drv["FLDSTARTDATE"].ToString()).ToString("dd-MMM-yyyy")+" / </br>"+ Convert.ToDateTime(drv["FLDSTARTDATE"].ToString()).ToString("HH:mm");
                }
                else if (c.UniqueName == "FLDSTARTLOCATION")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDSTARTLOCATIONTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDSTARTLOCATIONHISTORY"].ToString() + "</S></br>" + drv["FLDSTARTLOCATIONTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDSTARTLOCATIONTRANSACTION"].ToString();
                }
                else if (c.UniqueName == "FLDCOMPLETEDATE")
                {
                    item[c.UniqueName].Text = Convert.ToDateTime(drv["FLDCOMPLETEDATE"].ToString()).ToString("dd-MMM-yyyy") +" / "+ Convert.ToDateTime(drv["FLDCOMPLETEDATE"].ToString()).ToString("HH:mm");
                }
                else if (c.UniqueName == "FLDCOMPLETELOCATION")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDCOMPLETELOCATIONTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDCOMPLETELOCATIONHISTORY"].ToString() + "</S></br>" + drv["FLDCOMPLETELOCATIONTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDCOMPLETELOCATIONTRANSACTION"].ToString();
                }
                else if (c.UniqueName == "FLDENTRYLOCATION")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDENTRYLOCATIONTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDENTRYLOCATIONHISTORY"].ToString() + "</S></br>" + drv["FLDENTRYLOCATIONTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDENTRYLOCATIONTRANSACTION"].ToString();
                }
                else if (c.UniqueName == "FLDMACHINERY")
                {
                    if (drv["FLDISDELETED"].ToString() == "True")
                        item[c.UniqueName].Text = "<S>" + drv["FLDMACHINERYTRANSACTION"].ToString() + "</S>";
                    else if (drv["FLDISAMEND"].ToString() == "True" && drv["FLDISDELETED"].ToString() == "")
                        item[c.UniqueName].Text = "<S>" + drv["FLDMACHINERYHISTORY"].ToString() + "</S></br>" + drv["FLDMACHINERYTRANSACTION"].ToString();
                    else
                        item[c.UniqueName].Text = drv["FLDMACHINERYTRANSACTION"].ToString();
                }
            }

            item["FLDSIGNATURE"].Text = drv["FLDCHIEFRANK"].ToString() + " - " + drv["FLDCHIEFNAME"].ToString();
            RadLabel lblIsMissedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");
            LinkButton MissedEntry = (LinkButton)e.Item.FindControl("imgFlag");
            if (lblIsMissedEntry != null && MissedEntry != null)
            {
                if (lblIsMissedEntry.Text == "1")
                {
                    MissedEntry.Visible = true;

                }
            }
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
                // check the access rights
                Dictionary<string, bool> accessRight = GetAccessRights();
                if (accessRight["AMEND"] == false)
                {
                    throw new Exception("You don't have access rights");
                }

                RadLabel lblLogBookId = (RadLabel)e.Item.FindControl("lblLogBookId");
                string scriptpopup = "javascript:parent.openNewWindow('LogDelete','','" + Session["sitepath"] + "/Log/ElectricLogFuelChangeBookEntryDelete.aspx?TxnId=" + lblLogBookId.Text + "', 'false', '400', '250', null, null, { 'disableMinMax': true });";
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
                string scriptpopup = String.Format("javascript:parent.openNewWindow('Log','','{0}/Log/{1}?View=true&LogBookId={2}');", Session["sitepath"], "ElectricLogFuelChangeOver.aspx", lblLogBookId.Text);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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
                RadLabel lblLogId = (RadLabel)e.Item.FindControl("lblLogId");
                LinkButton btnCheifEngineerSignature = (LinkButton)e.Item.FindControl("btnCheifEngineerSignature");
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
        LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgFlag");
        RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");

        if (IsBackDatedEntry.Text == "1")
        {
            BackDatedEntry.Visible = true;
        }

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
        GetChiefEnggSignedName(txids);
        ShowToolBar();

        foreach (GridColumn c in gvElogTransaction.Columns)
        {
            if (c.UniqueName == "ACTION")
                c.OrderIndex = gvElogTransaction.Columns.Count + 2;
        }
        gvElogTransaction.MasterTableView.Rebind();

    }
}