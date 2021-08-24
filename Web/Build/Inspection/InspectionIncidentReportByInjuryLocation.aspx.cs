using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class InspectionIncidentReportByInjuryLocation : PhoenixBasePage
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
                BindCriteria();
                BindWorkInjuryLocationList();

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionIncidentReportByInjuryLocation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvIL')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Inspection/InspectionIncidentReportByInjuryLocation.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionIncidentReportByInjuryLocation.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuWorkInjuryLocation.AccessRights = this.ViewState;
            MenuWorkInjuryLocation.MenuList = toolbar.Show();
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

            MenuWorkInjuryLocationGeneral.AccessRights = this.ViewState;
            MenuWorkInjuryLocationGeneral.MenuList = toolbar.Show();
            MenuWorkInjuryLocationGeneral.SelectedMenuIndex = 5;
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

    }
    protected void BindWorkInjuryLocationList()
    {
        chkWorkInjuryLocation.Items.Clear();
        chkWorkInjuryLocation.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 66);
        chkWorkInjuryLocation.DataTextField = "FLDQUICKNAME";
        chkWorkInjuryLocation.DataValueField = "FLDQUICKCODE";
        chkWorkInjuryLocation.DataBind();
       
    }
    protected string GetSelectedWorkLocationType()
    {
        StringBuilder strWorkInjuryLocation = new StringBuilder();
        foreach (ListItem item in chkWorkInjuryLocation.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strWorkInjuryLocation.Append(item.Value.ToString());
                strWorkInjuryLocation.Append(",");
            }
        }

        if (strWorkInjuryLocation.Length > 1)
            strWorkInjuryLocation.Remove(strWorkInjuryLocation.Length - 1, 1);

        string WorkInjury = strWorkInjuryLocation.ToString();
        return WorkInjury;
    }
    protected void chkWorkInjuryLocationAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkWorkInjuryLocationAll.Checked == true)
            {
                foreach (ListItem li in chkWorkInjuryLocation.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ListItem li in chkWorkInjuryLocation.Items)
                    li.Selected = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }   
    protected void MenuWorkInjuryLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                     return;
                }
                gvIL.EditIndex = -1;
                gvIL.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                AssginFilter();
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();
                string WorkInjuryLocationList = GetSelectedWorkLocationType();
                PhoenixInspection2XL.Export2XLInspectionIncidentCountByInjuryLocation(General.GetNullableString(WorkInjuryLocationList),
                    General.GetNullableInteger(ucFromMonth.SelectedHard), General.GetNullableInteger(ucToMonth.SelectedHard),
                    General.GetNullableInteger(ddlFromYear.SelectedQuick), General.GetNullableInteger(ddlToYear.SelectedQuick)                    
                    );
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                foreach (ListItem li in chkWorkInjuryLocation.Items)
                    li.Selected = false;
                ucFromMonth.SelectedHard = "";
                ddlFromYear.SelectedQuick = "";
                ucToMonth.SelectedHard = "";
                ddlToYear.SelectedQuick = "";
                chkWorkInjuryLocationAll.Checked = false;
                Filter.CurrentPeriodFilterForIncident = null;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AssginFilter()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucFromMonth", ucFromMonth.SelectedHard);
        criteria.Add("ucToMonth", ucToMonth.SelectedHard);
        criteria.Add("ddlFromYear", ddlFromYear.SelectedQuick);
        criteria.Add("ddlToYear", ddlToYear.SelectedQuick);
        Filter.CurrentPeriodFilterForIncident = criteria;
    }
    protected void BindCriteria()
    {
        NameValueCollection nvc = Filter.CurrentPeriodFilterForIncident;
        if (nvc != null)
        {
            ucFromMonth.SelectedHard = (nvc != null ? nvc["ucFromMonth"] : null);
            ucToMonth.SelectedHard = (nvc != null ? nvc["ucToMonth"] : null);
            ddlFromYear.SelectedQuick = (nvc != null ? nvc["ddlFromYear"] : null);
            ddlToYear.SelectedQuick = (nvc != null ? nvc["ddlToYear"] : null);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINCIDENTLOCATIONNAME", "FLDPERIODNAME", "FLDINCIDENTCOUNT" };
        string[] alCaptions = { "Injury Location", "Period", "Incident Count" };

        DataSet ds = new DataSet();
        string WorkInjuryLocationList = GetSelectedWorkLocationType();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForIncident;
        ds = PhoenixInspectionReports.SearchIncidentByLocation(
                                                               General.GetNullableString(WorkInjuryLocationList)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ucFromMonth"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ucToMonth"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlFromYear"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlToYear"] : null)
                                                             , (int)ViewState["PAGENUMBER"]
                                                             , General.ShowRecords(null)
                                                             , ref iRowCount
                                                             , ref iTotalPageCount
                                                             );

        General.SetPrintOptions("gvIL", "Incidents by Injury Location", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvIL.DataSource = ds.Tables[0];
            gvIL.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvIL);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvIL_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        gvIL.EditIndex = -1;
        gvIL.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvIL.EditIndex = -1;
        gvIL.SelectedIndex = -1;
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
        gvIL.SelectedIndex = -1;
        gvIL.EditIndex = -1;
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

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucFromMonth.SelectedHard) == null)
            ucError.ErrorMessage = "From Month is required.";

        if (General.GetNullableString(ddlFromYear.SelectedQuick) == null)
            ucError.ErrorMessage = "From Year is required.";

        if (General.GetNullableString(ucToMonth.SelectedHard) == null)
            ucError.ErrorMessage = "To Month is required.";

        if (General.GetNullableString(ddlToYear.SelectedQuick) == null)
            ucError.ErrorMessage = "To Year is required.";

        if (General.GetNullableString(ddlFromYear.SelectedQuick) != null && General.GetNullableString(ddlToYear.SelectedQuick) != null &&
            General.GetNullableInteger(ddlToYear.SelectedQuick) < General.GetNullableInteger(ddlFromYear.SelectedQuick))
        {
            ucError.ErrorMessage = "To Year should be greater than From Year.";
            return (!ucError.IsError);
        }

        if (General.GetNullableString(ucFromMonth.SelectedHard) != null && General.GetNullableString(ucToMonth.SelectedHard) != null &&
            General.GetNullableInteger(ddlToYear.SelectedQuick) <= General.GetNullableInteger(ddlFromYear.SelectedQuick) &&
        General.GetNullableInteger(ucToMonth.SelectedHard) < General.GetNullableInteger(ucFromMonth.SelectedHard))
        {
            ucError.ErrorMessage = "To Month should be greater than From Month.";
            return (!ucError.IsError);
        }

        return (!ucError.IsError);
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
        gvIL.EditIndex = -1;
        gvIL.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void MenuWorkInjuryLocationGeneral_TabStripCommand(object sender, EventArgs e)
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
        if (dce.CommandName.ToUpper().Equals("INJURYTYPE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByWorkInjuryType.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("INJURYLOCATION"))
        {
            Response.Redirect("../Inspection/InspectionIncidentReportByWorkInjuryLocation.aspx");
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
