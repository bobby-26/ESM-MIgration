using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class OptionsChooseCompany : PhoenixBasePage
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        gvCompany.DataSource = PhoenixRegistersCompany.ListAssignedCompany();
        gvCompany.DataBind();
    }

    protected void OptionChooseCompany_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("CHOOSECOMPANY"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript' name='BookMarkScript'>" + "\n";
                Script += "showCompany();";
                Script += "</script>" + "\n";

                PhoenixSecurityContext.CurrentSecurityContext.CompanyID = int.Parse(((Label)_gridview.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblCompanyID")).Text);
                PhoenixSecurityContext.CurrentSecurityContext.CompanyName = ((Label)_gridview.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblCompanyCode")).Text;

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                ucStatus.Text = "You are now working in " + PhoenixSecurityContext.CurrentSecurityContext.CompanyName;

            }
        }
        catch { }
    }
}
