using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersDMRRangeConfigurationHistory : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in dgCompany.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClick");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuRegistersCompany.AccessRights = this.ViewState;
            MenuRegistersCompany.MenuList = toolbar.Show();
           // MenuRegistersCompany.SetTrigger(pnlCompanyEntry);

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                dgCompany.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
                //SetPageNavigator();
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
        string[] alColumns = { "FLDDISPLAYTEXT", "FLDMINVALUE", "FLDMAXVALUE", "FLDEFFECTIVEDATE", "FLDREQUIREDYESNO" };
        string[] alCaptions = { "Field Name", "Min Value", "Max Value", "Effective Date", "Active Y/N" };



        ds = PhoenixRegistersDMRRangeConfig.DMRRangeConfigHistoryList(
           General.GetNullableInteger(Request.QueryString["Vesselid"].ToString()),
           General.GetNullableGuid(Request.QueryString["Configid"].ToString()),
            dgCompany.CurrentPageIndex+1,
            dgCompany.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=DmrRangConfigHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>DMR Rang Config History</h3></td>");
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

    protected void RegistersCompany_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            //SetPageNavigator();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDISPLAYTEXT", "FLDMINVALUE", "FLDMAXVALUE", "FLDEFFECTIVEDATE", "FLDREQUIREDYESNO" };
        string[] alCaptions = { "Field Name", "Min Value", "Max Value", "Effective Date", "Active Y/N" };

        ds = PhoenixRegistersDMRRangeConfig.DMRRangeConfigHistoryList(
           General.GetNullableInteger(Request.QueryString["Vesselid"].ToString()),
           General.GetNullableGuid(Request.QueryString["Configid"].ToString()),
           dgCompany.CurrentPageIndex + 1,
            dgCompany.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            );



        General.SetPrintOptions("gvCompany", "DMR Rang Config History", alCaptions, alColumns, ds);
        
        dgCompany.DataSource = ds;
        dgCompany.DataBind();

        

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void dgCompany_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void dgCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void dgCompany_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void dgCompany_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    protected void dgCompany_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void dgCompany_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
       // SetPageNavigator();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    dgCompany.SelectedIndex = -1;
    //    dgCompany.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private void DeleteCompany(int CompanyId)
    {
        PhoenixRegistersCompany.DeleteCompany(0, CompanyId);
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void dgCompany_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
