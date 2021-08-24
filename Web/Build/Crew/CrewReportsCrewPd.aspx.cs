using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewReports;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewReportsCrewPd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            toolbar.AddButton("Compose Mail", "MAIL", ToolBarDirection.Right);
            
            MenuReportsFilter.MenuList = toolbar.Show();
            ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                NameValueCollection nvc = Filter.CurrentCrewChangeList;
                if (nvc != null)
                {
                    ddlVessel.SelectedValue = nvc.Get("ddlvessel");
                    ucDate.Text = nvc.Get("ucDate");
                    ucDate1.Text = nvc.Get("ucDate1");
                    ddlUnion.SelectedValue = nvc.Get("ddlUnion");

                    DataSet ds = new DataSet();
                    ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(nvc.Get("ddlUnion").ToString()));
                    ddlVessel.DataTextField = "FLDVESSELNAME";
                    ddlVessel.DataValueField = "FLDVESSELID";
                    ddlVessel.DataSource = ds;
                    ddlVessel.DataBind();
                    ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));

                }
                DataSet ds1 = new DataSet();
                ds1 = PhoenixCrewReportsCrewChange.GetUnions();
                ddlUnion.DataTextField = "FLDNAME";
                ddlUnion.DataValueField = "FLDADDRESSCODE";
                ddlUnion.DataSource = ds1;
                ddlUnion.DataBind();
                ddlUnion.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
     
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=CREWCHANGEPD&showmenu=1&vesselid="+ ddlVessel.SelectedValue +"&fromdate=" + ucDate.Text + "&todate=" + ucDate1.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlUnion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentCrewChangeList;

        if (nvc != null)
            ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(nvc.Get("ddlUnion").ToString()));
        else
            ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(ddlUnion.SelectedValue));
        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataSource = ds;
        ddlVessel.DataBind();
        ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlVessel.SelectedValue, ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Add("ddlvessel", ddlVessel.SelectedValue);
                    criteria.Add("ucDate", ucDate.Text);
                    criteria.Add("ucDate1", ucDate1.Text);
                    criteria.Add("ddlUnion", ddlUnion.SelectedValue);
                    Filter.CurrentCrewChangeList = criteria;
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=CREWCHANGEPD&showmenu=1&vesselid= "+ddlVessel.SelectedValue+"&fromdate=" + ucDate.Text + "&todate=" + ucDate1.Text;
                }
            }
            if (CommandName.ToUpper().Equals("MAIL"))
            {
                if (!IsValidFilter(ddlVessel.SelectedValue, ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("CrewEmail.aspx?itf=crewchange&vesselid=" + ddlVessel.SelectedValue + "&fromdate=" + ucDate.Text + "&todate=" + ucDate1.Text);
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidFilter(string vessel, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy") || vessel.Equals("0"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }
}
