using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionLongTermActionWorkOrderComplete : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "UPDATE");
        MenuReport.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["WORKORDERID"] = "";
            if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != string.Empty)
            {
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
            }
            EditWorkOrder();
        }
    }
    private void EditWorkOrder()
    {
        if (ViewState["WORKORDERID"] != null && ViewState["WORKORDERID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionLongTermAction.WorkOrderEdit(new Guid(ViewState["WORKORDERID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtWorkOrderNumer.Text = dr["FLDWONUMBER"].ToString();
                txtCompletionDate.Text = dr["FLDCOMPLETEDDATE"].ToString();
                lblActionTaken.Text = dr["FLDACTIONTAKEN"].ToString();
                lblWODescription.Text = dr["FLDWODESCRIPTION"].ToString();
                lblWOStatus.Text = dr["FLDSTATUS"].ToString();
            }
        }
    }

    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (!IsValidComplete())
            {
                lblMessage.Visible = true;
                return;
            }
         
            if (ViewState["WORKORDERID"] != null && ViewState["WORKORDERID"].ToString() != string.Empty)
            {
                string selectedagents = ",";
                if (dce.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (Filter.CurrentSelectedWOTasks != null)
                    {
                        ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;
                        if (SelectedPvs != null && SelectedPvs.Count > 0)
                        {
                            foreach (Guid index in SelectedPvs)
                            {
                                selectedagents = selectedagents + index + ",";
                            }

                            PhoenixInspectionLongTermAction.WorkOrderStatusUpdate(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , DateTime.Parse(txtCompletionDate.Text)
                            , lblActionTaken .Text 
                            , new Guid(ViewState["WORKORDERID"].ToString())
                            , 1
                            , selectedagents);

                            if (Filter.CurrentSelectedWOTasks != null)
                                Filter.CurrentSelectedWOTasks = null;

                            ucStatus.Text = "Information Updated.";


                        }
                        EditWorkOrder();
                        //String script = String.Format("javascript:parent.fnReloadList('codehelp1');");
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, null);", true);

                        //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        //Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                        //Script += "</script>" + "\n";
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                        lblMessage.Text = "";

                    }
                }
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    private bool IsValidComplete()
    {
        lblMessage.Text = "Please provide the following required information." + "<br/>";
        bool result = true;

        if (Filter.CurrentSelectedWOTasks == null)
        {
            if (General.GetNullableString(lblWOStatus.Text).ToUpper().Equals("2"))
            {
                lblMessage.Text = "Tasks Are Already Completed." + "<br/>";
                result = false;
            }
            else
            {
                lblMessage.Text += "Select the task to complete." + "<br/>";
                result = false;
            }
            
        }
        if (General.GetNullableString(lblActionTaken.Text) == null)
        {
            lblMessage.Text += "Action Taken is required." + "<br/>";
            result = false;
            
        }
        

        if (General.GetNullableDateTime(txtCompletionDate.Text) == null)
        {
            lblMessage.Text += "Completion Date is required." + "<br/>";
            result = false;
           
        }
        else if (General.GetNullableDateTime(txtCompletionDate.Text) > DateTime.Today)
        {
            lblMessage.Text += "Completion Date cannot be the future date." + "<br/>";
            result = false;
            //EditWorkOrder();           
        }
        return result;
    }
}
