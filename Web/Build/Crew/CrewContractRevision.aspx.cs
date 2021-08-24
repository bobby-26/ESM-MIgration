using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewContractRevision : PhoenixBasePage
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
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract", "CONTTRACT");
            toolbar.AddButton("Reimbursement/Deduction", "REIM");
            toolbar.AddButton("Contract Paper", "CONTRACTPAPER");
            toolbar.AddButton("Revise Contract", "REVISION");
            toolbar.AddButton("Contract Lock History", "LOCKHISTORY");
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.SelectedMenuIndex = 3;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCrewContractSub.AccessRights = this.ViewState;
            MenuCrewContractSub.MenuList = toolbar.Show();

            strEmployeeId = Request.QueryString["empid"];
            rankid = Request.QueryString["rnkid"];
            vesselid = Request.QueryString["vslid"];
            if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
                planid = Request.QueryString["planid"];
            if (!IsPostBack)
            {
                ViewState["SSCALE"] = "";
                ViewState["PAYDATE"] = Request.QueryString["date"];
                EditContractDetails();

                if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
                {
                    ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                    ddlUnion.DataBind();
                    ddlUnion.SelectedAddress = Request.QueryString["Unionid"];
                    FetchCBARevision();
                }
                ddlRankMonth.SelectedValue = PhoenixCrewContract.GetContractRankExp(int.Parse(strEmployeeId), int.Parse(ViewState["RANKID"].ToString()), int.Parse(vesselid)
                    , DateTime.Parse(ViewState["PAYDATE"].ToString())).Rows[0][0].ToString().TrimEnd('0').TrimEnd('.');
           
                //ucError.Text = "";
                //ucError.Visible = true;

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
        DataSet ds = PhoenixCrewContract.ListCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(string.IsNullOrEmpty(txtDate.Text) ? Request.QueryString["date"] : txtDate.Text)
            , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(ddlRankMonth.SelectedValue), null);

        txtWage.Text = ds.Tables[3].Rows[0]["FLDTOTALAMOUNT"].ToString();
        txtSubTotalCBA.Text = ds.Tables[3].Rows[0]["FLDCBAAMOUNT"].ToString();
        txtSubToatalESM.Text = ds.Tables[3].Rows[0]["FLDESMAMOUNT"].ToString();
        txtSubTotalCrew.Text = ds.Tables[3].Rows[0]["FLDCREWAMOUNT"].ToString();
        txtMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDTOTALMONTHLYAMOUNT"].ToString();
        txtNonMonthlyAmount.Text = ds.Tables[3].Rows[0]["FLDSUCCESSFULCCAMOUNT"].ToString();
        //lblMonthlyUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        //lblNonMonthlyUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        lblSubTotalCrewUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        lblSubToatalESMUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        lblSubTotalCBAUSD.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        lblUSDpermonth.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();

    }
    protected void gvContract_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixCrewContract.ListCrewContract(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(string.IsNullOrEmpty(txtDate.Text) ? Request.QueryString["date"] : txtDate.Text)
            , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale), General.GetNullableInteger(ddlRankMonth.SelectedValue), null);
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
        // txtWageCurrency.Text = ds.Tables[3].Rows[0]["FLDBASICCURRENCYCODE"].ToString();
        gvContract.DataSource = ds.Tables[0];
        gvESM.DataSource = ds.Tables[1];
        gvCrew.DataSource = ds.Tables[2];
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
                DataTable dt = PhoenixCrewContract.InsertCrewContractRevision(int.Parse(strEmployeeId), int.Parse(vesselid), int.Parse(rankid), DateTime.Parse(txtDate.Text)
                                                         , General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale)
                                                         , General.GetNullableInteger(ddlRankMonth.SelectedValue), new Guid(ddlRevision.SelectedValue), General.GetNullableGuid(planid)
                                                         , General.GetNullableInteger(ddlUnion.SelectedAddress).Value);
                ViewState["CONTRACTID"] = dt.Rows[0][0].ToString();
                if (ViewState["CONTRACTID"] != null && ViewState["CONTRACTID"].ToString() != string.Empty)
                {
                    PhoenixCrewContract.UpdateCrewContract(new Guid(ViewState["CONTRACTID"].ToString()), int.Parse(vesselid), int.Parse(rankid), int.Parse(ddlCompany.SelectedCompany)
                        , int.Parse(ddlSeaPort.SelectedValue), DateTime.Parse(txtDate.Text), byte.Parse(txtContractPeriod.Text), General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale)
                        , General.GetNullableInteger(ddlRankMonth.SelectedValue), byte.Parse(txtPlusMinusPeriod.Text), null, General.GetNullableGuid(planid), General.GetNullableInteger(ddlUnion.SelectedAddress));
                    ucStatus.Text = "Contract Updated";
                }
                ViewState["PAYDATE"] = txtDate.Text;
                EditContractDetails();
                BindData();
                Response.Redirect("CrewContract.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("CONTRACTPAPER"))
            {
                if (Request.QueryString["app"] == "0")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" +
                        (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                        + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                }
                else
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTER&contractid=" +
                      (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                      + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                }
            }
            else if (CommandName.ToUpper().Equals("REIM"))
            {
                Response.Redirect("CrewReimbursementContract.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("LOCKHISTORY"))
            {
                Response.Redirect("CrewContractLockHistory.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("CONTTRACT"))
            {
                Response.Redirect("CrewContract.aspx?" + Request.QueryString.ToString(), false);
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
        if (!int.TryParse(ddlCompany.SelectedCompany, out resultInt))
            ucError.ErrorMessage = "Contracting Party is required.";

        if (!int.TryParse(ddlSeaPort.SelectedValue, out resultInt))
            ucError.ErrorMessage = "Port of Engagement is required.";

        if (!DateTime.TryParse(txtDate.Text, out resultDate))
            ucError.ErrorMessage = "Contract/ Pay Commencement Date is required.";

        if (!int.TryParse(txtContractPeriod.Text, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        if (!int.TryParse(txtPlusMinusPeriod.Text, out resultInt))
            ucError.ErrorMessage = "Contract +/- Period is required.";

        if (ddlSeniority.CssClass == "input_mandatory" && (!int.TryParse(ddlSeniority.SelectedSeniorityScale, out resultInt)))
            ucError.ErrorMessage = "Seniority Wage Scale is required.";

        if (!General.GetNullableGuid(ddlRevision.SelectedValue).HasValue)
            ucError.ErrorMessage = "CBA Revision is required.";

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
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableDateTime(ViewState["PAYDATE"].ToString()), int.Parse(vesselid));
            if (dt.Rows.Count > 0)
            {
                ViewState["CONTRACTID"] = dt.Rows[0]["FLDCONTRACTID"].ToString();
                ViewState["BPPOOL"] = dt.Rows[0]["FLDBPPOOL"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtAddress.Text = dt.Rows[0]["FLDADDRESS"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlCompany.SelectedCompany = dt.Rows[0]["FLDCONTRACTCOMPANYID"].ToString();
                ddlSeniority.SeniorityScaleList = PhoenixRegistersSeniorityScale.ListSeniorityWageScale(string.Empty);
                ddlSeniority.SelectedSeniorityScale = dt.Rows[0]["FLDSENIORITYSCALEID"].ToString();
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                ViewState["SSCALE"] = dt.Rows[0]["FLDSENIORITYSCALE"].ToString();
                BindMonths();
                string rm = dt.Rows[0]["FLDRANKEXPERIENCE"].ToString();
                ddlRankMonth.SelectedValue = (rm != string.Empty ? rm.ToString() : string.Empty);
                ddlSeaPort.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
                ddlSeaPort.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                //txtDate.Text = dt.Rows[0]["FLDPAYDATE"].ToString();
                txtContractPeriod.Text = dt.Rows[0]["FLDCONTRACTTENURE"].ToString();
                txtPlusMinusPeriod.Text = dt.Rows[0]["FLDPLUSORMINUSPERIOD"].ToString();
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
                }
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
        BindMonths();
    }
    private void FetchCBARevision()
    {
        DataTable dt = PhoenixRegistersContract.FetchCBARevision(General.GetNullableInteger(ddlUnion.SelectedAddress));
        ddlRevision.DataSource = dt;
        ddlRevision.DataBind();
        if (dt.Rows.Count > 0) ddlRevision.SelectedIndex = 1;
    }
    private void BindMonths()
    {
        ddlRankMonth.Items.Clear();
        ddlRankMonth.SelectedIndex = -1;
        ddlRankMonth.SelectedValue = null;
        ddlRankMonth.ClearSelection();
        DataSet ds = new DataSet();
        // DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), int.Parse(vesselid));
        if (!string.IsNullOrEmpty(ViewState["SSCALE"].ToString()))
            ds = PhoenixCrewContract.GetSeniorityScaleMonths(General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale.ToString()),
                int.Parse(ViewState["RANKID"] != null ? ViewState["RANKID"].ToString() : Request.QueryString["rnkid"]),
                                                           int.Parse(Request.QueryString["vslid"].ToString()));
        else
            ds = PhoenixCrewContract.GetSeniorityScaleMonths(null, int.Parse(ViewState["RANKID"] != null ? ViewState["RANKID"].ToString() : Request.QueryString["rnkid"]),
                                                           int.Parse(Request.QueryString["vslid"].ToString()));

        ddlRankMonth.DataTextField = "FLDMONTHS";
        ddlRankMonth.DataValueField = "FLDMONTHS";
        ddlRankMonth.DataSource = ds;
        ddlRankMonth.DataBind();
    }
    protected void ddlRankMonth_DataBound(object sender, EventArgs e)
    {
        foreach (RadComboBoxItem item in ddlRankMonth.Items)
        {
            if (ViewState["BPPOOL"] != null && ViewState["BPPOOL"].ToString() == "1")
            {
                item.Text = (int.Parse(item.Text) / 12).ToString();
            }
        }
    }

    protected void ddlUnion_TextChangedEvent(object sender, EventArgs e)
    {
        FetchCBARevision();
    }
}
