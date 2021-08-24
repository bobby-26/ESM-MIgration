using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;


public partial class DryDockSuptRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["orderid"] != null)
        {
            toolbar.AddButton("Project", "PROJECT");
            toolbar.AddButton("Quotation", "QUOTATION");
            toolbar.AddButton("Supt Job Remarks", "JOBREMARKS");
            toolbar.AddButton("Supt Feedback", "SUPTFEEDBACK");



        }
        else
        {
            toolbar.AddButton("Supt Remarks", "SUPTREMARKS", ToolBarDirection.Right);
            toolbar.AddButton("Details", "DETAIL",ToolBarDirection.Right);
            
        }               
        MenuSuptRemark.AccessRights = this.ViewState;
        MenuSuptRemark.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuJobDetail.AccessRights = this.ViewState;
        MenuJobDetail.MenuList = toolbar.Show();
        if (Request.QueryString["orderid"] != null)
        {
            MenuSuptRemark.SelectedMenuIndex = 3;
        }
        else
        {
            MenuSuptRemark.SelectedMenuIndex = 3;
        }
               
        if (!IsPostBack)
        {
            if (Request.QueryString["REPAIRJOBID"] != null)
               Title1.Text = "Repair Job - Supt Remarks";
            if (Request.QueryString["STANDARDJOBID"] != null)
                Title1.Text = "Standard Job - Supt Remarks";
            if(Request.QueryString["orderid"] != null)
                Title1.Text = "Supt Feedback";
            BindFields();
    
        }    
    }    
    protected void MenuJobDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Request.QueryString["REPAIRJOBID"] != null)
                {
                    PhoenixDryDockJob.UpdateDryDockJobSuptRemark(new Guid(Request.QueryString["REPAIRJOBID"]), 0, txtRemarks.Text);
                     ucStatus.Text = "Remarks Updated.";
                }
                if (Request.QueryString["STANDARDJOBID"] != null)
                {
                    PhoenixDryDockJob.UpdateDryDockJobSuptRemark(new Guid(Request.QueryString["STANDARDJOBID"]), 2, txtRemarks.Text);
                     ucStatus.Text = "Remarks Updated.";
                }
               
                if (Request.QueryString["orderid"] != null)
                {
                    PhoenixDryDockOrder.UpdateDryDockSuptFeedback(int.Parse(Request.QueryString["vslid"]),new Guid(Request.QueryString["orderid"]),txtRemarks.Text);
                    ucStatus.Text = "Feedback Updated.";
                }
            }
            BindFields();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSuptRemark_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
            {

                if (Request.QueryString["REPAIRJOBID"] != null)
                    Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + Request.QueryString["REPAIRJOBID"] + "&pno=" + Request.QueryString["pno"], false);
                if (Request.QueryString["STANDARDJOBID"] != null)
                    Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + Request.QueryString["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);

            }
            if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                if (Filter.CurrentSelectedDryDockProject != null)
                    Response.Redirect("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"], false);
            }
            if (CommandName.ToUpper().Equals("PROJECT"))
            {
                Response.Redirect("../DryDock/DryDockProject.aspx", false);
            }
            if (CommandName.ToUpper().Equals("JOBREMARKS"))
            {
                if (Filter.CurrentSelectedDryDockProject != null)
                    Response.Redirect("../DryDock/DryDockJobSuptRemarks.aspx?vslid=" + Request.QueryString["vslid"] + "&orderid=" + Filter.CurrentSelectedDryDockProject, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        try
        {
            if (Request.QueryString["REPAIRJOBID"] != null)
            {
                DataSet ds = PhoenixDryDockJob.EditDryDockJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(Request.QueryString["REPAIRJOBID"]).Value);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtRemarks.Text = ds.Tables[0].Rows[0]["FLDSUPTREMARKS"].ToString();
                }
            }

            if (Request.QueryString["STANDARDJOBID"] != null)
            {
                DataSet ds = PhoenixDryDockJobGeneral.EditDryDockJobGeneral(General.GetNullableGuid(Request.QueryString["STANDARDJOBID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtRemarks.Text = ds.Tables[0].Rows[0]["FLDSUPTREMARKS"].ToString();
                }
            }
            if (Request.QueryString["orderid"] != null)
            {
                DataSet ds = PhoenixDryDockOrder.EditDryDockOrder(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtRemarks.Text = ds.Tables[0].Rows[0]["FLDSUPTFEEDBACK"].ToString();
                    txtLastModifiedDate.Text = string.Format("{0:dd/MMM/yyyy hh:mm:ss tt}", ds.Tables[0].Rows[0]["FLDSUPTFEEDDATE"]);
                    txtLastModifiedBy.Text = ds.Tables[0].Rows[0]["FLDSUPTFEEDBYNAME"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
