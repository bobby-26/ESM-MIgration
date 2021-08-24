using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections;
using System.Collections.Specialized;

public partial class InspectionLongTermWorkOrderDescription : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SAVE", "SAVE");
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            MenuWOGeneration.AccessRights = this.ViewState;
            MenuWOGeneration.MenuList = toolbarmain.Show();

            
        }
    }

    protected void MenuWOGeneration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (General.GetNullableString(txtWODescription.Text) == null)
            {
                lblMessage.Text = "Work Order is required.";
                return;
            }

            string Script = "";


                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    string selectedagents = ",";
                    if (Filter.CurrentSelectedOfficeTasks != null)
                    {
                        ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
                        if (SelectedPvs != null && SelectedPvs.Count > 0)
                        {
                            foreach (Guid index in SelectedPvs)
                            {
                                selectedagents = selectedagents + index + ",";
                            }
                            PhoenixInspectionLongTermAction.WorkOrderInsert(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , null
                                , selectedagents
                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                , txtWODescription.Text);

                            if (Filter.CurrentSelectedOfficeTasks != null)
                                Filter.CurrentSelectedOfficeTasks = null;

                            //ucStatus.Text = "Work Order is generated.";

                            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                            Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                            Script += "</script>" + "\n";
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                        }
                    }
                }
            
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}

