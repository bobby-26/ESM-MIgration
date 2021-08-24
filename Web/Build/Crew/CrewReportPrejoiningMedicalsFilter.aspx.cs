using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;
public partial class CrewReportPrejoiningMedicalsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar tb = new PhoenixToolbar();
                tb.AddButton("Show Report", "PREJOININGMEDICALREPORT");
                MenuReportsMedical.MenuList = tb.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuReportsMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("PREJOININGMEDICALREPORT"))
            {
                if (ddlMonthlist.SelectedHard.ToUpper() != "DUMMY" && ddlYearlist.SelectedQuick.ToUpper() != "DUMMY")
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=MEDICALREPORTFORJOINERS&month=" + int.Parse(ddlMonthlist.SelectedHard) + "&year=" + int.Parse(ddlYearlist.SelectedQuick), false);                    
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Month and Year";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
