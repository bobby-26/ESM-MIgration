using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using System.Text;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PreSeaOffice : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["VIEWONLY"] == null)
            {
                HookOnFocus(this.Page as Control);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                PhoenixToolbar toolbarAddress = new PhoenixToolbar();
                toolbarAddress.AddButton("Address", "ADDRESS");
                toolbarAddress.AddButton("Bank", "BANK");
                toolbarAddress.AddButton("Attachments", "ATTACHMENTS");
                toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT");
                toolbarAddress.AddButton("Address Correction", "CORRECTION");
                //toolbarAddress.AddButton("OfficeAdmin", "OFFICEADMIN");
                toolbarAddress.AddButton("Contacts", "CONTACTS");

                toolbar.AddButton("Save", "SAVE");
                MenuAddressMain.AccessRights = this.ViewState;
                MenuAddressMain.MenuList = toolbarAddress.Show();
                MenuOfficeMain.AccessRights = this.ViewState;
                MenuOfficeMain.MenuList = toolbar.Show();
                MenuOfficeMain.SetTrigger(pnlAddressEntry);
            }
            if (!IsPostBack)
            {
                ddlAddressGrade.DataBind();
                if (Request.QueryString["VIEWONLY"] == null)
                {
                    HookOnFocus(this.Page as Control);
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    PhoenixToolbar toolbarAddress = new PhoenixToolbar();
                    toolbarAddress.AddButton("Address", "ADDRESS");
                    toolbarAddress.AddButton("Bank", "BANK");
                    toolbarAddress.AddButton("Attachments", "ATTACHMENTS");
                    toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT");
                    toolbarAddress.AddButton("Address Correction", "CORRECTION");
                    //toolbarAddress.AddButton("OfficeAdmin", "OFFICEADMIN");
                    toolbarAddress.AddButton("Contacts", "CONTACTS");

                    toolbar.AddButton("Save", "SAVE");
                    MenuAddressMain.AccessRights = this.ViewState;
                    MenuAddressMain.MenuList = toolbarAddress.Show();
                    MenuOfficeMain.AccessRights = this.ViewState;
                    MenuOfficeMain.MenuList = toolbar.Show();
                    MenuOfficeMain.SetTrigger(pnlAddressEntry);
                    RadTextBox ucadd = (RadTextBox)ucAddress.FindControl("txtName");
                    ucadd.Focus();
                    Page.ClientScript.RegisterStartupScript(
                        typeof(PreSeaOffice),
                        "ScriptDoFocus",
                        SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                        true);
                }

                DataSet ds = PhoenixRegistersDepartment.EditDepartmentByShortName("ACCT");

                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                if (ViewState["ADDRESSCODE"] == null)
                {
                    Session["COUNTRYCODE"] = "";
                }
                if (Request.QueryString["ADDRESSCODE"] == null)
                {
                    MenuAddressMain.Visible = false;
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE), 0, "TRI,SCL,CLG");
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();

                    cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
                    cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
                    cblAddressDepartment.DataBind();
                }
                else
                {
                    MenuAddressMain.Visible = true;
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE), 0, "TRI,SCL,CLG");
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();

                    cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
                    cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
                    cblAddressDepartment.DataBind();
                    AddressEdit();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void HookOnFocus(Control CurrentControl)
    {
        if ((CurrentControl is RadTextBox) ||
            (CurrentControl is DropDownList))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }

    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../PreSea/PreSeaBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../PreSea/PreSeaAddressAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (dce.CommandName.ToUpper().Equals("AGREEMENTSATTACHMENT"))
            {
                Response.Redirect("../PreSea/PreSeaAgreementsAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (dce.CommandName.ToUpper().Equals("CORRECTION"))
            {
                Response.Redirect("../PreSea/PreSeaAddressCorrection.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (dce.CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../PreSea/PreSeaAddressInvoiceApprovalUserMap.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (dce.CommandName.ToUpper().Equals("CONTACTS"))
            {
                Response.Redirect("../PreSea/PreSeaAddressPurpose.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OfficeMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptClosePopup += "fnReloadList('AddAddress');";
            scriptClosePopup += "</script>" + "\n";

            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)ucAddress.FindControl("txtPhone2");

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
            scriptRefreshDontClose += "</script>" + "\n";

            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["addresscode"] = null;
                Reset();
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                int prioritysupplier = 0;
                if (!ucAddress.IsValidAddress())
                {
                    ucError.ErrorMessage = ucAddress.ErrorMessage;
                    ucError.Visible = true;
                    return;
                }
                if (!General.IsvalidEmail(ucAddress.Email1))
                {
                    ucError.ErrorMessage = "Please enter valid e-mail1";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableString(ucAddress.PostalCode) == null)
                {
                    ucError.ErrorMessage = "Please enter postal code";
                    ucError.Visible = true;
                    return;
                }
                if (!phoneno.IsValidPhoneNumber())
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                   
                    StringBuilder straddresstype = new StringBuilder();

                    foreach (ListItem item in cblAddressType.Items)
                    {
                        if (item.Selected == true)
                        {
                            straddresstype.Append(item.Value.ToString());
                            straddresstype.Append(",");
                        }
                    }
                    if (straddresstype.Length > 1)
                    {
                        straddresstype.Remove(straddresstype.Length - 1, 1);
                    }
                    if (straddresstype.ToString() == "")
                    {
                        ucError.ErrorMessage = "Please Select Atleast One Address Type";
                        ucError.Visible = true;
                        return;
                    }
                    if (straddresstype.ToString() != "")
                    {
                        if (straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "SUP")))
                        {
                            foreach (ListItem item in cblAddressType.Items)
                            {
                                if (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "SUP"))
                                    prioritysupplier = 1;
                            }
                        }
                        else
                            prioritysupplier = 0;
                    }
                   

                    StringBuilder strAddressDepartment = new StringBuilder();
                    foreach (ListItem item in cblAddressDepartment.Items)
                    {
                        if (item.Selected == true)
                        {
                            strAddressDepartment.Append(item.Value.ToString());
                            strAddressDepartment.Append(",");
                        }
                    }
                    if (strAddressDepartment.Length > 1)
                    {
                        strAddressDepartment.Remove(strAddressDepartment.Length - 1, 1);
                    }


                    if (ViewState["ADDRESSCODE"] == null)
                    {

                        DataTable dt = PhoenixPreSeaAddress.InsertAddress(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            ucAddress.Name,
                            General.GetNullableString(ucAddress.Address1),
                            General.GetNullableString(ucAddress.Address2),
                            General.GetNullableString(ucAddress.Address3),
                            General.GetNullableString(ucAddress.Address4),
                            null, null,
                            General.GetNullableInteger(ucAddress.Country),
                            General.GetNullableInteger(ucAddress.State),
                            General.GetNullableInteger(ucAddress.City),
                            General.GetNullableString(ucAddress.PostalCode),
                            General.GetNullableString(ucAddress.Phone1),
                            General.GetNullableString(ucAddress.Phone2),
                            General.GetNullableString(ucAddress.Fax1),
                            General.GetNullableString(ucAddress.Fax2),
                            ucAddress.Email1,
                            General.GetNullableString(ucAddress.Email2),
                            General.GetNullableString(ucAddress.Url), null,
                            straddresstype.ToString(),
                            General.GetNullableInteger(ucAddress.Status),
                            General.GetNullableString(""),
                            General.GetNullableString(ucAddress.Attention),
                            General.GetNullableString(ucAddress.InCharge),
                            ucAddress.code,
                            General.GetNullableInteger(ucAddress.QAGrading),
                            null,
                            null,
                            null,
                            General.GetNullableInteger(""),
                            null, null,
                            General.GetNullableInteger(ucPreSeaZone.SelectedZone),
                            null,
                            prioritysupplier,
                            null,
                            null,
                            null,
                            null,
                            //General.GetNullableInteger(txtTimelimit.Text),
                            General.GetNullableString(ucAddress.Telephoneno),
                            General.GetNullableString(ucAddress.Mobileno),
                            txtBusinessProfile.Text,
                            null,
                            null,
                            General.GetNullableString(strAddressDepartment.ToString()),
                            General.GetNullableString(ddlAddressGrade.SelectedValue),
                            General.GetNullableInteger(ucAcademicBoard.SelectedQuick));

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["ADDRESSCODE"] = dt.Rows[0][0].ToString();
                            MenuAddressMain.Visible = true;
                        }
                        AddressEdit();
                        ucStatus.Text = "Address information saved";
                    }
                    else if (ViewState["ADDRESSCODE"] != null)
                    {

                        PhoenixPreSeaAddress.UpdateAddress(
                            Convert.ToInt64(ViewState["addresscode"]),
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            ucAddress.Name,
                            General.GetNullableString(ucAddress.Address1),
                            General.GetNullableString(ucAddress.Address2),
                            General.GetNullableString(ucAddress.Address3),
                            General.GetNullableString(ucAddress.Address4),
                            null, null,
                            General.GetNullableInteger(ucAddress.Country),
                            General.GetNullableInteger(ucAddress.State),
                            General.GetNullableInteger(ucAddress.City),
                            General.GetNullableString(ucAddress.PostalCode),
                            General.GetNullableString(ucAddress.Phone1),
                            General.GetNullableString(ucAddress.Phone2),
                            General.GetNullableString(ucAddress.Fax1),
                            General.GetNullableString(ucAddress.Fax2),
                            ucAddress.Email1,
                            General.GetNullableString(ucAddress.Email2),
                            General.GetNullableString(ucAddress.Url), null,
                            straddresstype.ToString(),
                            General.GetNullableInteger(ucAddress.Status),
                            General.GetNullableString(""),
                            General.GetNullableString(ucAddress.Attention),
                            General.GetNullableString(ucAddress.InCharge),
                            ucAddress.code,
                            General.GetNullableInteger(ucAddress.QAGrading),
                            null,
                            null,null,
                            null,
                            null,
                            null, null,
                            General.GetNullableInteger(ucPreSeaZone.SelectedZone),
                            null,
                            prioritysupplier,
                            null,
                            null,
                            null,
                            null,
                            //General.GetNullableInteger(txtTimelimit.Text),
                            General.GetNullableString(ucAddress.Telephoneno),
                            General.GetNullableString(ucAddress.Mobileno),
                            txtBusinessProfile.Text,
                            null,
                            null,
                            General.GetNullableString(strAddressDepartment.ToString()),
                            General.GetNullableString(ddlAddressGrade.SelectedValue),
                            General.GetNullableInteger(ucAcademicBoard.SelectedQuick)
                            );

                        AddressEdit();
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);
                        ucStatus.Text = "Address information updated";
                    }
                    string Script = "";
                    Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList(null, null, 'yes');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        ViewState["ADDRESSCODE"] = null;
        ucAddress.Name = "";
        ucAddress.Address1 = "";
        ucAddress.Address2 = "";
        ucAddress.Address3 = "";
        ucAddress.Country = null;
        ucAddress.State = null;
        ucAddress.City = "";
        ucAddress.PostalCode = "";
        ucAddress.Phone1 = "";
        ucAddress.Phone2 = "";
        ucAddress.Fax1 = "";
        ucAddress.Fax2 = "";
        ucAddress.Email1 = "";
        ucAddress.Email2 = "";
        ucAddress.Url = "";
        ucAddress.QAGrading = null;
        ucAddress.Telephoneno = "";
        ucAddress.Mobileno = "";

        foreach (ListItem item in cblAddressType.Items)
        {
            item.Selected = false;
        }

        ucAddress.Attention = "";
        ucAddress.InCharge = "";
        ucAddress.code = "";

        foreach (ListItem item in cblAddressDepartment.Items)
        {
            item.Selected = false;
        }
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
    }

    protected void AddressEdit()
    {
        try
        {
            UserControlPhoneNumber ucaddphoneno = (UserControlPhoneNumber)ucAddress.FindControl("txtPhone1");
            Int64 addresscode = Convert.ToInt64(ViewState["ADDRESSCODE"]);
            DataSet dsaddress = PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);

            if (dsaddress.Tables.Count > 0)
            {
                DataRow draddress = dsaddress.Tables[0].Rows[0];
                ucAddress.Name = draddress["FLDNAME"].ToString();
                ucAddress.Address1 = draddress["FLDADDRESS1"].ToString();
                ucAddress.Address2 = draddress["FLDADDRESS2"].ToString();
                ucAddress.Address3 = draddress["FLDADDRESS3"].ToString();
                ucAddress.Address4 = draddress["FLDADDRESS4"].ToString();
                ucAddress.Country = draddress["FLDCOUNTRYID"].ToString();
                ucAddress.QAGrading = draddress["FLDQAGRADING"].ToString();
                ucAddress.State = draddress["FLDSTATE"].ToString();
                ucAddress.City = draddress["FLDCITY"].ToString();
                ucAddress.PostalCode = draddress["FLDPOSTALCODE"].ToString();
                ucaddphoneno.ISDCode = draddress["FLDISDCODE"].ToString();
                ucAddress.Phone1 = draddress["FLDPHONE1"].ToString();
                ucAddress.Phone2 = draddress["FLDPHONE2"].ToString();
                ucAddress.Email1 = draddress["FLDEMAIL1"].ToString();
                ucAddress.Email2 = draddress["FLDEMAIL2"].ToString();
                ucAddress.Fax1 = draddress["FLDFAX1"].ToString();
                ucAddress.Fax2 = draddress["FLDFAX2"].ToString();
                ucAddress.Url = draddress["FLDURL"].ToString();
                ucAddress.code = draddress["FLDCODE"].ToString();
                ucAddress.Attention = draddress["FLDATTENTION"].ToString();
                ucAddress.InCharge = draddress["FLDINCHARGE"].ToString();
                ucAddress.Status = draddress["FLDSTATUS"].ToString();
                //ucFlag.SelectedFlag = draddress["FLDFLAGID"].ToString();

                //ddlZone.SelectedZone = draddress["FLDZONEID"].ToString();
                ucPreSeaZone.SelectedZone = draddress["FLDZONEID"].ToString();
                ddlAddressGrade.SelectedValue = String.IsNullOrEmpty(draddress["FLDADDRESSGRADE"].ToString()) ? "DUMMY" : draddress["FLDADDRESSGRADE"].ToString();

                txtBusinessProfile.Text = draddress["FLDBUSINESSPROFILE"].ToString();
                ucAcademicBoard.SelectedQuick = draddress["FLDBOARD"].ToString();

               
                ucAddress.Telephoneno = draddress["FLDAOHTELEPHONENO"].ToString();
                ucAddress.Mobileno = draddress["FLDAOHMOBILENO"].ToString();

                string[] addresstype = draddress["FLDADDRESSTYPE"].ToString().Split(',');
                string[] producttype = draddress["FLDPRODUCTTYPE"].ToString().Split(',');
                string[] addressdepartment = draddress["FLDADDRESSDEPARTMENT"].ToString().Split(',');
                Session["COUNTRYCODE"] = draddress["FLDCOUNTRYID"].ToString();

                foreach (string item in addresstype)
                {
                    if (item.Trim() != "")
                    {
                        cblAddressType.Items.FindByValue(item).Selected = true;
                    }
                }

               foreach (string item in addressdepartment)
                {
                    if (item.Trim() != "")
                    {
                        cblAddressDepartment.Items.FindByValue(item).Selected = true;
                    }
                }
            }
           

            ViewState["addresscode"] = addresscode;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}

