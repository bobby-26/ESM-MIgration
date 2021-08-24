using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonPickListSubAccount : PhoenixBasePage
{
    public DataSet dsaccount = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:None");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuBudget.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] == "openingbalance")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"];
            }

            if (Session["SelectedAccountId"] != null && Session["SelectedAccountId"].ToString().Length > 0)
            {
                ViewState["SelectedAccountId"] = Session["SelectedAccountId"].ToString();
                dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(ViewState["SelectedAccountId"].ToString()));
            }
            else
                ViewState["SelectedAccountId"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (dsaccount != null)
            {
                if (dsaccount.Tables[0].Rows.Count > 0)
                {
                    DataRow draccount = dsaccount.Tables[0].Rows[0];
                    ViewState["ACCOUNTUSAGE"] = draccount["FLDACCOUNTUSAGE"].ToString();
                }
            }
            gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }

    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvBudget.SelectedIndexes.Clear();
                gvBudget.EditIndexes.Clear();
                gvBudget.DataSource = null;
                gvBudget.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;
            string selectedaccid = null;
            string accusage = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            int? showall = null;


            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["SelectedAccountId"] != null)
                selectedaccid = ViewState["SelectedAccountId"].ToString();
            if (ViewState["ACCOUNTUSAGE"] != null)
                accusage = ViewState["ACCOUNTUSAGE"].ToString();
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "openingbalance")
                showall = 0;

            ds = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                  , General.GetNullableInteger(selectedaccid)
                  , General.GetNullableInteger(accusage)
                  , General.GetNullableString(txtSubAccountCodeSearch.Text)  //txtDescriptionNameSearch.Text
                  , General.GetNullableString(txtDescriptionSearch.Text)
                  , sortexpression, sortdirection,
                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                  General.ShowRecords(null),
                  ref iRowCount,
                  ref iTotalPageCount, showall
                  );
            gvBudget.DataSource = ds;
            gvBudget.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;
            nvc = Filter.CurrentPickListSelection;

            if (dsaccount != null)
            {
                if (dsaccount.Tables[0].Rows.Count > 0)
                {
                    DataRow draccount = dsaccount.Tables[0].Rows[0];
                    nvc.Set(nvc.GetKey(1), draccount["FLDACCOUNTCODE"].ToString());
                    nvc.Set(nvc.GetKey(2), draccount["FLDDESCRIPTION"].ToString());
                    nvc.Set(nvc.GetKey(3), draccount["FLDACCOUNTID"].ToString());
                    nvc.Set(nvc.GetKey(4), draccount["FLDACCOUNTSOURCENAME"].ToString());
                    nvc.Set(nvc.GetKey(5), draccount["FLDACCOUNTUSAGENAME"].ToString());
                }
            }
            if (Request.QueryString["mode"] == "custom")
            {
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(6), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                nvc.Set(nvc.GetKey(7), lbl.Text);
                RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");
                nvc.Set(nvc.GetKey(8), lblBudgetCodeId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(9), lblBudgetGroupId.Text);

                Filter.CurrentSelectedESMBudgetCode = lblBudgetCodeId.Text;
            }
            else
            {
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(6), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                nvc.Set(nvc.GetKey(7), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)e.Item.FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(8), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(9), lblBudgetGroupId.Text);

                Filter.CurrentSelectedESMBudgetCode = lblBudgetId.Text;

            }

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
        if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        {
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    protected void gvBudget_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void gvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudget.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
}
