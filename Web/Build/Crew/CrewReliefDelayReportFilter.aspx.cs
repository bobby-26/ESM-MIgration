using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewReliefDelayReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYear();
            BindMonth();
            BindQuarter();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
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
        criteria.Add("ddlYear", getSelectedDataFromListBox(ddlYear));
        criteria.Add("ddlQuarter", getSelectedDataFromListBox(ddlQuarter));
        criteria.Add("ddlMonth", getSelectedDataFromListBox(ddlMonth));
        criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("ucRank", ucRank.selectedlist);
        criteria.Add("ucZone", ucZone.SelectedZoneValue);
        criteria.Add("ucPool", ucPool.SelectedPool);

        Filter.CurrentCrewReliefDelayFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentCrewReliefDelayFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        txtFileNumber.Text = CheckIsNull(nvc.Get("txtFileNumber"));
        setListBoxSelected(ddlYear , CheckIsNull(nvc.Get("ddlYear")));
        setListBoxSelected(ddlQuarter , CheckIsNull(nvc.Get("ddlQuarter")));
        setListBoxSelected(ddlMonth, CheckIsNull(nvc.Get("ddlMonth")));
        ucPrincipal.SelectedList = CheckIsNull(nvc.Get("ucPrincipal"));
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ucZone.selectedlist = CheckIsNull(nvc.Get("ucZone"));
        ucPool.SelectedPool = CheckIsNull(nvc.Get("ucPool"));
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
                Filter.CurrentCrewReliefDelayFilter = null;
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
    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }

    #region Year List
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
    #endregion

    #region Month List
    private void BindMonth()
    {
        string[] months = new string[] { "" ,"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        for (int i = 1; i < months.Length; i++)
        {
            RadListBoxItem li = new RadListBoxItem(months[i], i.ToString());
            ddlMonth.Items.Add(li);
        }
        ddlMonth.DataBind();
        ddlMonth.SelectedValue = DateTime.Today.Year.ToString();
        ddlMonth.Items.Insert(0, new RadListBoxItem("--Select--", ""));
    }
  
    #endregion

    #region Quarter List
    private void BindQuarter()
    {
        string[] quarters = new string[] { "","Q1", "Q2", "Q3", "Q4"};
        for (int i = 1; i < quarters.Length; i++)
        {
            RadListBoxItem li = new RadListBoxItem(quarters[i], i.ToString());
            ddlQuarter.Items.Add(li);
        }
        ddlQuarter.DataBind();
        ddlQuarter.SelectedValue = DateTime.Today.Year.ToString();
        ddlQuarter.Items.Insert(0, new RadListBoxItem("--Select--", ""));
    }
  
    #endregion

    private string getSelectedDataFromListBox(RadListBox li)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in li.Items)
        {
            if (item.Selected == true && item.Value != "")
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

    private void setListBoxSelected(RadListBox li, string selectedValue)
    {
        string strlist = "," + selectedValue + ",";
        foreach (RadListBoxItem item in li.Items)
        {
            item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
        }
    }
}