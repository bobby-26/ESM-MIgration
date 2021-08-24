using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewLicenceProcessAddress : PhoenixBasePage
{
    Guid g = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Request.QueryString["pid"]))
            {
                Response.Redirect("CrewLicenceRequestFilter.aspx?e=1", false);
                return;
            }
			PhoenixToolbar toolbarAddress = new PhoenixToolbar();
			//toolbarAddress.AddButton("Save", "SAVE");
			toolbarAddress.AddButton("Make Payment", "PO");
			MenuAddressMain.AccessRights = this.ViewState;
			MenuAddressMain.MenuList = toolbarAddress.Show();
            g = new Guid(Request.QueryString["pid"]);
            if (!IsPostBack)
            {
                ViewState["ADDRESSCODE"] = string.Empty;
                ViewState["FLAGID"] = string.Empty;
                if (Request.QueryString["pid"] != null)
                {
                   Filter.CurrentLicenceRequestFilter = Request.QueryString["pid"];
                   ViewState["PID"] = Request.QueryString["pid"];
                }
                ProcessEdit();
                DisableAddress();
                ddlFlagAddress.DataSource = PhoenixRegistersAddress.ListFlagAddress(General.GetNullableInteger(ViewState["FLAGID"].ToString()));
                ddlFlagAddress.DataTextField = "FLDNAME";
                ddlFlagAddress.DataValueField = "FLDADDRESSCODE";
                ddlFlagAddress.DataBind();
                BankBind();

				PhoenixToolbar toolbarmenu = new PhoenixToolbar();
				toolbarmenu.AddButton("Seafarers", "LINEITEM");
				toolbarmenu.AddButton("Payment", "FORM");
				MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbarmenu.Show();

				MenuHeader.SelectedMenuIndex = 1;

                if (ViewState["ADDRESSCODE"].ToString() != string.Empty) ddlFlagAddress.SelectedValue = ViewState["ADDRESSCODE"].ToString();
                AddressEdit(); 
            }
            
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

		if (dce.CommandName.ToUpper().Equals("LINEITEM"))
		{

			Response.Redirect("../Crew/CrewLicenceProcessGeneral.aspx?pid=" + Request.QueryString["pid"], false);
		}

	}
    private void DisableAddress()
    {
        foreach (Control c in ucAddress.Controls)
        {
            if (c is TextBox)
            {
                TextBox txt = c as TextBox;
                txt.ReadOnly = true;
                txt.CssClass = "readonlytextbox";
            }
            if (c.GetType().FullName.Contains("usercontrols"))
            {
                foreach (Control cs in c.Controls)
                {
                    if (cs is DropDownList)
                    {
                        DropDownList ddl = cs as DropDownList;
                        ddl.Enabled = false;
                        ddl.CssClass = "readonlytextbox";
                    }
                }
            }
        }
    }
	protected void BankBind()
	{
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBank.DataSource = ds;
            ddlBank.DataTextField = "FLDBANKNAME";
            ddlBank.DataValueField = "FLDBANKID";
            ddlBank.DataBind();
            ProcessEdit();
        }
        else
        {
            ddlBank.Items.Clear();
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            txtCurrency.Text = "";
        }
    }
	protected void BankCurrency(object sender, EventArgs e)
	{
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()),
            General.GetNullableInteger(ddlBank.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows.Count == 1)
            {

                txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                txtBeneficiaryName.Text = ds.Tables[0].Rows[0]["FLDBENEFICIARYNAME"].ToString();
                txtBankAccount.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNUMBER"].ToString();
            }
            else
            {
                txtCurrency.Text = "";
            }
        }
		
	}
    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("CrewLicenceRequestFilter.aspx", false);
            }
            //else if (dce.CommandName.ToUpper().Equals("SAVE"))
            //{
            //    string p = Filter.CurrentLicenceRequestFilter;

            //    if (!IsValidAddress())
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }

            //    if (p == null)
            //    {
            //        PhoenixCrewLicenceRequest.UpdateCrewLicenceProcess(g, int.Parse(ddlFlagAddress.SelectedValue),
            //            General.GetNullableInteger(ddlBank.SelectedValue));
            //        ucStatus.Text = "Flag Address Updated.";
            //        ProcessEdit();
            //    }
            //    else
            //    {
            //        // Retreiving process ids of all selected requests(which are not paid yet).
            //        string pid = Filter.CurrentLicenceRequestFilter;
            //        DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(null
            //                                                                , null
            //                                                                , null, null
            //                                                                , 1, General.ShowRecords(null)
            //                                                                , ref iRowCount, ref iTotalPageCount
            //                                                                , null
            //                                                                , null
            //                                                                , pid,null,null,1,null,null);

            //        // Saving same consulate address for all selected requests(which are not paid yet).
            //        for (int j = 0; j < dt.Rows.Count; j++)
            //        {
            //            string prid = dt.Rows[j]["FLDPROCESSID"].ToString();
            //            PhoenixCrewLicenceRequest.UpdateCrewLicenceProcess(new Guid(prid), int.Parse(ddlFlagAddress.SelectedValue),
            //                            General.GetNullableInteger(ddlBank.SelectedValue));
            //            ucStatus.Text = "Flag Address Updated for all selected requests.";
            //        }
            //        ProcessEdit();
            //        String script = String.Format("javascript:parent.fnReloadList('code1');");
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            //    }
            //}
            else if (dce.CommandName.ToUpper().Equals("PO"))
            {
                //string pids = Filter.CurrentLicenceRequestFilter;
                string pids = ViewState["PID"].ToString();

                if (!string.IsNullOrEmpty(pids))
                {
                    PhoenixCrewLicenceRequest.UpdateCrewLicenceProcess(new Guid(pids), int.Parse(ddlFlagAddress.SelectedValue),
                                      General.GetNullableInteger(ddlBank.SelectedValue));


                    // Retreiving process ids of all selected requests(which are not paid yet).
                    DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(null
                                                                             , null
                                                                             , null, null
                                                                             , 1, General.ShowRecords(null)
                                                                             , ref iRowCount, ref iTotalPageCount
                                                                             , null
                                                                             , null
                                                                             , pids, null, null, 1, null, null);

                    // Saving same consulate address for all selected requests(which are not paid yet).
                    if (!IsValidPaymentAddress())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    string pid = dt1.Rows[j]["FLDPROCESSID"].ToString();
                    //    PhoenixCrewLicenceRequest.UpdateCrewLicenceProcess(new Guid(pid), int.Parse(ddlFlagAddress.SelectedValue),
                    //                    General.GetNullableInteger(ddlBank.SelectedValue));
                    //}

                    // Make payment for all selected requests.

                    if (!IsValidPO(dt))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string SupplierCode = dt.Rows[i]["FLDADDRESSCODE"].ToString();
                        string Amount = dt.Rows[i]["FLDAMOUNT"].ToString();
                        string CurrencyId = dt.Rows[i]["FLDCURRENCYID"].ToString();
                        string RefNo = dt.Rows[i]["FLDREFNUMBER"].ToString();
                        string ProcessId = dt.Rows[i]["FLDPROCESSID"].ToString();
                        string VesselId = dt.Rows[i]["FLDVESSELLIST"].ToString();
                        string BudgetCode = dt.Rows[i]["FLDBUDGETID"].ToString();
                        string BankId = dt.Rows[i]["FLDBANKID"].ToString();
                        string BillToCompanyId = dt.Rows[i]["FLDBILLTOCOMPANYID"].ToString();

                        DataSet dspaidbyowner = new DataSet();

                        dspaidbyowner = PhoenixCrewLicenceRequest.LicenceRequestPaidbyOwnerDocuments(new Guid(ProcessId), 1);

                        if (dspaidbyowner.Tables[0].Rows.Count > 0)
                        {

                            PhoenixCrewLicenceRequest.UpdateCrewLicenceProcessStatus(
                                                new Guid(ProcessId), int.Parse(PhoenixCommonRegisters.GetHardCode(1, 123, "LPO")));

                            PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                int.Parse(SupplierCode), // supplier
                                decimal.Parse(Amount), // amount
                                DateTime.Parse(DateTime.Now.ToString()), // pay date
                                int.Parse(CurrencyId), //currency
                                null, //remarks
                                RefNo, // reference document
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "APD")), // payment status -- draft
                                General.GetNullableGuid(ProcessId), // order id -- licence process id
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")), // type - licence req.
                                General.GetNullableInteger(BillToCompanyId), // companyid
                                General.GetNullableInteger(BudgetCode), // budget code
                                General.GetNullableInteger(VesselId), // vessel id
                                General.GetNullableInteger(BankId) // bankid
                                );

                            ucStatus.Text = "Payment Created successfully.";
                        }

                        DataSet dsnotpaidbyowner = new DataSet();

                        dsnotpaidbyowner = PhoenixCrewLicenceRequest.LicenceRequestPaidbyOwnerDocuments(new Guid(ProcessId), 0);

                        if (dsnotpaidbyowner.Tables[0].Rows.Count > 0)
                        {

                            PhoenixCrewLicenceRequest.UpdateCrewLicenceProcessStatus(
                                                new Guid(ProcessId), int.Parse(PhoenixCommonRegisters.GetHardCode(1, 123, "LPO")));

                            PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                int.Parse(SupplierCode), // supplier
                                decimal.Parse(Amount), // amount
                                DateTime.Parse(DateTime.Now.ToString()), // pay date
                                int.Parse(CurrencyId), //currency
                                null, //remarks
                                RefNo, // reference document
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "APD")), // payment status -- draft
                                General.GetNullableGuid(ProcessId), // order id -- licence process id
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")), // type - licence req.
                                General.GetNullableInteger(BillToCompanyId), // companyid
                                General.GetNullableInteger(BudgetCode), // budget code
                                General.GetNullableInteger(VesselId), // vessel id
                                General.GetNullableInteger(BankId) // bankid
                                );

                            ucStatus.Text = "Payment Created successfully.";
                        }

                        String script = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                        //PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(
                        //    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        //    int.Parse(VesselId),
                        //    int.Parse(BudgetCode), // BUDGET CODE
                        //    DateTime.Parse(DateTime.Now.ToString()), // BUDGET DATE
                        //    decimal.Parse(Amount), // AMOUNT
                        //    0, // TRANS TYPE
                        //    'C', // DEBIT OR CREDIT
                        //    RefNo,
                        //    ProcessId,
                        //    int.Parse(CurrencyId),
                        //    decimal.Parse(Amount), // amount in usd
                        //    General.GetNullableDateTime(DateTime.Now.ToString()), // date of approval
                        //    null, // date  of posting
                        //    null, // date of removal
                        //    General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 123, "LPO")), // status
                        //    General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")), // type
                        //    General.GetNullableInteger(SupplierCode));
                    }
                    Filter.CurrentLicenceRequestFilter = null;
                }
                else
                {
                    ucError.ErrorMessage = "Please select atleast one request to make payment";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAddress()
    {
        if(ddlFlagAddress.SelectedItem.Text == "--Select--")
            ucError.ErrorMessage = "'Consulate Address' Required";

        return (!ucError.IsError);
    }

    private bool IsValidPaymentAddress()
    {
        if (ddlFlagAddress.SelectedItem.Text == "--Select--")
            ucError.ErrorMessage = "'Consulate Address' Required";

        if (General.GetNullableInteger (ddlBank.SelectedValue) == null)
            ucError.ErrorMessage = "'Bank is required'";

        return (!ucError.IsError);
    }

    private bool IsValidPO(DataTable dt)
    {
        ucError.HeaderMessage = "Mandatory Information Missing for Making Payment";

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string SupplierCode = dt.Rows[i]["FLDADDRESSCODE"].ToString();
                string Amount = dt.Rows[i]["FLDAMOUNT"].ToString();
                string CurrencyId = dt.Rows[i]["FLDCURRENCYID"].ToString();
                string RefNo = dt.Rows[i]["FLDREFNUMBER"].ToString();
                string ProcessId = dt.Rows[i]["FLDPROCESSID"].ToString();
                string VesselId = dt.Rows[i]["FLDVESSELLIST"].ToString();
                string BudgetCode = dt.Rows[i]["FLDBUDGETID"].ToString();
                string BankId = dt.Rows[i]["FLDBANKID"].ToString();
                string BillToCompanyId = dt.Rows[i]["FLDBILLTOCOMPANYID"].ToString();

                if (string.IsNullOrEmpty(SupplierCode) || string.IsNullOrEmpty(Amount) || string.IsNullOrEmpty(CurrencyId) ||
                    string.IsNullOrEmpty(RefNo) || string.IsNullOrEmpty(ProcessId) || string.IsNullOrEmpty(VesselId) ||
                    string.IsNullOrEmpty(BudgetCode) || string.IsNullOrEmpty(BankId) || string.IsNullOrEmpty(BillToCompanyId))
                {
                    ucError.ErrorMessage = "Please ensure that the following information are filled for each selected request:";
                    ucError.ErrorMessage = "'Consulate Address'";
                    ucError.ErrorMessage = "'Bank'";
                    ucError.ErrorMessage = "'Currency'";
                    ucError.ErrorMessage = "'Request Number'";                    
                    ucError.ErrorMessage = "'Vessel'";
                    ucError.ErrorMessage = "'Bill To Company'";
                    ucError.ErrorMessage = "'Budget code'";
                    ucError.ErrorMessage = "'Amount'";
                    break;        
                }
            }
        }        
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
        ucAddress.Attention = "";
        ucAddress.InCharge = "";
        ucAddress.code = "";
        ucAddress.Status = "";
    }
    private void ProcessEdit()
    {
        DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceProcess(g);
        if (dt.Rows.Count > 0)
        {
            ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();
            if (dt.Rows[0]["FLDBANKID"].ToString() != "")
            {
                ddlBank.SelectedValue = dt.Rows[0]["FLDBANKID"].ToString();
            }
            //txtBankName.Text = dt.Rows[0]["FLDBANKNAME"].ToString();
            //txtAccountNo.Text = dt.Rows[0]["FLDACCOUNTNUMBER"].ToString();
            //imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + ViewState["ADDRESSCODE"].ToString() + "', true);");

            if (dt.Rows[0]["FLDBANKID"].ToString() != "")
            {
                ddlBank.SelectedValue = dt.Rows[0]["FLDBANKID"].ToString();
                txtCurrency.Text = dt.Rows[0]["FLDCURRENCYCODE"].ToString();
                txtBeneficiaryName.Text = dt.Rows[0]["FLDBENEFICIARYNAME"].ToString();
                txtBankAccount.Text = dt.Rows[0]["FLDACCOUNTNUMBER"].ToString();
            }
            else
            {
                txtCurrency.Text = "";
                txtBeneficiaryName.Text = "";
            }
        }
    }

    protected void AddressEdit()
    {
        try
        {
			if (!string.IsNullOrEmpty(ViewState["ADDRESSCODE"].ToString()) && ViewState["ADDRESSCODE"].ToString()!="Dummy")
            {
                Int64 addresscode = Convert.ToInt64(ViewState["ADDRESSCODE"]);
                DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);
                if (dsaddress.Tables.Count > 0)
                {
                    DataRow draddress = dsaddress.Tables[0].Rows[0];
                    ucAddress.Name = draddress["FLDNAME"].ToString();
                    ucAddress.Address1 = draddress["FLDADDRESS1"].ToString();
                    ucAddress.Address2 = draddress["FLDADDRESS2"].ToString();
                    ucAddress.Address3 = draddress["FLDADDRESS3"].ToString();
                    ucAddress.Country = draddress["FLDCOUNTRYID"].ToString();
                    ucAddress.QAGrading = draddress["FLDQAGRADING"].ToString();
                    ucAddress.State = draddress["FLDSTATE"].ToString();
                    ucAddress.City = draddress["FLDCITY"].ToString();
                    ucAddress.PostalCode = draddress["FLDPOSTALCODE"].ToString();
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
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlFlagAddress_DataBound(object sender, EventArgs e)
    {
        ddlFlagAddress.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void ddlFlagAddress_TextChanged(object sender, EventArgs e)
    {
        if (ddlFlagAddress.SelectedValue != string.Empty)
        {
            ViewState["ADDRESSCODE"] = ddlFlagAddress.SelectedValue;
            ddlBank.SelectedValue = "";
			AddressEdit();
			BankBind(); 
        }
        else
            Reset();
    }
}
