using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceVesselCertificateTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCertificateTemplate')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplate.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplate.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuCertificateTemplate.AccessRights = this.ViewState;
            MenuCertificateTemplate.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);
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
    protected void MenuCertificateTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvCertificateTemplate.EditIndex = -1;
                gvCertificateTemplate.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                txtTemplatecode.Text = "";
                txtTemplatename.Text = "";
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Template Code", "Template Name" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselCertificateTemplateSearch(General.GetNullableString(txtTemplatecode.Text), General.GetNullableString(txtTemplatename.Text), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Vessel Certificate Template", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Template Code", "Template Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselCertificateTemplateSearch(General.GetNullableString(txtTemplatecode.Text), General.GetNullableString(txtTemplatename.Text), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCertificateTemplate", "Vessel Certificate Template", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCertificateTemplate.DataSource = ds;
            gvCertificateTemplate.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCertificateTemplate);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            
            gvCertificateTemplate.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificateTemplate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;


            UpdateCertificateTemplate(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplateidEdit")).Text.Trim()),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTemplatecodeEdit")).Text.Trim(),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTemplatenameEdit")).Text.Trim());

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
     
    }
    private void UpdateCertificateTemplate(int Templateid, string Templatecode, string Templatename)
    {
        if (!IsValidTemplate(Templatecode, Templatename))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselCertificateTemplateUpdate(Templateid, Templatecode, Templatename);
    }
    protected void gvCertificateTemplate_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
       

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtTemplatecodeEdit")).Focus();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtTemplatenameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificateTemplate_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = sender as GridView;
            string templateid = ((Label)_gridView.Rows[de.RowIndex].FindControl("lblTemplateId")).Text;
            int itemplateid = int.Parse(templateid);
            PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselCertificateTemplateDelete(itemplateid);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificateTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
              
                string templatecode = ((TextBox)_gridView.FooterRow.FindControl("txtTemplatecodeAdd")).Text.Trim();
                string templatename = ((TextBox)_gridView.FooterRow.FindControl("txtTemplatenameAdd")).Text.Trim();

               if (!IsValidTemplate(templatecode, templatename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselCertificateTemplateInsert
                    (templatecode, templatename);
                BindData();
            }

            //if (e.CommandName.ToUpper().Equals("SELECT"))
            //{
            //    string templateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplateid")).Text.Trim();
            //    string templatename = ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblTemplatename")).Text.Trim();
            //    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplateDetail.aspx?TEMPLATEID=" + templateid + "&TEMPLATENAME=" + templatename, false);
            //}
            if (e.CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                string templateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplateid")).Text.Trim();
                string templatename = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplatename")).Text.Trim();
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplateDetail.aspx?TEMPLATEID=" + templateid + "&TEMPLATENAME=" + templatename, false);
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidTemplate(string Templatecode, string Templatename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Templatecode.Trim().Equals(""))
            ucError.ErrorMessage = "Template Code is required.";

        if (Templatename.Trim().Equals(""))
            ucError.ErrorMessage = "Template Name is required.";

        return (!ucError.IsError);
    }
    protected void gvCertificateTemplate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
        SetPageNavigator();
    }
    protected void gvCertificateTemplate_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
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
    protected void gvCertificateTemplate_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            gvCertificateTemplate.SelectedIndex = -1;
            gvCertificateTemplate.EditIndex = -1;

            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
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

}
