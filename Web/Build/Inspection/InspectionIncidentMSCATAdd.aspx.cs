using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionIncidentMSCATAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();
        if (!IsPostBack)
        {            
            ucAccidentDescription.bind();
            BindTC(null,null);
            BindFindings();
            txtBCRemarks.Focus();
            txtICRemarks.Focus();
        }
    }

    protected void MenuMSCAT_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidAdd())
            {
                ucError.Visible = true;
                return;
            }
            DataSet ds;
            ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID.ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            }

            Guid? newinsertedid = null;
            Guid? newinsertedid1 = null;
            Guid? newinsertedid2 = null;
            Guid? newinsertedid3 = null;
            //contact type
            PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(Filter.CurrentIncidentID),
                new Guid(lblContactTypeId.Text), null, int.Parse(ViewState["VESSELID"].ToString()), 1, null
                , ref newinsertedid,null
                , General.GetNullableGuid(rblFindings.SelectedValue)
                , null,null);

            //immediate cause
            PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(Filter.CurrentIncidentID),
                new Guid(rblIC.SelectedValue), General.GetNullableGuid(newinsertedid.ToString()),
                int.Parse(ViewState["VESSELID"].ToString()), 2, null, ref newinsertedid1, null,
                General.GetNullableGuid(rblFindings.SelectedValue), 
                General.GetNullableString(txtICRemarks.Text),
                null);

            //basic cause
            if (General.GetNullableGuid(rblBC.SelectedValue) != null)
            {
                PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(Filter.CurrentIncidentID),
                    new Guid(rblBC.SelectedValue), General.GetNullableGuid(newinsertedid.ToString()),
                    int.Parse(ViewState["VESSELID"].ToString()), 3, null, ref newinsertedid2, null,
                    General.GetNullableGuid(rblFindings.SelectedValue),
                    null,
                    General.GetNullableString(txtBCRemarks.Text));
            }

            //CAN
            if (General.GetNullableGuid(rblCAN.SelectedValue) != null)
            {
                PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(Filter.CurrentIncidentID),
                    new Guid(rblCAN.SelectedValue), General.GetNullableGuid(newinsertedid.ToString()),
                    int.Parse(ViewState["VESSELID"].ToString()), 4, null, ref newinsertedid3, null,
                    General.GetNullableGuid(rblFindings.SelectedValue), null, null);
            }

            //String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
            //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            ucStatus.Text = "Information Updated";

            Response.Redirect("../Inspection/InspectionIncidentMSCATEdit.aspx?mscatid="+ newinsertedid, true);
        }

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Inspection/InspectionIncidentMSCAT.aspx?", true);
        }
    }

    protected void BindFindings()
    {
        DataTable dt = PhoenixInspectionIncident.ListIncidentFindings(new Guid(Filter.CurrentIncidentID));
        rblFindings.DataSource = dt;
        rblFindings.DataValueField = "FLDINCIDENTFINDINGSID";
        rblFindings.DataTextField = "FLDFINDINGS";
        rblFindings.DataBind();
    }

    protected void BindTC(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionContractType.ListContactType(General.GetNullableInteger(ucAccidentDescription.SelectedHard));
        //rblContactType.DataSource = ds;
        //rblContactType.DataValueField = "FLDCONTACTTYPEID";
        //rblContactType.DataTextField = "FLDCONTACTTYPE";
        //rblContactType.DataBind();
        BindIC();
    }

    protected void BindIC()
    {
        DataSet ds = PhoenixInspectionImmediateCause.ListImmediateCauses(General.GetNullableGuid(lblContactTypeId.Text));
        rblIC.DataSource = ds;
        rblIC.DataValueField = "FLDIMMEDIATECAUSEID";
        rblIC.DataTextField = "FLDIMMEDIATECAUSE";
        rblIC.DataBind();
        BindBC();
    }

    protected void BindBC()
    {
        DataSet ds = PhoenixInspectionBasicCause.ListBasicCauseWithSubcause(General.GetNullableGuid(rblIC.SelectedValue));        
        rblBC.DataSource = ds;
        rblBC.DataValueField = "FLDID";
        rblBC.DataTextField = "FLDNAME";
        rblBC.DataBind();        
        BindCAN(); 

        foreach(ListItem li in rblBC.Items)
        {
            int level = 0;
            DataSet dss = PhoenixInspectionBasicCause.GetBasicCauseLevel(new Guid(li.Value), ref level);
            if (level == 1)
                li.Enabled = false;
            else
                li.Enabled = true;
        }
    }

    protected void BindCAN()
    {
        DataSet ds = PhoenixInspectionMscatControlActionNeeded.ListCANwithsubCAN(General.GetNullableGuid(rblBC.SelectedValue));
        rblCAN.DataSource = ds;
        rblCAN.DataValueField = "FLDID";
        rblCAN.DataTextField = "FLDNAME";
        rblCAN.DataBind();
        //BindComplianceLevel();

        foreach (ListItem li in rblCAN.Items)
        {
            int level = 0;
            DataSet dss = PhoenixInspectionMscatControlActionNeeded.GetCANLevel(new Guid(li.Value), ref level);
            if (level == 1)
                li.Enabled = false;
            else
                li.Enabled = true;
        }
    }

    protected void BindComplianceLevel()
    {
        //DataSet ds = PhoenixRegistersHard.ListHard(1, 207);
        //rblComplianceLevel.DataSource = ds;
        //rblComplianceLevel.DataValueField = "FLDHARDCODE";
        //rblComplianceLevel.DataTextField = "FLDSHORTNAME";
        //rblComplianceLevel.DataBind();
    }

    protected void rblFindings_Changed(object sender, EventArgs e)
    {
        lblContactTypeId.Text = "";
        DataTable dt = PhoenixInspectionIncident.EditIncidentFindings(General.GetNullableGuid(rblFindings.SelectedValue));
        if (dt.Rows.Count > 0)
            lblContactTypeId.Text = dt.Rows[0]["FLDCONTACTTYPEID"].ToString();
        BindIC();
    }

    protected void rblContactType_Changed(object sender, EventArgs e)
    {
        BindIC();
    }

    protected void rblIC_Changed(object sender, EventArgs e)
    {
        BindBC();
    }

    protected void rblBC_Changed(object sender, EventArgs e)
    {
        BindCAN();
    }

    private bool IsValidAdd()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableGuid(rblFindings.SelectedValue) == null)
            ucError.ErrorMessage = "Findings is required.";

        if (General.GetNullableGuid(lblContactTypeId.Text) == null)
            ucError.ErrorMessage = "Contact Type is not mapped for the finding in incident details screen.";

        if (General.GetNullableGuid(rblIC.SelectedValue) == null)
            ucError.ErrorMessage = "Immediate cause is required.";

        //if (General.GetNullableGuid(rblBC.SelectedValue) == null)
        //    ucError.ErrorMessage = "Basic cause is required.";

        //if (General.GetNullableGuid(rblCAN.SelectedValue) == null)
        //    ucError.ErrorMessage = "Conrol action need is required.";        

        return (!ucError.IsError);
    }
}
