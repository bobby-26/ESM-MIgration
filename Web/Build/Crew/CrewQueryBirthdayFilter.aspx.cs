using System;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewQueryBirthdayFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        
        BirthdayListFilterMain.AccessRights = this.ViewState;
        BirthdayListFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        { 
            ucToDate.Text = DateTime.Now.ToShortDateString();
        }
    }

    protected void BirthdayListFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;



        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            criteria.Clear();
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
            criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
            criteria.Add("ucManager", ucManager.SelectedList);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucRank", ucRank.selectedlist);
            criteria.Add("ucZone", ucZone.selectedlist);
            criteria.Add("ucBatch", ucBatch.SelectedList);
            criteria.Add("ucStatus", ddlSelectFrom.SelectedValue);
            criteria.Add("chkInActive", (chkInActive.Checked == true) ? "" : "1");

            Filter.BirthdayReportFilter = criteria;            
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

    }

    public bool IsValidFilter()
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ucFromDate.Text))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        if (string.IsNullOrEmpty(ucToDate.Text))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(ucFromDate.Text)
            && DateTime.TryParse(ucToDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucFromDate.Text)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }   
}
