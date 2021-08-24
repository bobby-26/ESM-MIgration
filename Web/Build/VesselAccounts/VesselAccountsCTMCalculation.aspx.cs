using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsCTMCalculation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 2;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["REFNO"] = null;
                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                ViewState["CSHONB"] = null;
                ViewState["CALAMT"] = null;
                ViewState["CURRENCY"] = null;
                SetCaptainCash(new Guid(ViewState["CTMID"].ToString()));
                if (ViewState["ACTIVEYN"].ToString() == "0") MenuCTM.Visible = false;
                txtCTM.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((decimal.Parse(ViewState["CALAMT"].ToString()) - Decimal.Parse(ViewState["REFNO"] == null ? "0" : ViewState["REFNO"].ToString())) > 0)
                {
                    txtCTM.Enabled = true;
                }
                else
                {
                    if (ViewState["ACTIVEYN"].ToString() != "0")
                    {
                        ucError.ErrorMessage = "Short fall should be greater than Zero.";
                        ucError.Visible = true;
                    }
                    txtCTM.Enabled = false;
                }
                string cshonb = txtCashonBoard.Text;
                string CTMamt = txtCTM.Text;
                if ((String.IsNullOrEmpty(txtBalance.Text) ? 0 : decimal.Parse(txtBalance.Text)) > 0)
                {
                    if (!IsValidRoundOff(CTMamt, txtBalance.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string ramt = (decimal.Parse(txtBalance.Text) - decimal.Parse(CTMamt)).ToString();
                    PhoenixVesselAccountsCTM.UpdateCaptainCashCalculationRoundOff(new Guid(ViewState["CTMID"].ToString()), decimal.Parse(ramt), decimal.Parse(CTMamt));
                    SetCaptainCash(new Guid(ViewState["CTMID"].ToString()));
                    ucStatus.Text = "Updated successfully";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOW.aspx";
            }
            else if (CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenomination.aspx";
            }
            else if (CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculation.aspx";
            }
            if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsCTM.aspx", false);
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
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
            decimal d = 0;
            string currencycode = "";
            DataTable dt = PhoenixVesselAccountsCTM.ListCaptainCashCTM(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                        , new Guid(ViewState["CTMID"].ToString()), ref d, ref currencycode);
            gvCTM.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                lblCashOnBoard.Text = "Cash On Board (" + currencycode + ")";
                lblEstimate.Text = "Estimate Expenses (" + currencycode + ")";
                Shortfall.Text = "Shortfall (" + currencycode + ")";
                lblRoundOffAmount.Text = "Round Off Amount (" + currencycode + ")";
                lblCTMtobearranged.Text = "CTM to be arranged (" + currencycode + ")";
                lblOpeningBalance.Text = "Opening Balance (" + currencycode + ")";
                txtCashonBoard.Text = d.ToString();
                txtOpening.Text = dt.Rows[0]["FLDOPENINGBALANCE"].ToString();
                ViewState["REFNO"] = d.ToString();
                object sumObject;
                sumObject = dt.Compute("Sum(FLDAMOUNT)", string.Empty);
                ViewState["CURRENCY"] = currencycode.ToString();

                ViewState["CALAMT"] = sumObject.ToString();
                txtBalance.Text = (decimal.Parse(ViewState["CALAMT"].ToString()) - d).ToString();
                {
                    if ((decimal.Parse(ViewState["CALAMT"].ToString()) - d) > 0)
                        txtCTM.Enabled = true;
                    else
                        txtCTM.Enabled = false;
                    if (txtRoundOffAmount.Text == string.Empty)
                        txtRoundOffAmount.Text = String.IsNullOrEmpty(txtCTM.Text.Trim()) ? txtBalance.Text.Trim() : (decimal.Parse(txtCTM.Text.Trim()) - decimal.Parse(txtBalance.Text.Trim())).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblbowId")).Text.Trim());
                string type = ((RadLabel)e.Item.FindControl("lblType")).Text;
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                if (!IsValidCTM(amt))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!id.HasValue)
                    PhoenixVesselAccountsCTM.InsertCaptainCashCalculation(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["CTMID"].ToString())
                                                                                , byte.Parse(type), decimal.Parse(amt));
                else
                    PhoenixVesselAccountsCTM.UpdateCaptainCashCalculation(id.Value, decimal.Parse(amt));
                Rebind();
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

    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
           
            if (e.Item is GridEditableItem)
            {

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");

                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                if (drv["FLDTYPE"].ToString() == "2")
                    if (ed != null) ed.Visible = false;
                if (ViewState["ACTIVEYN"].ToString() != "1")
                {
                    if (ed != null) ed.Visible = false;
                }
                 

            }
            if (e.Item is GridFooterItem)
            {
                RadLabel lblTotalamount = (RadLabel)e.Item.FindControl("lblTotalamount");
                lblTotalamount.Text = ViewState["CALAMT"] != null ? ViewState["CALAMT"].ToString() : "";
                txtEstimate.Text = ViewState["CALAMT"] != null ? ViewState["CALAMT"].ToString() : "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCTM(string amt)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(amt).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidRoundOff(string ctmAmt, string balance)
    {
        decimal ramt = 0;
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(ctmAmt).HasValue && balance != string.Empty)
        {
            ucError.ErrorMessage = "CTM to be arranged Amount is required.";
        }
        else
        {
            ramt = decimal.Parse(balance) - decimal.Parse(ctmAmt);
        }
        if ((ViewState["CURRENCY"].ToString() == "INR") && (ramt > 1000000 || ramt < -1000000))
        {
            ucError.ErrorMessage = "CTM to be arranged cannot differ the short fall amount by more than INR 10,00,000";
        }
        else if((ViewState["CURRENCY"].ToString() != "INR") && (ramt > 1000 || ramt < -1000))
        {
            ucError.ErrorMessage = "CTM to be arranged cannot differ the short fall amount by more than $1000";
        }
        return (!ucError.IsError);
    }
    private void SetCaptainCash(Guid CTMId)
    {
        DataTable dt = PhoenixVesselAccountsCTM.EditCTMRequest(CTMId);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDROUNDOFFAMOUNT"].ToString() != string.Empty)
            {
                txtRoundOffAmount.Text = dt.Rows[0]["FLDROUNDOFFAMOUNT"].ToString();
                txtCTM.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            }
        }
    }
}