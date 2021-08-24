using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class InspectionVesselListWithNilIncidents : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionVesselListWithNilIncidents.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvNIL')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Inspection/InspectionIncidentReportByPartOfBodyInjury.aspx", "Search", "search.png", "FIND");
            //toolbar.AddImageButton("../Inspection/InspectionIncidentReportByPartOfBodyInjury.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuNilIncident.AccessRights = this.ViewState;
            MenuNilIncident.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();

            toolbar.AddButton("By Category", "CATEGORY");
            toolbar.AddButton("By Vessel Type", "VESSELTYPE");
            toolbar.AddButton("By Cons. Category", "CONSEQUENCETYPE");
            toolbar.AddButton("By Work Inj. Category", "WORKINJURY");
            toolbar.AddButton("By Inj. Type", "INJURYTYPE");
            toolbar.AddButton("By Inj. Location", "INJURYLOCATION");
            toolbar.AddButton("By Part Of Body", "PARTOFBODYINJURY");
            toolbar.AddButton("By Unsafe Acts/Conditions", "UNSAFE");
            toolbar.AddButton("Vsls with NIL Inc./Near Miss", "NILINCIDENT");

            MenuNilIncidentGeneral.AccessRights = this.ViewState;
            MenuNilIncidentGeneral.MenuList = toolbar.Show();
            MenuNilIncidentGeneral.SelectedMenuIndex = 8;
            BindData();
            SetPageNavigator();

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

        string[] alColumns = { "FLDSERIALNO", "FLDVESSELNAME" };
        string[] alCaptions = { "S.No", "Vessel Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixInspectionReports.SearchNilIncidentsByVessel(
                                                    (int)ViewState["PAGENUMBER"]
                                                    , iRowCount
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselWithNilIncidentsList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Vessel List with NIL Incidents </h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }


    protected void MenuNilIncident_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();

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

        string[] alColumns = { "FLDSERIALNO", "FLDVESSELNAME" };
        string[] alCaptions = { "S.No", "Vessel Name" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionReports.SearchNilIncidentsByVessel(
                                                    (int)ViewState["PAGENUMBER"]
                                                    , General.ShowRecords(null)
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    );

        General.SetPrintOptions("gvNIL", "Vessels with Nil Incidents/Near Miss", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvNIL.DataSource = ds.Tables[0];
            gvNIL.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvNIL);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvNIL_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvNIL.EditIndex = -1;
        gvNIL.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvNIL.EditIndex = -1;
        gvNIL.SelectedIndex = -1;
        int result;
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
        gvNIL.SelectedIndex = -1;
        gvNIL.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvNIL.EditIndex = -1;
        gvNIL.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void MenuNilIncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("CATEGORY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByCategory.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("VESSELTYPE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByVesselType.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("CONSEQUENCETYPE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByConsquenceCategory.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("WORKINJURY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByWorkInjury.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("INJURYLOCATION"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByInjuryLocation.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("INJURYTYPE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByWorkInjuryType.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("PARTOFBODYINJURY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByPartOfBodyInjury.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("UNSAFE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByUnSafeActsConditions.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("NILINCIDENT"))
        {
            Response.Redirect("../Inspection/InspectionVesselListWithNilIncidents.aspx");
        }
    }
}
