using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Export2XL;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

public partial class Common_CommonReportRequestList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDeficiencyXLReportRequestList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClick");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClickEdit");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Common/CommonReportRequestList.aspx", "Download Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDeficiencyXLReportRequestList')", "Print Grid", "icon_print.png", "PRINT");
            MenuDeficiencyXLReportRequestList.AccessRights = this.ViewState;
            MenuDeficiencyXLReportRequestList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void DeficiencyXLReportRequestList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREPORTNAME", "FLDREQUESTEDBY", "FLDREQUESTEDDATE", "FLDREPORTGENERATED", "FLDMAILSENTYN", "FLDSTATUS", "FLDCOMPLETEDDATE" };
        string[] alCaptions = { "Report Name", "Requested By", "Requested Date", "Report Generated Y/N", "Mail Sent Y/N", "Status", "Completed Date" };

        DataSet ds = PhoenixXLReportRequest.ReportRequestSearch((int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDeficiencyXLReportRequestList", "XL Report Request Status", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDeficiencyXLReportRequestList.DataSource = ds;
            gvDeficiencyXLReportRequestList.DataBind();

            if (ViewState["CURRENTINDEX"] == null)
            {
                ViewState["CURRENTINDEX"] = ds.Tables[0].Rows[0]["FLDREPORTREQUESTID"].ToString();
                Session["REPORTREQUESTDTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                gvDeficiencyXLReportRequestList.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            ds.Tables[0].Rows.Clear();
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDeficiencyXLReportRequestList);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREPORTNAME", "FLDREQUESTEDBY", "FLDREQUESTEDDATE", "FLDREPORTGENERATED", "FLDMAILSENTYN", "FLDSTATUS", "FLDCOMPLETEDDATE" };
            string[] alCaptions = { "Report Name", "Requested By", "Requested Date", "Report Generated Y/N", "Mail Sent Y/N", "Status", "Completed Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixXLReportRequest.ReportRequestSearch((int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("XL Report Request Status", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    private void SetRowSelection()
    {
        string a = ViewState["CURRENTINDEX"].ToString();
        gvDeficiencyXLReportRequestList.SelectedIndex = -1;
        for (int i = 0; i < gvDeficiencyXLReportRequestList.Rows.Count; i++)
        {
            if (gvDeficiencyXLReportRequestList.DataKeys[i].Value.ToString().Equals(ViewState["CURRENTINDEX"].ToString()))
            {
                gvDeficiencyXLReportRequestList.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    private static string AttachmentsFolderPath(int ConfigCode)
    {
        string attachmentfolder = string.Empty;

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@CONFIGCODE", SqlDbType.Int, 4, ParameterDirection.Input, ConfigCode));
        DataTable dt = DataAccess.ExecSPReturnDataTable("PRCONFIGURATIONSETTINGEDIT", ParameterList);
        if (dt.Rows.Count > 0)
        {
            attachmentfolder = dt.Rows[0]["FLDATTACHMENTPATH"].ToString();
        }
        return attachmentfolder;
    }

    protected void gvDeficiencyXLReportRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }

            if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
                Label lblReportName = (Label)_gridView.Rows[nCurrentRow].FindControl("lblReportName");
                Label lblReportRequestId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblReportRequestId");

                string file = AttachmentsFolderPath(1) + "/XLReportRequest/" + lblReportName.Text + "_" + lblReportRequestId.Text + ".xls";
                System.IO.FileInfo file1 = new System.IO.FileInfo(file);
                if (file1.Exists)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name);
                    HttpContext.Current.Response.AddHeader("Content-Length", file1.Length.ToString());
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.WriteFile(file1.FullName);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    ucError.ErrorMessage = "Report excel does not exists";
                    ucError.Visible = true;
                    return;
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmComplete_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

        //    if (ucCM.confirmboxvalue == 1)
        //    {
        //        if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId.ToString() != string.Empty)
        //        {
        //            PhoenixInspectionAuditSchedule.UpdateAuditScheduleStatus(
        //               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //               new Guid(Filter.CurrentAuditScheduleId.ToString()));

        //            ucStatus.Text = "Audit is Completed";
        //            Filter.CurrentAuditScheduleId = null;
        //            BindData();
        //            SetPageNavigator();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //    return;
        //}
    }
    protected void gvDeficiencyXLReportRequestList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        BindPageURL(nCurrentRow);
    }


    protected void gvDeficiencyXLReportRequestList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string lblReportRequestId = ((Label)e.Row.FindControl("lblReportRequestId")).Text;
            LinkButton lnkReportName = (LinkButton)e.Row.FindControl("lnkReportRequest");
            string reportname = lnkReportName.Text;

            if (reportname != null)
            {
                lnkReportName.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Common/CommonReportRequestCriteria.aspx?REPORTREQUESTID=" + lblReportRequestId + "'); return true;");
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblReportRequestId = (Label)gvDeficiencyXLReportRequestList.Rows[rowindex].FindControl("lblReportRequestId");
            if (lblReportRequestId != null)
            {
                ViewState["CURRENTINDEX"] = lblReportRequestId.Text;
                Label lblReportRequestDtKey = (Label)gvDeficiencyXLReportRequestList.Rows[rowindex].FindControl("lblReportRequestDtKey");
                if (lblReportRequestDtKey != null)
                    Session["REPORTDTKEY"] = lblReportRequestDtKey.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDeficiencyXLReportRequestList.SelectedIndex = -1;
        gvDeficiencyXLReportRequestList.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvDeficiencyXLReportRequestList.SelectedIndex = -1;
        gvDeficiencyXLReportRequestList.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDeficiencyXLReportRequestList.SelectedIndex = -1;
        gvDeficiencyXLReportRequestList.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
