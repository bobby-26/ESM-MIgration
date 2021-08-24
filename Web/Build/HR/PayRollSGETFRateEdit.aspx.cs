using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class HR_PayRollSGETFRateEdit : System.Web.UI.Page
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
        DataTable dt = PhoenixPayRollSingapore.PayrollSGETFRateDetail(General.GetNullableGuid(Request.QueryString["etfrateid"]));
        if (dt.Rows.Count > 0)
        {
            radlblfundname.Text = dt.Rows[0]["FLDETHNICFUNDNAME"].ToString();
            radtbminimumtw.Text = dt.Rows[0]["FLDMINGROSSWAGE"].ToString();
            radtbmaximumtw.Text = dt.Rows[0]["FLDMAXGROSSWAGE"].ToString();
            radtbcontribution.Text = dt.Rows[0]["FLDETNICFUNDMONTHLYCONTRIBUTION"].ToString();
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
                PhoenixPayRollSingapore.PayrollSingaporeETFRateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(Request.QueryString["etfrateid"])
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