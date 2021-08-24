using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class HR_PayRollSGFWLEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        ShowToolBar();
        if (!IsPostBack)
            Loaddata();
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);


        gvTabStrip.MenuList = toolbarmain.Show();
    }

    public void Loaddata()
    {
        DataTable dt = PhoenixPayRollSingapore.PayrollSGFWLDetail(General.GetNullableGuid(Request.QueryString["fwlid"]));

        radddworkcat.SelectedHard = dt.Rows[0]["FLDEMPLOYEEWORKCATEGORYID"].ToString();
        radddskilllevel.SelectedHard = dt.Rows[0]["FLDSKILLLTYPEID"].ToString();
        radtbtier.Text = dt.Rows[0]["FLDSFPRFWLTEIR"].ToString();
        radtbmintw.Text = dt.Rows[0]["FLDMINQUOTAPERCENTAGE"].ToString();
        radtbmaxtw.Text = dt.Rows[0]["FLDMAXQUOTAPERCENTAGE"].ToString();
        radtblevydaily.Text = dt.Rows[0]["FLDDAILYLEVYRATE"].ToString();
        radtblevymonthly.Text = dt.Rows[0]["FLDMONTHLYLEVYRATE"].ToString();


    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (IsValidInput())
            {
                ucError.Visible = true;
                return;
            }



            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.PayrollSingaporeFWLUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(Request.QueryString["fwlid"])
                    , General.GetNullableInteger(radddworkcat.SelectedHard)
                    , General.GetNullableInteger(radddskilllevel.SelectedHard)
                    , General.GetNullableString(radtbtier.Text)
                    , General.GetNullableDecimal(radtbmintw.Text)
                    , General.GetNullableDecimal(radtbmaxtw.Text)
                    , General.GetNullableDecimal(radtblevydaily.Text)
                    , General.GetNullableDecimal(radtblevymonthly.Text)
                    );
            }



            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following ";
        if (General.GetNullableInteger(radddworkcat.SelectedHard) == null)
        {
            ucError.ErrorMessage = "Employee work category.";
        }
        if (General.GetNullableString(radtbtier.Text) == null)
        {
            ucError.ErrorMessage = "FWL Tier. ";
        }
        if (General.GetNullableDecimal(radtbmintw.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Total Workforce (%) ";
        }

        if (General.GetNullableDecimal(radtbmintw.Text) != null & General.GetNullableDecimal(radtbmaxtw.Text) != null)
        {
            if (!(General.GetNullableDecimal(radtbmaxtw.Text) > General.GetNullableDecimal(radtbmintw.Text)))
            {
                ucError.ErrorMessage = "Maximum Workforce % should be greater than minimum Workforce % .";
            }
        }

        return ucError.IsError;
    }
}