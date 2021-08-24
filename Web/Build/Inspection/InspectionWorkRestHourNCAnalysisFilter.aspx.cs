using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;

public partial class Inspection_InspectionWorkRestHourNCAnalysisFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYear();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Clear", "CLEAR");
        toolbar.AddButton("Cancel", "CANCEL");
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
        string Quarter = selectedQuarterList() == "" ? null : selectedQuarterList();

        criteria.Add("ddlYear", ddlYear.SelectedValue);
        criteria.Add("Quarter", selectedQuarterList());
        criteria.Add("Month", selectedMonthList());
        criteria.Add("ucFleet", ucFleet.SelectedList);
        criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("ucVesselType", ucVesselType.SelectedHard);
        criteria.Add("ucRank", ucRank.selectedlist);

        InspectionFilter.CurrentWRHNcAnalysisFilter = criteria;
        return criteria;
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = InspectionFilter.CurrentWRHNcAnalysisFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }

        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ddlYear.SelectedValue = CheckIsNull(nvc.Get("ddlYear"));
        string Quarter = CheckIsNull(nvc.Get("Quarter"));
        CheckUncheck(Quarter, "Quarter");
        string Month = CheckIsNull(nvc.Get("Month"));
        CheckUncheck(Month, "Month");
        ucFleet.SelectedList = CheckIsNull(nvc.Get("ucFleet"));
        ucPrincipal.SelectedList = CheckIsNull(nvc.Get("ucPrincipal"));
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ucVesselType.SelectedHard = CheckIsNull(nvc.Get("ucVesselType"));
    }

    protected void CheckUncheck(string Values, string ID)
    {
        if (Values != "" && Values != null)
        {
            string[] values = Values.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                if (ID == "Quarter")
                    lstQuarter.Items.FindByValue(values[i]).Selected = true;
                else if (ID == "Month")
                    lstMonth.Items.FindByValue(values[i]).Selected = true;
            }
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                setFilterCriteria();
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCrewContractFilter = null;

                ddlYear.SelectedValue = System.DateTime.Today.Year.ToString();
                foreach (ListItem item in lstQuarter.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in lstMonth.Items)
                {
                    item.Selected = false;
                }
                ucFleet.SelectedList = "";
                ucPrincipal.SelectedList = "";
                ucVessel.SelectedVessel = "";
                ucVesselType.SelectedHard = "";
                ucRank.selectedlist = "";

            }
            if (dce.CommandName.ToUpper().Equals("CANCEL"))
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
        ddlYear.Items.Insert(0, new ListItem("--Select--", ""));
        for (int i = 2010; i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlYear.DataBind();

    }

    private string selectedQuarterList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstQuarter.Items)
        {
            if (item.Text != "--Select--")
            {
                lstQuarter.Items.FindByText("--Select--").Selected = false;
                if (item.Selected == true)
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();

    }


    private string selectedMonthList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstMonth.Items)
        {
            if (item.Text != "--Select--")
            {
                if (item.Selected == true)
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();

    }

    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
}