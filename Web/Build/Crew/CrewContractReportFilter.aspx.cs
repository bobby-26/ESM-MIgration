using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewContractReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYear();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
    
        MenuReportsFilter.MenuList = toolbar.Show();
        MenuReportsFilter.AccessRights = this.ViewState;
        btnSubmit.Attributes.Add("style", "display:none;");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // bind filter criteria if any
        BindFilterCriteria();
    }

    protected NameValueCollection setFilterCriteria()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("txtFileNumber", txtFileNumber.Text);
        criteria.Add("ddlYear", getSelectedYearData());
        criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("ucRank", ucRank.selectedlist);
        criteria.Add("ucZone", ucZone.SelectedZoneValue);
        criteria.Add("ucPool", ucPool.SelectedPool);
        //criteria.Add("ddlReason", ucSignOff.selectedlist);
        criteria.Add("ddlReason",null);

        Filter.CurrentCrewContractFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentCrewContractFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        txtFileNumber.Text = CheckIsNull(nvc.Get("txtFileNumber"));
        setSelectedYear(CheckIsNull(nvc.Get("ddlYear")));
        ucPrincipal.SelectedList = CheckIsNull(nvc.Get("ucPrincipal"));
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ucZone.selectedlist = CheckIsNull(nvc.Get("ucZone"));
        ucPool.SelectedPool = CheckIsNull(nvc.Get("ucPool"));
        ucSignOff.selectedlist = CheckIsNull(nvc.Get("ddlReason"));
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("GO"))
            {
                setFilterCriteria();
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCrewContractFilter = null;
                BindFilterCriteria();
            }
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void BindYear()
    {
        for (int i = 2010; i <= (DateTime.Today.Year); i++)
        {
            RadListBoxItem li = new RadListBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlYear.Items.Insert(0, new RadListBoxItem("--Select--", ""));
    }

    private string  CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }

    // get year List data
    private string getSelectedYearData()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in ddlYear.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    private void setSelectedYear(string value)
    {
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in ddlYear.Items)
            {
                item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
            }
    }
}