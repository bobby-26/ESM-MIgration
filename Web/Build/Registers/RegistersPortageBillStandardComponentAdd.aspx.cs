using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPortageBillStandardComponentAdd : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                DataSet dsCL = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponentHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1, 0, "OED,OBI,OCA,OAT,ORL,OBF,OSA,SOA,OPC,OFB,AWT,ODP,ORO,OSO,OFA,OCR");
                ddlComponentTypeAdd.DataSource = dsCL;
                ddlComponentTypeAdd.DataBind();
                ddlComponentTypeAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                btnShowBudgetAdd.Attributes.Add("onclick", "showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

                if (Request.QueryString["PortageBillId"] != null && Request.QueryString["PortageBillId"].ToString() != "")
                {
                    DataTable dt = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponent(new Guid(Request.QueryString["PortageBillId"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        ddlComponentTypeAdd.SelectedValue = dt.Rows[0]["FLDLOGTYPE"].ToString();
                        ddlComponentType_SelectedIndexChanged(null, null);
                        ddlHardAdd.SelectedHard = dt.Rows[0]["FLDWAGEHEADID"].ToString();
                        txtCodeAdd.Text = dt.Rows[0]["FLDCODE"].ToString();
                        txtDescAdd.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
                        txtBudgetIdAdd.Text = dt.Rows[0]["FLDBUDGETID"].ToString();
                        txtBudgetCodeAdd.Text = dt.Rows[0]["FLDBUDGETCODE"].ToString();
                    }
                }
                else
                    ddlHardAdd.ShortNameFilter = "BSH,BSU,HRA,BSU,REM,BRF,CWB,CWO,CWP,AOT,ARR";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string wagehead = ddlHardAdd.SelectedHard;
                string code = txtCodeAdd.Text;
                string desc = txtDescAdd.Text;
                string budget = txtBudgetIdAdd.Text;
                if (!IsValidPBStandardComponent(ddlComponentTypeAdd.SelectedValue, desc, budget, code))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["PortageBillId"] != null && Request.QueryString["PortageBillId"].ToString() != "")
                {
                    PhoenixRegistersPortageBillStandardComponent.UpdatePortageBillComponent(new Guid(Request.QueryString["PortageBillId"].ToString()), int.Parse(ddlComponentTypeAdd.SelectedValue), General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));
                }
                else
                {
                    PhoenixRegistersPortageBillStandardComponent.InsertPortageBillComponent(int.Parse(ddlComponentTypeAdd.SelectedValue), General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));
                }
                Response.Redirect("RegistersPortageBillStandardComponent.aspx", true);
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("RegistersPortageBillStandardComponent.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidPBStandardComponent(string logtype, string desc, string budgetid, string Code)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(logtype).HasValue)
            ucError.ErrorMessage = "Component Type is required.";
        if (Code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";
        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget Code is required.";

        return (!ucError.IsError);
    }
    protected void ddlComponentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).HasValue && (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 1))
            {
                ddlHardAdd.HardTypeCode = "128";
                ddlHardAdd.ShortNameFilter = "BSH,BSU,HRA,BSU,REM,BRF,AOT,ARR";
                ddlHardAdd.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "BSH,BSU,HRA,BSU,REM,BRF,AOT,ARR");
                ddlHardAdd.Enabled = true;
            }
            else if (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).HasValue && (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 23 || General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 24))
            {
                ddlHardAdd.HardTypeCode = "128";
                ddlHardAdd.ShortNameFilter = "CWB,CWO";
                ddlHardAdd.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "CWB,CWO");
                ddlHardAdd.Enabled = true;
            }
            else if (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).HasValue && (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 6 || General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 10))
            {
                ddlHardAdd.HardTypeCode = "128";
                ddlHardAdd.ShortNameFilter = "CWP,CWB,CWO";
                ddlHardAdd.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "CWP,CWB,CWO");
                ddlHardAdd.Enabled = true;
            }
            else if (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).HasValue && (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 25))
            {
                ddlHardAdd.HardTypeCode = "128";
                ddlHardAdd.ShortNameFilter = "CWP,CWB";
                ddlHardAdd.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "CWP,CWB");
                ddlHardAdd.Enabled = true;
            }
            else if (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).HasValue && (General.GetNullableInteger(ddlComponentTypeAdd.SelectedValue).Value == 28))
            {
                ddlHardAdd.HardTypeCode = "142";
                ddlHardAdd.ShortNameFilter = "CSH";
                ddlHardAdd.HardList = PhoenixRegistersHard.ListHard(1, 142, 0, "CSH");
                ddlHardAdd.Enabled = true;
            }
            else
            {
                ddlHardAdd.SelectedHard = "";
                ddlHardAdd.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
