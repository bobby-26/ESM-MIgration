using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsOwnerOfficeSingleDepartmentAllocate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["Source"] = "";
                ViewState["Ownerfundreceived"] = "";
                if (Request.QueryString["OwnerOfficeFundId"] != null && Request.QueryString["OwnerOfficeFundId"] != string.Empty)
                {
                    ViewState["OwnerOfficeFundId"] = Request.QueryString["OwnerOfficeFundId"];

                    ViewState["ispopup"] = Request.QueryString["Ispopup"];
                }
                if (Request.QueryString["Source"] != null && Request.QueryString["Source"] != string.Empty)
                    ViewState["Source"] = Request.QueryString["Source"].ToString();
                if (Request.QueryString["Ownerfundreceived"] != null && Request.QueryString["Ownerfundreceived"] != string.Empty)
                    ViewState["Ownerfundreceived"] = Request.QueryString["Ownerfundreceived"].ToString();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            
            toolbar.AddButton("Find", "FIND",ToolBarDirection.Right);
            toolbar.AddButton("Allocate", "ALLOCATE", ToolBarDirection.Right);
            MenuSingleDepartment.AccessRights = this.ViewState;
            MenuSingleDepartment.MenuList = toolbar.Show();
            gvAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteList(txtAddressee.Text
                                                                               , txtSubject.Text
                                                                               , General.GetNullableInteger(ddlPrincipal.SelectedAddress)
                                                                               , txtRefNo.Text
                                                                               , General.GetNullableInteger(ViewState["Source"].ToString())
                                                
                                                                               , (int)ViewState["PAGENUMBER"]
                                                                               , gvAllocation.PageSize
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount
                                                                               , General.GetNullableInteger(ViewState["Ownerfundreceived"].ToString())
                                                                               , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                                                                               , txtcnrno.Text
                                                                  );
        gvAllocation.DataSource = ds;
        gvAllocation.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAllocation_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void MenuSingleDepartment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ALLOCATE"))
            {
                StringBuilder strallocatingid = new StringBuilder();

                RadGrid gv = (RadGrid)gvAllocation;
                foreach (GridDataItem row in gv.Items)
                {
                    CheckBox chkAllocate = (CheckBox)row.FindControl("chkAllocate");
                    if (chkAllocate != null && chkAllocate.Checked == true)
                    {
                        string strtemp;
                        strtemp = ((RadLabel)row.FindControl("lblDebitCreditNoteIdItem")).Text.ToString();
                        strallocatingid.Append(((RadLabel)row.FindControl("lblDebitCreditNoteIdItem")).Text.ToString());
                        strallocatingid.Append(",");
                    }
                }
                if (strallocatingid.Length > 1)
                {
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherAllocateInsert(strallocatingid.ToString()
                                                                                                 , new Guid(ViewState["OwnerOfficeFundId"].ToString())
                                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                }
             
                if (ViewState["ispopup"].ToString() != "")
                {
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                Rebind();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvAllocation.SelectedIndexes.Clear();
        gvAllocation.EditIndexes.Clear();
        gvAllocation.DataSource = null;
        gvAllocation.Rebind();
    }
    protected void gvAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllocation.CurrentPageIndex + 1;
        BindData();
    }
}
