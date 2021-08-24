using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;

public partial class PreSeaAddressInvoiceApprovalUserMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ADDRESSCODE"] != null && Request.QueryString["ADDRESSCODE"] != string.Empty)
            {
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                PhoenixToolbar toolbarAddress = new PhoenixToolbar();
               // toolbarAddress.AddButton("Bank", "BANK");
                toolbarAddress.AddButton("Address", "ADDRESS");
                PhoenixToolbar toolbarMain = new PhoenixToolbar();
                MenuOfficeAdmin.AccessRights = this.ViewState;
                MenuOfficeAdmin.MenuList = toolbarAddress.Show();
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELDISCOUNTPERCENTAGE" };
        string[] alCaptions = { "Vessel Name", "SupplierCode", "Supplier Name", "%Return" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ADDRESSCODE"] != null && !string.IsNullOrEmpty(ViewState["ADDRESSCODE"].ToString()))
        {

            DataSet ds = PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapSearch(null, General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), sortexpression, sortdirection,
                1,
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,null);

            General.SetPrintOptions("gvVesselAdminUser", "Discount", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvVesselAdminUser.DataSource = ds;
                gvVesselAdminUser.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvVesselAdminUser);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
    }

    protected void MenuOfficeAdmin_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../PreSea/PreSeaOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
        }

    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }
    protected void gvVesselAdminUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvVesselAdminUser_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvVesselAdminUser.EditIndex = -1;
        gvVesselAdminUser.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvVesselAdminUser_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvVesselAdminUser, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvVesselAdminUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidData((((UserControlDesignation)_gridView.FooterRow.FindControl("ucDesignation")).SelectedDesignation),
                    ((UserControlUserName)_gridView.FooterRow.FindControl("ucPIC")).SelectedUser))
                {
                    ucError.Visible = true;
                    return;
                }
                int UserDesignation = int.Parse(((UserControlDesignation)_gridView.FooterRow.FindControl("ucDesignation")).SelectedDesignation);
                int PICUserId = int.Parse(((UserControlUserName)_gridView.FooterRow.FindControl("ucPIC")).SelectedUser);

                InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), UserDesignation, PICUserId);
                ucStatus.Text = "Vessel Admin Uesr Information added.";
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((UserControlDesignation)_gridView.Rows[nCurrentRow].FindControl("ucDesignationEdit")).SelectedDesignation,
                                    ((UserControlUserName)_gridView.Rows[nCurrentRow].FindControl("ucPICEdit")).SelectedUser))
                {

                    ucError.Visible = true;
                    return;
                }
                int UserDesignation = int.Parse(((UserControlDesignation)_gridView.Rows[nCurrentRow].FindControl("ucDesignationEdit")).SelectedDesignation);
                int PICUserId = int.Parse(((UserControlUserName)_gridView.Rows[nCurrentRow].FindControl("ucPICEdit")).SelectedUser);
                Guid VesselAdminUserMapCode = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselAdminUserMapCodeEdit")).Text);

                UpdateVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode, UserDesignation, PICUserId);
                ucStatus.Text = "Vessel Admin Uesr Information updated.";
                _gridView.EditIndex = -1;
                BindData();
                SetPageNavigator();
            }


            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid VesselAdminUserMapCode = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselAdminUserMapCode")).Text);
                PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode);
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

    protected void gvVesselAdminUser_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvVesselAdminUser_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvVesselAdminUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void gvVesselAdminUser_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            TextBox tb1 = (TextBox)e.Row.FindControl("txtSupplierIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnSuppllierEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx')");

            UserControlDesignation ucDesignationEdit = (UserControlDesignation)e.Row.FindControl("ucDesignationEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ucDesignationEdit != null) ucDesignationEdit.SelectedDesignation = drv["FLDDESIGNATIONID"].ToString();

            UserControlUserName ucPICEdit = (UserControlUserName)e.Row.FindControl("ucPICEdit");
            if (ucPICEdit != null) ucPICEdit.SelectedUser = drv["FLDPICUSERID"].ToString();
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvVesselAdminUser.EditIndex = -1;
        gvVesselAdminUser.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvVesselAdminUser.EditIndex = -1;
        gvVesselAdminUser.SelectedIndex = -1;
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
        gvVesselAdminUser.SelectedIndex = -1;
        gvVesselAdminUser.EditIndex = -1;
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

    private void InsertVesselAdminUser(int rowusercode, int? Addresscode, int DesignationId, int PICUserId)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapInsert(rowusercode, null, Addresscode, DesignationId, PICUserId,null);
    }

    private void UpdateVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode, int DesignationId, int PICUserId)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapUpdate(rowusercode, VesselAdminUserMapCode, DesignationId, PICUserId);
    }

    private bool IsValidData(string designationid, string userid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (designationid.Trim().Equals("") || designationid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Designation is required.";

        if (userid.Trim().Equals("") || userid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "PIC User is required.";
        return (!ucError.IsError);
    }

    private void DeleteVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(rowusercode, VesselAdminUserMapCode);
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
