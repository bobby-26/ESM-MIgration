using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceWODocking : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWOWorkRequest.AccessRights = this.ViewState;
        MenuWOWorkRequest.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["VESSELID"] = "0";
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ViewState["WORKORDERID"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["woid"]))
                ViewState["WORKORDERID"] = Request.QueryString["woid"].ToString();

            ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            DataSet ds = PhoenixRegistersQuick.GetQuickCode(((int)(PhoenixQuickTypeCode.MAINTCLASS)), "DOK");
            ViewState["DOK"] = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["DOK"] = ds.Tables[0].Rows[0]["FLDQUICKCODE"].ToString();
            }
            BindCheckBox();
            BindFields();
        }
    }
    private void BindFields()
    {
        DataTable dt = PhoenixPlannedMaintenanceWorkOrder.EditWorkOrderJob(new Guid(ViewState["WORKORDERID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
        for(int i = 0;i<dt.Rows.Count;i++)
        {
            DataRow dr = dt.Rows[i];
            txtJobNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
            txtJobName.Text = dr["FLDWORKORDERNAME"].ToString();
            BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
            BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
            BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
            BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
            ucMaintClass.SelectedQuick = ViewState["DOK"].ToString();
            if (ViewState["DOK"].ToString() != string.Empty)
                ucMaintClass.SelectedText = "DOCKING";
            ucMaintClass.Enabled = false;
            ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
            ddlJobType.SelectedValue = dr["FLDJOBTYPE"].ToString();
        }
    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = String.Join(",", cbl.SelectedValues);
       
        list = list.TrimEnd(',');
        return list;
    }
    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {        
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {                                
                cbl.SelectedValue = item;
            }
        }
    }
    protected void MenuWOWorkRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strMat = ReadCheckBoxList(cblMaterial);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude); 
                PhoenixPlannedMaintenanceWorkOrder.UpdateWorkOrderJob(new Guid(ViewState["WORKORDERID"].ToString()),
                         int.Parse(ViewState["VESSELID"].ToString()),
                         General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick),                         
                         strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlJobType.SelectedValue));
            }
                       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SaveDefect()
    {
        

    }
    
    protected void BindCheckBox()
    {
        cblWorkSurvey.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(2);
        cblWorkSurvey.DataBindings.DataTextField = "FLDNAME";
        cblWorkSurvey.DataBindings.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        cblMaterial.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(1);
        cblMaterial.DataBindings.DataTextField = "FLDNAME";
        cblMaterial.DataBindings.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

        cblEnclosed.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(4);
        cblEnclosed.DataBindings.DataTextField = "FLDNAME";
        cblEnclosed.DataBindings.DataValueField = "FLDMULTISPECID";
        cblEnclosed.DataBind();

        cblInclude.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(3);
        cblInclude.DataBindings.DataTextField = "FLDNAME";
        cblInclude.DataBindings.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();

        ddlJobType.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();
    }
    
    public static void RadBindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }
}