using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
public partial class Registers_RegistersMoreInfoCertificateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = PhoenixRegistersVesselSurvey.SurveyCertificatesList(
                General.GetNullableGuid(Request.QueryString["SurveyId"])
                , General.GetNullableGuid(Request.QueryString["ScheduleId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblCertificates.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATES"].ToString();
            }
        }
    }
}
