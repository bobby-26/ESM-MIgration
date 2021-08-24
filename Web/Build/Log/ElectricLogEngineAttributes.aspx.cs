using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Elog;
using Telerik.Web.UI;

public partial class ElectricLogEngineAttributes : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["ACTIVEYN"] = "0";
            BindCategory();
            BindRevDetails();
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (ViewState["ACTIVEYN"].ToString() == "1")
        {
            toolbarmain.AddButton("Revise", "REVISE", ToolBarDirection.Right);
            MenuOperational.AccessRights = this.ViewState;
            MenuOperational.MenuList = toolbarmain.Show();
        }
        if (ViewState["ACTIVEYN"].ToString() != "1")
        {
            toolbarmain.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuOperational.AccessRights = this.ViewState;
            MenuOperational.MenuList = toolbarmain.Show();
        }

        cmdRevision.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Log/ElectricLogEngineAttributeHistory.aspx?TypeId=" + ddlType.SelectedValue + "&VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID+"');return true;");
    }

    protected void gvOperationalHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void gvtank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindTank();
    }

    protected void BindCategory()
    {
       DataSet ds = new DataSet();

        ds = PhoenixEngineLogAttributes.EngineLogAttributesTypeList();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlType.Items.Clear();
            ddlType.DataSource = ds.Tables[0];
            ddlType.DataBind();
        }
    }
    private void BindGrid()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesList(General.GetNullableInteger(ddlType.SelectedValue),General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),0);
        gvOperationalHazard.DataSource = ds;
    }

    private void BindTank()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesList(General.GetNullableInteger(ddlType.SelectedValue), General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),1);
        gvtank.DataSource = ds;
    }

    private void BindRevDetails()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesList(General.GetNullableInteger(ddlType.SelectedValue), General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),1);
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtrevno.Text = ds.Tables[1].Rows[0]["FLDREVISIONNO"].ToString();
            ucdate.Text = ds.Tables[1].Rows[0]["FLDREVISEDDATE"].ToString();
            ViewState["ACTIVEYN"] = ds.Tables[1].Rows[0]["FLDACTIVEYN"].ToString();
            if (ds.Tables[1].Rows[0]["FLDACTIVEYN"].ToString().Equals("1"))
                ChkPublishedYN.Checked = true;
            else
                ChkPublishedYN.Checked = false;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (ViewState["ACTIVEYN"].ToString() == "1")
            {
                toolbarmain.AddButton("Revise", "REVISE", ToolBarDirection.Right);
                MenuOperational.AccessRights = this.ViewState;
                MenuOperational.MenuList = toolbarmain.Show();
            }
            if (ViewState["ACTIVEYN"].ToString() != "1")
            {
                toolbarmain.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuOperational.AccessRights = this.ViewState;
                MenuOperational.MenuList = toolbarmain.Show();
            }
        }
    }

    protected void Operational_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1','ifMoreInfo');";
                //Script += "</script>" + "\n";

                if (General.GetNullableInteger(ddlType.SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Type is required";
                    ucError.Visible = true;
                    return;
                }

                foreach (GridDataItem gvr in gvOperationalHazard.Items)
                {

                    PhoenixEngineLogAttributes.EngineLogAttributesInsert(General.GetNullableInteger(ddlType.SelectedValue)
                                                      , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableInteger(((RadLabel)gvr.FindControl("lblParameterid")).Text)
                                                      , General.GetNullableInteger(((UserControlMaskNumber)gvr.FindControl("txtcount")).Text));

                }

                foreach (GridDataItem gvr in gvtank.Items)
                {

                    PhoenixEngineLogAttributes.EngineLogAttributesInsert(General.GetNullableInteger(ddlType.SelectedValue)
                                                      , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableInteger(((RadLabel)gvr.FindControl("lblParameterid")).Text)
                                                      , General.GetNullableInteger(((UserControlMaskNumber)gvr.FindControl("txtcount")).Text));

                }

                gvOperationalHazard.Rebind();
                gvtank.Rebind();
                BindRevDetails();
                ucStatus.Text = "Informantion Updated";
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }

            if (CommandName.ToUpper().Equals("REVISE"))
            {
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1','ifMoreInfo');";
                //Script += "</script>" + "\n";

                if (General.GetNullableInteger(ddlType.SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Type is required";
                    ucError.Visible = true;
                    return;
                }

                    PhoenixEngineLogAttributes.EngineLogAttributesRevise(General.GetNullableInteger(ddlType.SelectedValue)
                                                      , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

                gvOperationalHazard.Rebind();
                BindRevDetails();
                ucStatus.Text = "Informantion Updated";
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }

            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1','ifMoreInfo');";
                //Script += "</script>" + "\n";

                if (General.GetNullableInteger(ddlType.SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Type is required";
                    ucError.Visible = true;
                    return;
                }
                    PhoenixEngineLogAttributes.EngineLogAttributesSubmit(General.GetNullableInteger(ddlType.SelectedValue)
                                                      , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

                gvOperationalHazard.Rebind();
                BindRevDetails();
                ucStatus.Text = "Informantion Updated";
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlType_TextChanged(object sender, EventArgs e)
    {
        Response.Redirect("../Log/ElectronicLogEngineParameterConfig.aspx");
        gvOperationalHazard.Rebind();
        gvtank.Rebind();
        BindRevDetails();
    }
}