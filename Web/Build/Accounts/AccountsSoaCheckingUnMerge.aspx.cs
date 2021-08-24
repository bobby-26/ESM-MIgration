using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsSoaCheckingUnMerge : PhoenixBasePage
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../Accounts/AccountsSoaChecking.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //MenuOrderForm.AccessRights = this.ViewState;
            //MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Un Merge", "UNMERGE",ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();

            //PhoenixToolbar toolbarsub = new PhoenixToolbar();
            //toolbarsub.AddButton("Details", "DETAILS");
            //toolbarsub.AddButton("Voucher", "VOUCHER");
            //toolbarsub.AddButton("Query", "QUERY");
            //MenuGenralSub.AccessRights = this.ViewState;
            //MenuGenralSub.MenuList = toolbarsub.Show();
            //MenuGenralSub.SelectedMenuIndex = 0;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["vouchernumber"] = Request.QueryString["vouchernumber"].ToString();
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UNMERGE"))
        {
            string selectedrequests="";

            getSelectedRequests(ref selectedrequests);

            PhoenixAccountsSoaChecking.SoaCheckingVoucherUnMergeDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,selectedrequests);

            BindData();
            gvOwnersAccount.Rebind();
        }
    }

    protected void getSelectedRequests(ref string selectedrequests)
    {
        //string selectedrequests = "";
        for (int i = 0; i < gvOwnersAccount.Items.Count; i++)
        {
            CheckBox cb = (CheckBox)gvOwnersAccount.Items[i].FindControl("chkChecked");
            RadLabel lblproid = (RadLabel)gvOwnersAccount.Items[i].FindControl("lblVoucherLineItemId");

            if (cb != null && lblproid != null && (cb.Checked == true))
            {
                selectedrequests += lblproid.Text;
                selectedrequests += ",";
            }
        }
        if (selectedrequests.Length > 0)
        {
            selectedrequests = selectedrequests.Remove(selectedrequests.Length - 1);
        }
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

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

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherUnMergerSearch(int.Parse(ViewState["accountid"].ToString()), ViewState["vouchernumber"].ToString());
        gvOwnersAccount.DataSource = ds;

       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);
    }

   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    

    protected void gvOwnersAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

     
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();

    }
}
