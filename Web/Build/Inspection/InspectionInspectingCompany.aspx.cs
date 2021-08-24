using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionInspectingCompany : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionInspectingCompany.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInspectionCompany')", "Print Grid", "icon_print.png", "PRINT");
            MenuInspectionCompany.AccessRights = this.ViewState;
            MenuInspectionCompany.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
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

        string[] alColumns = { "FLDCOMPANYNAME", "FLDSHORTCODE" };
        string[] alCaptions = { "Company Name", "Audit/Inspection/Vetting" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionInspectingCompany.ListInspectionCompany(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=InspectingCompany.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspecting Company</h3></td>");
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

    protected void InspectionCompany_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDCOMPANYNAME", "FLDSHORTCODE" };
        string[] alCaptions = { "Company Name", "Audit/Inspection/Vetting" };
        DataSet ds = new DataSet();

        ds = PhoenixInspectionInspectingCompany.ListInspectionCompany(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvInspectionCompany", "Inspecting Company", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvInspectionCompany.DataSource = ds;
            gvInspectionCompany.DataBind();

        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvInspectionCompany);
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
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCompanyNameEdit")).Focus();
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
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv1 = (DataRowView)e.Row.DataItem;
            DropDownList ucInspection = (DropDownList)e.Row.FindControl("ucInspectionEdit");
            if (ucInspection != null && drv1 != null)
            {
                ucInspection.DataSource = PhoenixInspection.ListInspectionByCompany(null, null, null, 1, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucInspection.DataTextField = "FLDSHORTCODE";
                ucInspection.DataValueField = "FLDINSPECTIONID";
                ucInspection.DataBind();
                ucInspection.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                ucInspection.SelectedValue = drv1["FLDVETTINGINSPECTIONID"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList ucInspectionAdd = (DropDownList)e.Row.FindControl("ucInspectionAdd");

            if (ucInspectionAdd != null)
            {
                ucInspectionAdd.DataSource = PhoenixInspection.ListInspectionByCompany(null, null, null, 1, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucInspectionAdd.DataTextField = "FLDSHORTCODE";
                ucInspectionAdd.DataValueField = "FLDINSPECTIONID";
                ucInspectionAdd.DataBind();
                ucInspectionAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
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
                if (!IsValidData(((TextBox)_gridView.FooterRow.FindControl("txtCompanyNameAdd")).Text,
                                   ((DropDownList)_gridView.FooterRow.FindControl("ucInspectionAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                
                PhoenixInspectionInspectingCompany.InsertInspectionCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         ((TextBox)_gridView.FooterRow.FindControl("txtCompanyNameAdd")).Text,
                           General.GetNullableGuid(((DropDownList)_gridView.FooterRow.FindControl("ucInspectionAdd")).SelectedValue));

                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtCompanyNameAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixInspectionInspectingCompany.DeleteInspectionCompany (PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                dtkey);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCompanyNameEdit")).Text,
                                   ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ucInspectionEdit")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixInspectionInspectingCompany.UpdateInspectionCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCompanyNameEdit")).Text,
                                                        new Guid(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ucInspectionEdit")).SelectedValue),
                                                        dtkey);

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
    private bool IsValidData(string companyname, string vettinginspection)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (companyname.Equals(""))
            ucError.ErrorMessage = "Company Name is required.";

        if (vettinginspection.Equals("Dummy") || vettinginspection.Equals(""))
            ucError.ErrorMessage = "AuditVetting Inspection is required.";

        return (!ucError.IsError);

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvInspectionCompany .SelectedIndex = -1;
        gvInspectionCompany .EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvInspectionCompany .SelectedIndex = -1;
        gvInspectionCompany .EditIndex = -1;
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
        gvInspectionCompany .SelectedIndex = -1;
        gvInspectionCompany .EditIndex = -1;
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
