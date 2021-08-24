using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using System.IO;
using Telerik.Web.UI;
public partial class CrewOfferLetterWages : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        }
    }
    private bool IsValidComponent(string contractid, string componentid, string amount, string Currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec;

        if (General.GetNullableInteger(Currency) == null)
            ucError.ErrorMessage = "Currency is required";
        if (string.IsNullOrEmpty(contractid))
            ucError.ErrorMessage = "Offer Letter is required";
        if (string.IsNullOrEmpty(componentid))
            ucError.ErrorMessage = "Component is required";
        if (!decimal.TryParse(amount, out resultDec))
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                UserControlContractCrew ddlCrewComp = (UserControlContractCrew)item.FindControl("ddlCrewComponentsAdd");
                UserControlNumber txtAmount = (UserControlNumber)item.FindControl("txtAmountAdd");
                UserControlCurrency currency = (UserControlCurrency)item.FindControl("ddlCurrencyAdd");
                if (!IsValidComponent(Request.QueryString["offerid"].ToString(), ddlCrewComp.SelectedComponent.ToString(), txtAmount.Text, currency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOfferLetter.OfferletterWageComponentUPDATE(new Guid(Request.QueryString["offerid"].ToString()), new Guid(ddlCrewComp.SelectedComponent), decimal.Parse(txtAmount.Text)
                    , int.Parse(currency.SelectedCurrency), null);
                ucStatus.Text = "Component is saved successfully";
                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblCrewComp = (RadLabel)e.Item.FindControl("lblCompIdEdit");
                RadTextBox lblPayabledesc = (RadTextBox)e.Item.FindControl("TxtPayableEdit");
                UserControlNumber txtAmount = (UserControlNumber)e.Item.FindControl("txtAmount");
                UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
                if (!IsValidComponent(Request.QueryString["offerid"].ToString(), lblCrewComp.Text, txtAmount.Text, currency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOfferLetter.OfferletterWageComponentUPDATE(new Guid(Request.QueryString["offerid"].ToString()), new Guid(lblCrewComp.Text), decimal.Parse(txtAmount.Text)
                     , int.Parse(currency.SelectedCurrency), lblPayabledesc.Text);
                ucStatus.Text = "Component is updated successfully";

                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblid")).Text.Trim();
               
                PhoenixCrewOfferLetter.OfferletterWageComponentdelete(new Guid(Request.QueryString["offerid"].ToString()),new Guid(id));
                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvContract_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindData();
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
                currency.SelectedCurrency = drv["FLDCURRENCY"].ToString();
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
                //if (ViewState["FLDISOFFERLETTERACTIVE"].ToString() == "0")
                //    ed.Visible = false;
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
            UserControlContractCrew ddlCrewComponents = (UserControlContractCrew)e.Item.FindControl("ddlCrewComponents");
            if (ddlCrewComponents != null)
            {

                DataSet ds = Component();

                ddlCrewComponents.CrewComponentsList = ds;
                ddlCrewComponents.DataBind();
                //ddlCrewComponents.da = PhoenixRegistersContract.ListContractCrew(null);
                ddlCrewComponents.DataBind();
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                //if (ViewState["FLDISOFFERLETTERACTIVE"].ToString() == "0")
                //    db.Visible = false;
            }

            UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyAdd");
            if (currency != null)
            {
                currency.CurrencyList = PhoenixRegistersCurrency.ListCurrency(1);
                currency.SelectedCurrency = "10";
            }
        }
    }
    public DataSet Component()
    {
        DataTable dt1 = PhoenixRegistersContract.ListContractCrew(null);
        DataSet ds = new DataSet();

        ds.Merge(dt1);
        return ds;
    }
    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewOfferLetter.OfferletterWageComponentList(new Guid(Request.QueryString["offerid"].ToString()));
            gvCrew.DataSource = ds.Tables[0];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
