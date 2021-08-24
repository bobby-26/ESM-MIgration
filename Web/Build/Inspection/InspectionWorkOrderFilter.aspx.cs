using System;
using System.Collections.Specialized ;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;


public partial class InspectionWorkOrderFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuWorkOrderScheduleFilter.AccessRights = this.ViewState;
            MenuWorkOrderScheduleFilter.MenuList = toolbar.Show();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");

            }
 
        }
    }
    protected void MenuWorkOrderScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtWorkOrderNo", txtWorkOrderNo.Text);
            criteria.Add("ucDepartment", ucDepartment.SelectedDepartment);
            criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            criteria.Add("ddlStatus", ddlAcceptance.SelectedValue);
            Filter.CurrentOfficeWorkOrderFilter = criteria;

            //Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderList.aspx?CalledFrom=WO", false);
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }




}
