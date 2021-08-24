using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Telerik.Web.UI;
public partial class CrewVesselPosition : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["TABNAME"] = "";

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Arrival", "ARRIVAL");
                toolbarmain.AddButton("Noon Report", "NOONREPORT");
                MenuVesselPosition.AccessRights = this.ViewState;
                MenuVesselPosition.MenuList = toolbarmain.Show();

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselPosition_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (General.GetNullableString(ddlVessel.SelectedVessel) == null)
                return;

            if (dce.CommandName.ToUpper().Equals("ARRIVAL"))
            {
                ViewState["TABNAME"] = "ARRIVAL";
                ifVslPosition.Attributes["Src"] = "../Crew/CrewVesselPositionArrival.aspx?vesselid=" + ddlVessel.SelectedVessel;
            }
            if (dce.CommandName.ToUpper().Equals("NOONREPORT"))
            {
                ViewState["TABNAME"] = "NOONREPORT";
                ifVslPosition.Attributes["Src"] = "../Crew/CrewVesselPositionNoonReportList.aspx?vesselid=" + ddlVessel.SelectedVessel;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData(object sender, EventArgs e)
    {
        BindData();
    }
    protected void BindData()
    {   
        try
        {
            if (General.GetNullableString(ddlVessel.SelectedVessel) == null)
                return;

            if (General.GetNullableString(ddlVessel.SelectedVessel) != null)
            {
                ifVslPosition.Attributes["Src"] = "../Crew/CrewVesselPositionArrival.aspx?vesselid=" + ddlVessel.SelectedVessel;
            }

            if ((ViewState["TABNAME"].ToString()) == "ARRIVAL")
            {
                ifVslPosition.Attributes["Src"] = "../Crew/CrewVesselPositionArrival.aspx?vesselid=" + ddlVessel.SelectedVessel;
            }

            if ((ViewState["TABNAME"].ToString()) == "NOONREPORT")
            {
                ifVslPosition.Attributes["Src"] = "../Crew/CrewVesselPositionNoonReportList.aspx?vesselid=" + ddlVessel.SelectedVessel;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

}
