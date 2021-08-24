using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Presea_PreSeaCourseFeedbackReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuPreSeaScoreCradSummary.AccessRights = this.ViewState;
                MenuPreSeaScoreCradSummary.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";

            }
            //ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=VESSELCONTACTLIST&principal=null&vessel=null&vesseltype=null&showmenu=0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (ucBatch.SelectedBatch.ToUpper().Equals("DUMMY") || ucBatch.SelectedBatch.Equals(""))
                {
                    ucError.ErrorMessage = "Batch is required.";
                    ucError.Visible = true;
                    return;
                }

                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=10&reportcode=COURSEFEEDBACK&batchid=" + ucBatch.SelectedBatch + "&showmenu=0";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
