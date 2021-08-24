using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaMoreInfoExamInvigilator : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string batchexamid = Request.QueryString["batchexamid"].ToString();
        DataTable dt = PhoenixPreSeaBatchExamInvigilator.ListBatchExamInvigilator(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(batchexamid));

        if (dt.Rows.Count > 0)
        {
            gvInvigilator.DataSource = dt;
            gvInvigilator.DataBind();
        }
        else
        {
            Response.Write("Invigilator is not assigned.");
        }
    }
}
