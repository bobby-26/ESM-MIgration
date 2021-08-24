using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewOffShoreReliefPlanFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        
        PlanRelieveeFilterMain.AccessRights = this.ViewState;
        PlanRelieveeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
           
            txtReliefDue.Text = "90";

            
            txtName.Focus();
            BindEmployeeStatus();
            //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            //{
            //    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            //    ucVessel.bind();
            //    ucVessel.Enabled = false;
            //}
            //else
            //{
                ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, "", 0);
           // }
            NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;
            if (nvc != null)
            {
                txtReliefDue.Text = nvc.Get("txtReliefDue");
                ucRank.SelectedRank = nvc.Get("ucRank") == "Dummy" ? string.Empty : nvc.Get("ucRank");
                ucVessel.SelectedVessel = nvc.Get("ucVessel") == "Dummy" ? string.Empty : nvc.Get("ucVessel");
            }
        }
    }

    protected void PlanRelieveeFilterMain_TabStripCommand(object sender, EventArgs e)
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
            if (!IsValidDateTime())
            {
                ucError.Visible = true;
                return;
            }
            criteria.Clear();
            criteria.Add("txtReliefDue", txtReliefDue.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("ucRank", ucRank.SelectedRank);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucVesselType", lstVesselType.SelectedVesseltype);
            criteria.Add("ucPool", ucPool.SelectedPool);//ucPool.SelectedPool);
            criteria.Add("ucZone", ucZone.selectedlist);
            criteria.Add("lstNationality", string.Empty);//lstNationality.SelectedList);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtFromTo.Text);
            criteria.Add("status", "0");
            criteria.Add("ddlEmployeeStatus", ddlEmployeeStatus.SelectedValue);
            criteria.Add("ddlOffshoreStage", ddlOffshoreStage.SelectedHard);
            criteria.Add("txtExpectedDate", txtExpectedDate.Text);
            criteria.Add("txtExpectedtoDate", txtExpectedtoDate.Text);
            criteria.Add("ucPort", ucPort.SelectedValue);
            Filter.CurrentPlanRelieveeFilterSelection = criteria;
        }
        //Response.Redirect("CrewPlanRelievee.aspx");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void CalulateDays(object sender, EventArgs e)
    {
        try
        {

            if (txtFromTo.Text != null && txtFromDate.Text != null)
            {
                DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
                DateTime dtto = Convert.ToDateTime(txtFromTo.Text);


                TimeSpan ts = dtto - dtfrom;
                string days = ts.Days.ToString();
                txtReliefDue.Text = days;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CalculateDatetime(object sender, EventArgs e)
    {
        try
        {

            if (txtFromDate.Text != null && txtFromTo.Text != null)
            {
                DateTime dt = DateTime.Parse(txtFromDate.Text).AddDays(int.Parse(txtReliefDue.Text));
                txtFromTo.Text = dt.ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public bool IsValidDateTime()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtFromDate.Text != "" && txtFromTo.Text != null)
        {
            DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
            DateTime dtto = Convert.ToDateTime(txtFromTo.Text);
            if (dtto < dtfrom)
                ucError.ErrorMessage = "To date should be greater than From date.";
        }
        return (!ucError.IsError);
    }
    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        StringBuilder strVesselType = new StringBuilder();
        ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
        ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableString(ucPrincipal.SelectedAddress), 0, lstVesselType.SelectedVesseltype);
    }

    public void BindEmployeeStatus()
    {
        try
        {
            ddlEmployeeStatus.DataSource = PhoenixCrewOffshoreCrewChange.ListEmployeeStatus();
            ddlEmployeeStatus.DataBind();
            ddlEmployeeStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        catch { }
    }
    protected void ddlEmployeeStatus_TextChanged(object sender, EventArgs e)
    {
       
    }
}
