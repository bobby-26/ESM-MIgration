using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLandTravelOtherCharges : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewLandTravelOtherCharges.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOtherCharges')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuOtherCharges.AccessRights = this.ViewState;
            MenuOtherCharges.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Land Travel Request", "LANDTRAVELREQUEST");
            toolbar1.AddButton("Request Details", "REQUESTDETAILS");
            toolbar1.AddButton("Tariff", "TARIFF");
            MenuLandTravelRequest.AccessRights = this.ViewState;
            MenuLandTravelRequest.MenuList = toolbar1.Show();
            MenuLandTravelRequest.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Details", "DETAIL");
            toolbar2.AddButton("Other Charges", "OTHERCHARGES");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbar2.Show();
            MenuTab.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                if (Request.QueryString["requestid"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                else
                    ViewState["REQUESTID"] = "";
                if (ViewState["REQUESTID"].ToString() != string.Empty)
                    EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));

                gvOtherCharges.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            Response.Redirect("../Crew/CrewLandTravelRequestGeneral.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
        }
        if (CommandName.ToUpper().Equals("OTHERCHARGES"))
        {
            Response.Redirect("../Crew/CrewLandTravelOtherCharges.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
        }

    }

    protected void MenuLandTravelRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : null);

            if (CommandName.ToUpper().Equals("LANDTRAVELREQUEST"))
            {
                Response.Redirect("../Crew/CrewLandTravelRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("REQUESTDETAILS"))
            {
                Response.Redirect("../Crew/CrewLandTravelRequestGeneral.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TARIFF"))
            {
                Response.Redirect("../Crew/CrewLandTravelTariff.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditLandTravelRequest(Guid requestid)
    {
        DataTable dt = PhoenixCrewLandTravelRequest.EditLandTravelRequest(requestid);

        if (dt.Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            txtAmount.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            txtpayableamount.Text = dt.Rows[0]["FLDTOTALAMOUNT"].ToString();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDREASON", "FLDAMOUNT" };
        string[] alCaptions = { "Charges Description", "Amount" };

        DataTable dt = PhoenixCrewLandTravelOtherCharges.ListLandTravelOtherCharges(new Guid(ViewState["REQUESTID"].ToString()));

        General.ShowExcel("Other Charcharges", dt, alColumns, alCaptions, null, string.Empty);
    }

    protected void OtherCharges_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvOtherCharges.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        string[] alColumns = { "FLDREASON", "FLDAMOUNT" };
        string[] alCaptions = { "Charges Description", "Amount" };

        DataTable dt = PhoenixCrewLandTravelOtherCharges.ListLandTravelOtherCharges(new Guid(ViewState["REQUESTID"].ToString()));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvOtherCharges", "Other Charges", alCaptions, alColumns, ds);

        gvOtherCharges.DataSource = dt;

    }

    protected void gvOtherCharges_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOtherCharges_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30&vesselid='); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30&vesselid='); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
            }
        }
    }

    protected void gvOtherCharges_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string txtReasonAdd = ((RadTextBox)e.Item.FindControl("txtReasonAdd")).Text;
                string txtAmountAdd = ((UserControlMaskNumber)e.Item.FindControl("txtValueAdd")).Text;
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string taxtype = ((UserControlTaxType)e.Item.FindControl("ucTaxTypeAdd")).TaxType;

                if (!IsValidOtherCharges(txtReasonAdd, txtAmountAdd, budgetcode))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLandTravelOtherCharges.InsertLandTravelOtherCharges(new Guid(ViewState["REQUESTID"].ToString())
                                , txtReasonAdd
                                , int.Parse(taxtype)
                                , int.Parse(budgetcode)
                                , decimal.Parse(txtAmountAdd));
                BindData();
                gvOtherCharges.Rebind();
                EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
            }

            if (e.CommandName.ToUpper().Equals("DELETERECORD"))
            {
                string lblOtherChargesId = ((RadLabel)e.Item.FindControl("lblOtherCharges")).Text;

                PhoenixCrewLandTravelOtherCharges.DeleteLandTravelOtherCharges(new Guid(ViewState["REQUESTID"].ToString()), new Guid(lblOtherChargesId));

                BindData();
                gvOtherCharges.Rebind();
                EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherCharges_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string lblOtherChargesId = ((RadLabel)e.Item.FindControl("lblOtherChargesId")).Text;
            string txtReasonEdit = ((RadLabel)e.Item.FindControl("txtReasonEdit")).Text;
            string amount = ((UserControlMaskNumber)e.Item.FindControl("txtValueEdit")).Text;
            string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
            string taxtype = ((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType;

            if (!IsValidOtherCharges(txtReasonEdit, amount, budgetcode))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewLandTravelOtherCharges.UpdateLandTravelOtherCharges(new Guid(ViewState["REQUESTID"].ToString())
                                                    , new Guid(lblOtherChargesId)
                                                    , txtReasonEdit
                                                    , int.Parse(taxtype)
                                                    , int.Parse(budgetcode)
                                                    , decimal.Parse(amount));

            BindData();
            gvOtherCharges.Rebind();
            EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherCharges_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvOtherCharges_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    private bool IsValidOtherCharges(string Reason, string Amount, string budgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Reason.Trim().Equals(""))
            ucError.ErrorMessage = "Charges Description is required.";

        if (General.GetNullableDecimal(Amount) == null)
            ucError.ErrorMessage = "Value is required.";

        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


}
