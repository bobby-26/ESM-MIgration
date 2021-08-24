using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using  SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CommonPickListOwnerBudget : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
      
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuBudget.MenuList = toolbarmain.Show();
        
        if (!IsPostBack)
        {
            rgvBudget.PageSize = General.ShowRecords(null);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] =   null;
            ViewState["SORTDIRECTION"]  =   null;
            ViewState["BUDGETID"]       =   null ;
            ViewState["ALTERBUDGETID"]   =    null;
            ViewState["OWNERID"]        = null;
            ViewState["BUDGETGROUP"] = "";

            if (Request.QueryString["budgetid"] != null && Filter.CurrentSelectedESMBudgetCode == null)
            {
                ViewState["BUDGETID"] = Request.QueryString["budgetid"].ToString();
                chkShowAll.Visible = true;
            }
            if (Request.QueryString["budgetid"] != null && Filter.CurrentSelectedESMBudgetCode != null)
            {
                ViewState["BUDGETID"] = Filter.CurrentSelectedESMBudgetCode;
                chkShowAll.Visible = true;
            }
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            if (Request.QueryString["windowname"] != null)
                ViewState["windowname"] = Request.QueryString["windowname"].ToString();
            if (Request.QueryString["Ownerid"] != null)
            {
                ViewState["OWNERID"] = Request.QueryString["Ownerid"].ToString();
                ucOwnerBudgetGroup.OwnerId = ViewState["OWNERID"].ToString();
            }
            else if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Request.QueryString["vesselid"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["OWNERID"] = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();
                    ucOwnerBudgetGroup.OwnerId = ViewState["OWNERID"].ToString();
                }
            }
            

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
                rgvBudget.Rebind();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rgvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int iownerid = 0;

        DataSet ds;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        string budgetid = (ViewState["BUDGETID"] == null) ? "0" : (ViewState["BUDGETID"].ToString());
        //string budgetid = (ViewState["BUDGETID"] == null) ? "NULL" : (ViewState["BUDGETID"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonRegisters.OwnerBudgetCodeSearch(
            txtOwenrBudgetCode.Text
            , General.GetNullableGuid(ucOwnerBudgetGroup.SelectedBudgetCode)
            , General.GetNullableInteger(ViewState["OWNERID"].ToString()), General.GetNullableInteger(Request.QueryString["vesselid"])
            , General.GetNullableInteger(budgetid)
            , sortexpression, sortdirection
            , rgvBudget.CurrentPageIndex+1
            , rgvBudget.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , ref iownerid);

        rgvBudget.DataSource = ds;
        rgvBudget.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["BUDGETGROUP"] = ds.Tables[0].Rows[0]["FLDOWNERBUDGETGROUPID"].ToString();
        }
        if (ViewState["OWNERID"] == null)
        {
            ViewState["OWNERID"] = iownerid.ToString();
            ucOwnerBudgetGroup.OwnerId = ViewState["OWNERID"].ToString();
        }
    }

    protected void rgvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            
            string Script = "";
            NameValueCollection nvc;
            if (Request.QueryString["POPUP"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "populateTelerikPick('" + ViewState["windowname"].ToString() + "','" + ViewState["framename"].ToString() + "','codehelp1');";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["GROUP"].FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);
            }
            else if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Add(lbl.ID, lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["GROUP"].FindControl("lblBudgetId");
                nvc.Add(lblBudgetId.ID, lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Add(lblBudgetGroupId.ID, lblBudgetGroupId.Text);
            }
            else
            {

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["GROUP"].FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }
            Filter.CurrentSelectedESMBudgetCode = null;
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
            
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
        if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        {
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
    }
    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkShowAll.Checked==true)
        {
            if (ViewState["BUDGETID"] != null)
                ViewState["ALTERBUDGETID"] = ViewState["BUDGETID"].ToString();
            ViewState["BUDGETID"] = null;
        }
        else
        {
            if (ViewState["ALTERBUDGETID"] != null)
                ViewState["BUDGETID"] = ViewState["ALTERBUDGETID"].ToString();
        }
        rgvBudget.Rebind();
    }
}
