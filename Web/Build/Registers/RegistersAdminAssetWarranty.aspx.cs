using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersAdminAssetWarranty : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAdminWarranty.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvAdminWarranty.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminAssetWarranty.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAdminWarranty')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersAdminAssetWarranty.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersAdminAssetWarranty.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuAdminAssetWarranty.AccessRights = this.ViewState;
            MenuAdminAssetWarranty.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindType(ddlType);
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
    protected void BindType(DropDownList ddlType)
    {
        ddlType.Items.Clear();
        ListItem li = new ListItem("--Select--", "");
        DataTable dt = PhoenixRegistersAssetType.ListCategory(null);
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDTYPEID";
        ddlType.DataSource = dt;
        ddlType.DataBind();
        ddlType.Items.Insert(0, li);
    }
    protected void MenuAdminAssetWarranty_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            if (IsValidSearch(ddlType.SelectedValue,
                    ddlLocation.SelectedZone
                    ))
            {
                gvAdminWarranty.SelectedIndex = -1;
                gvAdminWarranty.EditIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else
            {
                ucError.Visible = true;
                return;
            }
            
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            Reset();
        }
    }
    protected void Reset()
    {
        txtAssetName.Text = string.Empty;
        ddlType.SelectedValue = null;
        ddlActive.SelectedValue = null;
        ddlLocation.SelectedZone = "";
        gvAdminWarranty.SelectedIndex = -1;
        gvAdminWarranty.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDASSETNAME", "FLDADDRESSNAME", "FLDVALIDUNTIL", "FLDACTIVE" };
        string[] alCaptions = { "Name", "Address", "Valid Until", "Active" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationAssetWarranty.SearchAssetWarranty(txtAssetName.Text, sortdirection, sortexpression
            , Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null)
            , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlType.SelectedValue), General.GetNullableInteger(ddlLocation.SelectedZone)
            , General.GetNullableInteger(ddlActive.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=AssetCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Warranty </h3></td>");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression;
        int? sortdirection = null;

        string[] alColumns = { "FLDASSETNAME", "FLDADDRESSNAME", "FLDVALIDUNTIL", "FLDACTIVE" };
        string[] alCaptions = { "Name", "Address", "Valid Until", "Active" };

        DataSet ds = new DataSet();

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationAssetWarranty.SearchAssetWarranty(txtAssetName.Text, sortdirection, sortexpression
            , Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null)
            , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlType.SelectedValue),General.GetNullableInteger(ddlLocation.SelectedZone)
            , General.GetNullableInteger(ddlActive.SelectedValue));

        General.SetPrintOptions("gvAdminWarranty", "Warranty", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdminWarranty.DataSource = ds;
            gvAdminWarranty.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAdminWarranty);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvAdminWarranty_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdminWarranty.SelectedIndex = -1;
        gvAdminWarranty.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvAdminWarranty_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvAdminWarranty_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvAdminWarranty.SelectedIndex = e.NewSelectedIndex;
    }
    protected void gvAdminWarranty_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (IsValidWarranty(ddlType.SelectedValue
                     , ((TextBox)_gridView.FooterRow.FindControl("txtAssetIdAdd")).Text
                     , ((TextBox)_gridView.FooterRow.FindControl("txtAddressCodeAdd")).Text
                     , ((UserControlDate)_gridView.FooterRow.FindControl("ucValidUntilAdd")).Text
                     , ddlLocation.SelectedZone
                    ))
                {
                    PhoenixAdministrationAssetWarranty.InsertAssetWarranty
                        (
                            General.GetNullableGuid(((TextBox)_gridView.FooterRow.FindControl("txtAssetIdAdd")).Text)
                            , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtAddressCodeAdd")).Text)
                            , General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucValidUntilAdd")).Text)
                            , General.GetNullableInteger(ddlType.SelectedValue)
                            , General.GetNullableInteger(ddlLocation.SelectedZone)
                        );
                    BindData();
                    SetPageNavigator();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidWarranty(ddlType.SelectedValue
                     , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetIdEdit")).Text
                     , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAddressCodeEdit")).Text
                     , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucValidUntilEdit")).Text
                     , ddlLocation.SelectedZone
                    ))
                {
                    PhoenixAdministrationAssetWarranty.UpdateAssetWarranty
                        (
                            General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetWarrantyIDEdit")).Text)
                            , General.GetNullableGuid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetIdEdit")).Text)
                            , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAddressCodeEdit")).Text)
                            , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucValidUntilEdit")).Text)
                            , General.GetNullableInteger(ddlType.SelectedValue)
                            , General.GetNullableInteger(ddlLocation.SelectedZone)
                            , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveEdit")).Checked) ? 1 : 0
                        );
                    _gridView.EditIndex = -1;
                    BindData();
                    SetPageNavigator();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAdministrationAssetWarranty.DeleteAssetWarranty
                    (
                        General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetWarrantyID")).Text)
                    );
                _gridView.EditIndex = -1;
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
    private bool IsValidWarranty(string Category, string AssetId, string AddressCode, string ValidDate, string ZoneId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Category) == null)
        {
            ucError.ErrorMessage = "Category is Required.";
        }

        if (General.GetNullableGuid(AssetId) == null)
        {
            ucError.ErrorMessage = "Asset is Required.";
        }

        if (General.GetNullableInteger(AddressCode) == null)
        {
            ucError.ErrorMessage = "Address is Required.";
        }

        if (General.GetNullableDateTime(ValidDate) == null)
        {
            ucError.ErrorMessage = "Valid Date is Required.";
        }

        if (General.GetNullableInteger(ZoneId) == null)
        {
            ucError.ErrorMessage = "Zone is Required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidSearch(string Category, string ZoneId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Category) == null)
            ucError.ErrorMessage = "Category is Required.";
        if (General.GetNullableInteger(ZoneId) == null)
            ucError.ErrorMessage = "Zone is Required.";
        return (!ucError.IsError);
    }
    protected void gvAdminWarranty_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvAdminWarranty_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdminWarranty_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;


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
    protected void gvAdminWarranty_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            TextBox txtAddressCodeEdit = (TextBox)e.Row.FindControl("txtAddressCodeEdit");
            TextBox txtAddressSupCodeEdit = (TextBox)e.Row.FindControl("txtAddressSupCodeEdit");
            if (txtAddressCodeEdit != null)
                txtAddressCodeEdit.Attributes.Add("style", "visibility:hidden");
            if (txtAddressSupCodeEdit != null)
                txtAddressSupCodeEdit.Attributes.Add("style", "visibility:hidden");

            ImageButton imgAddressEdit = (ImageButton)e.Row.FindControl("imgAddressEdit");
            
            if (imgAddressEdit != null)
            {
                imgAddressEdit.Attributes.Add("onclick", "return showPickList('spnAddressEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true);");
            }

            TextBox txtAssetIdEdit = (TextBox)e.Row.FindControl("txtAssetIdEdit");
            if (txtAssetIdEdit != null)
                txtAssetIdEdit.Attributes.Add("style", "visibility:hidden");

            ImageButton imgAssetNameEdit = (ImageButton)e.Row.FindControl("imgAssetNameEdit");

            if (imgAssetNameEdit != null)
            {
                imgAssetNameEdit.Attributes.Add("onclick", "return showPickList('spnAssetNameEdit', 'codehelp1', '', '../Common/CommonPickListAdminAssetName.aspx?CategoryType=" + General.GetNullableInteger(ddlType.SelectedValue) + "&ZoneId=" + General.GetNullableInteger(ddlLocation.SelectedZone) + "', true); ");
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

            TextBox txtAddresscode = (TextBox)e.Row.FindControl("txtAddressCodeAdd");
            TextBox txtAddressSupCodeAdd = (TextBox)e.Row.FindControl("txtAddressSupCodeAdd");
            if (txtAddresscode != null)
                txtAddresscode.Attributes.Add("style", "visibility:hidden");
            if (txtAddressSupCodeAdd != null)
                txtAddressSupCodeAdd.Attributes.Add("style", "visibility:hidden");

            ImageButton imgAddressAdd = (ImageButton)e.Row.FindControl("imgAddressAdd");
            if (imgAddressAdd != null)
            {
                imgAddressAdd.Attributes.Add("onclick", "return showPickList('spnAddressAdd', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true);");
            }

            TextBox txtAssetIdAdd = (TextBox)e.Row.FindControl("txtAssetIdAdd");
            if (txtAssetIdAdd != null)
                txtAssetIdAdd.Attributes.Add("style", "visibility:hidden");

            ImageButton imgAssetNameAdd = (ImageButton)e.Row.FindControl("imgAssetNameAdd");
            if (General.GetNullableInteger(ddlType.SelectedValue) == null || General.GetNullableInteger(ddlLocation.SelectedZone) == null)
            {
                imgAssetNameAdd.Enabled = false;
            }
            else
            {
                if (imgAssetNameAdd != null)
                {
                    imgAssetNameAdd.Attributes.Add("onclick", "return showPickList('spnAssetNameAdd', 'codehelp1', '', '../Common/CommonPickListAdminAssetName.aspx?CategoryType=" + General.GetNullableInteger(ddlType.SelectedValue) + "&ZoneId=" + General.GetNullableInteger(ddlLocation.SelectedZone) + "', true);");
                }
            }
            
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvAdminWarranty.SelectedIndex = -1;
        gvAdminWarranty.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvAdminWarranty.SelectedIndex = -1;
        gvAdminWarranty.EditIndex = -1;

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
        gvAdminWarranty.SelectedIndex = -1;
        gvAdminWarranty.EditIndex = -1;
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
}
