using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionDirectNonConformityMSCATAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["REVIEWDNC"] != null && Request.QueryString["REVIEWDNC"].ToString() != string.Empty)
            {
                ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"].ToString();
                EditNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
            }
            BindCAR();
            ucCategory.bind();
            BindIC(null, null);
            txtBCRemarks.Focus();
            txtICRemarks.Focus();
        }
    }

    protected void BindCAR()
    {
        DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));
        chkCAR.DataSource = ds;
        chkCAR.DataTextField = "FLDDEFICIENCYDETAILS";
        chkCAR.DataValueField = "FLDINSPECTIONCORRECTIVEACTIONID";
        chkCAR.DataBind();
    }

    protected void EditNonConformity(Guid reviewnonconformity)
    {
        DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(reviewnonconformity);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }
    }

    protected void MenuMSCAT_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["REVIEWDNC"] == null || ViewState["REVIEWDNC"].ToString() == string.Empty)
            {
                ucError.ErrorMessage = "Please record Non Conformity details before starting M-SCAT Analysis.";
                ucError.Visible = true;
                return;
            }
            if (!IsValidAdd())
            {
                ucError.Visible = true;
                return;
            }
            EditNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));

            for (int i=0; i < chkCAR.Items.Count; i++)
            {
                if (chkCAR.Items[i].Selected)
                {
                    Guid? newinsertedid = null;
                    //Guid? newinsertedid1 = null;
                    Guid? newinsertedid2 = null;
                    Guid? newinsertedid3 = null;

                    //contact type
                    //PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(ViewState["REVIEWDNC"].ToString()),
                    //    new Guid(rblContactType.SelectedValue), null, int.Parse(ViewState["VESSELID"].ToString()), 1, null
                    //    , ref newinsertedid, General.GetNullableInteger(ucAccidentDescription.SelectedHard), null);

                    //immediate cause
                    PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(ViewState["REVIEWDNC"].ToString()),
                        new Guid(rblIC.SelectedValue), null, int.Parse(ViewState["VESSELID"].ToString()), 2, null,
                        ref newinsertedid, General.GetNullableInteger(ucCategory.SelectedHard), General.GetNullableGuid(chkCAR.Items[i].Value),
                        General.GetNullableString(txtICRemarks.Text), null);

                    //basic cause
                    if (General.GetNullableGuid(rblBC.SelectedValue) != null)
                    {
                        PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(ViewState["REVIEWDNC"].ToString()),
                            new Guid(rblBC.SelectedValue), General.GetNullableGuid(newinsertedid.ToString()),
                            int.Parse(ViewState["VESSELID"].ToString()), 3, null, ref newinsertedid2, null, General.GetNullableGuid(chkCAR.Items[i].Value),
                            null, General.GetNullableString(txtBCRemarks.Text));
                    }

                    //CAN
                    if (General.GetNullableGuid(rblCAN.SelectedValue) != null)
                    {
                        PhoenixInspectionIncidentMSCAT.MSCATItemInsert(new Guid(ViewState["REVIEWDNC"].ToString()),
                            new Guid(rblCAN.SelectedValue), General.GetNullableGuid(newinsertedid.ToString()),
                            int.Parse(ViewState["VESSELID"].ToString()), 4, null, ref newinsertedid3, null, General.GetNullableGuid(chkCAR.Items[i].Value), null, null);
                    }
                }
            }

            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            //Response.Redirect("../Inspection/InspectionIncidentMSCAT.aspx", true);
        }
    }

    protected void BindIC(object sender, EventArgs e)
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

    protected void BindComplianceLevel()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(1, 207);
        //rblComplianceLevel.DataSource = ds;
        //rblComplianceLevel.DataValueField = "FLDHARDCODE";
        //rblComplianceLevel.DataTextField = "FLDSHORTNAME";
        //rblComplianceLevel.DataBind();
    }

    //protected void rblContactType_Changed(object sender, EventArgs e)
    //{
    //    BindIC();
    //}

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

        if (General.GetNullableGuid(chkCAR.SelectedValue) == null)
            ucError.ErrorMessage = "Please select atleast one CAR.";

        if (General.GetNullableGuid(rblIC.SelectedValue) == null)
            ucError.ErrorMessage = "Immediate cause is required.";

        //if (General.GetNullableGuid(rblBC.SelectedValue) == null)
        //    ucError.ErrorMessage = "Basic cause is required.";

        //if (General.GetNullableGuid(rblCAN.SelectedValue) == null)
        //    ucError.ErrorMessage = "Conrol action need is required.";

        return (!ucError.IsError);
    }
}
