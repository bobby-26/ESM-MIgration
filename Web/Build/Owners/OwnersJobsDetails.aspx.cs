using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners ;
using SouthNests.Phoenix.Registers;


public partial class Owners_OwnersJobsDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["JobId"] != null && Request.QueryString["JobId"].ToString() != "")
            {
                ViewState["JobId"] = Request.QueryString["JobId"].ToString();
                JobDetails(ViewState["JobId"].ToString());

            }
            else
            {
                ViewState["JobId"] = "";
            }

        }
    }


    private void JobDetails(string  JobId)
    {
        DataSet ds = PhoenixOwnersPlannedMaintenance.ShowJobDetails(General.GetNullableGuid (JobId));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtJobCode.Text = dr["FLDJOBCODE"].ToString();
            txtJobTitle.Text = dr["FLDJOBTITLE"].ToString();
            txtJobDetail.Content   = dr ["FLDJOBDETAILS"].ToString (); 
        }
    }   
}
