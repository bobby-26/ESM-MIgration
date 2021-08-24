using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using Telerik.Web.UI;

public partial class CrewQueryBirthday : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewQueryBirthday.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewQueryBirthdayFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewQueryBirthday.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.BirthdayReportFilter = null;
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuBirthday_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDZONE", "FLDBATCH", "FLDDATEOFBIRTH", "FLDRANKNAME", "FLDLASTVESSEL", "FLDPRESENTVESSEL", "FLDSTATUS" };
        string[] alCaptions = { "File No", "Name", "Zone", "Batch", "Birth Date", "Rank", "Last Vessel", "Present Vessel", "Status" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.BirthdayReportFilter;

        ds = PhoenixCrewReports.BirthdaySearch(nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate").ToString()) : null
                        , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucVessel").ToString()) : null
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucRank").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucZone").ToString()) : null
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrew.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucBatch").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucVesselType").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucManager").ToString()) : null
                        , nvc != null ? General.GetNullableByte(nvc.Get("chkInActive").ToString()) : null);

        General.SetPrintOptions("gvCrew", "Crew Birthday", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDZONE", "FLDBATCH", "FLDDATEOFBIRTH", "FLDLASTVESSEL", "FLDPRESENTVESSEL", "FLDSTATUS" };
        string[] alCaptions = { "File No", "Name", "Rank", "Zone", "Batch", "Birth Date", "Last Vessel", "Present Vessel", "Status" };
        string[] filtercolumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDPRINCIPAL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDMANAGER",
                                     "FLDSELECTEDVESSEL", "FLDSELECTEDRANK", "FLDSELECTEDZONE", "FLDSELECTEDBATCH", "FLDSELECTEDSTATUS" };
        string[] filtercaptions = { "From Date", "To Date", "Principal", "Vessel Type", "Manager", "Vessel", "Rank", "Zone", "Batch", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.BirthdayReportFilter;
        ds = PhoenixCrewReports.BirthdaySearch(nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate").ToString()) : null
                        , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucVessel").ToString()) : null
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucRank").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucZone").ToString()) : null
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount
                        , nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucBatch").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucVesselType").ToString()) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("ucManager").ToString()) : null
                        , nvc != null ? General.GetNullableByte(nvc.Get("chkInActive").ToString()) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewBirthday.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Birthday List</center></h5></td></tr>");
        Response.Write("<tr></tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }



    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            if (lbr != null)
            {
                if (drv["FLDSUPY"].ToString() == "2")
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                else
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDID"].ToString() + "&familyid=" + empid.Text + "'); return false;");
            }
            LinkButton imgSendMail = (LinkButton)e.Item.FindControl("imgSendMail");
            if (imgSendMail != null)
            {
                imgSendMail.Visible = SessionUtil.CanAccess(this.ViewState, imgSendMail.CommandName);
            }
        }
    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SENDMAIL"))
            {

                RadLabel empid = ((RadLabel)e.Item.FindControl("lblEmpNo"));
                RadLabel lblVesselid = ((RadLabel)e.Item.FindControl("lblVesselid"));
                RadLabel lblRankid = ((RadLabel)e.Item.FindControl("lblRankid"));
                RadLabel lblSupy = ((RadLabel)e.Item.FindControl("lblSupy"));
                Response.Redirect("../Crew/CrewEmail.aspx?empid=" + empid.Text + "&vesselid=" + lblVesselid.Text + "&rankid=" + lblRankid.Text + "&bday=1&supy=" + lblSupy.Text);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ShowReport();
        gvCrew.Rebind();
    }
}
