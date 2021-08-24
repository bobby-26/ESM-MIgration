using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceVesselCertificateTemplateDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarheader = new PhoenixToolbar();
            toolbarheader.AddButton("Back", "BACK");
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbarheader.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplateDetail.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvVesselCertificateTemplateDetail')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplateDetail.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplateDetail.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            
            MenuVesselCertificateTemplateDetail.AccessRights = this.ViewState;
            MenuVesselCertificateTemplateDetail.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            
            if (!IsPostBack)
            {
                ViewState["TEMPLATEID"] = null;
                if (Request.QueryString["TEMPLATEID"] != null)
                {
                    ViewState["TEMPLATEID"] = Request.QueryString["TEMPLATEID"];
                    ViewState["TEMPLATENAME"] = Request.QueryString["TEMPLATENAME"];
                }
                ViewState["PAGENUMBER"] = 1;
            }
            txtTemplateName.Text = ViewState["TEMPLATENAME"].ToString();
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselCertificateTemplate.aspx", false);
        }
    }
    protected void MenuVesselCertificateTemplateDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvVesselCertificateTemplateDetail.EditIndex = -1;
                gvVesselCertificateTemplateDetail.SelectedIndex = -1;
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
                txtCertificateName.Text="";
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

        string[] alColumns = {"FLDCERTIFICATENAME","FLDSORTORDER" };
        string[] alCaptions = {"Certificate","Sortorder" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        int itemplateid = int.Parse(ViewState["TEMPLATEID"].ToString());

        ds = PhoenixPlannedMaintenanceVesselCertificateTemplateDetail.VesselCertificateTemplateDetailSearch(itemplateid,
            txtCertificateName.Text
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Vessel Certificate Template", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        if (ViewState["TEMPLATEID"] == null)
            return;

        int itemplateid = int.Parse(ViewState["TEMPLATEID"].ToString());
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCERTIFICATENAME", "FLDSORTORDER"};
        string[] alCaptions = { "Certificate", "Sort Order"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateTemplateDetail.VesselCertificateTemplateDetailSearch(itemplateid,
            txtCertificateName.Text
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVesselCertificateTemplateDetail", "Vessel Certificate Template", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselCertificateTemplateDetail.DataSource = ds;
            gvVesselCertificateTemplateDetail.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselCertificateTemplateDetail);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvVesselCertificateTemplateDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string Certificateid = ((UserControlCertificate)_gridView.FooterRow.FindControl("ucCertificateAdd")).SelectedCountry;
                string sortorder = ((TextBox)_gridView.FooterRow.FindControl("txtSortorderAdd")).Text.Trim();
                int itemplateid = int.Parse(ViewState["TEMPLATEID"].ToString());

                if (!IsValidCertificateTemplate(Certificateid, sortorder))
                {
                    ucError.Visible = true;
                    return;
                }
                int icertificate =int.Parse(Certificateid);
                int isortorder = int.Parse(sortorder);
                PhoenixPlannedMaintenanceVesselCertificateTemplateDetail.VesselCertificateTemplateDetailInsert(itemplateid, icertificate, isortorder);

                
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
    private bool IsValidCertificateTemplate(string Certificateid, string sortorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Certificateid) == null)
            ucError.ErrorMessage = "Certificate is required.";

        if (sortorder.Trim() == "")
            ucError.ErrorMessage = "Sort Order is required";
        else if (General.GetNullableInteger(sortorder) == null)
            ucError.ErrorMessage = "Sort Order accepts only numbers.";

        return (!ucError.IsError);
    }
    protected void gvVesselCertificateTemplateDetail_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            UserControlCertificate ucCertificate = (UserControlCertificate)e.Row.FindControl("ucCertificateAdd");

            if (ucCertificate != null)
            {
                ucCertificate.CountryList= PhoenixRegistersCertificates.ListCertificates();
                ucCertificate.DataBind();
            }
        }
    }
    protected void gvVesselCertificateTemplateDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvVesselCertificateTemplateDetail_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtSortorderEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselCertificateTemplateDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string Certificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateid")).Text;

            UpdateCertificateTemplateDetail(((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text, Certificateid,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSortorderEdit")).Text.Trim());

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
    private void UpdateCertificateTemplateDetail(string dtkey, string Certificateid, string sortorder)
    {
        if (!IsValidCertificateTemplate(Certificateid, sortorder))
        {
            ucError.Visible = true;
            return;
        }
        int icertificate = int.Parse(Certificateid);
        int isortorder = int.Parse(sortorder);

        PhoenixPlannedMaintenanceVesselCertificateTemplateDetail.VesselCertificateTemplateDetailUpdate(new Guid(dtkey), isortorder);
    }
    protected void gvVesselCertificateTemplateDetail_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = sender as GridView;

            string dtkey = ((Label)_gridView.Rows[de.RowIndex].FindControl("lbldtkey")).Text;
            PhoenixPlannedMaintenanceVesselCertificateTemplateDetail.VesselCertificateTemplateDetailDelete(new Guid(dtkey));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselCertificateTemplateDetail_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvVesselCertificateTemplateDetail.SelectedIndex = -1;
        gvVesselCertificateTemplateDetail.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvVesselCertificateTemplateDetail_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVesselCertificateTemplateDetail.SelectedIndex = e.NewSelectedIndex;
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {

            gvVesselCertificateTemplateDetail.EditIndex = -1;
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
