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
public partial class AccountsPhoenixErmAccountCodeMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsPhoenixErmAccountCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolgrid.AddFontAwesomeButton("javascript:CallPrint('gvPhoenixErmAccountCodeMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolgrid.AddImageButton("../Accounts/AccountsPhoenixErmAccountCodeMapFilter.aspx", "Find", "search.png", "FIND");

            MenuPhoenixErmAccountCodeMap.AccessRights = this.ViewState;
            MenuPhoenixErmAccountCodeMap.MenuList = toolgrid.Show();

            toolgrid = new PhoenixToolbar();
            toolgrid.AddButton("Ledger Mapping", "LEDGERACCOUNTMAPPING");
            toolgrid.AddButton("Sub Account Mapping", "SUBACCOUNTMAPPING");
            MenuSubAccountMap.AccessRights = this.ViewState;
            MenuSubAccountMap.MenuList = toolgrid.Show();
            MenuSubAccountMap.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhoenixErmAccountCodeMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "ERM Account Code" };

        string[] alColumns = { "FLDPHOENIXACCOUNTCODE", "FLDPHOENIXACCOUNTDESCRIPTION", "FLDERMACCOUNTCODE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentPhoenixErmAccountCodeMapSelection;

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmAccountCodeMapSearch(
                                                  null
                                                , nvc != null ? General.GetNullableString(nvc.Get("PhoenixAccountCode")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("PhoenixAccountDescription")) : null
                                                , sortexpression, sortdirection
                                                , gvPhoenixErmAccountCodeMap.CurrentPageIndex + 1
                                                , gvPhoenixErmAccountCodeMap.PageSize
                                                , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoenixErmAccountCodeMap.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phoenix-Erm Account Code Map</h3></td>");
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentPhoenixErmAccountCodeMapSelection;

        ds = PhoenixAccountsPhoenixErmIntegration.PhoenixErmAccountCodeMapSearch(
                                                  null
                                                , nvc != null ? General.GetNullableString(nvc.Get("PhoenixAccountCode")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("PhoenixAccountDescription")) : null
                                                , sortexpression, sortdirection
                                                , gvPhoenixErmAccountCodeMap.CurrentPageIndex + 1
                                                , gvPhoenixErmAccountCodeMap.PageSize
                                                , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "ERM Account Code" };

        string[] alColumns = { "FLDPHOENIXACCOUNTCODE", "FLDPHOENIXACCOUNTDESCRIPTION", "FLDERMACCOUNTCODE" };

        General.SetPrintOptions("gvPhoenixErmAccountCodeMap", "Phoenix-Erm Account Code Map", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["PhoenixAccountId"] == null)
            {
                ViewState["PhoenixAccountId"] = ds.Tables[0].Rows[0]["FLDPHOENIXACCOUNTID"].ToString();
                gvPhoenixErmAccountCodeMap.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }
        gvPhoenixErmAccountCodeMap.DataSource = ds;
        gvPhoenixErmAccountCodeMap.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        gvPhoenixErmAccountCodeMap.SelectedIndexes.Clear();
        //for (int i = 0; i < gvPhoenixErmAccountCodeMap.Rows.Count; i++)
        //{
        //    if (gvPhoenixErmAccountCodeMap.DataKeys[i].Value.ToString().Equals(ViewState["PhoenixAccountId"].ToString()))
        //    {
        //        gvPhoenixErmAccountCodeMap.SelectedItems = i;
        //    }
        //} //WebForm
        for (int i = 0; i < gvPhoenixErmAccountCodeMap.Items.Count; i++) 
        {
            if (gvPhoenixErmAccountCodeMap.MasterTableView.DataKeyValues.ToString().Equals(ViewState["PhoenixAccountId"].ToString()))
            {
                //gvPhoenixErmAccountCodeMap.SelectedItems = i;
            }
        }   //Telerik
    }

    protected void gvPhoenixErmAccountCodeMap_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvPhoenixErmAccountCodeMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }
    protected void MenuSubAccountMap_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUBACCOUNTMAPPING"))
            {
                Response.Redirect("../Accounts/AccountsPhoenixErmSubAccountCodeMap.aspx");
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvPhoenixErmAccountCodeMap.SelectedIndexes.Clear();
                gvPhoenixErmAccountCodeMap.EditIndexes.Clear();
                gvPhoenixErmAccountCodeMap.DataSource = null;
                gvPhoenixErmAccountCodeMap.Rebind();
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
        gvPhoenixErmAccountCodeMap.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            gvPhoenixErmAccountCodeMap.SelectedIndexes.Clear();
            Session["New"] = "N";
            BindPageURL(int.Parse(gvPhoenixErmAccountCodeMap.SelectedIndexes.ToString()));
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["PhoenixAccountId"] = ((RadLabel)gvPhoenixErmAccountCodeMap.Items[rowindex].FindControl("lblPhoenixAccountId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhoenixErmAccountCodeMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        UserControlMaskNumber ucErmAccountCode = (UserControlMaskNumber)e.Item.FindControl("ucErmAccountCode");
        try
        {
            if (ucErmAccountCode != null)
            {
                RadLabel lblPhoenixAccountId = ((RadLabel)e.Item.FindControl("lblPhoenixAccountId"));
                RadLabel lblPhoenixAccountCode = ((RadLabel)e.Item.FindControl("lblPhoenixAccountCode"));
                PhoenixAccountsPhoenixErmIntegration.PhoenixErmAccountCodeMapUpdate(
                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(lblPhoenixAccountId.Text)
                                                    , int.Parse(lblPhoenixAccountCode.Text)
                                                    , General.GetNullableString(ucErmAccountCode.Text)
                                                       );

                ucStatus.Text = "ERM Account Code is updated successfully";
                gvPhoenixErmAccountCodeMap.EditIndexes.Clear();
                gvPhoenixErmAccountCodeMap.SelectedIndexes.Clear();
                gvPhoenixErmAccountCodeMap.DataSource = null;
                gvPhoenixErmAccountCodeMap.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhoenixErmAccountCodeMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}
