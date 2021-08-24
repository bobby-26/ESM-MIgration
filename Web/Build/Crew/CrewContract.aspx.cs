using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.Common;
public partial class CrewContract : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    string rankid = string.Empty;
    string vesselid = string.Empty;
    string planid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract", "CONTTRACT");
            toolbar.AddButton("Reimbursement/Deduction", "REIM");
            toolbar.AddButton("Contract Paper", "CONTRACT");
            toolbar.AddButton("Revise Contract", "REVISION");
            toolbar.AddButton("Contract Lock History", "LOCKHISTORY");
            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 0;
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;

            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            rankid = Request.QueryString["rnkid"];
            vesselid = Request.QueryString["vslid"];
            if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
                planid = Request.QueryString["planid"];
            if (!IsPostBack)
            {
                btnconfirm.Attributes.Add("style", "display:none");
                btndelete.Attributes.Add("style", "display:none");
                btnunlock.Attributes.Add("style", "display:none");
                ViewState["PAYDATE"] = null;
                ViewState["PRANK"] = null;
                ViewState["LOCKED"] = "0";
                ViewState["APP"] = "0";
                ViewState["EXTENSIONYN"] = "0";
                ViewState["SSCALE"] = "";
                PhoenixCrewContract.ArchiveOldCrewContract(int.Parse(strEmployeeId));
                EditContractDetails();
                string offerletter = PhoenixCommonRegisters.GetHardCode(1, 266, "OFC");
                if (offerletter == null || offerletter == "")
                    tblofferletter.Attributes.Add("style", "display:none;");
                if (ViewState["CONTRACTID"] == null)
                {

                    DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(int.Parse(vesselid));
                    string s = ds.Tables[0].Rows[0]["FLDSENIORITYSCALE"].ToString();
                    if (!string.IsNullOrEmpty(s))
                    {
                        ddlSeniority.SeniorityScaleList = PhoenixRegistersSeniorityScale.ListSeniorityWageScale(string.Empty);
                        ddlSeniority.SelectedSeniorityScale = s;
                    }

                    DataTable dt = PhoenixCrewContract.GetEmployeeRankExp(int.Parse(strEmployeeId), int.Parse(rankid), General.GetNullableInteger(vesselid));
                    string se = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEXPERIENCE"].ToString() : "0.0";
                    string re = (se != string.Empty ? se.Substring(0, se.IndexOf('.')) : string.Empty);
                    //Inserting contract for the first time, if contract is not generated initially
                    if (string.IsNullOrEmpty(txtDate.Text) && string.IsNullOrEmpty(Request.QueryString["date"]))
                    {
                        ucError.ErrorMessage = "To Generate a contract,Planned Relief Due Date is Missing";
                        ucError.Visible = true;
                        return;

                    }
                    DataSet ds1 = PhoenixCrewContract.ListCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(string.IsNullOrEmpty(txtDate.Text) ? Request.QueryString["date"] : txtDate.Text)
                                     , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(re.ToString()), null, General.GetNullableGuid(planid), null);
                    ViewState["CONTRACTID"] = ds1.Tables[3].Rows[0]["FLDCONTRACTID"].ToString();
                    BindMonths();
                    ddlRankMonth.SelectedValue = ds1.Tables[3].Rows[0]["FLDRANKEXP"].ToString();
                }
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
            }
            BindData();
            if (!IsPostBack)
            {
                ListRevisionHistory();
                EditContractDetails();
            }
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("CrewContract.aspx?" + Request.QueryString.ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvContract_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixCrewContract.ListCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(string.IsNullOrEmpty(txtDate.Text) ? Request.QueryString["date"] : txtDate.Text)
              , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(ddlRankMonth.SelectedValue)
              , General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), General.GetNullableGuid(planid), General.GetNullableInteger(ddlUnion.SelectedAddress));
            txtWage.Text = ds.Tables[3].Rows[0]["FLDTOTALAMOUNT"].ToString();
            txtSubTotalCBA.Text = ds.Tables[3].Rows[0]["FLDCBAAMOUNT"].ToString();
            txtSubToatalESM.Text = ds.Tables[3].Rows[0]["FLDESMAMOUNT"].ToString();
            txtSubTotalCrew.Text = ds.Tables[3].Rows[0]["FLDCREWAMOUNT"].ToString();
            txtMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDTOTALMONTHLYAMOUNT"].ToString();
            txtNonMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDSUCCESSFULCCAMOUNT"].ToString();
            ViewState["LOCKED"] = ds.Tables[3].Rows[0]["FLDLOCKEDYN"].ToString();
            ViewState["CHM"] = ds.Tables[3].Rows[0]["FLDCHEMICALTANKER"].ToString();
            lblSubToatalESMUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblSubTotalCBAUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblSubTotalCrewUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblbasicCurrencyCode.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblBasicCurrency.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            txtWageCurrency.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();

            gvContract.DataSource = ds.Tables[0];
            gvESM.DataSource = ds.Tables[1];
            gvCrew.DataSource = ds.Tables[2];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewContract.ListCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(string.IsNullOrEmpty(txtDate.Text) ? Request.QueryString["date"] : txtDate.Text)
                , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(ddlRankMonth.SelectedValue)
                , General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), General.GetNullableGuid(planid), General.GetNullableInteger(ddlUnion.SelectedAddress));
            txtWage.Text = ds.Tables[3].Rows[0]["FLDTOTALAMOUNT"].ToString();
            txtSubTotalCBA.Text = ds.Tables[3].Rows[0]["FLDCBAAMOUNT"].ToString();
            txtSubToatalESM.Text = ds.Tables[3].Rows[0]["FLDESMAMOUNT"].ToString();
            txtSubTotalCrew.Text = ds.Tables[3].Rows[0]["FLDCREWAMOUNT"].ToString();
            txtMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDTOTALMONTHLYAMOUNT"].ToString();
            txtNonMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDSUCCESSFULCCAMOUNT"].ToString();
            ViewState["LOCKED"] = ds.Tables[3].Rows[0]["FLDLOCKEDYN"].ToString();
            ViewState["CHM"] = ds.Tables[3].Rows[0]["FLDCHEMICALTANKER"].ToString();
            lblSubToatalESMUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblSubTotalCBAUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblSubTotalCrewUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblbasicCurrencyCode.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            lblBasicCurrency.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            txtWageCurrency.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
            txtofferLetter.Text = ds.Tables[3].Rows[0]["FLDOFFERDAMOUNT"].ToString();
            txtothermonthlyamount.Text = ds.Tables[3].Rows[0]["FLDOTHERMONTHLYAMOUNT"].ToString();
            txtCBAAmount.Text = ds.Tables[3].Rows[0]["FLDCBATOTALAMOUNTINCLUDEDINOFFER"].ToString();
            txtstancomponent.Text = ds.Tables[3].Rows[0]["FLDSTANDARDTOTALAMOUNTINCLUDEDINOFFER"].ToString();
            txtTotalOfferletter.Text = ds.Tables[3].Rows[0]["FLDOFFERLETTERTOTAL"].ToString();
            ViewState["EXTENSIONYN"] = ds.Tables[3].Rows[0]["FLDEXTENSIONYN"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                UserControlContractCrew ddlCrewComp = (UserControlContractCrew)item.FindControl("ddlCrewComponentsAdd");
                UserControlMaskNumber txtAmount = (UserControlMaskNumber)item.FindControl("txtAmountAdd");
                UserControlCurrency currency = (UserControlCurrency)item.FindControl("ddlCurrencyAdd");
                if (!IsValidComponent(ViewState["CONTRACTID"].ToString(), ddlCrewComp.SelectedComponent.ToString(), txtAmount.Text, currency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewContract.InsertCrewContractComponent(new Guid(ViewState["CONTRACTID"].ToString()),
                    new Guid(ddlCrewComp.SelectedComponent.ToString()), decimal.Parse(txtAmount.Text), int.Parse(currency.SelectedCurrency));
                ucStatus.Text = "Component is saved successfully";
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblCrewComp = (RadLabel)e.Item.FindControl("lblCompIdEdit");
                UserControlMaskNumber txtAmount = (UserControlMaskNumber)e.Item.FindControl("txtAmount");
                UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
                if (!IsValidComponent(ViewState["CONTRACTID"].ToString(), lblCrewComp.Text, txtAmount.Text, currency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewContract.InsertCrewContractComponent(new Guid(ViewState["CONTRACTID"].ToString()),
                    new Guid(lblCrewComp.Text), decimal.Parse(txtAmount.Text), int.Parse(currency.SelectedCurrency));
                ucStatus.Text = "Component is updated successfully";
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //    string id = ((RadLabel)e.Item.FindControl("lblpaymentid")).Text.Trim();

                //    PhoenixCrewReimbursement.DleteCrewReimbursementPayment(new Guid(id));
                //    Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblComponentName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipDescription");
            if (lbtn != null && uct != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            if (currency != null)
            {
                currency.CurrencyList = PhoenixRegistersCurrency.ListCurrency(1);
                currency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
            }
            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete();");
            }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                if (ViewState["LOCKED"].ToString() == "1")
                    ed.Visible = false;
            }

            if (drv["FLDCOMPONENTID"].ToString() == string.Empty)
            {
                if (ed != null) ed.Visible = false;
                RadLabel lblComponentName = (RadLabel)e.Item.FindControl("lblComponentName");
                RadLabel lblamount = (RadLabel)e.Item.FindControl("lblamount");
                if (lblamount != null)
                    lblamount.Font.Bold = true;
                lblComponentName.Font.Bold = true;
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (ViewState["LOCKED"].ToString() == "1")
                    db.Visible = false;
            }
            UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyAdd");
            if (currency != null)
            {
                currency.CurrencyList = PhoenixRegistersCurrency.ListCurrency(1);
                currency.SelectedCurrency = "10";
            }
        }
    }

    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidContract())
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to save the record?"
                    + (ViewState["CHM"].ToString() == "1" ? "<br/>LFT of the offsigning seafarer to be completed latest by 2 weeks from the date of sign off" : string.Empty), "confirm", 320, 150, null, "Confirm");

            }
            else if (CommandName.ToUpper().Equals("DELETE"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to delete this record?", "contractDelete", 320, 150, null, "Delete");


                //PhoenixCrewContract.DeleteCrewContract(new Guid(ViewState["CONTRACTID"].ToString()), int.Parse(Request.QueryString["empid"]), int.Parse(Request.QueryString["vslid"]));
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "BookMarkScript", "parent.CloseFilterWindow('chml');", true);
            }
            else if (CommandName.ToUpper().Equals("CONTRACT"))
            {
                if (ViewState["LOCKED"].ToString() == "0")
                {
                    ucError.ErrorMessage = "Contract is not lock,you cannot view the Contact Paper.";
                    ucError.Visible = true;
                    return;
                }
                DataTable dt = PhoenixReportsCommon.LogoBind();
                if (ViewState["APP"] != null && ViewState["APP"].ToString() == "1")
                {
                    if (dt.Rows[0]["FLDLICENCECODE"].ToString() == "ESM")
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTER&contractid=" +
                           (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                           + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                    else
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTEROTHER&contractid=" +
                           (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                           + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                }
                else
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" +
                        (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                        + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "&accessfrom=1&showword=no&showexcel=no", false);
                }
            }
            else if (CommandName.ToUpper().Equals("REIM"))
            {
                Response.Redirect("CrewReimbursementContract.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&empid=" + Request.QueryString["empid"]
                       + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress, false);
            }
            else if (CommandName.ToUpper().Equals("EXTENTION"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "  setTimeout(function(){ top.openNewWindow('ext', 'Contract Extension', '" + Session["sitepath"] + "/Crew/CrewContractExtention.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "')},1000)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                //imgBtnContractExtension.Attributes.Add("onclick", "javascript:openNewWindow('ext', 'Extension', '" + Session["sitepath"] + "/Crew/CrewContractExtention.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                //    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "'); ");

                //Response.Redirect("CrewContractExtention.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                //    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress, false);
            }
            else if (CommandName.ToUpper().Equals("REVISION"))
            {
                Response.Redirect("CrewContractRevision.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress, false);
            }
            else if (CommandName.ToUpper().Equals("LOCKHISTORY"))
            {
                Response.Redirect("CrewContractLockHistory.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress, false);
            }
            else if (CommandName.ToUpper().Equals("LOCK"))
            {
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to Lock the record?", "confirm", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to UnLock?", "Unlock", 320, 150, null, "Unlock");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidContract()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        DateTime resultDate;
        decimal decimalresult;
        if (!int.TryParse(ddlCompany.SelectedCompany, out resultInt))
            ucError.ErrorMessage = "Contracting Party is required.";

        if (!int.TryParse(ddlSeaPort.SelectedValue, out resultInt))
            ucError.ErrorMessage = "Port of Engagement is required.";

        if (!DateTime.TryParse(txtDate.Text, out resultDate))
            ucError.ErrorMessage = "Pay Commences on is required.";

        if (!int.TryParse(txtContractPeriod.Text, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        if (!decimal.TryParse(txtPlusMinusPeriod.Text, out decimalresult))
            ucError.ErrorMessage = "Contract +/- Period is required.";

        if (ddlSeniority.CssClass == "input_mandatory" && (!int.TryParse(ddlSeniority.SelectedSeniorityScale, out resultInt)))
            ucError.ErrorMessage = "Seniority Wage Scale is required.";
        if (General.GetNullableInteger(txtContractPeriod.Text).HasValue
            && General.GetNullableInteger(txtPlusMinusPeriod.Text).HasValue
             && int.Parse(txtPlusMinusPeriod.Text) >= int.Parse(txtContractPeriod.Text))
            ucError.ErrorMessage = "Contract +/- Period cannot exceed the Contract Period.";
        if (!General.GetNullableInteger(ddlUnion.SelectedAddress).HasValue)
            ucError.ErrorMessage = "CBA is required.";

        return (!ucError.IsError);
    }
    private bool IsValidComponent(string contractid, string componentid, string amount, string Currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec;

        if (General.GetNullableInteger(Currency) == null)
            ucError.ErrorMessage = "Currency is required";
        if (string.IsNullOrEmpty(contractid))
            ucError.ErrorMessage = "Contract is required";
        if (string.IsNullOrEmpty(componentid))
            ucError.ErrorMessage = "Component is required";
        if (!decimal.TryParse(amount, out resultDec))
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), int.Parse(vesselid));
            if (dt.Rows.Count > 0)
            {

                ViewState["CONTRACTID"] = dt.Rows[0]["FLDCONTRACTID"].ToString();
                ViewState["PAYDATE"] = dt.Rows[0]["FLDPAYDATE"].ToString();
                ViewState["PRANK"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                ViewState["CONTRANK"] = dt.Rows[0]["FLDRANKID"].ToString();
                ViewState["BPPOOL"] = dt.Rows[0]["FLDBPPOOL"].ToString();

                //imgBtnContractExtension.Attributes.Add("onclick", "javascript:openNewWindow('ext', 'Extension', '" + Session["sitepath"] + "/Crew/CrewContractExtention.aspx?cid=" + ViewState["CONTRACTID"].ToString() + "&app=" + ViewState["APP"] + "&" + "empid=" + Request.QueryString["empid"]
                //    + "&rnkid=" + ViewState["PRANK"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + ViewState["PAYDATE"] + "&planid=" + planid + "&Unionid=" + ddlUnion.SelectedAddress + "'); ");
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtAddress.Text = dt.Rows[0]["FLDADDRESS"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtPBDate.Text = dt.Rows[0]["FLDPBDATE"].ToString();
                ddlCompany.SelectedCompany = dt.Rows[0]["FLDCONTRACTCOMPANYID"].ToString();
                ddlSeniority.SeniorityScaleList = PhoenixRegistersSeniorityScale.ListSeniorityWageScale(string.Empty);
                ddlSeniority.SelectedSeniorityScale = dt.Rows[0]["FLDSENIORITYSCALEID"].ToString();
                ViewState["SSCALE"] = dt.Rows[0]["FLDSENIORITYSCALE"].ToString();
                BindMonths();
                string rm = dt.Rows[0]["FLDRANKEXPERIENCE"].ToString();
                ddlRankMonth.SelectedValue = (rm != string.Empty ? rm.ToString() : string.Empty);
                ddlSeaPort.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
                ddlSeaPort.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                txtDate.Text = dt.Rows[0]["FLDPAYDATE"].ToString();
                txtContractPeriod.Text = dt.Rows[0]["FLDCONTRACTTENURE"].ToString();
                txtPlusMinusPeriod.Text = dt.Rows[0]["FLDPLUSORMINUSPERIOD1"].ToString();
                lblIncrementDate.Visible = false;
                txtNextIncrementDate.Visible = false;
                btnGenExpCon.Visible = false;
                if (dt.Rows[0]["FLDWAGESCALEMANDATORY"].ToString() == "1")
                    ddlSeniority.CssClass = "input_mandatory";
                else
                    ddlSeniority.CssClass = "input";

                if (dt.Rows[0]["FLDBPPOOL"].ToString() == "1")
                {
                    ddlSeniority.CssClass = "input";
                    ddlSeniority.Enabled = false;
                    ddlSeniority.SelectedSeniorityScale = "";
                    lblRankExpCaption.Text = "Years in Rank";
                    lblIncrementDate.Visible = true;
                    txtNextIncrementDate.Visible = true;
                    btnGenExpCon.Visible = true;
                    txtNextIncrementDate.Text = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString();
                    btnGenExpCon.Enabled = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString() == string.Empty ? true : false;
                    btnGenExpCon.CssClass = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString() == string.Empty ? "input" : "readonlytextbox";
                    ViewState["APP"] = dt.Rows[0]["FLDNEXTINCREMENTDATE"].ToString() == string.Empty ? "0" : "1";
                }
                ddlUnion.SelectedAddress = dt.Rows[0]["FLDUNIONID"].ToString();
                rankid = dt.Rows[0]["FLDRANKID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlSeniory_Changed(object sender, EventArgs e)
    {
        try
        {
            BindMonths();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlRevision_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["CONTRACTID"] = ddlRevision.SelectedValue;
            EditContractDetails();
            BindData();
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ListRevisionHistory()
    {
        try
        {
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableGuid(string.Empty), int.Parse(vesselid));
            ddlRevision.DataSource = dt;
            ddlRevision.DataTextFormatString = "{0:dd/MMM/yyyy}";
            ddlRevision.DataBind();
            if (dt.Rows.Count > 0)
                ViewState["PAYDATE"] = dt.Rows[0]["FLDPAYDATE"].ToString();
            if (ViewState["CONTRACTID"] != null)
                ddlRevision.SelectedValue = ViewState["CONTRACTID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMonths()
    {
        try
        {
            ddlRankMonth.Items.Clear();
            ddlRankMonth.SelectedIndex = -1;
            ddlRankMonth.SelectedValue = null;
            ddlRankMonth.ClearSelection();
            DataSet ds = new DataSet();
            // DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), int.Parse(vesselid));
            if (!string.IsNullOrEmpty(ViewState["SSCALE"].ToString()))
                ds = PhoenixCrewContract.GetSeniorityScaleMonths(General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale.ToString()),
                                                               int.Parse(Request.QueryString["rnkid"].ToString()),
                                                               int.Parse(Request.QueryString["vslid"].ToString()));
            else
                ds = PhoenixCrewContract.GetSeniorityScaleMonths(null, int.Parse(Request.QueryString["rnkid"].ToString()),
                                                               int.Parse(Request.QueryString["vslid"].ToString()));

            ddlRankMonth.DataTextField = "FLDMONTHS";
            ddlRankMonth.DataValueField = "FLDMONTHS";
            ddlRankMonth.DataSource = ds;
            ddlRankMonth.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlRankMonth_DataBound(object sender, EventArgs e)
    {
        try
        {
            foreach (RadComboBoxItem item in ddlRankMonth.Items)
            {
                if (ViewState["BPPOOL"] != null && ViewState["BPPOOL"].ToString() == "1")
                {
                    item.Text = (int.Parse(item.Text) / 12).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public DataSet Component()
    {
        DataSet ds = new DataSet();
        try
        {
            DataTable dt1 = PhoenixRegistersContract.ListContractCrew(null);


            ds.Merge(dt1);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return ds;
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            DataTable dt = PhoenixCrewContract.ListCrewContractLock(General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()).Value);
            if (dt.Rows.Count > 0)
            {
                ViewState["LOCKED"] = dt.Rows[0]["FLDLOCKEDYN"].ToString();
                if (dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0")
                {
                    toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                    toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                }
                else
                {
                    toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
                    string ContractExtenstion = PhoenixCommonRegisters.GetHardCode(1, 266, "COE");
                    if (ContractExtenstion != null && ContractExtenstion != "")
                    {
                        if (ViewState["EXTENSIONYN"].ToString() == "1")
                            toolbar.AddButton("Contract Extention", "EXTENTION", ToolBarDirection.Right);
                    }
                }
            }
            else
            {
                toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuCrewContractSub.AccessRights = this.ViewState;
            MenuCrewContractSub.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewContract.InsertCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(ViewState["CONTRANK"] == null ? rankid : ViewState["CONTRANK"].ToString()), DateTime.Parse(txtDate.Text)
                                                    , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(ddlRankMonth.SelectedValue), General.GetNullableGuid(ViewState["CONTRACTID"].ToString()), General.GetNullableInteger(ddlUnion.SelectedAddress));
            if (ViewState["CONTRACTID"] != null && ViewState["CONTRACTID"].ToString() != string.Empty)
            {
                PhoenixCrewContract.UpdateCrewContract(new Guid(ViewState["CONTRACTID"].ToString()), int.Parse(vesselid), int.Parse(rankid), int.Parse(ddlCompany.SelectedCompany)
                    , int.Parse(ddlSeaPort.SelectedValue), DateTime.Parse(txtDate.Text), byte.Parse(txtContractPeriod.Text), General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale)
                    , General.GetNullableInteger(ddlRankMonth.SelectedValue)
                    , decimal.Parse(txtPlusMinusPeriod.Text)
                    , General.GetNullableDateTime(txtPBDate.Text), General.GetNullableGuid(planid), General.GetNullableInteger(ddlUnion.SelectedAddress));
                ucStatus.Text = "Contract Updated";
            }

            PhoenixCrewContract.InsertCrewContractLock(new Guid(ViewState["CONTRACTID"].ToString()));
            ListRevisionHistory();
            EditContractDetails();
            ResetMenu(); Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewContract.DeleteCrewContract(new Guid(ViewState["CONTRACTID"].ToString()), int.Parse(Request.QueryString["empid"]), int.Parse(Request.QueryString["vslid"]));

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('chml', 'ifMoreInfo', null);", true);
            ResetMenu();
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnUnLockConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewContract.InsertCrewContractLock(new Guid(ViewState["CONTRACTID"].ToString()));
            ResetMenu();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnGenExpCon_OnClick(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewContract.UpdateCrewContractIncrementDueDate(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), new Guid(ViewState["CONTRACTID"].ToString()));
            EditContractDetails();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
