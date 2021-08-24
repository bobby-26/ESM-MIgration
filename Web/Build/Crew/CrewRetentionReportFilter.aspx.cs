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
public partial class Crew_CrewRetentionReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYear();
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

        criteria.Add("txtFrom", txtFrom.Text == null ? "" : txtFrom.Text);
        criteria.Add("txtTo", txtTo.Text == null ? "" : txtTo.Text);
        criteria.Add("ddlYear", getSelectedDataFromListBox(ddlYear));
        criteria.Add("ucRank", ucRank.selectedlist);
        criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
        criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
        
        Filter.CurrentCrewRetentionFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentCrewRetentionFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        txtFrom.Text = CheckIsNull(nvc.Get("txtFrom"));
        txtTo.Text = CheckIsNull(nvc.Get("txtTo"));
        setListBoxSelected(ddlYear, nvc.Get("ddlYear"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ucPrincipal.SelectedList = CheckIsNull(nvc.Get("ucPrincipal"));
        ucVesselType.SelectedVesseltype = CheckIsNull(nvc.Get("ucVesselType"));
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
                Filter.CurrentCrewRetentionFilter = null;
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
        int yearDiff = Math.Abs(DateTime.Parse("2011-01-01").Year - DateTime.Now.Year);
        for (int i = 1; i <= yearDiff; i++)
        {
            RadListBoxItem li = new RadListBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlYear.Items.Insert(0, new RadListBoxItem("--Select--", ""));
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