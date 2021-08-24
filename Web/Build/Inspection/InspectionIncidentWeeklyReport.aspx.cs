using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionIncidentWeeklyReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentWeeklyReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvIncident')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentWeeklyReport.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentWeeklyReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuIncident.AccessRights = this.ViewState;
            MenuIncident.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.Equals(0))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Reference Number", "Classification", "Category", "Subcategory", "Consequence Category", "Title", "Reported Date", "Incident Date", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();


        ds = PhoenixInspectionIncidentReport.IncidentWeeklyReportSearch(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue)
                                                               , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                               , General.GetNullableInteger(ucStatus.SelectedHard)
                                                               , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"]
                                                               , gvIncident.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


        General.ShowExcel("Incident/Near Miss", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuIncident_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvIncident.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucStatus.SelectedHard = "";
                ddlIncidentNearmiss.SelectedValue = "DUMMY";
                ucVessel.SelectedVessel = "";
                BindData();
                gvIncident.Rebind();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Reference Number", "Classification", "Category", "Subcategory", "Consequence Category", "Title", "Reported Date", "Incident Date", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionIncidentReport.IncidentWeeklyReportSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue)
                                                                , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                , General.GetNullableInteger(ucStatus.SelectedHard)
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvIncident.CurrentPageIndex + 1
                                                                , gvIncident.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvIncident", "Incident/Near Miss", alCaptions, alColumns, ds);

        gvIncident.DataSource = ds;
        gvIncident.VirtualItemCount = iRowCount;
    }

    protected void gvIncident_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    //protected void gvIncident_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //{
    //    //    if (ViewState["SORTEXPRESSION"] != null)
    //    //    {
    //    //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //    //        if (img != null)
    //    //        {
    //    //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //    //                img.Src = Session["images"] + "/arrowUp.png";
    //    //            else
    //    //                img.Src = Session["images"] + "/arrowDown.png";

    //    //            img.Visible = true;
    //    //        }
    //    //    }
    //    //}

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //    }
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();

    }
    protected void ddlIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindData();
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvIncident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

