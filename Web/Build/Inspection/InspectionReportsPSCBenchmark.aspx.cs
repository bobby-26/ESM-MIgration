using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionReportsPSCBenchmark : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsPSCBenchmark.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPSC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsPSCBenchmark.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsPSCBenchmark.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");

        MenuPSCList.AccessRights = this.ViewState;
        MenuPSCList.MenuList = toolbar.Show();
        try
        {
            ViewState["PSCTOTALCOUNT"] = "0";
            ViewState["NOOFDEFCOUNT"] = "0";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPANYID"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                SetDefaultDates();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetDefaultDates()
    {
        ucToDate.Text = DateTime.Today.ToString();
        ucFromDate.Text = DateTime.Today.AddMonths(-6).ToString();
    }

    protected void MenuPSCList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPSC.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                ucCountry.SelectedCountry = "";
                ucPort.SelectedSeaport = "";
                ucFlag.SelectedFlag = "";
                SetDefaultDates();
                chkDetainedYN.Checked = false;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindData();
                gvPSC.Rebind();
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

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDDONEDATE", "FLDPORTNAME", "FLDCOUNTRYNAME", "FLDNOOFDEFICIENCIES", "FLDDETAINEDYN", "FLDNOOFDETAINEDDEFICIENCIES" };
        string[] alCaptions = { "Vessel", "IMO No.", "Date of PSC Inspection", "Port of PSC Inspection", "Country Of PSC Inspection", "No.of Deficiencies", "Detained(Yes/No)", "No. of detainable deficiencies" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string detainedYN = chkDetainedYN.Checked == true ? "1" : "0";
        ds = PhoenixInspectionPSCBenchmark.InspectionPSCBenchmarkSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableDateTime(ucFromDate.Text)
                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                                , General.GetNullableInteger(ucCountry.SelectedCountry)
                                                                , General.GetNullableInteger(ucPort.SelectedSeaport)
                                                                , General.GetNullableInteger(detainedYN)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvPSC.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );


        Response.AddHeader("Content-Disposition", "attachment; filename=PSC_Benchmark.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PSC Benchmark</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["PSCTOTALCOUNT"] = ds.Tables[0].Rows[0]["FLDPSCTOTALCOUNT"].ToString();
            ViewState["NOOFDEFCOUNT"] = ds.Tables[0].Rows[0]["FLDTOTALDEFICIENCYCOUNT"].ToString();
        }
        else
        {
            ViewState["PSCTOTALCOUNT"] = "0";
            ViewState["NOOFDEFCOUNT"] = "0";
        }
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        int k = 0;
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
            k = k + 1;
        }
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            if (i == 3)
                Response.Write("<b>" + ViewState["PSCTOTALCOUNT"].ToString() + "</b>");
            if (i == 5)
                Response.Write("<b>" + ViewState["NOOFDEFCOUNT"].ToString() + "</b>");
            else
                Response.Write("");
            Response.Write("</td>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDDONEDATE", "FLDPORTNAME", "FLDCOUNTRYNAME", "FLDNOOFDEFICIENCIES", "FLDDETAINEDYN", "FLDNOOFDETAINEDDEFICIENCIES" };
        string[] alCaptions = { "Vessel", "IMO No.", "Date of PSC Inspection", "Port of PSC Inspection", "Country Of PSC Inspection", "No.of Deficiencies", "Detained(Yes/No)", "No. of detainable deficiencies" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        string detainedYN = chkDetainedYN.Checked == true ? "1" : "0";
        ds = PhoenixInspectionPSCBenchmark.InspectionPSCBenchmarkSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableDateTime(ucFromDate.Text)
                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                                , General.GetNullableInteger(ucCountry.SelectedCountry)
                                                                , General.GetNullableInteger(ucPort.SelectedSeaport)
                                                                , General.GetNullableInteger(detainedYN)
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvPSC.CurrentPageIndex + 1
                                                                , gvPSC.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        General.SetPrintOptions("gvPSC", "PSC Benchmarking Data", alCaptions, alColumns, ds);

        gvPSC.DataSource = ds;
        gvPSC.VirtualItemCount = iRowCount;
    }

    protected void gvPSC_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPSC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (General.GetNullableInteger(drv["FLDPSCTOTALCOUNT"].ToString()) != null)
                {
                    ViewState["PSCTOTALCOUNT"] = int.Parse(drv["FLDPSCTOTALCOUNT"].ToString());
                }
                if (General.GetNullableInteger(drv["FLDTOTALDEFICIENCYCOUNT"].ToString()) != null)
                {
                    ViewState["NOOFDEFCOUNT"] = int.Parse(drv["FLDTOTALDEFICIENCYCOUNT"].ToString());
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblTotalPSCInspection = (RadLabel)e.Item.FindControl("lblTotalPSCInspectionFooter");
            RadLabel lblTotalDeficienciesCount = (RadLabel)e.Item.FindControl("lblTotalDeficienciesCountFooter");

            if (lblTotalPSCInspection != null)
            {
                lblTotalPSCInspection.Text = ViewState["PSCTOTALCOUNT"].ToString();
            }
            if (lblTotalDeficienciesCount != null)
            {

                lblTotalDeficienciesCount.Text = ViewState["NOOFDEFCOUNT"].ToString();
            }
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ucCountry_Cahnged(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ucCountry.SelectedCountry) != null)
            {
                ucPort.SeaportList = PhoenixRegistersSeaport.EditSeaport(null, General.GetNullableInteger(ucCountry.SelectedCountry));
                ucPort.DataBind();
            }
            else
            {
                ucPort.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                ucPort.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPSC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
