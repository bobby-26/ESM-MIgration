using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Dashboard_DashboardSKValuesPopulate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            DataTable dt = PheonixDashboardSKSPI.SPIList();

            Radcombospilist.DataSource = dt;
            Radcombospilist.DataBind();

            DataTable dt1 = PheonixDashboardSKKPI.KPIList();
            RadComboKpilist.DataSource = dt1;
            RadComboKpilist.DataBind();

            DataTable dt2 = PheonixDashboardSKPI.PIList();
            Radcombopilist.DataSource = dt2;
            Radcombopilist.DataBind();


        }
    }

    protected void picaluclate_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixDashboardSKPIValue.PopulatePi(Radcombopilist.Value);

            ucNotification.Show("Calculated SuccessFully");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void kpicalculate_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixDashboardSKPIValue.PopulatePi(RadComboKpilist.Value);
            ucNotification.Show("Calculated SuccessFully");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void spicalculate_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixDashboardSKPIValue.PopulatePi(Radcombospilist.Value);
            ucNotification.Show("Calculated SuccessFully");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void changetimeline_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixDashboardSKPIValue.ChangeKPITiming(quarter.SelectedValue, year.SelectedYear);
            ucNotification.Show("Changed SuccessFully");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}