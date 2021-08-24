using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewTravelCrewChangeLogFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);        
        travelrequestfilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            if (Request.QueryString["office"] != null)
            {
                ddlofficetravelyn.SelectedValue = "1";
                ddlofficetravelyn.Enabled = false;
            }
            BindFilter();
            
        }
    }
    protected void BindFilter()
    {

        NameValueCollection nvc = Filter.CurrentTravelRequestFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
          
        }
        if (nvc != null && !IsPostBack)
        {
            ucvessel.SelectedVessel = (General.GetNullableInteger(nvc.Get("ucvessel")) == null) ? "" : nvc.Get("ucvessel").ToString();
            ucpurpose.SelectedReason = (General.GetNullableInteger(nvc.Get("ucpurpose")) == null) ? "" : nvc.Get("ucpurpose").ToString();
            uctravelstatus.SelectedHard = (General.GetNullableInteger(nvc.Get("uctravelstatus")) == null) ? "" : nvc.Get("uctravelstatus").ToString();
            txtStartDate.Text = (nvc.Get("txtStartDate") == null) ? "" : nvc.Get("txtStartDate").ToString();
            txtEndDate.Text = (nvc.Get("txtEndDate") == null) ? "" : nvc.Get("txtEndDate").ToString();
            txtOrigin.SelectedValue = (General.GetNullableInteger(nvc.Get("txtOrigin")) == null) ? "" : nvc.Get("txtOrigin").ToString();
            txtDestination.SelectedValue = (General.GetNullableInteger(nvc.Get("txtDestination")) == null) ? "" : nvc.Get("txtDestination").ToString();
            ddlofficetravelyn.SelectedValue = (General.GetNullableInteger(nvc.Get("ddlofficetravelyn")) == null) ? "" : nvc.Get("ddlofficetravelyn").ToString();

            if ((nvc.Get("chkCanceledEmployees") != "") && (nvc.Get("chkCanceledEmployees") != null))
                chkCanceledEmployees.Checked = (nvc.Get("chkCanceledEmployees").ToString() == "0") ? false : true;
            else
            {
                chkCanceledEmployees.Checked = false;
            }

            ucport.SelectedSeaport = (General.GetNullableInteger(nvc.Get("ucport")) == null) ? "" : nvc.Get("ucport").ToString();
            txtTravelRequestNo.Text = (nvc.Get("txtTravelRequestNo") == null) ? "" : nvc.Get("txtTravelRequestNo").ToString();
            txtPassengerName.Text = (nvc.Get("txtPassengerName") == null) ? "" : nvc.Get("txtPassengerName").ToString();

            if (General.GetNullableDateTime(txtStartDate.Text) == null)
            {
                txtStartDate.Text = DateTime.UtcNow.Date.AddMonths(-3).ToShortDateString();         
            }
            if (General.GetNullableDateTime(txtEndDate.Text) == null)
            {         
                txtEndDate.Text = DateTime.UtcNow.Date.ToString();
            }
        }
    }

    
    protected void travelrequestfilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            if (!IsValidTravelDate())
            {
                ucError.Visible = true;
                return;
            }

            criteria.Add("ucvessel", ucvessel.SelectedVessel);
            criteria.Add("ucpurpose", ucpurpose.SelectedReason);
            criteria.Add("uctravelstatus", uctravelstatus.SelectedHard);
            criteria.Add("txtStartDate", txtStartDate.Text);
            criteria.Add("txtEndDate", txtEndDate.Text);
            criteria.Add("txtOrigin", txtOrigin.SelectedValue);
            criteria.Add("txtDestination", txtDestination.SelectedValue);
            criteria.Add("ddlofficetravelyn", ddlofficetravelyn.SelectedValue);
            criteria.Add("chkCanceledEmployees", chkCanceledEmployees.Checked == true ? "1" : "0");
            criteria.Add("ucport",ucport.SelectedSeaport);
            criteria.Add("txtTravelRequestNo", txtTravelRequestNo.Text);
            criteria.Add("txtPassengerName", txtPassengerName.Text);

            if (Request.QueryString["office"] != null && Request.QueryString["office"] != string.Empty)
            {
                Filter.CurrentOfficeTravelRequestFilter = criteria;
            }
            else
            {
                Filter.CurrentTravelRequestFilter = criteria;
            }
        }

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
    private bool IsValidTravelDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtStartDate.Text) == null)
            ucError.ErrorMessage = "Travel start date required.";

        if (General.GetNullableDateTime(txtEndDate.Text) == null)
            ucError.ErrorMessage = "Travel end date required.";

        return (!ucError.IsError);
    }
}
