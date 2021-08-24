using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportCertifcates : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdcertificate.Visible = SessionUtil.CanAccess(this.ViewState, "CERTIFICATE");
        rdregulatory.Visible = SessionUtil.CanAccess(this.ViewState, "REGULATORY");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkCertificatesandSurveysComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Certificates and Surveys', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=CER');return false; ");
            lnkRegulatoryMattersComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Regulatory Matters', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=REG');return false; ");
            lnkCDISIRESchedule.Attributes.Add("onclick", "javascript:parent.openNewWindow('code1', 'CDI / SIRE Schedule', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportSIREScheduleList.aspx?');return false;");
            lnkCDISIREScheduleComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'CDI / SIRE Schedule', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=VET');return false; ");
            BtnCertificatesandSurveyInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Certificates and Surveys','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=CER" + "',false, 320, 250,'','',options); return false;");
            BtnRegulatory.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Regulatory Matters','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=REG" + "',false, 320, 250,'','',options); return false;");
            BtnCDISIRESchedule.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','CDI / SIRE Schedule','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=VET" + "',false, 320, 250,'','',options); return false;");

            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("CER", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkCertificatesandSurveysComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("REG", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkRegulatoryMattersComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("VET", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkCDISIREScheduleComments.Controls.Add(html);
        }
    }
    protected void gvCertificateSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportCertificateScheduleSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvCertificateSchedule.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[1].Visible = false;

            if (dt.Rows[0]["FLD30VISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[2].Visible = false;

            if (dt.Rows[0]["FLD60VISIBLE"].ToString().Equals("0"))
                gvCertificateSchedule.Columns[3].Visible = false;

        }
    }

    protected void gvCertificateSchedule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk60Days = (LinkButton)e.Item.FindControl("lnk60Days");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk30Days");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl60Daysurl = (RadLabel)e.Item.FindControl("lbl60Daysurl");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl30Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if ((lnkOverdue != null) && (lnkOverdue.Text != "") && (lnkOverdue.Text != null))
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDOVERDUECOUNT"].ToString()) && int.Parse(dr["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if ((lnk30Days != null) && (lnk30Days.Text != "") && (lnk30Days.Text != null))
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 30 Days','" + lbl30Daysurl.Text + "'); return false;");
            }

            if ((lnk60Days != null) && (lnk60Days.Text != "") && (lnk60Days.Text != null))
            {
                lnk60Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60Daysurl.Text + "'); return false;");
            }

        }
    }

    protected void GVR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportLastRegulatery(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVR.DataSource = dt;

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
        GVR.Rebind();
        GVR2.Rebind();
    }

    protected void GVR2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportNextRegulatery(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVR2.DataSource = dt;
    }

    protected void GVR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton lnkCount = (LinkButton)e.Item.FindControl("lnkCount");

        RadLabel lblcount = (RadLabel)e.Item.FindControl("lblcount");
        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

        if ((lnkCount != null) && (lblcount != null) && (lnkCount.Text != ""))
        {
            lblcount.Visible = false;
            lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo',' - Regulatory " + lblmeasure.Text + " - Last ','" + lblurl.Text + "'); return false;");
        }

        if ((lnkCount != null) && (lblurl != null) && (lblcount != null) && (lblurl.Text == ""))
        {
            lnkCount.Visible = false;
            lblcount.Visible = true;
        }


        RadLabel lblShortcode = (RadLabel)e.Item.FindControl("lblShortCode");
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");

        if ((lblShortcode != null) && lblShortcode.Text == "QMS-REG-COCL")
        {
            if (cmdEdit != null) cmdEdit.Visible = false;
        }
    }

    protected void GVR2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton lnkCount = (LinkButton)e.Item.FindControl("lnkCount");

        RadLabel lblcount = (RadLabel)e.Item.FindControl("lblcount");
        RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

        if ((lnkCount != null) && (lblcount != null) && (lnkCount.Text != ""))
        {
            lblcount.Visible = false;
            lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo',' - Regulatory " + lblmeasure.Text + " - Last ','" + lblurl.Text + "'); return false;");
        }

        if ((lnkCount != null) && (lblurl != null) && (lblcount != null) && (lblurl.Text == ""))
        {
            lnkCount.Visible = false;
            lblcount.Visible = true;
        }
        RadLabel lblShortcode = (RadLabel)e.Item.FindControl("lblShortCode");
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");

        if ((lblShortcode != null) && lblShortcode.Text == "QMS-REG-CLRN")
        {
            if (cmdEdit != null) cmdEdit.Visible = false;
        }

    }

    protected void gvScheduleForCompany_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dv = (DataRowView)e.Item.DataItem;
            Image imgFlag = e.Item.FindControl("imgFlag") as Image;

            RadLabel DueDate = (RadLabel)e.Item.FindControl("lblDueDate");
            if (DueDate != null)
            {
                if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("1"))
                {
                    //DueDate.Attributes.Add("style", "background-color:#00FF00;font-weight:bold;");
                    //DueDate.ToolTip = "Due within 60 days";
                    DueDate.Attributes["style"] = "color:Green !important";
                    DueDate.ToolTip = "Due within 60 days";
                    DueDate.Font.Bold = true;
                }
                else if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("2"))
                {
                    //DueDate.Attributes.Add("style", "background-color:#FFFF00;font-weight:bold;");
                    //DueDate.ToolTip = "Due within 30 days";
                    DueDate.Attributes["style"] = "color:darkorange !important";
                    DueDate.ToolTip = "Due within 30 days";
                    DueDate.Font.Bold = true;
                }
                else if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("3"))
                {
                    //DueDate.Attributes.Add("style", "background-color:#FF0000;font-weight:bold;");
                    //DueDate.ToolTip = "Overdue";
                    DueDate.Attributes["style"] = "color:Red !important";
                    DueDate.ToolTip = "Overdue";
                    DueDate.Font.Bold = true;
                }
                else
                {
                    if (imgFlag != null) imgFlag.Visible = false;
                }
            }

        }
    }

    protected void gvScheduleForCompany_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportVettingSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvScheduleForCompany.DataSource = dt;
    }

    protected void GVR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                if (((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text != null && ((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text != string.Empty)
                {
                    PhoenixOwnerReportQuality.OwnersMonthlReportVesselSurveyDateUpdate(int.Parse(Filter.SelectedOwnersReportVessel)
                        , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                        , General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text)
                        , General.GetNullableString(((RadLabel)eeditedItem.FindControl("lblShortCodeEdit")).Text));
                }
                GVR.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void GVR2_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                if (((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text != null && ((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text != string.Empty)
                {
                    PhoenixOwnerReportQuality.OwnersMonthlReportVesselSurveyDateUpdate(int.Parse(Filter.SelectedOwnersReportVessel)
                        , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                        , General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("ucDateEdit")).Text)
                        , General.GetNullableString(((RadLabel)eeditedItem.FindControl("lblShortCodeEdit")).Text));
                }
                GVR2.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}