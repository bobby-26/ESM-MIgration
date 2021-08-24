using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselPosition;
using System.Web.UI;

public partial class VesselPositionShiftingReportReview : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send E.Mail", "MAIL",ToolBarDirection.Right);
        toolbar.AddButton("Accept", "ACCEPT", ToolBarDirection.Right);
        Review.AccessRights = this.ViewState;
        Review.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            binddetail();
        }
    }
    private void binddetail()
    {
        DataSet ds = PhoenixVesselPositionNoonReport.ShiftingReportReviewList(General.GetNullableInteger(Request.QueryString["VesselId"]), General.GetNullableGuid(Request.QueryString["NoonReportID"]));
        DataRow dr = ds.Tables[1].Rows[0];
        lblvesselname.Text = dr["FLDVESSELNAME"].ToString();
        lblDate.Text = General.GetDateTimeToString(dr["FLDDATE"].ToString());
        lblisicing.Text= dr["FLDICINGONDEC"].ToString();
        if (lblisicing.Text.Trim().ToUpper() == "YES")
            lblisicing.Style.Add("Color", "Red !important;");
        else
            lblisicing.Style.Add("Color", "Green !important;");
        lblisinuswater.Text = dr["FLDINUSWATER"].ToString();
        if (lblisinuswater.Text.Trim().ToUpper() == "YES")
            lblisinuswater.Style.Add("Color", "Red !important;");
        else
            lblisinuswater.Style.Add("Color", "Green !important;");
        lblisInHighRiskArea.Text = dr["FLDINECA"].ToString();
        if (lblisInHighRiskArea.Text.Trim().ToUpper() == "YES")
            lblisInHighRiskArea.Style.Add("Color", "Red !important;");
        else
            lblisInHighRiskArea.Style.Add("Color", "Green !important;");
        lblmasterremarktext.Text= dr["FLDREMARKS"].ToString();

        lblAertText.Text= dr["FLDALERT"].ToString();
    }
    protected void gvParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionNoonReport.ShiftingReportReviewList(General.GetNullableInteger(Request.QueryString["VesselId"]),General.GetNullableGuid( Request.QueryString["NoonReportID"]));
        gvParameter.DataSource = ds.Tables[0];
    }

    protected void Review_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ACCEPT"))
            {
                PhoenixVesselPositionNoonReport.NoonReportReviewUpdate(General.GetNullableInteger(Request.QueryString["VesselId"]), General.GetNullableGuid(Request.QueryString["NoonReportID"]));
                ucStatus.Text = "Accepted.";
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            if (CommandName.ToUpper().Equals("MAIL"))
            {
                String scriptpopup = String.Format(
                 "javascript:parent.openNewWindow('Filter', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionShiftingReviewMail.aspx?VesselId="
             + Request.QueryString["VesselId"] + "&NoonReportID=" + Request.QueryString["NoonReportID"] + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string script = "closeTelerikWindow('Filter', 'review')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            RadLabel nval = (RadLabel)e.Item.FindControl("lblreportedvalue");
            if(nval!=null)
            {
                nval.Style.Add("Color", "Red !important;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
