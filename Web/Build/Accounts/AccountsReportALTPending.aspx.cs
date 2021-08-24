using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsReportALTPending : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Confirmation Pending", "PENDINGLIST");
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbarmain.Show();
            MenuRequest.SelectedMenuIndex = 1;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsReportALTPending.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAllotmentreport')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsReportALTPending.aspx", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsReportALTPending.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;
                ClearFilter();
                gvAllotmentreport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAllotmentreport.SelectedIndexes.Clear();
        gvAllotmentreport.EditIndexes.Clear();
        gvAllotmentreport.DataSource = null;
        gvAllotmentreport.Rebind();
    }
    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsReportAllotmentPending.aspx";
            }
            else if (CommandName.ToUpper().Equals("PENDINGLIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsReportALTPending.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?MONTH = " + ddlMonth.SelectedMonth + " & YEAR = " + ddlYear.SelectedYear);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDTYPEDESC", "FLDMONTH", "FLDYEAR", "FLDAMOUNT", "FLDCONFIRMEDYNDESC", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Vessel", "Type", "Month", "Year", "Amount", "Confirm YN", "Created By", "Created On" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string Vessellist = ucVessel.SelectedVessel;
            ds = PhoenixAccountsAllotmentRequest.SearchAlTPendingReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableString(Vessellist)
               , General.GetNullableInteger(ddlMonth.SelectedMonth)
               , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
               , General.GetNullableInteger(ddlType.SelectedValue)
               , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvAllotmentreport.PageSize, ref iRowCount, ref iTotalPageCount);
            gvAllotmentreport.DataSource = ds;
            gvAllotmentreport.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvAllotmentreport", "Allotment Report", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDTYPEDESC", "FLDMONTH", "FLDYEAR", "FLDAMOUNT", "FLDCONFIRMEDYNDESC", "FLDCREATEDBY", "FLDCREATEDDATE" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Vessel", "Type", "Month", "Year", "Amount", "Confirm YN", "Created By", "Created On" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string Vessellist = ucVessel.SelectedVessel;

            ds = PhoenixAccountsAllotmentRequest.SearchAlTPendingReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableString(Vessellist)
            , General.GetNullableInteger(ddlMonth.SelectedMonth)
            , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
            , General.GetNullableInteger(ddlType.SelectedValue)
            , sortexpression, sortdirection, 1, PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);


            Response.AddHeader("Content-Disposition", "attachment; filename= AllotmentReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment  Report </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");

            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td  class='text'>" + dr[alColumns[i]].ToString() + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentreport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotmentreport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotmentreport.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        nvc.Clear();
        ddlMonth.SelectedMonth = DateTime.Now.Month.ToString();
        ddlYear.SelectedYear = DateTime.Now.Year;
        ddlType.SelectedValue = "";
    }
}
