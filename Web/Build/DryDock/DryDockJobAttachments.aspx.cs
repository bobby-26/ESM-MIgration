using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;

public partial class DryDockJobAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Attachment", "ATTACHMENTS");

            toolbar.AddButton("Details", "DETAIL");
            

            MenuAttachment.MenuList = toolbar.Show();
           // MenuAttachment.SetTrigger(pnlInvoice);

            if (Request.QueryString["REPAIRJOBID"] != null)
            {
                ViewState["REPAIRJOBID"] = Request.QueryString["REPAIRJOBID"].ToString();
                 ViewState["ATTCHMENTTYPE"] = "";
            }

            if (Request.QueryString["ADHOCJOBID"] != null)
            {
                ViewState["ADHOCJOBID"] = Request.QueryString["ADHOCJOBID"].ToString();
                ViewState["ATTCHMENTTYPE"] = "";
            }

            if (Request.QueryString["DTKEY"] != null)
            {
                ViewState["DTKey"] = Request.QueryString["DTKEY"].ToString();
            }

            //ttlAttachment.Text = "Attachments";
            MenuAttachment.SelectedMenuIndex = 0;


            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&BYVESSEL=1";
        }
       
       
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            if (Request.QueryString["REPAIRJOBID"] != null)
                Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + ViewState["REPAIRJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            if (Request.QueryString["STANDARDJOBID"] != null)
                Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + Request.QueryString["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            if (Request.QueryString["ADHOCJOBID"] != null)
                Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + ViewState["ADHOCJOBID"].ToString() , false);

        }
    }
 
}
