using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;



public partial class AccountsProjectBillingChargingFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
    
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.Title = "Invoice";
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {   
            BindVesselList();
            BindProjectBillingGroup();
            ViewState["SelectedVesselList"] = "";
            
        }
    }
    
    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";                      
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindProjectBillingGroup()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        ,98);
        ChkProjectBillingGroup.Items.Add("select");
        ChkProjectBillingGroup.DataSource = ds;
        ChkProjectBillingGroup.DataTextField = "FLDQUICKNAME";
        ChkProjectBillingGroup.DataValueField = "FLDQUICKCODE";
        ChkProjectBillingGroup.DataBind();
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVesselList.Items)
        {            
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {                
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();                
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {             
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");                
            }
        }        
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            StringBuilder strbillinggroup = new StringBuilder();

            foreach (ListItem item in ChkProjectBillingGroup.Items)
            {
                if (item.Selected == true )
                {                   
                    strbillinggroup.Append(item.Value.ToString());
                    strbillinggroup.Append(",");                
                }
            }
            if (strbillinggroup.Length > 1)
            {
                strbillinggroup.Remove(strbillinggroup.Length - 1, 1);
            }            
            if (strbillinggroup.ToString().Contains("Dummy"))
            {
                strbillinggroup = new StringBuilder();
                strbillinggroup.Append("Dummy");

            }
            //string cancelled = chkCancelled.Checked ? 1 : 0;

            criteria.Clear();
            criteria.Add("txtProjectBillingName", txtProjectBillingName.Text);
            criteria.Add("txtIssueFromDate", txtIssueFromDate.Text);
            criteria.Add("txtIssueToDate", txtIssueToDate.Text);
            criteria.Add("ddlLiabilitycompany", ddlLiabilitycompany.SelectedCompany);
            criteria.Add("ucVessel", ViewState["SelectedVesselList"].ToString());
            criteria.Add("voucherstatus", ddlVoucherStatus.SelectedValue);
            criteria.Add("billinggrouplist", strbillinggroup.ToString());
            criteria.Add("cancelled", (chkCancelled.Checked ? 1 : 0).ToString()); 
            
            Filter.CurrentSelectedProjectBillingVoucher = criteria;

            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentSelectedProjectBillingVoucher = criteria;
            
            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        
    }    
}



