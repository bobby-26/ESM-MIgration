using SouthNests.Phoenix.Framework;
using System;

public partial class Log_ElectricLogGRB2Operation : PhoenixBasePage
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
        btnPlannedDisposal.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogGRB2PlannedDisposal.aspx', null, null, null, null, null, {{'closeAlert': true }});", Session["sitepath"]));
        btnExceptionalDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogGRB2ExceptionalDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
    }

}