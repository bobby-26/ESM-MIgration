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
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class InspectionNonConformityReportByCategoryWiseAudit : PhoenixBasePage
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
                BindDeficiencyCategoryList();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionNonConformityReportByCategoryWiseAudit.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvCNC')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Inspection/InspectionNonConformityReportByCategoryWiseAudit.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionNonConformityReportByCategoryWiseAudit.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuCategoryWiseNC.AccessRights = this.ViewState;
          
            MenuCategoryWiseNC.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Year wise NC Trend", "YEARWISENC");
            toolbar.AddButton("By Category", "CATEGORYWISENC");
            toolbar.AddButton("By Vessel Type", "VESSELTYPEWISENC");
            toolbar.AddButton("By Category & Vessel Type", "BREAKUPWISENC");
            toolbar.AddButton("ISM/ISPS Summary", "AUDITSUMMARY");

            MenuCategoryWiseNCGeneral.AccessRights = this.ViewState;
            MenuCategoryWiseNCGeneral.MenuList = toolbar.Show();
            MenuCategoryWiseNCGeneral.SelectedMenuIndex = 1;
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
    protected void BindDeficiencyCategoryList()
    {
        chkDeficiencyCategory.Items.Clear();
        chkDeficiencyCategory.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 47);
        chkDeficiencyCategory.DataTextField = "FLDQUICKNAME";
        chkDeficiencyCategory.DataValueField = "FLDQUICKCODE";
        chkDeficiencyCategory.DataBind();
    }
    protected string GetSelectedCategory()
    {
        StringBuilder strCategorytype = new StringBuilder();
        foreach (ListItem item in chkDeficiencyCategory.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strCategorytype.Append(item.Value.ToString());
                strCategorytype.Append(",");
            }
        }

        if (strCategorytype.Length > 1)
            strCategorytype.Remove(strCategorytype.Length - 1, 1);

        string Categorytype = strCategorytype.ToString();
        return Categorytype;
    }
    protected void AssginFilter()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucFromMonth", ucFromMonth.SelectedHard);
        criteria.Add("ucToMonth", ucToMonth.SelectedHard);
        criteria.Add("ddlFromYear", ddlFromYear.SelectedQuick);
        criteria.Add("ddlToYear", ddlToYear.SelectedQuick);
        Filter.CurrentPeriodFilterForNC = criteria;
    }
    protected void BindCriteria()
    {
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        if (nvc != null)
        {
            ucFromMonth.SelectedHard = (nvc["ucFromMonth"]);
            ucToMonth.SelectedHard = (nvc != null ? nvc["ucToMonth"] : null);
            ddlFromYear.SelectedQuick = (nvc != null ? nvc["ddlFromYear"] : null);
            ddlToYear.SelectedQuick = (nvc != null ? nvc["ddlToYear"] : null);
        }
    }
    protected void MenuCategoryWiseNC_TabStripCommand(object sender, EventArgs e)
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
                gvCNC.EditIndex = -1;
                gvCNC.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                AssginFilter();
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();    
                String CategoryList = GetSelectedCategory();               
                NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;

                PhoenixInspection2XL.Export2XLInspectionNonConformityCountByCategoryWiseAudit(General.GetNullableString(ddlAuditName.SelectedValue),
                   General.GetNullableInteger(ucInspectionCategory.SelectedHard), General.GetNullableString(CategoryList),
                   General.GetNullableInteger(ucFromMonth.SelectedHard), General.GetNullableInteger(ucToMonth.SelectedHard),
                   General.GetNullableInteger(ddlFromYear.SelectedQuick), General.GetNullableInteger(ddlToYear.SelectedQuick)
                   );
               
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                foreach (ListItem li in chkDeficiencyCategory.Items)
                    li.Selected = false;
                ddlAuditName.SelectedValue = "";
                ucInspectionCategory.SelectedHard = "";
                ucFromMonth.SelectedHard = "";
                ddlFromYear.SelectedQuick = "";
                ucToMonth.SelectedHard = "";
                ddlToYear.SelectedQuick = "";
                chkDeficiencyCategoryAll.Checked = false;
                Filter.CurrentPeriodFilterForNC = null;
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
    protected void chkDeficiencyCategoryAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkDeficiencyCategoryAll.Checked == true)
            {
                foreach (ListItem li in chkDeficiencyCategory.Items)
                    li.Selected = true;              
            }
            else
            {
                foreach (ListItem li in chkDeficiencyCategory.Items)
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDCATEGORYNAME","FLDSHORTCODE", "FLDPERIODNAME", "FLDAVGNCCOUNTPERAUDIT" };
        string[] alCaptions = { "Category","ShortCode", "Period", "Avg NC Count per Audit" };
        DataSet ds = new DataSet();
        String CategoryList = GetSelectedCategory();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        ds = PhoenixInspectionReports.SearchNCCountByCategoryWiseAudit(
                                                               General.GetNullableString(ddlAuditName.SelectedValue)
                                                             , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                                             , General.GetNullableString(CategoryList)
                                                             , General.GetNullableInteger (nvc != null ? nvc["ucFromMonth"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ucToMonth"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlFromYear"] : null)
                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlToYear"] : null)
                                                             , (int)ViewState["PAGENUMBER"]
                                                             , General.ShowRecords(null)
                                                             , ref iRowCount
                                                             , ref iTotalPageCount
                                                             );

        General.SetPrintOptions("gvCNC", "Category Wise NC Trend in Audits", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCNC.DataSource = ds.Tables[0];
            gvCNC.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCNC);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCNC_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        gvCNC.EditIndex = -1;
        gvCNC.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCNC.EditIndex = -1;
        gvCNC.SelectedIndex = -1;
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
        gvCNC.SelectedIndex = -1;
        gvCNC.EditIndex = -1;
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
        if (General.GetNullableString(ddlAuditName.SelectedValue) == null)
            ucError.ErrorMessage = "Audit is required";

        if (General.GetNullableInteger(ucInspectionCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Inspection Category is required";

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
        gvCNC.EditIndex = -1;
        gvCNC.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void MenuCategoryWiseNCGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
       
        if (dce.CommandName.ToUpper().Equals("YEARWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByYearWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("CATEGORYWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByCategoryWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("VESSELTYPEWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByVesselTypeWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("BREAKUPWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("AUDITSUMMARY"))
        {
            Response.Redirect("../Inspection/InspectionNCReportByAuditSummary.aspx");
        }
    }
}
