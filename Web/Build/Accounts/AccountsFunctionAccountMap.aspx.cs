using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class AccountsFunctionAccountMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);           
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Accounts Function Map", "ACCOUNT");
            toolbar1.AddButton("Budgets Function Map", "BUDGET");
            
            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbar1.Show();
            MenuMain.SelectedMenuIndex = 0;

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddFontAwesomeButton("../Accounts/AccountsFunctionAccountMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolgrid.AddFontAwesomeButton("javascript:CallPrint('gvCompanyAccountMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCompanyAccountMap.AccessRights = this.ViewState;
            MenuCompanyAccountMap.MenuList = toolgrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCompanyAccountMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ACCOUNT"))
        {
            Response.Redirect("../Accounts/AccountsFunctionAccountMap.aspx");
        }
        else if (CommandName.ToUpper().Equals("BUDGET"))
        {
            Response.Redirect("../Accounts/AccountsFunctionBudgetMap.aspx");
        }

    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Module",
                                "Function",
                                "Account",                                                                                            
                                "Description",
                                "Remarks"
                              };

        string[] alColumns = {  "FLDMODULE",
                                 "FLDDESCRIPTION", 
                                "FLDACCOUNTCODE", 
                                "FLDACCOUNTDESCRIPTION",
                                "FLDREMARKS"
                                
                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixRegistersAccount.AccountFunctionMapSearch(
                                                   null
                                                 , null
                                                 , sortexpression, sortdirection
                                                 , gvCompanyAccountMap.CurrentPageIndex + 1
                                                 , gvCompanyAccountMap.PageSize
                                                 , ref iRowCount, ref iTotalPageCount
                                                 , 1);

        Response.AddHeader("Content-Disposition", "attachment; filename=Accounts_Function_Map.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Function Map</h3></td>");
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


        ds = PhoenixRegistersAccount.AccountFunctionMapSearch(
                                                  null
                                                , null                                               
                                                , sortexpression, sortdirection
                                                , gvCompanyAccountMap.CurrentPageIndex + 1
                                                , gvCompanyAccountMap.PageSize
                                                , ref iRowCount, ref iTotalPageCount
                                                , 1);


        string[] alCaptions = { "Module",
                                "Function",
                                "Account",                                                                                            
                                "Description",
                                "Remarks"
                              };

        string[] alColumns = {  "FLDMODULE",
                                 "FLDDESCRIPTION", 
                                "FLDACCOUNTCODE", 
                                "FLDACCOUNTDESCRIPTION",
                                "FLDREMARKS"
                                
                             };

        General.SetPrintOptions("gvCompanyAccountMap", "Accounts Function Map", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["AccountMapId"] == null)
            {
                ViewState["AccountMapId"] = ds.Tables[0].Rows[0]["FLDACCOUNTMAPID"].ToString();
                gvCompanyAccountMap.SelectedIndexes.Clear();
            }
            
            //SetRowSelection();
        }
        gvCompanyAccountMap.DataSource = ds;
        gvCompanyAccountMap.VirtualItemCount = iRowCount;
    }
    protected void gvCompanyAccountMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadTextBox txtAccountId = (RadTextBox)e.Item.FindControl("txtAccountId");
            RadTextBox txtAccountSource = (RadTextBox)e.Item.FindControl("txtAccountSource");
            RadTextBox txtAccountUsage = (RadTextBox)e.Item.FindControl("txtAccountUsage");
            RadTextBox txtAccountDescription = (RadTextBox)e.Item.FindControl("txtAccountDescription");
            RadTextBox txtBudgetCode = (RadTextBox)e.Item.FindControl("txtBudgetCode");
            RadTextBox txtBudgetName = (RadTextBox)e.Item.FindControl("txtBudgetName");
            RadTextBox txtBudgetId = (RadTextBox)e.Item.FindControl("txtBudgetId");
            RadTextBox txtBudgetgroupId = (RadTextBox)e.Item.FindControl("txtBudgetgroupId");

            if (txtAccountId != null && txtAccountSource != null && txtAccountUsage != null && txtAccountDescription != null 
                && txtAccountId != null && txtAccountSource != null && txtAccountUsage != null && txtAccountDescription != null)
            {
                txtAccountId.Attributes.Add("style", "visibility:hidden");
                txtAccountSource.Attributes.Add("style", "visibility:hidden");
                txtAccountUsage.Attributes.Add("style", "visibility:hidden");
                //txtAccountDescription.Attributes.Add("style", "visibility:hidden");
                txtBudgetCode.Attributes.Add("style", "visibility:hidden");
                txtBudgetName.Attributes.Add("style", "visibility:hidden");
                txtBudgetId.Attributes.Add("style", "visibility:hidden");
                txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            }

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowAccountPiclist");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListAccount.aspx', true); ");
            }
        }
    }

    protected void MenuCompanyAccountMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCompanyAccountMap.SelectedIndexes.Clear();
                gvCompanyAccountMap.EditIndexes.Clear();
                gvCompanyAccountMap.DataSource = null;
                gvCompanyAccountMap.Rebind();
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
    private bool IsValidForm(string accountcode)
    {
        if (accountcode.Trim().Equals(""))
            ucError.ErrorMessage = "Account code is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCompanyAccountMap.Rebind();
    }

    protected void gvCompanyAccountMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvCompanyAccountMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        if (!IsValidForm(
                    ((RadTextBox)e.Item.FindControl("txtAccountCode")).Text.ToString().Trim()
                ))
        {
            e.Canceled = true;
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersAccount.AccountFunctionMapUpdate(
                                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , int.Parse(((RadLabel)e.Item.FindControl("lblAccountMapId")).Text)
                                            , ((RadTextBox)e.Item.FindControl("txtAccountCode")).Text
                                            , null
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarks")).Text)
                                               );
        ucStatus.Text = "Account Code is updated";

        gvCompanyAccountMap.SelectedIndexes.Clear();
        gvCompanyAccountMap.EditIndexes.Clear();
        gvCompanyAccountMap.DataSource = null;
        gvCompanyAccountMap.Rebind();
    }
}
