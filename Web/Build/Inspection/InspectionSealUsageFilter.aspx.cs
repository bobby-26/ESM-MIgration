using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionSealUsageFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuSealFilterMain.AccessRights = this.ViewState;
            MenuSealFilterMain.MenuList = toolbar.Show();
            BindLocation();
            ucSealType.Focus();            
        }
    }

    protected void BindLocation()
    {
        ddlLocation.DataSource = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlLocation.DataTextField = "FLDLOCATIONNAME";
        ddlLocation.DataValueField = "FLDLOCATIONID";
        ddlLocation.DataBind();
        //ddlLocation.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void MenuSealFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("sealtype", ucSealType.SelectedQuick);
            criteria.Add("location", ddlLocation.SelectedValue);
            criteria.Add("sealno", txtSealNumber.Text);
            criteria.Add("sealaffixedby", txtSealAffixedby.Text);
            criteria.Add("affixedfrom", txtFromDate.Text);
            criteria.Add("affixedto", txtToDate.Text);
            criteria.Add("reason", ucReason.SelectedQuick);
            Filter.CurrentSealUsageFilter = criteria;
            Session["FILTER"] = "1";        
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }   
}
