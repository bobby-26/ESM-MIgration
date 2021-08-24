using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVProcedureDetailsF2 : PhoenixBasePage
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
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL");
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL");
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
                MenuProcedureDetailList.Visible = false;

            if (Request.QueryString["view"] == null)
            {
                PhoenixToolbar toolbartab = new PhoenixToolbar();
                toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
                TabProcedure.AccessRights = this.ViewState;
                TabProcedure.MenuList = toolbartab.Show();
            }
            else
            {
                txtDesc.Enabled = false;
            }
            if (!IsPostBack)
            {
                ViewState["ID"] = null;

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
                PhoenixVesselPositionEUMRVProcedureDetailsF2.InsertEUMRVProcedureDetailsF2(Request.QueryString["Table"].ToString(), new Guid(Request.QueryString["ProcedureId"].ToString()), txtDesc.Text.Trim(), General.GetNullableGuid(ViewState["ID"] == null ? null : ViewState["ID"].ToString()));
                ucstatus.Text = "updated successfully.";
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
        DataSet ds = PhoenixVesselPositionEUMRVProcedureDetailsF2.ListEUMRVProcedureDetailsEdit(Request.QueryString["Table"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtDesc.Text= ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
            ViewState["ID"] = ds.Tables[0].Rows[0]["FLDID"].ToString();
        }

    }


}
