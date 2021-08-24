using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class PortalSeafarerDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DataTable dt = PhoenixCrewManagement.PortalEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            if(dt.Rows.Count> 0)
            {
                Filter.CurrentCrewSelection = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                Response.Redirect("../CrewOffshore/CrewOffshorePortalPersonalGeneral.aspx?empid=" + dt.Rows[0]["FLDEMPLOYEEID"].ToString()+ "&portal=1");

                //if (dt.Rows[0]["FLDNEWAPP"].ToString() == "1")
                //{
                //    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                //    {
                //        Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + dt.Rows[0]["FLDEMPLOYEEID"].ToString() + "&portal=1");
                //    }
                //    else
                //    {
                //        Response.Redirect("../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + dt.Rows[0]["FLDEMPLOYEEID"].ToString() + "&portal=1");
                //    }
                //}
                //else
                //{
                //    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                //    {
                //        Response.Redirect("../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + dt.Rows[0]["FLDEMPLOYEEID"].ToString() + "&portal=1");
                //    }
                //    else
                //    {
                //        Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + dt.Rows[0]["FLDEMPLOYEEID"].ToString() + "&portal=1");
                //    }

                //}


            }



        }
    }
    
}