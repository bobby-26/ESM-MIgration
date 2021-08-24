using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaAddressPurpose : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAddressPurpose.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvAddressPurpose.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            toolbarAddress.AddButton("Address", "ADDRESS");
            MenuAddressMain.AccessRights = this.ViewState;
            MenuAddressMain.MenuList = toolbarAddress.Show();

            PhoenixToolbar toolbarAddressPurpose = new PhoenixToolbar();
            toolbarAddressPurpose.AddButton("New", "NEW");
            toolbarAddressPurpose.AddButton("Save", "SAVE");
            MenuAddressPurpose.AccessRights = this.ViewState;
            MenuAddressPurpose.MenuList = toolbarAddressPurpose.Show();

            PhoenixToolbar toolbarSearch = new PhoenixToolbar();
            toolbarSearch.AddImageButton("../PreSea/PreSeaAddressPurpose.aspx", "Find", "search.png", "FIND");
            MenuAddressFind.AccessRights = this.ViewState;
            MenuAddressFind.MenuList = toolbarSearch.Show();
            if (!IsPostBack)
            {
                ViewState["OPERATION"] = "ADD";
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                ViewState["ADDRESSCONTACTID"] = null;

                if (Request.QueryString["VIEWONLY"] == null)
                {
                    ListItem li = new ListItem("--Select--", "DUMMY");

                    ddlPurpose.Items.Clear();
                    ddlPurpose.Items.Add(li);
                    ddlPurpose.DataSource = PhoenixRegistersContactPurpose.ListContactPurpose();
                    ddlPurpose.DataTextField = "FLDPURPOSENAME";
                    ddlPurpose.DataValueField = "FLDPURPOSEID";
                    ddlPurpose.DataBind();

                    cblRegisteredCompany.DataSource = PhoenixRegistersCompany.ListCompany();
                    cblRegisteredCompany.DataTextField = "FLDSHORTCODE";
                    cblRegisteredCompany.DataValueField = "FLDCOMPANYID";
                    cblRegisteredCompany.DataBind();

                    MenuAddressMain.SetTrigger(pnlAddressPurpose);
                }
            }
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixPreSeaAddressPurpose.ListAddressPurpose(int.Parse(ViewState["ADDRESSCODE"].ToString()),
                General.GetNullableInteger(ddlPurpose.SelectedValue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAddressPurpose.DataSource = ds;
                gvAddressPurpose.DataBind();

                if (ViewState["ADDRESSCONTACTID"] == null)
                {
                    ViewState["ADDRESSCONTACTID"] = ds.Tables[0].Rows[0]["FLDADDRESSCONTACTID"].ToString();
                    gvAddressPurpose.SelectedIndex = 0;
                    //gvAddressPurpose.EditIndex = 0;
                }
                //SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAddressPurpose);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFields(int AddressContactId)
    {
        DataSet ds  = PhoenixPreSeaAddressPurpose.EditAddressPurpose(AddressContactId);
        DataRow dr = ds.Tables[0].Rows[0];

        lblAddressContactID.Text = AddressContactId.ToString();
        txtName.Text = dr["FLDNAME"].ToString();
        txtEmail1.Text = dr["FLDEMAIL1"].ToString();
        txtTitle.Text = dr["FLDJOBTITLE"].ToString();
        txtEmail2.Text = dr["FLDEMAIL2"].ToString();
        txtDepartment.Text = dr["FLDDEPARTMENTNAME"].ToString();
        txtPhone1.Text = dr["FLDPHONE1"].ToString();
        txtMobile.Text = dr["FLDMOBILE"].ToString();
        txtPhone2.Text = dr["FLDPHONE2"].ToString();
        txtFax.Text = dr["FLDFAX"].ToString();
        BindCheckBoxList(cblRegisteredCompany, dr["FLDREGISTEREDCOMPANY"].ToString());

        
    }

    private void BindCheckBoxList(CheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindByValue(item) != null)
                    cbl.Items.FindByValue(item).Selected = true;
            }
        }
    }

    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../PreSea/PreSeaOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressPurpose_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Clear();
                ViewState["OPERATION"] = "ADD";
            }

            else if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAddressPurpose(txtName.Text,
                    txtTitle.Text, txtEmail1.Text, txtDepartment.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATION"] == "EDIT")
                {
                    updateAddressPurpose(int.Parse(ViewState["ADDRESSCODE"].ToString()),
                        int.Parse(lblAddressContactID.Text),
                        txtEmail1.Text,
                        txtEmail2.Text,
                        txtName.Text,
                        txtTitle.Text,
                        txtDepartment.Text,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        txtMobile.Text,
                        txtFax.Text,
                        ReadCheckBoxList(cblRegisteredCompany)
                        );
                }

                else if ((String)ViewState["OPERATION"] == "ADD")
                {
                    insertAddressPurpose(int.Parse(ViewState["ADDRESSCODE"].ToString()),
                        int.Parse(ddlPurpose.SelectedValue),
                        txtEmail1.Text,
                        txtEmail2.Text,
                        txtName.Text,
                        txtTitle.Text,
                        txtDepartment.Text,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        txtMobile.Text,
                        txtFax.Text,
                        ReadCheckBoxList(cblRegisteredCompany)
                        );
                }
            }
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressFind_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                cblRegisteredCompany.ClearSelection();
                ViewState["ADDRESSCONTACTID"] = int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressContactIdEdit")).Text);
                ViewState["OPERATION"] = "EDIT";
                BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaAddressPurpose.DeleteAddressPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressContactIdEdit")).Text));
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressPurpose_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
    }

    protected void gvAddressPurpose_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvAddressPurpose_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvAddressPurpose_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

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

    private bool IsValidAddressPurpose(string name, string title, string email1, string department) 
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name cannot be blank.";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title cannot be blank.";

        if (email1.Trim().Equals(""))
            ucError.ErrorMessage = "Email cannot be blank.";

        if (department.Trim().Equals(""))
            ucError.ErrorMessage = "Department cannot be blank";

        return (!ucError.IsError);
    }

    private void insertAddressPurpose( int addresscode
        , int purpose
        , string email1
        , string email2
        , string name
        , string jobtitle
        , string department
        , string phone1
        , string phone2
        , string mobile
        , string fax
        , string registeredcompany)
    {
        PhoenixPreSeaAddressPurpose.InsertAddresspurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
            addresscode, purpose, email1,
            email2, name, jobtitle, department, phone1, phone2, mobile, fax, registeredcompany);
    }

    private void updateAddressPurpose(int addresscode
        ,int addresscontactid
        , string email1
        , string email2
        , string name
        , string jobtitle
        , string department
        , string phone1
        , string phone2
        , string mobile
        , string fax
        , string registeredcompany)
    {
        PhoenixPreSeaAddressPurpose.UpdateAddressPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode,
            addresscontactid, email1, email2, 
            name, jobtitle, department, phone1, phone2, mobile, fax, registeredcompany);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void SetRowSelection()
    {
        gvAddressPurpose.SelectedIndex = -1;
        for (int i = 0; i < gvAddressPurpose.Rows.Count; i++)
        {
            if (gvAddressPurpose.DataKeys[i].Value.ToString().Equals(ViewState["ADDRESSCONTACTID"].ToString()))
            {
                gvAddressPurpose.SelectedIndex = i;
                //PhoenixAccountsVoucher.InvoiceNumber = ((Label)gvInvoice.Rows[i].FindControl("lblInvoiceid")).Text;
                //BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
            }
        }
    }
    protected void ddlPurpose_TextChanged(object sender, EventArgs e)
    {
        Clear();
    }

    protected void Clear()
    {
        txtName.Text = "";
        txtEmail1.Text = "";
        txtEmail2.Text = "";
        txtTitle.Text = "";
        txtDepartment.Text = "";
        txtPhone1.Text = "";
        txtPhone2.Text = "";
        txtMobile.Text = "";
        txtFax.Text = "";
        cblRegisteredCompany.ClearSelection();
    }
}
