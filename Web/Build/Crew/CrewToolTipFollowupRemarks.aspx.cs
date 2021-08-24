using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewToolTipFollowupRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"] !="")

                ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {

        DataTable dt = new DataTable();

        int employeeid ;

        if (ViewState["employeeid"] != null)
        {
            employeeid = Convert.ToInt32(ViewState["employeeid"].ToString());
        }


        dt = PhoenixCrewManagement.EditFollowupRemarks(General.GetNullableInteger(ViewState["employeeid"].ToString()));

        if (dt.Rows.Count > 0)
        {


            lblremarks.Text = dt.Rows[0]["FLDLASTREMARKS"].ToString();

            lblLastcontactdate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLASTCONTACTDATE"].ToString());
        }
    }
}