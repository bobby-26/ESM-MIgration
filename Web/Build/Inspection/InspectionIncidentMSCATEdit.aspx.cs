using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionIncidentMSCATEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();
        if (!IsPostBack)
        {            
            ViewState["MSCATID"] = Request.QueryString["mscatid"].ToString();
            toolbar = new PhoenixToolbar();            
            BindMenu();
            BindData();
            txtBCRemarks.Focus();
            txtICRemarks.Focus();            
        }
    }
    protected void BindMenu()
    {
        DataSet ds = PhoenixInspectionIncidentMSCAT.EditIncidentMSCATItem(new Guid(ViewState["MSCATID"].ToString()));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    MenuMSCAT.Title = "Edit - " + dr["FLDFINDINGS"].ToString() + "/" + dr["FLDIMMEDIATECAUSE"].ToString();
        //}
    }
    protected void BindData()
    {
        DataSet ds = PhoenixInspectionIncidentMSCAT.EditIncidentMSCATItem(new Guid(ViewState["MSCATID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucAccidentDescription.DataBind();
            ucAccidentDescription.SelectedHard = dr["FLDACCIDENTDESC"].ToString();            
            BindTC();
            lblContactTypeId.Text = dr["FLDTCID"].ToString();
            BindIC();
            rblIC.SelectedValue = dr["FLDICID"].ToString();
            rblIC.Items.FindByValue(dr["FLDICID"].ToString()).Attributes.Add("style", "font-weight:bold;");
            BindBC();
            if (rblBC.Items.FindByValue(dr["FLDBCID"].ToString()) != null)
            {
                rblBC.SelectedValue = dr["FLDBCID"].ToString();
                rblBC.Items.FindByValue(dr["FLDBCID"].ToString()).Attributes.Add("style", "font-weight:bold;");
            }
            BindCAN();
            if (rblCAN.Items.FindByValue(dr["FLDCANID"].ToString()) != null)
            {
                rblCAN.SelectedValue = dr["FLDCANID"].ToString();
                rblCAN.Items.FindByValue(dr["FLDCANID"].ToString()).Attributes.Add("style", "font-weight:bold;");
            }
            BindFindings();
            rblFindings.SelectedValue = dr["FLDINCIDENTFINDINGID"].ToString();
            if(rblFindings.Items.FindByValue(dr["FLDINCIDENTFINDINGID"].ToString()) != null)
                rblFindings.Items.FindByValue(dr["FLDINCIDENTFINDINGID"].ToString()).Attributes.Add("style", "font-weight:bold;");
            txtICRemarks.Text = dr["FLDICREMARKS"].ToString();
            txtBCRemarks.Text = dr["FLDBCREMARKS"].ToString();
            //BindComplianceLevel();
            //rblComplianceLevel.SelectedValue = dr["FLDCOMPLIANCELEVEL"].ToString();
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

            PhoenixInspectionIncidentMSCAT.IncidentMSCATItemUpdate(new Guid(ViewState["MSCATID"].ToString())
                            , new Guid(lblContactTypeId.Text), new Guid(rblIC.SelectedValue)
                            , General.GetNullableGuid(rblBC.SelectedValue), General.GetNullableGuid(rblCAN.SelectedValue), null
                            , null
                            , General.GetNullableGuid(rblFindings.SelectedValue)
                            , General.GetNullableString(txtICRemarks.Text)
                            , General.GetNullableString(txtBCRemarks.Text));

            ucStatus.Text = "Information Updated";
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

    protected void BindTC()
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

        foreach (ListItem li in rblBC.Items)
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

    protected void ucAccidentDescription_Changed(object sender, EventArgs e)
    {
        BindTC();
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
