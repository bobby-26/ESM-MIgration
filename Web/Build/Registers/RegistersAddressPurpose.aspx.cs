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
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAddressPurpose : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarSearch = new PhoenixToolbar();
            toolbarSearch.AddFontAwesomeButton("../Registers/RegistersAddressPurpose.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuAddressFind.AccessRights = this.ViewState;
            MenuAddressFind.MenuList = toolbarSearch.Show();

            if (!IsPostBack)
            {
                ViewState["OPERATION"] = "ADD";
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                ViewState["ADDRESSCONTACTID"] = null;

                if (Request.QueryString["VIEWONLY"] == null)
                {
                    PhoenixToolbar toolbarAddress = new PhoenixToolbar();
                    toolbarAddress.AddButton("Address", "ADDRESS", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Bank", "BANK", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Question", "QUESTION", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Address Correction", "CORRECTION", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Contacts", "CONTACTS", ToolBarDirection.Left);
                    MenuAddressMain.AccessRights = this.ViewState;
                    MenuAddressMain.MenuList = toolbarAddress.Show();
                    MenuAddressMain.SelectedMenuIndex = 6;

                    PhoenixToolbar toolbarAddressPurpose = new PhoenixToolbar();
                    toolbarAddressPurpose.AddButton("New", "NEW", ToolBarDirection.Right);
                    toolbarAddressPurpose.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    MenuAddressPurpose.AccessRights = this.ViewState;
                    MenuAddressPurpose.MenuList = toolbarAddressPurpose.Show();

                    
                    ddlPurpose.DataSource = PhoenixRegistersContactPurpose.ListContactPurpose();
                    ddlPurpose.DataTextField = "FLDPURPOSENAME";
                    ddlPurpose.DataValueField = "FLDPURPOSEID";
                    ddlPurpose.DataBind();
                    ddlPurpose.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));


                    cblRegisteredCompany.DataSource = PhoenixRegistersCompany.ListCompany();
                    cblRegisteredCompany.DataTextField = "FLDSHORTCODE";
                    cblRegisteredCompany.DataValueField = "FLDCOMPANYID";
                    cblRegisteredCompany.DataBind();

                }
            }
           // BindData();
            gvAddressPurpose.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressPurpose_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
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
            DataSet ds = PhoenixRegistersAddressPurpose.ListAddressPurpose(int.Parse(ViewState["ADDRESSCODE"].ToString()),
                General.GetNullableInteger(ddlPurpose.SelectedValue));

            gvAddressPurpose.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["ADDRESSCONTACTID"] == null)
                {
                    ViewState["ADDRESSCONTACTID"] = ds.Tables[0].Rows[0]["FLDADDRESSCONTACTID"].ToString();
                }
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
        DataSet ds  = PhoenixRegistersAddressPurpose.EditAddressPurpose(AddressContactId);
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

        ViewState["OPERATION"] = "EDIT";
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../Registers/RegistersOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=");
            }
            if (CommandName.ToUpper().Equals("QUESTION"))
            {
                Response.Redirect("../Registers/RegistersAddressQuestion.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Registers/RegistersAddressAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("AGREEMENTSATTACHMENT"))
            {
                Response.Redirect("../Registers/RegistersAgreementsAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CORRECTION"))
            {
                Response.Redirect("../Registers/RegistersAddressCorrection.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersSupplierInvoiceApprovalUserMap.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CONTACTS"))
            {
                Response.Redirect("../Registers/RegistersAddressPurpose.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                Clear();
                ViewState["OPERATION"] = "ADD";
            }

            else if (CommandName.ToUpper().Equals("SAVE"))
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
                    BindData();
                    gvAddressPurpose.Rebind();
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
                    BindData();
                    gvAddressPurpose.Rebind();
                }
                ViewState["OPERATION"] = "EDIT";
            }
           
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
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

    protected void gvAddressPurpose_ItemCommand(object sender, GridCommandEventArgs e)
    { 
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        try
        {
            if (e.CommandName == "UPDATE")
            {
                cblRegisteredCompany.ClearSelection();
                ViewState["ADDRESSCONTACTID"] = int.Parse(((RadLabel)eeditedItem.FindControl("lblAddressContactIdEdit")).Text);
                RadLabel lbl = (RadLabel)eeditedItem.FindControl("lblAddressContactIdEdit");
                lbl.Focus();
                BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
            }
            if (e.CommandName == "DELETE")
            {
                PhoenixRegistersAddressPurpose.DeleteAddressPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            int.Parse(((RadLabel)eeditedItem.FindControl("lblAddressContactId")).Text));
            }
            gvAddressPurpose.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvAddressPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {
    //            cblRegisteredCompany.ClearSelection();
    //            ViewState["ADDRESSCONTACTID"] = int.Parse(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAddressContactIdEdit")).Text);
    //            BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixRegistersAddressPurpose.DeleteAddressPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                int.Parse(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAddressContactId")).Text));
    //        }
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvAddressPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressPurpose_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAddressPurpose_SortCommand(object sender, GridSortCommandEventArgs e) 
    {

    }

    //protected void gvAddressPurpose_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindData();
    //}

    //protected void gvAddressPurpose_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}
    //protected void gvAddressPurpose_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{

    //}



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
        PhoenixRegistersAddressPurpose.InsertAddresspurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
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
        PhoenixRegistersAddressPurpose.UpdateAddressPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode,
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
        GridDataItem selectedItem = (GridDataItem)gvAddressPurpose.SelectedItems[0];

        //gvAddressPurpose.SelectedItems = -1;
        for (int i = 0; i < gvAddressPurpose.SelectedItems.Count; i++)
        {
            if (gvAddressPurpose.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["ADDRESSCONTACTID"].ToString()))
            {
                gvAddressPurpose.Items[i].Selected = true;
                //PhoenixAccountsVoucher.InvoiceNumber = ((Label)gvInvoice.Rows[i].FindControl("lblInvoiceid")).Text;
                //BindFields((Int32)ViewState["ADDRESSCONTACTID"]);
            }
        }
    }
    protected void ddlPurpose_TextChanged(object sender, EventArgs e)
    {
        Clear();
        gvAddressPurpose.Rebind();
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
