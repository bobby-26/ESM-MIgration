using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionLongTermActionWorkOrderAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();

                toolbar.AddButton("Work Order", "WORKORDER");
                toolbar.AddButton("Attachments", "ATTACHEMENTS");

                MenuInspectionGeneral.AccessRights = this.ViewState;
                MenuInspectionGeneral.MenuList = toolbar.Show();
                MenuInspectionGeneral.SelectedMenuIndex = 1;

                //ViewState["LONGTERMACTIONID"] + "&DTKEY=
                if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != null)
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];

                if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                    ViewState["DTKEY"] = Request.QueryString["DTKEY"];

                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.QUALITY + "&U=0";
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

    }

    protected void InspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("WORKORDER"))
        {
            Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&DTKEY=" + ViewState["DTKEY"]);
        }
        else if (dce.CommandName.ToUpper().Equals("ATTACHEMENTS"))
        {
            Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderAttachment.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&DTKEY=" + ViewState["DTKEY"]);
        }

    }
}
