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
public partial class CrewTravelReportAvgTicketFilter : PhoenixBasePage
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

       }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindFilterCriteria();
    }

    protected NameValueCollection setFilterCriteria()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("ddlYear", getSelectedDataFromListBox(ddlYear));
        criteria.Add("ddlOrigin", ddlOrigin.SelectedValue);
        criteria.Add("ddlDestination", ddlDestination.SelectedValue);

        Filter.CurrentTravelReportAvgTicketFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentTravelReportAvgTicketFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        //txtAgentId.Text = CheckIsNull(nvc.Get("txtAgent"));
        setListBoxSelected(ddlYear, nvc.Get("ddlYear"));
       
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
                Filter.CurrentTravelReportAvgTicketFilter = null;
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