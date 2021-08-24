using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;


public partial class Registers_RegisterSupplierDiscountandTax : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSupplierType.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvSupplierType.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtVendor.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersSupplierDiscountandTax.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvSupplierType')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Registers/RegistersSupplierDiscountandTax.aspx", "Find", "search.png", "FIND");
        MenuRegistersSupplierType.AccessRights = this.ViewState;
        MenuRegistersSupplierType.MenuList = toolbar.Show();
        MenuRegistersSupplierType.SetTrigger(pnlSupplier);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDTYPE", "FLDVALUEPERCENTAGE", "FLDVALUE", "REMARKS" };
        string[] alCaptions = { "SupplierCode", "Name", "Type", "Value/Percentage", "Value", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersSupplierDiscountandTax.SupplierSearch(General.GetNullableInteger(txtVendor.Text), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
             General.ShowRecords(null),
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SupplierDiscountandTax.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Type</h3></td>");
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

    protected void RegistersSupplierType_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            gvSupplierType.EditIndex = -1;
            gvSupplierType.SelectedIndex = -1;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUPPLIERCODE", "FLDNAME", "FLDTYPE", "FLDVALUEPERCENTAGE", "FLDVALUE", "FLDDISCOUNTEFFECTIVEDATE", "FLDISDELAYEDUTILIZATIONOFDISCOUNT", "REMARKS" };
        string[] alCaptions = { "SupplierCode", "SupplierName", "Type", "Value/Percentage", "Value", "Discount Upto", "Is Valid After Expiry Date", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersSupplierDiscountandTax.SupplierSearch(General.GetNullableInteger(txtVendor.Text), sortexpression, sortdirection,
        (int)ViewState["PAGENUMBER"],
        General.ShowRecords(null),
        ref iRowCount,
        ref iTotalPageCount);

        General.SetPrintOptions("gvSupplierType", "Registers", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSupplierType.DataSource = ds;
            gvSupplierType.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSupplierType);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSupplierType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSupplierType_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSupplierType.EditIndex = -1;
        gvSupplierType.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSupplierType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidSupplier(int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtVendor")).Text),

                    int.Parse(((UserControlHard)_gridView.FooterRow.FindControl("ucTypeAdd")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.FooterRow.FindControl("ucValueTypeAdd")).TaxType),
                    decimal.Parse(((TextBox)_gridView.FooterRow.FindControl("txtValueAdd")).Text),
                    DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucDiscountDateAdd")).Text),
                    ((CheckBox)_gridView.FooterRow.FindControl("chkDiscountAfterAdd")).Checked ? 1 : 0,
                ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSupplier(
                    int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtVendor")).Text),

                    int.Parse(((UserControlHard)_gridView.FooterRow.FindControl("ucTypeAdd")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.FooterRow.FindControl("ucValueTypeAdd")).TaxType),
                    decimal.Parse(((TextBox)_gridView.FooterRow.FindControl("txtValueAdd")).Text),
                   Convert.ToDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucDiscountDateAdd")).Text),
                    ((CheckBox)_gridView.FooterRow.FindControl("chkDiscountAfterAdd")).Checked ? 1 : 0,
                ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text
                );
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                if (!IsValidSupplier(int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierCodeEdit")).Text),

                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucTypeEdit")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucTaxTypeEdit")).TaxType),
                    decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text),
                    Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDiscountDateEdit")).Text),
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDiscountAfterEdit")).Checked ? 1 : 0,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierRemarkEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateSupplier(
                     int.Parse((((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierCodeEdit")).Text).ToString()),

                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucTypeEdit")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucTaxTypeEdit")).TaxType),
                    decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text),
                    Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDiscountDateEdit")).Text),
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDiscountAfterEdit")).Checked ? 1 : 0,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierRemarkEdit")).Text
                 );
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                DeleteSupplier(Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSupplierCode")).Text));
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSupplierType_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSupplierType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSupplierType, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvSupplierType_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtSupplierCodeEdit")).Focus();
        SetPageNavigator();
    }

    protected void gvSupplierType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidSupplier(int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierCodeEdit")).Text),

                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucTypeEdit")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucValueTypeEdit")).TaxType),
                    decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text),
                   Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDiscountDateEdit")).Text),
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDiscountAfterEdit")).Checked ? 1 : 0,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateSupplier(
                 int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSupplierCodeEdit")).Text),

                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucTypeEdit")).SelectedHard),
                    int.Parse(((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucValueTypeEdit")).TaxType),
                    decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text),
                   Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucDiscountDateEdit")).Text),
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDiscountAfterEdit")).Checked ? 1 : 0,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text
             );
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

    protected void gvSupplierType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

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
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ucTypeEdit");
            DataRowView drvHard = (DataRowView)e.Row.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDTDCTYPE"].ToString();

            UserControlTaxMaster ucTaxMaster = (UserControlTaxMaster)e.Row.FindControl("ucValueTypeEdit");
            DataRowView drvTaxMaster = (DataRowView)e.Row.DataItem;
            if (ucTaxMaster != null) ucTaxMaster.SelectedTax = drvTaxMaster["FLDTDCVALUETYPE"].ToString();
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSupplierType.EditIndex = -1;
        gvSupplierType.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvSupplierType.EditIndex = -1;
        gvSupplierType.SelectedIndex = -1;
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
        gvSupplierType.SelectedIndex = -1;
        gvSupplierType.EditIndex = -1;
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

    private void InsertSupplier(int suppliercode, int type, int valuetype, decimal value, DateTime discountdate, int validdiscount, string remarks)
    {

        PhoenixRegistersSupplierDiscountandTax.InsertSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode, suppliercode, type, valuetype, value, discountdate, validdiscount, remarks);
    }

    private void UpdateSupplier(int suppliercode, int type, int valuetype, decimal value, DateTime discountdate, int validdiscount, string remarks)
    {

        PhoenixRegistersSupplierDiscountandTax.UpdateSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode, suppliercode, type, valuetype, value, discountdate, validdiscount, remarks);
    }

    private void DeleteSupplier(int suppliercode)
    {
        PhoenixRegistersSupplierDiscountandTax.DeleteSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode, suppliercode);
    }

    private bool IsValidSupplier(int suppliercode, int type, int valuetype, decimal value, DateTime discountdate, int validdiscount, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvSupplierType;

        if (suppliercode.Equals(""))
            ucError.ErrorMessage = "Supplier is required.";

        if (type.Equals(""))
            ucError.ErrorMessage = "Type is required";

        if (valuetype.Equals(""))
            ucError.ErrorMessage = "Select Value Type is required";

        if (value.Equals(""))
            ucError.ErrorMessage = "Value is required.";

        if (discountdate.Equals(""))
            ucError.ErrorMessage = "Discount Date is required";

        if (validdiscount.Equals(""))
            ucError.ErrorMessage = "Select Allow Discount After Date Expired";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";

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
}
