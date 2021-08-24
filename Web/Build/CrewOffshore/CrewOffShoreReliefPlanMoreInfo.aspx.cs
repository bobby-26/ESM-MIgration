using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using SouthNests.Phoenix.Inspection;


public partial class CrewOffShoreReliefPlanMoreInfo : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            

            if (!IsPostBack)
            {
                ViewState["crewplanid"] = "";

                if (Request.QueryString["crewplanid"] != null)
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                BindDataEdit();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataEdit()
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(ViewState["crewplanid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtComment.Content = dt.Rows[0]["FLDPROPOSALREMARKS"].ToString();
            //txtAreaComment.Text = dt.Rows[0]["FLDPROPOSALREMARKS"].ToString();
        }
    }


    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
      
    }
}
