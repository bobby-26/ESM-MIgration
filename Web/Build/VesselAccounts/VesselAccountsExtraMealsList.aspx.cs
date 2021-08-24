using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsExtraMealsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuExtraMeals.AccessRights = this.ViewState;
            ViewState["PURPOSE"] = null;
            ViewState["ACCOUNTTYPE"] = null;
            ViewState["DEFAULTRATE"] = null;
            txtRate.ReadOnly = false;
            if ((Request.QueryString["purpose"] == "EXTRAMEALSEDIT") && (Request.QueryString["extramealsid"] != null))
            {
                BindData();
                ViewState["PURPOSE"] = "update";
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuExtraMeals.MenuList = toolbar.Show();
                if (chkVictualRate.Checked == true)
                {
                    txtRate.Text = "";
                    txtRate.CssClass = "readonlytextkbox";
                    txtRate.ReadOnly = true;
                }
                else
                {
                    chkVictualRate.CssClass = "readonlycheckbox";
                }
            }
            else//add
            {
                toolbar.AddButton("Add", "SAVE", ToolBarDirection.Right);
                MenuExtraMeals.MenuList = toolbar.Show();
                ViewState["PURPOSE"] = "insert";

            }
        }
    }
    protected void MenuExtraMeals_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PURPOSE"] != null && ViewState["PURPOSE"].ToString() == "insert")
                {
                    InsertExtraMeals(ddlAccountType.SelectedValue, ucExMealsFromDate.Text, ucExMealsToDate.Text, ucMandays.Text
                      , txtRate.Text, txtServedTo.Text, (chkVictualRate.Checked == true ? "1" : "0"));

                }
                if (ViewState["PURPOSE"] != null && ViewState["PURPOSE"].ToString() == "update")
                {
                    Guid id = new Guid(Request.QueryString["extramealsid"].ToString());
                    UpdateExtraMeals(id, ddlAccountType.SelectedValue, ucExMealsFromDate.Text, ucExMealsToDate.Text
                      , ucMandays.Text, txtRate.Text, txtServedTo.Text, (chkVictualRate.Checked == true ? "1" : "0"));
                }

                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

            }
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
            chkVictualRate.Enabled = true;
            Guid id = new Guid(Request.QueryString["extramealsid"].ToString());
            DataSet ds = PhoenixVesselAccountsExtraMeals.ExtraMealsEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int type;
                DataRow drExtraMeals = ds.Tables[0].Rows[0];
                type = int.Parse(drExtraMeals["FLDACCOUNTTYPE"].ToString());
                if (type == -1)
                    ddlAccountType.SelectedIndex = 1;
                else if (type == -2)
                    ddlAccountType.SelectedIndex = 2;
                else if (type == -3)
                {
                    chkVictualRate.Enabled = false;
                    txtRate.ReadOnly = true;
                    txtRate.CssClass = "readonlytextbox";
                    txtRate.ReadOnly = false;
                    ddlAccountType.SelectedIndex = 3;
                }
                ucExMealsFromDate.Text = drExtraMeals["FLDFROMDATE"].ToString();
                ucExMealsToDate.Text = drExtraMeals["FLDTODATE"].ToString();
                ucMandays.Text = drExtraMeals["FLDNOOFMANDAYS"].ToString();
                if (type != -3)
                {
                    txtRate.Text = drExtraMeals["FLDRATE"].ToString();
                }
                txtServedTo.Text = drExtraMeals["FLDSERVERDTO"].ToString();
                ViewState["ACCOUNTTYPE"] = drExtraMeals["FLDACCOUNTTYPE"].ToString();
                ViewState["DEFAULTRATE"] = drExtraMeals["FLDRATE"].ToString();
                if (drExtraMeals["FLDISVICTUALLINGRATEYN"].ToString() == "1")
                {
                    chkVictualRate.Checked = true;
                }
                else
                {
                    chkVictualRate.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InsertExtraMeals(string accounttype, string fromdate, string todate, string mandays, string mealsrate, string servedto, string isvictualrate)
    {
        if (!IsValidExtraMeals(accounttype, fromdate, todate, mandays, mealsrate, servedto))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixVesselAccountsExtraMeals.InsertExtraMeals(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , Convert.ToDateTime(fromdate), Convert.ToDateTime(todate), General.GetNullableInteger(accounttype), int.Parse(mandays.ToString())
                    , General.GetNullableDecimal(mealsrate), servedto, int.Parse(isvictualrate));
        ucStatus.Text = "Extrameals Information Saved";
    }
    protected void UpdateExtraMeals(Guid id, string accounttype, string fromdate, string todate, string mandays, string rate, string servedto, string isvictualrate)
    {
        if (!IsValidExtraMeals(accounttype, fromdate, todate, mandays, rate, servedto))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixVesselAccountsExtraMeals.UpdateExtraMeals(id, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToDateTime(fromdate)
            , Convert.ToDateTime(todate), Int32.Parse(accounttype), int.Parse(mandays.ToString()), General.GetNullableDecimal(rate), servedto
                  , Int16.Parse(isvictualrate)); ucStatus.Text = "Extrameals Information Saved";
    }
    protected void chkVictualRate_OnCheckedChanged(object sender, EventArgs args)
    {
        if (chkVictualRate.Checked == true)
        {
            txtRate.Text = "";
            txtRate.CssClass = "readonlytextbox";
            txtRate.ReadOnly = true;
        }
        if (chkVictualRate.Checked == false)
        {
            if (ViewState["DEFAULTRATE"] != null)
            {
                txtRate.Text = ViewState["DEFAULTRATE"].ToString();
            }
            txtRate.CssClass = "input_mandatory";
            txtRate.ReadOnly = false;
        }
        if (ViewState["ACCOUNTTYPE"].ToString() == "-3")
        {
            txtRate.Text = "";
            txtRate.CssClass = "readonlytextbox";
            txtRate.ReadOnly = true;
        }
    }
    protected void ddlAccountType_OnTextChanged(object sender, EventArgs args)
    {
        try
        {
            ViewState["ACCOUNTTYPE"] = ddlAccountType.SelectedValue;
            if (ddlAccountType.SelectedValue == "-SELECT-")
            {
                txtRate.Text = "";
            }
            else if (ddlAccountType.SelectedValue == "-3")
            {
                txtRate.Text = "";
                chkVictualRate.Checked = true;
                txtRate.CssClass = "readonlytextbox";
                txtRate.ReadOnly = true;
                chkVictualRate.Enabled = false;
            }
            else
            {
                chkVictualRate.Enabled = true;
                txtRate.Text = "";
                DataTable dt = PhoenixVesselAccountsExtraMeals.ExtraMealsDefaultRatelList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Int16.Parse(ddlAccountType.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    ViewState["DEFAULTRATE"] = dr["FLDDEFAULTRATE"].ToString();
                    if (dr["FLDACTUALVICTUALINGRATE"].ToString() == "1")
                    {
                        chkVictualRate.Checked = true;
                        txtRate.Text = "";// dr["FLDDEFAULTRATE"].ToString();
                        txtRate.ReadOnly = true;
                        txtRate.CssClass = "readonlytextbox";
                        txtRate.ReadOnly = true;
                    }
                    else
                    {

                        txtRate.Text = dr["FLDDEFAULTRATE"].ToString();
                        chkVictualRate.Checked = false;
                        txtRate.CssClass = "input_mandatory";
                        txtRate.ReadOnly = false;
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
    private bool IsValidExtraMeals(string accounttype, string fromdate, string todate, string mandays, string rate, string servedto)
    {
        DateTime datefrom;
        DateTime dateto;
        Int16 resultInt;
        Int16 resultmandays;
        decimal mealsrate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (!DateTime.TryParse(fromdate, out datefrom))
            ucError.ErrorMessage = "Valid From Date is required.";

        if (!DateTime.TryParse(fromdate, out dateto))
            ucError.ErrorMessage = "Valid To Date is required.";

        if (Int16.TryParse(accounttype, out resultInt))
        {
            if (Int32.Parse(accounttype) == 0)
                ucError.ErrorMessage = "Account type  is required.";
        }
        else
        {
            ucError.ErrorMessage = "Account type  is required.";
        }
        if (General.GetNullableDecimal(mandays) != null)
        {
            if (Int16.TryParse(mandays, out resultmandays))
            {

                if ((resultmandays < 1) || (resultmandays > 100))
                {
                    ucError.ErrorMessage = "Mandays should be between 1 to 99";
                }
            }
        }
        else
        {
            ucError.ErrorMessage = "Mandays Required";
        }
        if (General.GetNullableString(servedto) == null && servedto == "")
        {
            ucError.ErrorMessage = "Served To is Required";
        }
        if (General.GetNullableDecimal(rate) != null)
        {
            if (Decimal.TryParse(rate, out mealsrate))
            {
                if ((mealsrate < 0) || (mealsrate > Decimal.Parse("99.99")))
                {
                    ucError.ErrorMessage = "Rate should be in between 0.00 to 99.99.";
                }

            }

        }
        if (fromdate != null && todate != null)
        {
            if (DateTime.Parse(fromdate) > DateTime.Now)
            {
                ucError.ErrorMessage = "'From Date' should not be Later than 'Current Date'";
            }
            if (DateTime.Parse(todate) > DateTime.Now)
            {
                ucError.ErrorMessage = "'To Date' should not be Later than 'Current Date'";
            }
        }
        if (fromdate != null && todate != null)
        {
            if ((DateTime.TryParse(fromdate, out datefrom)) && (DateTime.TryParse(todate, out dateto)))
                if ((DateTime.Parse(fromdate)) > (DateTime.Parse(todate)))
                    ucError.ErrorMessage = "'To Date' should be Later than 'From Date'";
        }
        if (fromdate != null && todate != null)
        {
            if ((DateTime.TryParse(fromdate, out datefrom)) && (DateTime.TryParse(todate, out dateto)))
                if ((DateTime.Parse(fromdate).Month) != (DateTime.Parse(todate).Month))
                    ucError.ErrorMessage = "'From Date' and 'To Date' should be with in the month";
        }
        if ((General.GetNullableDecimal(rate) == null) && (chkVictualRate.Checked == false))
        {
            ucError.ErrorMessage = "Rate or Actual Victualling Needed ";
        }


        return (!ucError.IsError);
    }
}
