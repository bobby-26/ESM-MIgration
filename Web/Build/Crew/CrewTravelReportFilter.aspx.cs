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
public partial class Crew_CrewTravelReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYear();
            BindMonth();
            BindQuarter();
            BinidOfficeCrew();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);

        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        MenuReportsFilter.AccessRights = this.ViewState;
        btnSubmit.Attributes.Add("style", "display:none;");
        //txtAgentId.Attributes.Add("style", "visibility:hidden");
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
        criteria.Add("txtRequisition", txtRequisition.Text);
        criteria.Add("txtPassportno", txtPassportno.Text);
        criteria.Add("txtTicketno", txtTicketno.Text);
        //criteria.Add("txtAgent", txtAgentId.Text);
        criteria.Add("ddlOfficeCrew", getSelectedDataFromListBox(ddlOfficeCrew));
        criteria.Add("ucZone", ucZone.selectedlist);
        criteria.Add("ddlYear", getSelectedDataFromListBox(ddlYear));
        criteria.Add("ddlQuarter", getSelectedDataFromListBox(ddlQuarter));
        criteria.Add("ddlMonth", getSelectedDataFromListBox(ddlMonth));
        criteria.Add("ddlOrigin", ddlOrigin.SelectedValue);
        criteria.Add("ddlDestination", ddlDestination.SelectedValue);
        criteria.Add("ddlTravelreason", ddlTravelreason.SelectedValue);
        criteria.Add("ucVessel", ucVessel.SelectedVesselValue);
        criteria.Add("ucRank", ucRank.selectedlist);
        criteria.Add("ddlDesignation", ddlDesignation.SelectedDesignation);

        Filter.CurrentCrewTravelFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentCrewTravelFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        txtFileNumber.Text = CheckIsNull(nvc.Get("txtFileNumber"));
        txtRequisition.Text = CheckIsNull(nvc.Get("txtRequisition"));
        txtPassportno.Text = CheckIsNull(nvc.Get("txtPassportno"));
        txtTicketno.Text = CheckIsNull(nvc.Get("txtTicketno"));
        //txtAgentId.Text = CheckIsNull(nvc.Get("txtAgent"));
        ucZone.selectedlist = CheckIsNull(nvc.Get("ucZone"));
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        setListBoxSelected(ddlYear, nvc.Get("ddlYear"));
        setListBoxSelected(ddlMonth, nvc.Get("ddlMonth"));
        setListBoxSelected(ddlQuarter, nvc.Get("ddlQuarter"));

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
                Filter.CurrentCrewTravelFilter = null;
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

    protected void BinidOfficeCrew()
    {
        string[] data = new string[] { "Crew Travel", "Office Travel", "Family Travel" };
        for (int i = 0; i < data.Length; i++)
        {
            RadListBoxItem li = new RadListBoxItem(data[i], i.ToString());
            ddlOfficeCrew.Items.Add(li);
        }
        ddlOfficeCrew.DataBind();
        ddlOfficeCrew.SelectedValue = DateTime.Today.Year.ToString();
        ddlOfficeCrew.Items.Insert(0, new RadListBoxItem("--Select--", ""));
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
        string[] months = new string[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
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
        string[] quarters = new string[] { "", "Q1", "Q2", "Q3", "Q4" };
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