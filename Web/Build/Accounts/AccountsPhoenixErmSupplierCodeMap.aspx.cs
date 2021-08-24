using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class AccountsPhoenixErmSupplierCodeMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsPhoenixErmSupplierCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolgrid.AddFontAwesomeButton("javascript:CallPrint('gvPhoenixErmSupplierCodeMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsPhoenixErmSupplierCodeMapFilter.aspx?callfrom=phoenixermsupplier", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuPhoenixErmSupplierCodeMap.AccessRights = this.ViewState;
            MenuPhoenixErmSupplierCodeMap.MenuList = toolgrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhoenixErmSupplierCodeMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string addresstype = null;

        string[] alCaptions = { "Phoenix Supplier Code", "ERM Company ID", "ERM Supplier Code" };

        string[] alColumns = { "FLDCODE", "FLDERMCOMPANYID", "FLDERMSUPPLIERCODE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentPhoenixErmSupplierCodeMapSelection;

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmSupplierCodeMapSearch(
                                                   null
                                                 , nvc != null ? General.GetNullableInteger(nvc.Get("PhoenixSupplierId")) : null
                                                 , nvc != null ? General.GetNullableString(nvc.Get("AddressType")) : addresstype                                             
                                                 , sortexpression, sortdirection
                                                 , gvPhoenixErmSupplierCodeMap.CurrentPageIndex + 1
                                                 , gvPhoenixErmSupplierCodeMap.PageSize
                                                 , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoenixErmSupplierCodeMap.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phoenix-Erm Supplier Code Map</h3></td>");
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
    protected DataSet BindErmCompanyID()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsPhoenixErmIntegration.ErmCompanyIdList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        return ds;
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string addresstype = null;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentPhoenixErmSupplierCodeMapSelection;      

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmSupplierCodeMapSearch(
                                                   null
                                                 , nvc != null ? General.GetNullableInteger(nvc.Get("PhoenixSupplierId")) : null
                                                 , nvc != null ? General.GetNullableString(nvc.Get("AddressType")) : addresstype
                                                 , sortexpression, sortdirection
                                                 , gvPhoenixErmSupplierCodeMap.CurrentPageIndex + 1
                                                 , gvPhoenixErmSupplierCodeMap.PageSize
                                                 , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = { "Phoenix Supplier Code", "ERM Company ID", "ERM Supplier Code" };

        string[] alColumns = { "FLDCODE", "FLDERMCOMPANYID", "FLDERMSUPPLIERCODE" };

        General.SetPrintOptions("gvPhoenixErmSupplierCodeMap", "Phoenix-Erm Supplier Code Map", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["PhoenixSupplierId"] == null)
            {
                ViewState["PhoenixSupplierId"] = ds.Tables[0].Rows[0]["FLDADDRESSCODE"].ToString();
                gvPhoenixErmSupplierCodeMap.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }
        gvPhoenixErmSupplierCodeMap.DataSource = ds;
        gvPhoenixErmSupplierCodeMap.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        gvPhoenixErmSupplierCodeMap.SelectedIndexes.Clear();
        for (int i = 0; i < gvPhoenixErmSupplierCodeMap.Items.Count; i++)
        {
            if (gvPhoenixErmSupplierCodeMap.MasterTableView.DataKeyValues.ToString().Equals(ViewState["PhoenixSupplierId"].ToString()))
            {
                //gvPhoenixErmSupplierCodeMap.SelectedIndex = i;
            }
        }
    }
    protected void gvPhoenixErmSupplierCodeMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            RadGrid _gridView = (RadGrid)sender;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
        }
        else if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            return;
        }
    }
    protected void gvPhoenixErmSupplierCodeMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadDropDownList ddlErmCompanyId = (RadDropDownList)e.Item.FindControl("ddlErmCompanyId");
            RadLabel lblErmCompanyIdEdit = (RadLabel)e.Item.FindControl("lblErmCompanyIdEdit");
            if (ddlErmCompanyId != null)
            {
                ddlErmCompanyId.DataSource = BindErmCompanyID();
                ddlErmCompanyId.DataBind();
                ddlErmCompanyId.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                ddlErmCompanyId.SelectedValue = (lblErmCompanyIdEdit == null || lblErmCompanyIdEdit.Text == null || lblErmCompanyIdEdit.Text == "") ? "Dummy" : lblErmCompanyIdEdit.Text;
            }
        }
    }

    protected void MenuPhoenixErmSupplierCodeMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidForm(string ermaccountcode)
    {
        if (ermaccountcode.Trim().Equals(""))
            ucError.ErrorMessage = "ERM account code is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
       
        if (Session["New"].ToString() == "Y")
        {
            gvPhoenixErmSupplierCodeMap.SelectedIndexes.Clear();
            Session["New"] = "N";
            BindPageURL(int.Parse(gvPhoenixErmSupplierCodeMap.SelectedIndexes.ToString()));
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["PhoenixSupplierId"] = ((RadLabel)gvPhoenixErmSupplierCodeMap.Items[rowindex].FindControl("lblPhoenixSupplierId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvPhoenixErmSupplierCodeMap_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvPhoenixErmSupplierCodeMap.SelectedIndex = e.NewSelectedIndex;
    //    BindPageURL(e.NewSelectedIndex);
    //}

    protected void gvPhoenixErmSupplierCodeMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvPhoenixErmSupplierCodeMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        RadDropDownList ddlErmCompanyId = (RadDropDownList)e.Item.FindControl("ddlErmCompanyId");

        try
        {
            RadLabel lblPhoenixSupplierId = ((RadLabel)e.Item.FindControl("lblPhoenixSupplierId"));
            LinkButton lnkPhoenixSupplierCode = ((LinkButton)e.Item.FindControl("lnkPhoenixSupplierCode"));
            RadTextBox txtErmSupplierCode = ((RadTextBox)e.Item.FindControl("txtErmSupplierCode"));
            PhoenixAccountsPhoenixErmIntegration.PhoenixErmSupplierCodeMapUpdate(
                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , int.Parse(lblPhoenixSupplierId.Text)
                                                , lnkPhoenixSupplierCode.Text
                                                , ddlErmCompanyId.SelectedValue != "Dummmy" ? General.GetNullableInteger(ddlErmCompanyId.SelectedValue) : null
                                                , txtErmSupplierCode.Text.Trim()
                                                   );
            ucStatus.Text = "ERM Supplier Code is updated successfully";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
