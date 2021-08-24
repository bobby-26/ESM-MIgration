using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionMSCATEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            ViewState["MSCATID"] = Request.QueryString["mscatid"].ToString();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            BindMenu();
            MenuMSCAT.AccessRights = this.ViewState;
            MenuMSCAT.MenuList = toolbar.Show();
            
            if (Request.QueryString["DEFICIENCYID"] != null && Request.QueryString["DEFICIENCYID"].ToString() != string.Empty)
            {
                ViewState["DEFICIENCYID"] = Request.QueryString["DEFICIENCYID"].ToString();
            }
            BindData();
            txtBCRemarks.Focus();
            txtICRemarks.Focus();
        }
    }
    protected void BindMenu()
    {
        DataSet ds = PhoenixInspectionIncidentMSCAT.EditMSCATItem(new Guid(ViewState["MSCATID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            MenuMSCAT.Title = "Edit - " + dr["FLDIMMEDIATECAUSE"].ToString() + "/" + dr["FLDBASICSUBCAUSE"].ToString();
        }
    }
    protected void BindData()
    {
        DataSet ds = PhoenixInspectionIncidentMSCAT.EditMSCATItem(new Guid(ViewState["MSCATID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucCategory.DataBind();
            ucCategory.SelectedHard = dr["FLDACCIDENTDESC"].ToString();
            BindCAR();
            if (rblCAR.Items.FindByValue(dr["FLDINCIDENTFINDINGID"].ToString()) != null)
            {
                rblCAR.SelectedValue = dr["FLDINCIDENTFINDINGID"].ToString();
                rblCAR.Items.FindByValue(dr["FLDINCIDENTFINDINGID"].ToString()).Attributes.Add("style", "font-weight:bold;");
            }
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
            txtICRemarks.Text = dr["FLDICREMARKS"].ToString();
            txtBCRemarks.Text = dr["FLDBCREMARKS"].ToString();
            //BindComplianceLevel();
            //rblComplianceLevel.SelectedValue = dr["FLDCOMPLIANCELEVEL"].ToString();
        }
    }

    protected void BindCAR()
    {
        DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString()));
        rblCAR.DataSource = ds;
        rblCAR.DataTextField = "FLDDEFICIENCYDETAILS";
        rblCAR.DataValueField = "FLDINSPECTIONCORRECTIVEACTIONID";
        rblCAR.DataBind();
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

            PhoenixInspectionIncidentMSCAT.MSCATItemUpdate(new Guid(ViewState["MSCATID"].ToString())
                            , new Guid(rblIC.SelectedValue)
                            , General.GetNullableGuid(rblBC.SelectedValue), General.GetNullableGuid(rblCAN.SelectedValue), null
                            , int.Parse(ucCategory.SelectedHard)
                            , General.GetNullableString(txtICRemarks.Text)
                            , General.GetNullableString(txtBCRemarks.Text)
                            , General.GetNullableGuid(rblCAR.SelectedValue)
                            );

            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }    

    protected void BindIC()
    {
        DataSet ds = PhoenixInspectionImmediateCause.ListImmediateCause(null);
        rblIC.DataSource = ds;
        rblIC.DataValueField = "FLDIMMEDIATECAUSEID";
        rblIC.DataTextField = "FLDIMMEDIATECAUSE";
        rblIC.DataBind();
        BindBC();
    }

    protected void BindBC()
    {
        if (General.GetNullableGuid(rblIC.SelectedValue) != null)
        {
            DataSet dsIC = PhoenixInspectionImmediateCause.EditImmediateCause(General.GetNullableGuid(rblIC.SelectedValue));
            if (dsIC.Tables[0].Rows.Count > 0)
            {
                ucCategory.SelectedHard = dsIC.Tables[0].Rows[0]["FLDCATEGORY"].ToString();
            }
        }
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

    protected void ucCategory_Changed(object sender, EventArgs e)
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

        if (General.GetNullableGuid(rblCAR.SelectedValue) == null)
            ucError.ErrorMessage = "CAR is required.";

        if (General.GetNullableGuid(rblIC.SelectedValue) == null)
            ucError.ErrorMessage = "Immediate cause is required.";

        //if (General.GetNullableGuid(rblBC.SelectedValue) == null)
        //    ucError.ErrorMessage = "Basic cause is required.";

        //if (General.GetNullableGuid(rblCAN.SelectedValue) == null)
        //    ucError.ErrorMessage = "Conrol action need is required.";

        return (!ucError.IsError);
    }
}
