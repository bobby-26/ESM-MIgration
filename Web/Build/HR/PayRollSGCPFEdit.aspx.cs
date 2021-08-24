using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGCPFEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        ShowToolBar();
        if (!IsPostBack)
            LoadData();
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);


        gvTabStrip.MenuList = toolbarmain.Show();
    }
    public void LoadData()
    {
        DataTable dt = PhoenixPayRollSingapore.PayrollSGCPFDetail(General.GetNullableGuid(Request.QueryString["cpfid"]));
        if (dt.Rows.Count > 0)
        {
            radtbminimumage.Text = dt.Rows[0]["FLDMINAGE"].ToString();
            radtblmaximumage.Text = dt.Rows[0]["FLDMAXAGE"].ToString();
            radminimumtw.Text = dt.Rows[0]["FLDMINITW"].ToString();
            radmaximumtw.Text = dt.Rows[0]["FLDMAXTW"].ToString();
            rademployerow.Text = dt.Rows[0]["FLDEMPLOYEROWCONTPERCENTAGE"].ToString();
            rademployeraw.Text = dt.Rows[0]["FLDEMPLOYERAWCONTPERCENTAGE"].ToString();
            rademployeeow.Text = dt.Rows[0]["FLDEMPLOYEEOWCONTPERCENTAGE"].ToString();
            rademployeeaw.Text = dt.Rows[0]["FLDEMPLOYEEAWCONTPERCENTAGE"].ToString();
            rademployeecorrectionfactor.Text = dt.Rows[0]["FLDEMPLOYEECONTRIBUTIONCORRECTIONFACTOR"].ToString();
            rademployerowlimit.Text = dt.Rows[0]["FLDEMPLOYEROWCONTLIMIT"].ToString();
            rademployeeowlimit.Text = dt.Rows[0]["FLDEMPLOYEEOWCONTLIMIT"].ToString();
        }


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
                PhoenixPayRollSingapore.PayrollSingaporeCPFUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableInteger(radtbminimumage.Text)
                    , General.GetNullableInteger(radtblmaximumage.Text)
                    , General.GetNullableDecimal(radminimumtw.Text)
                    , General.GetNullableDecimal(radmaximumtw.Text)
                    , General.GetNullableDecimal(rademployerow.Text)
                    , General.GetNullableDecimal(rademployeraw.Text)
                    , General.GetNullableDecimal(rademployeeow.Text)
                    , General.GetNullableDecimal(rademployeeaw.Text)
                    , General.GetNullableDecimal(rademployeecorrectionfactor.Text)
                    , General.GetNullableDecimal(rademployerowlimit.Text)
                    , General.GetNullableDecimal(rademployeeowlimit.Text)
                    , General.GetNullableGuid(Request.QueryString["cpfid"]));
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
        if (General.GetNullableInteger(radtbminimumage.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Age ";
        }
        if (General.GetNullableInteger(radtblmaximumage.Text) == null)
        {
            ucError.ErrorMessage = "Maximum Age ";
        }
        if (General.GetNullableInteger(radminimumtw.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Total Wage ";
        }

        if (General.GetNullableInteger(radtbminimumage.Text) != null & General.GetNullableInteger(radtblmaximumage.Text) != null)
        {
            if (!(General.GetNullableInteger(radtblmaximumage.Text) > General.GetNullableInteger(radtbminimumage.Text)))
            {
                ucError.ErrorMessage = "Maximum age should be greater than minimum age.";
            }
        }
        if (General.GetNullableInteger(radminimumtw.Text) != null & General.GetNullableInteger(radmaximumtw.Text) != null)
        {
            if (!(General.GetNullableInteger(radmaximumtw.Text) > General.GetNullableInteger(radminimumtw.Text)))
            {
                ucError.ErrorMessage = "Maximum total wage should be greater than minimum total wage.";
            }
        }
        return ucError.IsError;
    }
}