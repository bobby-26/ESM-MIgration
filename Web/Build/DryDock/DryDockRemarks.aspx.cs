using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DryDockRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRemark.AccessRights = this.ViewState;
        MenuRemark.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["OWNER"] != null)
               Title1.Text = "Owner Remarks";
            if (Request.QueryString["MANAGER"] != null)
                Title1.Text = "Manager Remarks";
            if(Request.QueryString["SUPT"] != null)
                Title1.Text = "Supt Feedback";
            BindFields();
    
        }    
    }

    private void BindFields()
    {
        try
        {
            DataTable dt = PhoenixDryDockOrder.EditDryDockOrderLine(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]), new Guid(Request.QueryString["orderlineid"]));
            if (dt.Rows.Count > 0)
            {
                if (Request.QueryString["OWNER"] != null)
                {
                    txtRemarks.Text = dt.Rows[0]["FLDOWNERREMARKS"].ToString();
                }
                else if (Request.QueryString["MANAGER"] != null)
                {
                    txtRemarks.Text = dt.Rows[0]["FLDMANAGERREMARKS"].ToString();
                }
                else if (Request.QueryString["SUPT"] != null)
                {
                    txtRemarks.Text = dt.Rows[0]["FLDSUPTREMARKS"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRemark_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRemarks(txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["MANAGER"] != null)
                {
                    PhoenixDryDockOrder.UpdateDryDockOrderLineRemark(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]), new Guid(Request.QueryString["orderlineid"]), txtRemarks.Text, 1);
                    ucStatus.Text = "Remarks Updated.";
                }
                if (Request.QueryString["OWNER"] != null)
                {
                    PhoenixDryDockOrder.UpdateDryDockOrderLineRemark(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]), new Guid(Request.QueryString["orderlineid"]), txtRemarks.Text, 2);
                    ucStatus.Text = "Remarks Updated.";
                }

                if (Request.QueryString["SUPT"] != null)
                {
                    PhoenixDryDockOrder.UpdateDryDockOrderLineRemark(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]), new Guid(Request.QueryString["orderlineid"]), txtRemarks.Text, 3);
                    
                }
                ucStatus.Text = "Remarks Updated.";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRemarks(string remarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        
        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";
       
        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
