using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLicenceRequestListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        LicenceRequestFilter.AccessRights = this.ViewState;
        LicenceRequestFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {  

        }
    }

    protected void LicenceRequestFilter_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("txtReqNo", txtReqNo.Text.Trim());
            criteria.Add("ddlFlag", ddlFlag.SelectedFlag);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("txtName", txtName.Text.Trim());
            criteria.Add("txtFileno", txtFileno.Text.Trim());
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            Filter.CurrentCrewLicenceRequestFilter = criteria;
            
        }

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

    }

  }