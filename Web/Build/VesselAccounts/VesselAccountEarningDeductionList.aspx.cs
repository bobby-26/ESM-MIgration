using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.IO;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountEarningDeductionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbars = new PhoenixToolbar();
        toolbars.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuEarDedGeneral.MenuList = toolbars.Show();

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString();

            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["MONTH"] = null;
            ViewState["YEAR"] = null;
            ViewState["DATE"] = null;
            ViewState["TYPE"] = 1;
            ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
            ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
            ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();

            BindEdit(0);

        }


    }

    public void BindEdit(int Bowyn)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBoardEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableInteger(ViewState["VESSELID"].ToString())
               , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
               , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString())
               , General.GetNullableDateTime(txtDate.Text)
               , General.GetNullableInteger(ViewState["TYPE"].ToString())
               , int.Parse(ViewState["MONTH"].ToString())
               , int.Parse(ViewState["YEAR"].ToString()), Bowyn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKCODE"].ToString();
                if (dr["FLDMONTH"].ToString() != string.Empty && dr["FLDYEAR"].ToString() != string.Empty)
                    txtMonth.Text = DateTime.Parse("01/" + dr["FLDMONTH"].ToString() + "/" + dr["FLDYEAR"].ToString()).ToString("MMM") + "-" + dr["FLDYEAR"].ToString();
                //txtType.Text = dr["FLDTYPENAME"].ToString();
                txtBalanceonWage.Text = dr["FLDBALANCEWAGE"].ToString();
                txtCashonBalance.Text = dr["FLDCASHBOARD"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEarningsDeductions.VesselEarningList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                 , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                 , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString())
                 , General.GetNullableInteger(ViewState["TYPE"].ToString())
                 , General.GetNullableInteger(ViewState["MONTH"].ToString())
                 , General.GetNullableInteger(ViewState["YEAR"].ToString())
                 );

            gvEarning.DataSource = dt;
            gvEarning.VirtualItemCount = dt.Rows.Count;
            gvEarning.PageSize = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRecords()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        string ota = "";
        string otd = "";
        ota = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 128, "OTA");
        otd = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 129, "OTD");
        int count = 0, i = 0;
        count = gvEarning.Items.Count;
        foreach (GridDataItem gv in gvEarning.Items)
        {
            string ID = ((RadLabel)gv.FindControl("lblEarningDeductionid")).Text;
            string EarningDeduction = ((RadLabel)gv.FindControl("lblEarningDeduction")).Text;
            string purpose = ((RadTextBox)gv.FindControl("txtPurposeEdit")).Text;
            string amount = ((UserControlMaskNumber)gv.FindControl("txtAmountEdit")).Text;
            string lblpurpose = ((RadLabel)gv.FindControl("lblPurpose")).Text;
            string lblamount = ((RadLabel)gv.FindControl("lblamount")).Text;
            string lblEntryType = ((RadLabel)gv.FindControl("lblEntryType")).Text;
            string hardcode = ((RadLabel)gv.FindControl("lblhardcode")).Text;

            if ((hardcode.ToString().Equals(ota) || hardcode.ToString().Equals(otd)) && (General.GetNullableDecimal(amount) != null))
            {
                if (string.IsNullOrEmpty(purpose))
                    ucError.ErrorMessage = lblEntryType + " Purpose is required.";
            }

            if (General.GetNullableDecimal(amount) == null && purpose.Trim().ToString() != string.Empty)
                ucError.ErrorMessage = lblEntryType + " Amount is Required.";

            if (General.GetNullableDecimal(amount) <= 0)
                ucError.ErrorMessage = lblEntryType + " Amount should be greater than 0.";
            i++;
        }

        return (!ucError.IsError);
    }
    protected void MenuEarDedGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                bool result;
                result = true;
                if (!IsValidRecords())
                {
                    ucError.Visible = true;
                    return;
                }
                InsertEarningDeductionBulk(ref result);


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertEarningDeductionBulk(ref bool result)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("FLDEARNINGDEDUCTIONID", typeof(Guid));
            table.Columns.Add("FLDEARNINGDEDUCTION", typeof(int));
            table.Columns.Add("FLDHARDCODE", typeof(int));
            table.Columns.Add("FLDPURPOSE", typeof(string));
            table.Columns.Add("FLDAMOUNT", typeof(decimal));
            table.Columns.Add("FLDBASECURRENCY", typeof(int));
            table.Columns.Add("FLDEXCHANGERATE", typeof(decimal));
            table.Columns.Add("FLDDATE", typeof(string));
            int count = 0, i = 0;
            count = gvEarning.Items.Count;
            foreach (GridDataItem gv in gvEarning.Items)
            {
                string ID = ((RadLabel)gv.FindControl("lblEarningDeductionid")).Text;
                string EarningDeduction = ((RadLabel)gv.FindControl("lblEarningDeduction")).Text;
                string purpose = ((RadTextBox)gv.FindControl("txtPurposeEdit")).Text;
                string amount = ((UserControlMaskNumber)gv.FindControl("txtAmountEdit")).Text;
                string lblCurrency = ((RadLabel)gv.FindControl("lblCurrency")).Text;
                string Currency = ((UserControlVesselMappingCurrency)gv.FindControl("ddlCurrency")).SelectedCurrency;
                string lblpurpose = ((RadLabel)gv.FindControl("lblPurpose")).Text;
                string lblamount = ((RadLabel)gv.FindControl("lblamount")).Text;
                string wageheadid = ((RadLabel)gv.FindControl("lblWageHeadId")).Text;
                string lblExchangeRate = ((RadLabel)gv.FindControl("lblExchangeRate")).Text;
                string ExchangeRate = ((UserControlMaskNumber)gv.FindControl("txtExchangeRate")).Text;
                string txtdate = ((UserControlDate)gv.FindControl("txtdate")).Text;
                string lbldate = ((RadLabel)gv.FindControl("lbldate")).Text;
                if (amount != string.Empty)
                {
                    if (purpose.ToString() != lblpurpose.ToString() || amount.ToString() != lblamount.ToString() || lblCurrency.ToString() != Currency.ToString() || ExchangeRate.ToString() != lblExchangeRate.ToString() || txtdate.ToString() != lbldate.ToString())
                    {
                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblEarningDeductionid")).Text),
                                  General.GetNullableInteger(((RadLabel)gv.FindControl("lblEarningDeduction")).Text),
                                  General.GetNullableInteger(((RadLabel)gv.FindControl("lblhardcode")).Text),
                                  ((RadTextBox)gv.FindControl("txtPurposeEdit")).Text,
                                  General.GetNullableDecimal(((UserControlMaskNumber)gv.FindControl("txtAmountEdit")).Text),
                                  ((UserControlVesselMappingCurrency)gv.FindControl("ddlCurrency")).SelectedCurrency,
                                  General.GetNullableDecimal(((UserControlMaskNumber)gv.FindControl("txtExchangeRate")).Text)
                                  , General.GetNullableDateTime(((UserControlDate)gv.FindControl("txtdate")).Text));
                    }
                }
                i++;
            }
            //BindData();
            ds.Tables.Add(table);
            StringWriter sw = new StringWriter();
            ds.WriteXml(sw);
            string resultstring = sw.ToString();
            PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionBulkInsert(
                                 General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                  , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                                  , General.GetNullableInteger(ViewState["MONTH"].ToString())
                                  , General.GetNullableInteger(ViewState["YEAR"].ToString())
                                  , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                  , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString())
                                  , resultstring
                                 );
            ucStatus.Text = "Saved Successfully.";
            BindData();
            gvEarning.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdCalculateWage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BindEdit(1);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int vesselcurrency = 0;
            decimal Exchagerate = 0;
            UserControlVesselMappingCurrency ddl = (UserControlVesselMappingCurrency)sender;
            GridDataItem row = (GridDataItem)ddl.Parent.Parent;
            UserControlVesselMappingCurrency Currency = row.FindControl("ddlCurrency") as UserControlVesselMappingCurrency;
            UserControlMaskNumber txtExchangeRate = row.FindControl("txtExchangeRate") as UserControlMaskNumber;
            UserControlDate txtdate = row.FindControl("txtdate") as UserControlDate;

            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(Currency.SelectedCurrency)
                                                                        , DateTime.Parse(txtdate.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
            {

                txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEarning_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvEarning_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton delt = (LinkButton)e.Item.FindControl("cmdDelete");
                if (drv["FLDAMOUNT"].ToString() != string.Empty)
                {
                    delt.Visible = true;
                    if (delt != null)
                    {
                        delt.Visible = SessionUtil.CanAccess(this.ViewState, delt.CommandName);
                        delt.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    }
                }
                UserControlVesselMappingCurrency ddlCurrency = ((UserControlVesselMappingCurrency)e.Item.FindControl("ddlCurrency"));
                if (ddlCurrency != null)
                {

                    ddlCurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
                    if (ddlCurrency.SelectedCurrency == null || ddlCurrency.SelectedCurrency == string.Empty)
                    {
                        ddlCurrency.SelectedCurrency = drv["FLDBASECURRENCY"].ToString();
                    }
                }
                RadTextBox purpse = ((RadTextBox)e.Item.FindControl("txtPurposeEdit"));
                if ((drv["FLDSHORTNAME"].ToString() == "BRF") || drv["FLDSHORTNAME"].ToString() == "REM" || drv["FLDSHORTNAME"].ToString() == "CRR" || drv["FLDSHORTNAME"].ToString() == "ARR" || drv["FLDSHORTNAME"].ToString() == "BSU")
                {
                    UserControlMaskNumber amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit"));
                    if (purpse != null)
                    {
                        purpse.CssClass = "gridinput readonlytextbox";
                        purpse.ReadOnly = true;
                    }
                    if (amt != null)
                    {
                        amt.CssClass = "readonlytextbox txtNumber";
                        amt.ReadOnly = true;
                    }
                }
                if (drv["FLDSHORTNAME"].ToString() == "OTA" || drv["FLDSHORTNAME"].ToString() == "OTD")
                    purpse.CssClass = "gridinput_mandatory";

                UserControlDate date = ((UserControlDate)e.Item.FindControl("txtdate"));
                RadLabel lbldate = ((RadLabel)e.Item.FindControl("lbldate"));
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    UserControlMaskNumber txtExchangeRate = (UserControlMaskNumber)e.Item.FindControl("txtExchangeRate");
                    if (txtExchangeRate != null)
                    {
                        txtExchangeRate.CssClass = "readonlytextbox txtNumber";
                        txtExchangeRate.ReadOnly = true;
                    }

                    if (date != null)
                    {
                        date.CssClass = "readonlytextbox";
                        date.ReadOnly = true;
                        date.Visible = false;
                    }
                    if (lbldate != null)
                    {
                        lbldate.Visible = true;
                    }
                }
                else
                {
                    if (date != null)
                    {
                        date.ReadOnly = false;
                        date.Visible = true;
                    }
                    if (lbldate != null)
                    {
                        lbldate.Visible = false;
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

    protected void gvEarning_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "DELETE")
        {
            GridDataItem item = (GridDataItem)e.Item;
            try
            {
                string id = ((RadLabel)item.FindControl("lblEarningDeductionid")).Text;
                if (id != "")
                    PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionDelete(new Guid(id));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            gvEarning.EditIndexes.Clear();
            BindData();
            gvEarning.Rebind();
        }
    }
}