using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;


public partial class Registers_RegistersSupplierConfiguration : PhoenixBasePage
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
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSupplierRegister.Title = "Suppier Register";
            MenuSupplierRegister.AccessRights = this.ViewState;
            MenuSupplierRegister.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                lblCreditNoteDisc.Visible = (showcreditnotedisc == 1) ? true : false;
                txtDefaultDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
                lblCreditNoteEffectiveDate.Visible = (showcreditnotedisc == 1) ? true : false;
                txtEffectiveDate.Visible = (showcreditnotedisc == 1) ? true : false;


                if (Request.QueryString["VIEWONLY"] == null)
                {
                    HookOnFocus(this.Page as Control);

                    //toolbarAddress.AddButton("Bank", "BANK");
                    //toolbarAddress.AddButton("Question", "QUESTION");
                    //toolbarAddress.AddButton("Attachments", "ATTACHMENTS");
                    //toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT");
                    //toolbarAddress.AddButton("Address Correction", "CORRECTION");
                    ////toolbarAddress.AddButton("Office Admin", "OFFICEADMIN");
                    //toolbarAddress.AddButton("Contacts", "CONTACTS");


                    RadTextBox ucadd = (RadTextBox)ucAddress.FindControl("txtName");
                    ucadd.Focus();
                    Page.ClientScript.RegisterStartupScript(
                        typeof(Registers_RegistersSupplierConfiguration),
                        "ScriptDoFocus",
                        SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                        true);
                }

                DataSet ds = PhoenixRegistersDepartment.EditDepartmentByShortName("ACCT");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int departmentid = int.Parse(ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString());

                    ucACAsstIncharge.DepartmentId = departmentid;
                    ucACManagerInCharge.DepartmentId = departmentid;
                }

                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                if (ViewState["ADDRESSCODE"] == null)
                {
                    Session["COUNTRYCODE"] = "";
                }
                if (Request.QueryString["ADDRESSCODE"] == null)
                {
                    //MenuAddressMain.Visible = false;
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE));
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();
                    cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
                    cblProduct.DataTextField = "FLDQUICKNAME";
                    cblProduct.DataValueField = "FLDQUICKCODE";
                    cblProduct.DataBind();
                    cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
                    cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
                    cblAddressDepartment.DataBind();
                }
                else
                {
                    // MenuAddressMain.Visible = true;
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE));
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();
                    cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
                    cblProduct.DataTextField = "FLDQUICKNAME";
                    cblProduct.DataValueField = "FLDQUICKCODE";
                    cblProduct.DataBind();
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
        if ((CurrentControl is TextBox) ||
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

    protected void MenuSupplierRegister_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            int? principalid;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptClosePopup += "fnReloadList('AddAddress');";
            scriptClosePopup += "</script>" + "\n";

            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)ucAddress.FindControl("txtPhone2");

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
            scriptRefreshDontClose += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["addresscode"] = null;
                Reset();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
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
                if (!IsOwnerBillingPartyMapping())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ucDepositAmount.Text != "")
                {
                    if (!IsValidCurrency())
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                if (chkPaymentOnInvReceived.Checked == true)
                {
                    if (chkreceiveinvoice.Checked == false)
                    {
                        ucError.ErrorMessage = "Receiving Invoices From Consulate should be 'YES'";
                        ucError.Visible = true;
                        return;
                    }
                }

                if (!phoneno.IsValidPhoneNumber())
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    StringBuilder strproducttype = new StringBuilder();
                    foreach (ListItem item in cblProduct.Items)
                    {
                        if (item.Selected == true)
                        {
                            strproducttype.Append(item.Value.ToString());
                            strproducttype.Append(",");
                        }
                    }
                    if (strproducttype.Length > 1)
                    {
                        strproducttype.Remove(strproducttype.Length - 1, 1);
                    }

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
                    if (chkPrioritySupplier.Checked == true)
                    {
                        prioritysupplier = 1;
                    }

                    if (straddresstype.ToString() != "")
                    {
                        if (straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "SUP")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "CHA")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "CST")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "OMJ")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "FLG")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "OWN")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "PAI")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "PRI")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "UON")) ||
                            straddresstype.ToString().Contains(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "UNP")) ||
                            chkPrioritySupplier.Checked == true)
                        {

                            foreach (ListItem item in cblAddressType.Items)
                            {
                                if (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "SUP") ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "CHA")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "CST")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "OMJ")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "FLG")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "OWN")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "PAI")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "PRI")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "UON")) ||
                                (item.Selected == true && item.Value.ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "UNP")) &&
                                 chkPrioritySupplier.Checked != true)

                                    prioritysupplier = 1;
                            }
                        }
                        else
                            prioritysupplier = 0;
                    }


                    string owner = straddresstype.ToString();
                    if (owner.Contains(PhoenixCommonRegisters.GetHardCode(1, 33, "OWN")) == true)
                    {
                        principalid = Convert.ToInt32(ucPrincipal.SelectedAddress);
                    }
                    else
                    {
                        principalid = null;
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
                        if (txtDefaultDiscount.Text != "")
                        {
                            if (!IsValidDiscount(txtDefaultDiscount.Text))
                            {
                                ucError.Visible = true;
                                return;
                            }
                        }
                        DataTable dt = PhoenixRegistersAddress.InsertAddress(
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
                            General.GetNullableString(strproducttype.ToString()),
                            General.GetNullableString(ucAddress.Attention),
                            General.GetNullableString(ucAddress.InCharge),
                            ucAddress.code,
                            General.GetNullableInteger(ucAddress.QAGrading),
                            principalid,
                            chkGSTApplicable.Checked ? 1 : 0,
                            General.GetNullableInteger(ucRFQPreference.SelectedHard),
                            General.GetNullableInteger(ucFlag.SelectedFlag),
                            null, General.GetNullableInteger(txtNoOfCreditDays.Text),
                            General.GetNullableInteger(ddlZone.SelectedZone),
                            chkDelayedUtilizationApplicable.Checked ? 1 : 0,
                            prioritysupplier,
                            General.GetNullableDecimal(txtDefaultDiscount.Text),
                            General.GetNullableDateTime(txtEffectiveDate.Text),
                            General.GetNullableInteger(ucACAsstIncharge.SelectedUser),
                            General.GetNullableInteger(ucACManagerInCharge.SelectedUser),
                            General.GetNullableInteger(txtTimelimit.Text),
                            General.GetNullableString(ucAddress.Telephoneno),
                            General.GetNullableString(ucAddress.Mobileno),
                            txtBusinessProfile.Text,
                            General.GetNullableDecimal(txtDefDiscount.Text),
                            General.GetNullableDateTime(txtEffDate.Text),
                            General.GetNullableString(strAddressDepartment.ToString()),
                            General.GetNullableString(txtFeedback.Text),
                            General.GetNullableInteger(chkreceiveinvoice.Checked ? "1" : "0"),
                            General.GetNullableInteger(ucDepositCurrency.SelectedCurrency),
                            General.GetNullableDecimal(ucDepositAmount.Text),
                            General.GetNullableInteger(chkPaymentOnInvReceived.Checked ? "1" : "0"),
                            General.GetNullableInteger(ddlBankChargebasis.SelectedHard),
                            General.GetNullableInteger(chkReceiveFromSupplier.Checked ? "1" : "0"),null,General.GetNullableInteger(txtportid.Text),null,null,
                            General.GetNullableInteger(txtduedays.Text));

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["ADDRESSCODE"] = dt.Rows[0][0].ToString();
                            //MenuAddressMain.Visible = true;
                        }
                        AddressEdit();
                        ucStatus.Text = "Address information saved";
                    }
                    else if (ViewState["ADDRESSCODE"] != null)
                    {
                        if (txtDefaultDiscount.Text != "")
                        {
                            if (!IsValidDiscount(txtDefaultDiscount.Text))
                            {
                                ucError.Visible = true;
                                return;
                            }
                        }

                        PhoenixRegistersAddress.UpdateAddress(
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
                            General.GetNullableString(strproducttype.ToString()),
                            General.GetNullableString(ucAddress.Attention),
                            General.GetNullableString(ucAddress.InCharge),
                            ucAddress.code,
                            General.GetNullableInteger(ucAddress.QAGrading),
                            General.GetNullableInteger(lblOwner.Text),
                            principalid,
                            chkGSTApplicable.Checked ? 1 : 0,
                            General.GetNullableInteger(ucRFQPreference.SelectedHard),
                            General.GetNullableInteger(ucFlag.SelectedFlag),
                            null, General.GetNullableInteger(txtNoOfCreditDays.Text),
                            General.GetNullableInteger(ddlZone.SelectedZone),
                            chkDelayedUtilizationApplicable.Checked ? 1 : 0,
                            prioritysupplier,
                            General.GetNullableDecimal(txtDefaultDiscount.Text),
                            General.GetNullableDateTime(txtEffectiveDate.Text),
                            General.GetNullableInteger(ucACAsstIncharge.SelectedUser),
                            General.GetNullableInteger(ucACManagerInCharge.SelectedUser),
                            General.GetNullableInteger(txtTimelimit.Text),
                            General.GetNullableString(ucAddress.Telephoneno),
                            General.GetNullableString(ucAddress.Mobileno),
                            txtBusinessProfile.Text,
                            General.GetNullableDecimal(txtDefDiscount.Text),
                            General.GetNullableDateTime(txtEffDate.Text),
                            General.GetNullableString(strAddressDepartment.ToString()),
                            General.GetNullableString(txtFeedback.Text),
                            General.GetNullableInteger(chkreceiveinvoice.Checked ? "1" : "0"),
                            General.GetNullableInteger(ucDepositCurrency.SelectedCurrency),
                            General.GetNullableDecimal(ucDepositAmount.Text),
                            General.GetNullableInteger(chkPaymentOnInvReceived.Checked ? "1" : "0"),
                            General.GetNullableInteger(ddlBankChargebasis.SelectedHard),
                            General.GetNullableInteger(chkReceiveFromSupplier.Checked ? "1" : "0"), null, null,General.GetNullableInteger(txtportid.Text), null,null,
                            General.GetNullableInteger(txtduedays.Text));

                        AddressEdit();
                        //   Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);
                        ucStatus.Text = "Address information updated";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsOwnerBillingPartyMapping()
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
        string owner = straddresstype.ToString();
        string flagid = straddresstype.ToString();
        string clinicid = straddresstype.ToString();
        string timeperiod = straddresstype.ToString();
        int result;
        if (owner.Contains(PhoenixCommonRegisters.GetHardCode(1, 33, "OWN")) == true)
            if (General.GetNullableInteger(ucPrincipal.SelectedAddress) == null)
                ucError.ErrorMessage = "Principal is required for the Registered Owner.";

        if (flagid.Contains(PhoenixCommonRegisters.GetHardCode(1, 33, "FLG")) == true)
            if (int.TryParse(ucFlag.SelectedFlag, out result) == false)
                ucError.ErrorMessage = "Flag is required";

        if (clinicid.Contains(PhoenixCommonRegisters.GetHardCode(1, 33, "VSL")) == true)
            if (ucAddress.Attention.Trim().Equals(""))
                ucError.ErrorMessage = "For Clinic, Attention (Doctor Name) is required";

        if (timeperiod.Contains(PhoenixCommonRegisters.GetHardCode(1, 33, "UON")) == true)
            if (txtTimelimit.Text.Trim().Equals(""))
                ucError.ErrorMessage = "For Union, Time Limit for the Medical Case is required";

        return (!ucError.IsError);
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
        chkGSTApplicable.Checked = false;
        foreach (ListItem item in cblAddressType.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in cblProduct.Items)
        {
            item.Selected = false;
        }
        ucAddress.Attention = "";
        ucAddress.InCharge = "";
        ucAddress.code = "";
        chkPrioritySupplier.Checked = false;
        txtDefaultDiscount.Text = "";
        txtEffectiveDate.Text = "";
        txtDefDiscount.Text = "";
        txtEffDate.Text = "";
        ucACManagerInCharge.SelectedUser = "";
        ucACAsstIncharge.SelectedUser = "";
        txtduedays.Text = "";
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
            RadLabel lblISDCode = (RadLabel)ucAddress.FindControl("lblISDCode");
            Int64 addresscode = Convert.ToInt64(ViewState["ADDRESSCODE"]);
            DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);

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
                lblISDCode.Text = (draddress["FLDISDCODE"].ToString().Equals("")) ? "" : "(" + draddress["FLDISDCODE"].ToString() + ")";
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
                ucFlag.SelectedFlag = draddress["FLDFLAGID"].ToString();
                ucRFQPreference.SelectedHard = draddress["FLDRFQPREFERENCE"].ToString();
                txtNoOfCreditDays.Text = draddress["FLDNOOFCREDITDAYS"].ToString();
                ddlZone.SelectedZone = draddress["FLDZONEID"].ToString();
                chkreceiveinvoice.Checked = draddress["FLDRECEIVINGINVOICE"].ToString() == "1" ? true : false;
                chkPaymentOnInvReceived.Checked = draddress["FLDPAYMENTONRECEIVEINVOICE"].ToString() == "1" ? true : false;
                ucDepositCurrency.SelectedCurrency = draddress["FLDDEPOSITCURRENCY"].ToString();
                ucDepositAmount.Text = draddress["FLDDEPOSITAMOUNT"].ToString();

                chkPrioritySupplier.Checked = draddress["FLDPRIORITYSUPPLIER"].ToString() == "1" ? true : false;
                txtEffectiveDate.Text = draddress["FLDEFFECTIVEDATE"].ToString();
                txtDefaultDiscount.Text = draddress["FLDDEFAULTDISCOUNT"].ToString();
                txtBusinessProfile.Text = draddress["FLDBUSINESSPROFILE"].ToString();

                txtEffDate.Text = draddress["FLDEFFDATE"].ToString();
                txtDefDiscount.Text = draddress["FLDDEFDISCOUNT"].ToString();

                ucACAsstIncharge.SelectedUser = draddress["FLDACCOUNTASSISTANT"].ToString();
                ucACManagerInCharge.SelectedUser = draddress["FLDACCOUNTMANAGER"].ToString();

                txtTimelimit.Text = draddress["FLDTIMELIMIT"].ToString();
                ucAddress.Telephoneno = draddress["FLDAOHTELEPHONENO"].ToString();
                ucAddress.Mobileno = draddress["FLDAOHMOBILENO"].ToString();
                txtduedays.Text = draddress["FLDREBATEDUEDAYS"].ToString();

                ddlBankChargebasis.SelectedHard = draddress["FLDSUPPLIERBANKCHARGEBASIS"].ToString();

                chkReceiveFromSupplier.Checked = draddress["FLDRECEIVINGINVOICEFROMSUPPLIER"].ToString() == "1" ? true : false;
                txtportid.Text = draddress["FLDSEAPORTID"].ToString();
                string[] addresstype = draddress["FLDADDRESSTYPE"].ToString().Split(',');
                string[] producttype = draddress["FLDPRODUCTTYPE"].ToString().Split(',');
                string[] addressdepartment = draddress["FLDADDRESSDEPARTMENT"].ToString().Split(',');
                Session["COUNTRYCODE"] = draddress["FLDCOUNTRYID"].ToString();
                txtFeedback.Text = draddress["FLDFEEDBACKONVENDOR"].ToString();
                if (draddress["FLDISGSTAPPLICABLE"].ToString() == "1")
                {
                    chkGSTApplicable.Checked = true;
                }
                else
                {
                    chkGSTApplicable.Checked = false;
                }
                if (draddress["FLDDELAYEDUTILIZATION"].ToString() == "1")
                {
                    chkDelayedUtilizationApplicable.Checked = true;
                }
                else
                {
                    chkDelayedUtilizationApplicable.Checked = false;
                }
                foreach (string item in addresstype)
                {
                    if (item.Trim() != "")
                    {
                        cblAddressType.Items.FindByValue(item).Selected = true;
                    }
                }

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        cblProduct.Items.FindByValue(item).Selected = true;
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
            DataTable dtOwnerMapping = dsaddress.Tables[1];

            if (dtOwnerMapping.Rows.Count > 0)
            {
                DataRow drowner = dsaddress.Tables[1].Rows[0];
                ucPrincipal.SelectedAddress = drowner["FLDOWNERID"].ToString();
                lblOwner.Text = drowner["FLDOWNERID"].ToString();
            }
            else
                lblOwner.Text = null;

            ViewState["addresscode"] = addresscode;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidDiscount(string discount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Convert.ToDecimal(discount) < 0 || Convert.ToDecimal(discount) > 100)

            ucError.ErrorMessage = "Please enter discount between 0% to 100%";

        return (!ucError.IsError);
    }

    public bool IsValidCurrency()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucDepositCurrency.SelectedCurrency.ToString().ToUpper().Equals("DUMMY") || ucDepositCurrency.SelectedCurrency.ToString().Equals(""))

            ucError.ErrorMessage = "Please Select Currency for the Deposit Amount";

        return (!ucError.IsError);
    }
}

