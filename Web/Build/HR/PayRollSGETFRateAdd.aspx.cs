using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGETFRateAdd : System.Web.UI.Page
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

        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);


        gvTabStrip.MenuList = toolbarmain.Show();
    }
    public void LoadData()
    {
        DataTable dt = PhoenixPayRollSingapore.PayrollSGETFDetail(General.GetNullableGuid(Request.QueryString["etfid"]));
        if (dt.Rows.Count > 0)
        {
            radlblfundname.Text = dt.Rows[0]["FLDETHNICFUNDNAME"].ToString();
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



            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.PayrollSingaporeETFRateInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(Request.QueryString["etfid"])
                    , General.GetNullableInteger(Request.QueryString["payrollid"])
                    , General.GetNullableDecimal(radtbminimumtw.Text)
                    , General.GetNullableDecimal(radtbmaximumtw.Text)
                    , General.GetNullableDecimal(radtbcontribution.Text)
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
       
        if (General.GetNullableDecimal(radtbminimumtw.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Total Wage.";
        }
       

        return ucError.IsError;
    }

}