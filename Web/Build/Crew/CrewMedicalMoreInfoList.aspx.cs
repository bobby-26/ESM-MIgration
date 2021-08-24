using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewMedicalMoreInfoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["requestid"] = Request.QueryString["requestid"].ToString();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        DataTable dt = PhoenixCrewMedical.EditMedicalRequest(new Guid(ViewState["requestid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            lblAppointmentDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDAPPOINTMENTDATE"].ToString());
            lblPaymentType.Text = dt.Rows[0]["FLDPAYMENTTYPE"].ToString();
        }
    }
}