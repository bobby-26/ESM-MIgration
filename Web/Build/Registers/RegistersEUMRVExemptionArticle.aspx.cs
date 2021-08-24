using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVExemptionArticle : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
                MenuProcedureDetailList.Visible = false;
            if (Request.QueryString["vesselid"] != null)
            {
                ddlVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                ddlVessel.Enabled = false;
            }
            if (Request.QueryString["view"] == null)
            {
                PhoenixToolbar toolbartab = new PhoenixToolbar();
                toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
                TabProcedure.AccessRights = this.ViewState;
                TabProcedure.MenuList = toolbartab.Show();
            }
            else
            {
                txtexpectedvoyage.Enabled = false;
                ddlexpectedvoyagenot.Enabled = false;
                ddlconsumed.Enabled = false;
                ddlConditions.Enabled = false;
            }
            if (!IsPostBack)
            {
                if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.bind();
                    ddlVessel.DataBind();
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                ProcedureDetailEdit();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["DTKEY"] != null && General.GetNullableGuid(ViewState["DTKEY"].ToString())!=null)
                {
                    PhoenixRegistersEUMRVMesurementinstrument.EUMRVExemptionArticleUpdate(General.GetNullableInteger(ddlVessel.SelectedVessel),
                                                                            General.GetNullableString(txtexpectedvoyage.Text.Trim()),
                                                                            General.GetNullableString(ddlexpectedvoyagenot.SelectedValue == "Dummy" ? null : ddlexpectedvoyagenot.SelectedValue),
                                                                            General.GetNullableString(ddlConditions.SelectedValue == "Dummy" ? null : ddlConditions.SelectedValue),
                                                                            General.GetNullableString(ddlconsumed.SelectedValue == "Dummy" ? null : ddlconsumed.SelectedValue),
                                                                            new Guid(ViewState["DTKEY"].ToString())
                                                                           );
                    
                    ucStatus.Text = "Procedure saved successfully.";
                }
                else
                {
                    PhoenixRegistersEUMRVMesurementinstrument.EUMRVExemptionArticleInsert(General.GetNullableInteger(ddlVessel.SelectedVessel),
                                                                            General.GetNullableString(txtexpectedvoyage.Text.Trim()),
                                                                            General.GetNullableString(ddlexpectedvoyagenot.SelectedValue == "Dummy" ? null : ddlexpectedvoyagenot.SelectedValue),
                                                                            General.GetNullableString(ddlConditions.SelectedValue == "Dummy" ? null : ddlConditions.SelectedValue),
                                                                            General.GetNullableString(ddlconsumed.SelectedValue == "Dummy" ? null : ddlconsumed.SelectedValue));
                    ucStatus.Text = "Procedure saved successfully.";
                }
                ProcedureDetailEdit(); 
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ProcedureDetailEdit()
    {
        int vesselid = General.GetNullableInteger(ddlVessel.SelectedVessel) != null ? int.Parse(ddlVessel.SelectedVessel) : 0;
        DataTable dt = PhoenixRegistersEUMRVMesurementinstrument.ListEUMRVExemptionArticle(General.GetNullableInteger(Request.QueryString["vesselid"] == null ? vesselid.ToString() : Request.QueryString["vesselid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtexpectedvoyage.Text = dt.Rows[0]["FLDMINEXPECTEDVOYAGEFALLING"].ToString();
            ddlexpectedvoyagenot.SelectedValue = dt.Rows[0]["FLDMINEXPECTEDVOYAGEFALLINGNOT"].ToString();
            ddlConditions.SelectedValue = dt.Rows[0]["FLDCONDITIONS"].ToString();
            ddlconsumed.SelectedValue = dt.Rows[0]["FLDFUELCONSUMED"].ToString();
            ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();

            if(dt.Rows[0]["FLDCONDITIONS"].ToString().ToUpper().Equals("NO"))
            {
                ddlconsumed.SelectedValue = "NA";
                ddlconsumed.Enabled = false;
            }
            else
            {
                ddlconsumed.SelectedValue = dt.Rows[0]["FLDFUELCONSUMED"].ToString();
                ddlconsumed.Enabled = true;
            }

        }
        else
        {
            txtexpectedvoyage.Text = "";
            ddlexpectedvoyagenot.SelectedValue = "Dummy";
            ddlConditions.SelectedValue = "Dummy";
            ddlconsumed.SelectedValue = "Dummy";
            ViewState["DTKEY"] = "";
        }
        //if (Request.QueryString["Lanchfrom"].ToString() == "0")
        //    ucTitle.Text = "EUMRV Procedure ";
        //if (Request.QueryString["Lanchfrom"].ToString() == "1")
        //    ucTitle.Text = "Ship Specific Procedure ";


    }
    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        ProcedureDetailEdit();
    }
    protected void ddlConditions_TextChanged(object sender,EventArgs e)
    {
        if(ddlConditions.SelectedValue == "No")
        {
            ddlconsumed.SelectedValue = "NA";
            ddlconsumed.Enabled = false;
        }
        else
        {
            ddlconsumed.SelectedValue = "Dummy";
            ddlconsumed.Enabled = true;
        }
    }


}
