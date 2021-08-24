using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersLicenceCost : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersLicenceCost.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLicenceCost')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuLicenceCost.AccessRights = this.ViewState;
            MenuLicenceCost.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Licence", "LICENCE");
            toolbar.AddButton("Flag List", "FLAGLIST");
            toolbar.AddButton("Cost of Licence", "COSTOFLICENCE");
            MenuLicenceCostMap.AccessRights = this.ViewState;
            MenuLicenceCostMap.MenuList = toolbar.Show();
            MenuLicenceCostMap.SelectedMenuIndex = 2;
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {              
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FLAGID"] = Request.QueryString["FLAGID"] != null ? Request.QueryString["FLAGID"] : "";
                ViewState["CONSULATEID"] = Request.QueryString["CONSULATEID"] != null ? Request.QueryString["CONSULATEID"] : "";
                gvLicenceCost.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void LicenceCostMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("COSTOFLICENCE"))
        {
            MenuLicenceCostMap.SelectedMenuIndex = 2;
            return;
        }
        else if (CommandName.ToUpper().Equals("LICENCE"))
        {
            Response.Redirect("../Registers/RegistersDocumentLicence.aspx");
        }
        else if (CommandName.ToUpper().Equals("FLAGLIST"))
        {
            Response.Redirect("../Registers/RegistersLicenceFlagList.aspx");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLICENCE", "FLDCURRENCYCODE","FLDCOST" };
        string[] alCaptions = { "Licence", "Currency","Cost" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersLicenceCost.LicenceCostSearch(
                General.GetNullableInteger(ViewState["FLAGID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["FLAGID"].ToString()),
                General.GetNullableInteger(ViewState["CONSULATEID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["CONSULATEID"].ToString()),
                General.GetNullableInteger(ucRank.SelectedRank)== null ? 0 : General.GetNullableInteger(ucRank.SelectedRank),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvLicenceCost.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LicenceCost.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Licence Cost</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void LicenceCost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLICENCE", "FLDCURRENCYCODE", "FLDCOST" };
        string[] alCaptions = { "Licence", "Currency", "Cost" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersLicenceCost.LicenceCostSearch(
                General.GetNullableInteger(ViewState["FLAGID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["FLAGID"].ToString()),
                General.GetNullableInteger(ViewState["CONSULATEID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["CONSULATEID"].ToString()),
                General.GetNullableInteger(ucRank.SelectedRank) == null ? 0 : General.GetNullableInteger(ucRank.SelectedRank),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvLicenceCost.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvLicenceCost", "Licence Cost", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLicenceCost.DataSource = ds;
            gvLicenceCost.VirtualItemCount = iRowCount;
        }
        else
        {
            gvLicenceCost.DataSource = "";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtFlag.Text = ds.Tables[1].Rows[0]["FLDFLAGNAME"].ToString();
            txtConsulate.Text = ds.Tables[1].Rows[0]["FLDCONSULATENAME"].ToString();
        }
        else
        {
            txtFlag.Text = string.Empty;
            txtConsulate.Text = string.Empty;
        }
    }
    

    private void InsertLicenceCost(int flagid, int consulateid, int rankid, string costcomponents, int currencyid, decimal cost)
    {
        PhoenixRegistersLicenceCost.InsertLicenceCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, flagid, consulateid, rankid, costcomponents, currencyid, cost);
    }

    private void UpdateLicenceCost(int licencecostid, int flagid, int consulateid, int rankid, string costcomponents, int currencyid, decimal cost)
    {
        PhoenixRegistersLicenceCost.UpdateLicenceCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , licencecostid, flagid, consulateid, rankid, costcomponents, currencyid, cost);
    }

    private bool IsValidLicenceCost(string cost, string costcomponents, string Currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (costcomponents.Trim() == "")
            ucError.ErrorMessage = "Select Atleast one Component";

        if (cost == "0.00" || General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(ViewState["FLAGID"].ToString()) == null)
            ucError.ErrorMessage = "Flag is required.";

        if (General.GetNullableInteger(ViewState["CONSULATEID"].ToString()) == null)
            ucError.ErrorMessage = "Consulate is required.";

        if (General.GetNullableInteger(ucRank.SelectedRank) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableInteger(Currency) == null)
            ucError.ErrorMessage = "Currency is required.";
        
        return (!ucError.IsError);
    }

    private void DeleteLicenceCost(int licencecostid)
    {
        PhoenixRegistersLicenceCost.DeleteLicenceCost (
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, licencecostid);
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }



    protected void gvLicenceCost_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblLicenceCostId")).Text) != null)
                    DeleteLicenceCost(Int32.Parse(((RadLabel)e.Item.FindControl("lblLicenceCostId")).Text));

                BindData();
                gvLicenceCost.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidLicenceCost(((UserControlMaskNumber)e.Item.FindControl("txtCostAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtComponentsIdAdd")).Text
                    , ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertLicenceCost(
                    int.Parse(ViewState["FLAGID"].ToString()),
                    int.Parse(ViewState["CONSULATEID"].ToString()),
                    int.Parse(ucRank.SelectedRank),
                    ((RadTextBox)e.Item.FindControl("txtComponentsIdAdd")).Text,
                    Int32.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency),
                    decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCostAdd")).Text));

                BindData();
                gvLicenceCost.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLicenceCost(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text
                  , ((RadTextBox)e.Item.FindControl("txtComponentsIdEdit")).Text
              , ((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateLicenceCost(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblLicenceCostIdEdit")).Text),
                    int.Parse(ViewState["FLAGID"].ToString()),
                    int.Parse(ViewState["CONSULATEID"].ToString()),
                    int.Parse(ucRank.SelectedRank),
                    ((RadTextBox)e.Item.FindControl("txtComponentsIdEdit")).Text,
                    Int32.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency),
                    decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text));

                BindData();
                gvLicenceCost.Rebind();
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

    protected void gvLicenceCost_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLicenceCost.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvLicenceCost_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");

            DataRowView drview = (DataRowView)e.Item.DataItem;

            if (ucCurrency != null)
            {
                string CID = drview["FLDCURRENCYID"].ToString();
                ucCurrency.SelectedCurrency = drview["FLDCURRENCYID"].ToString();
            }

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowComponentsEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListComponentsEdit', 'codehelp1', '', '../Registers/RegistersLicenceCostComponents.aspx', true); ");

            RadTextBox txt = (RadTextBox)e.Item.FindControl("txtComponentsIdEdit");
            if (txt != null) txt.Attributes.Add("style", "visibility:hidden;");

            char[] trimChar = { ' ', '+' };
            RadTextBox txt1 = (RadTextBox)e.Item.FindControl("txtComponentsEdit");
            if (txt1 != null) txt1.Text = txt1.Text.TrimEnd(trimChar);

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkLicenceName");
            if (lb != null) lb.Text = lb.Text.TrimEnd(trimChar);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }

        if (e.Item is GridFooterItem)
        {
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowComponentsAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListComponentsAdd', 'codehelp1', '', '../Registers/RegistersLicenceCostComponents.aspx', true); ");

            RadTextBox txt = (RadTextBox)e.Item.FindControl("txtComponentsIdAdd");
            txt.Attributes.Add("style", "visibility:hidden;");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
