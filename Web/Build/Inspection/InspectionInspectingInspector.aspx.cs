using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionInspectingInspector : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionInspectingInspector.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInspectionInspector')", "Print Grid", "icon_print.png", "PRINT");
            MenuInspectionInspector.AccessRights = this.ViewState;
            MenuInspectionInspector.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
            }
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

        string[] alColumns = { "FLDINSPECTORNAME", "FLDINSPECTORDESIGNATION", "FLDCOMPANYNAME" };
        string[] alCaptions = { "Name", "Vetting Inspection", "Company Name"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionInspectingCompany.ListInspectionInspector(
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
             General.ShowRecords(null),
             ref iRowCount,
             ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=InspectingCompany.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspecting Country</h3></td>");
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

    protected void InspectionInspector_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDINSPECTORNAME", "FLDINSPECTORDESIGNATION", "FLDCOMPANYNAME" };
        string[] alCaptions = { "Name", "Vetting Inspection", "Company Name" };
        DataSet ds = new DataSet();

        ds = PhoenixInspectionInspectingCompany.ListInspectionInspector (
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvInspectionInspector", "Inspecting Country", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvInspectionInspector.DataSource = ds;
            gvInspectionInspector.DataBind();

        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvInspectionInspector );
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();

    }
    protected void gvInspection_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtInspectorNameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvInspection_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            DropDownList ddlCompanyName = (DropDownList)e.Row.FindControl("ddlCompanyNameEdit");
            DataRowView drv1 = (DataRowView)e.Row.DataItem;
            if (ddlCompanyName != null && drv1 != null)
            {
                ddlCompanyName.DataSource = PhoenixInspectionInspectingCompany.ListInspectionCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ddlCompanyName.DataBind();

                //DataRowView drv = (DataRowView)e.Row.DataItem;
                ddlCompanyName.SelectedValue = drv1["FLDCOMPANYID"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {


            DropDownList ddlCompanyName = (DropDownList)e.Row.FindControl("ddlCompanyNameAdd");

            if (ddlCompanyName != null)
            {
                ddlCompanyName.DataSource = PhoenixInspectionInspectingCompany.ListInspectionCompany(PhoenixSecurityContext .CurrentSecurityContext .UserCode);
                ddlCompanyName.DataBind();

                //DataRowView drv = (DataRowView)e.Row.DataItem;
                //ddlVettingInspection.SelectedValue = drv1["FLDVETTINGINSPECTION"].ToString();
            }
        }
    }
    protected void gvInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((TextBox)_gridView.FooterRow.FindControl("txtInspectorNameAdd")).Text,
                                  ((TextBox)_gridView.FooterRow.FindControl("txtInspectorDesignationAdd")).Text,  
                                   ((DropDownList)_gridView.FooterRow.FindControl("ddlCompanyNameAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionInspectingCompany.InsertInspectionInspector(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         ((TextBox)_gridView.FooterRow.FindControl("txtInspectorNameAdd")).Text,
                          ((TextBox)_gridView.FooterRow.FindControl("txtInspectorDesignationAdd")).Text,
                           new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlCompanyNameAdd")).SelectedValue));

                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtInspectorNameAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixInspectionInspectingCompany.DeleteInspectionInspector(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                dtkey);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorNameEdit")).Text,
                                   ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorDesignationEdit")).Text,
                                   ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyNameEdit")).SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixInspectionInspectingCompany.UpdateInspectionInspector(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorNameEdit")).Text,
                                                        ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorDesignationEdit")).Text,
                                                        dtkey,
                                                        new Guid(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyNameEdit")).SelectedValue));

                _gridView.EditIndex = -1;
                BindData();

            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspection_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    private bool IsValidData(string inspectorname,string inspectordesignation, string companyname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (inspectorname.Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (inspectordesignation.Equals(""))
            ucError.ErrorMessage = "Designation is required.";

        if (companyname.Equals("Dummy") || companyname.Equals(""))
            ucError.ErrorMessage = "Company Name is required.";

        return (!ucError.IsError);

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvInspectionInspector.SelectedIndex = -1;
        gvInspectionInspector.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvInspectionInspector.SelectedIndex = -1;
        gvInspectionInspector.EditIndex = -1;
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
        gvInspectionInspector.SelectedIndex = -1;
        gvInspectionInspector.EditIndex = -1;
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
        {
            return true;
        }

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
}
