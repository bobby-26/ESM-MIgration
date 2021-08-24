using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Log_ElectricLogGRBOperation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if (!IsPostBack)
        {
            setButtonClick();
        }

    }
    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void setButtonClick()
    {
        btnPlannedDisposal.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogGRBPlannedDisposal.aspx', null, null, null, null, null, {{'closeAlert': true }});", Session["sitepath"]));
        btnExceptionalDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogGRBExceptionalDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
    }

}