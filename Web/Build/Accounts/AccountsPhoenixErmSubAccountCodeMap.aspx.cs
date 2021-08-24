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
public partial class AccountsPhoenixErmSubAccountCodeMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsPhoenixErmSubAccountCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolgrid.AddFontAwesomeButton("javascript:CallPrint('gvPhoenixErmSubAccountCodeMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolgrid.AddImageButton("../Accounts/AccountsPhoenixErmAccountCodeMapFilter.aspx", "Find", "search.png", "FIND");

            MenuPhoenixErmAccountCodeMap.AccessRights = this.ViewState;
            MenuPhoenixErmAccountCodeMap.MenuList = toolgrid.Show();

            toolgrid = new PhoenixToolbar();
            toolgrid.AddButton("Ledger Mapping", "LEDGERACCOUNTMAPPING");
            toolgrid.AddButton("Sub Account Mapping", "SUBACCOUNTMAPPING");
            MenuSubAccountMap.AccessRights = this.ViewState;
            MenuSubAccountMap.MenuList = toolgrid.Show();
            MenuSubAccountMap.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhoenixErmSubAccountCodeMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alCaptions = { "Company Name", "Phoenix Account Code", "Phoenix Sub Account Code", "ERM Account Code", "ERM Sub Account Code" };
        string[] alColumns = { "FLDCOMPANYCODE", "FLDPHOENIXACCOUNTCODE", "FLDSUBACCOUNT", "FLDERMACCOUNTCODE", "FLDERMSUBACCOUNTCODE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());        

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmSubAccountMapSearch(
                                                                                  General.GetNullableInteger(ucCompany.SelectedCompany)
                                                                                , null
                                                                                , null
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvPhoenixErmSubAccountCodeMap.CurrentPageIndex + 1
                                                                                , gvPhoenixErmSubAccountCodeMap.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount );

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoenixErmSubAccountCodeMap.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phoenix-Erm Sub Account Code Map</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alCaptions = {"Company Name", "Phoenix Account Code", "Phoenix Sub Account Code", "ERM Account Code", "ERM Sub Account Code" };
        string[] alColumns = { "FLDCOMPANYCODE", "FLDPHOENIXACCOUNTCODE", "FLDSUBACCOUNT", "FLDERMACCOUNTCODE", "FLDERMSUBACCOUNTCODE" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmSubAccountMapSearch(
                                                                                   General.GetNullableInteger(ucCompany.SelectedCompany)
                                                                                 , null
                                                                                 , null
                                                                                 , sortexpression
                                                                                 , sortdirection
                                                                                 , gvPhoenixErmSubAccountCodeMap.CurrentPageIndex + 1
                                                                                 , gvPhoenixErmSubAccountCodeMap.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount);  

        General.SetPrintOptions("gvPhoenixErmSubAccountCodeMap", "Phoenix-Erm Sub Account Code Map", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["Subaccountmapid"] == null)
            {
                ViewState["Subaccountmapid"] = ds.Tables[0].Rows[0]["FLDSUBACCOUNTMAPID"].ToString();
                gvPhoenixErmSubAccountCodeMap.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }
        gvPhoenixErmSubAccountCodeMap.DataSource = ds;
        gvPhoenixErmSubAccountCodeMap.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        gvPhoenixErmSubAccountCodeMap.SelectedIndexes.Clear();
        for (int i = 0; i < gvPhoenixErmSubAccountCodeMap.Items.Count; i++)
        {
            if (gvPhoenixErmSubAccountCodeMap.MasterTableView.DataKeyValues.ToString().Equals(ViewState["Subaccountmapid"].ToString()))
            {
                //gvPhoenixErmSubAccountCodeMap.SelectedIndex = i;
            }
        }
    }
    protected void gvPhoenixErmSubAccountCodeMap_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvPhoenixErmSubAccountCodeMap_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        
    }

    protected void MenuSubAccountMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LEDGERACCOUNTMAPPING"))
            {
                Response.Redirect("../Accounts/AccountsPhoenixErmAccountCodeMap.aspx");
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
    protected void MenuPhoenixErmAccountCodeMap_TabStripCommand(object sender, EventArgs e)
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
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        if (Session["New"].ToString() == "Y")
        {
            gvPhoenixErmSubAccountCodeMap.SelectedIndexes.Clear();
            Session["New"] = "N";
            BindPageURL(int.Parse(gvPhoenixErmSubAccountCodeMap.SelectedIndexes.ToString()));
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Subaccountmapid"] = ((RadLabel)gvPhoenixErmSubAccountCodeMap.Items[rowindex].FindControl("lblSubAccountMapId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhoenixErmSubAccountCodeMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvPhoenixErmSubAccountCodeMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        UserControlMaskNumber ucErmAccountCode = (UserControlMaskNumber)e.Item.FindControl("ucErmAccountCode");
        RadTextBox txtErmSubAccountCode = (RadTextBox)e.Item.FindControl("txtErmSubAccountCode");
        try
        {
            RadLabel lblPhoenixAccountId = ((RadLabel)e.Item.FindControl("lblPhoenixAccountId"));
            RadLabel lblPhoenixAccountCode = ((RadLabel)e.Item.FindControl("lblPhoenixAccountCode"));
            RadLabel lblSubAccountMapId = ((RadLabel)e.Item.FindControl("lblSubAccountMapId"));
            RadLabel lblPhoenixErmSubaAccountMapId = ((RadLabel)e.Item.FindControl("lblPhoenixErmSubaAccountMapId"));
            PhoenixAccountsPhoenixErmIntegration.PhoenixErmSubAccountMapInsert(
                                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , int.Parse(lblPhoenixAccountId.Text)
                                                                                , new Guid(lblSubAccountMapId.Text)
                                                                                , General.GetNullableString(ucErmAccountCode.Text)
                                                                                , General.GetNullableString(txtErmSubAccountCode.Text)
                                                                                , General.GetNullableGuid(lblPhoenixErmSubaAccountMapId.Text)
                                                                                );
            ucStatus.Text = "ERM Subaccount Code is updated successfully.";

            gvPhoenixErmSubAccountCodeMap.EditIndexes.Clear();
            gvPhoenixErmSubAccountCodeMap.SelectedIndexes.Clear();
            gvPhoenixErmSubAccountCodeMap.DataSource = null;
            gvPhoenixErmSubAccountCodeMap.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucCompany_Changed(object sender, EventArgs e)
    {
        gvPhoenixErmSubAccountCodeMap.SelectedIndexes.Clear();
        gvPhoenixErmSubAccountCodeMap.EditIndexes.Clear();
        gvPhoenixErmSubAccountCodeMap.DataSource = null;
        gvPhoenixErmSubAccountCodeMap.Rebind();
    }
}
