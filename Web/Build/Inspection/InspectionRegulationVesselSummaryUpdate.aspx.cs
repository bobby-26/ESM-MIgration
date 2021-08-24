using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationVesselSummaryUpdate : PhoenixBasePage
{
    Guid? RegulationId;
    Guid? VesselDtKey;
    protected void Page_Load(object sender, EventArgs e)
    {

        RegulationId = new Guid(Request.QueryString["RegulationId"]);
        VesselDtKey = new Guid(Request.QueryString["VesselDtkey"]);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        RegulationUpdate.AccessRights = this.ViewState;
        RegulationUpdate.MenuList = toolbarmain.Show();
        lnkattachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + VesselDtKey + "&mod=QUALITY'); return false;");
    }

    protected void RegulationUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                DateTime ClosedDate = txtClosedDateAdd.SelectedDate.Value;
                String ClosedRemarks = txtClosedRemarksAdd.Text;
                PhoenixInspectionNewRegulation.RegulationbyVesselUpdate(ClosedDate.ToString("yyyy-MM-dd"), RegulationId.Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID, ClosedRemarks, VesselDtKey.Value);
                PhoenixInspectionNewRegulation.RegulationbyClosedDateUpdate(RegulationId.Value);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearUserInput()
    {
        txtClosedDateAdd.SelectedDate = null;
        txtClosedRemarksAdd.Text = null;
    }
}