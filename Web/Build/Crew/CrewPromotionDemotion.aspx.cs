using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewPromotionDemotion : PhoenixBasePage
{

    string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE") ? "Save" : "Request", "SAVE", ToolBarDirection.Right);
            MenuSignOn.AccessRights = this.ViewState;
            MenuSignOn.MenuList = toolbar.Show();

            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentCrewSelection = Request.QueryString["empid"];
                strEmployeeId = Request.QueryString["empid"];
            }
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            if (ViewState["RANKID"] == null)
            {
                DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewPromotionDemotion.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvPromDem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + Filter.CurrentCrewSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&vslid=&rankid=" + ViewState["RANKID"].ToString() + "')", "PD Form", "<i class=\"fas fa-file-pr\"></i>", "PDForm");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                ViewState["RANKID"] = "";
                SetEmployeePrimaryDetails();
                ChkRankProm();
                gvPromDem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OnSelectRankChk(object sender, EventArgs e)
    {
        if (chkShowAllRank.Checked != false || chkShowDemotedRank.Checked != false)
        {
            if (chkShowAllRank.Checked != false && chkShowDemotedRank.Checked == false)
            {
                ChkAll();
            }
            else if (chkShowAllRank.Checked != false && chkShowDemotedRank.Checked != false)
            {
                ChkAll();
            }
            else if (chkShowAllRank.Checked == false && chkShowDemotedRank.Checked != false)
            {
                ChkRankDem();
            }
        }
        else
        {
            ChkRankProm();
        }

    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDRANKNAME", "FLDDATE", "FLDSTATUS", "FLDCREATEDDATE" };
        string[] alCaptions = { "Rank", "Date", "Status", "Created Date" };

        DataSet ds;
        ds = PhoenixCrewManagement.CrewPromotionDemotionList(General.GetNullableInteger(strEmployeeId));

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew Promotion/Demotion.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Promotion/Demotion</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void ChkAll()
    {
        ucRank.Items.Clear();
        ucRank.DataSource = PhoenixRegistersRank.ListRank();
        ucRank.DataTextField = "FLDRANKNAME";
        ucRank.DataValueField = "FLDRANKID";
        ucRank.DataBind();
        ucRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ChkRankDem()
    {
        ucRank.Items.Clear();

        ucRank.DataSource = PhoenixRegistersRank.ListRankFilter(null, null, General.GetNullableInteger(ViewState["RANKID"].ToString()), 1);
        ucRank.DataTextField = "FLDRANKNAME";
        ucRank.DataValueField = "FLDRANKID";
        ucRank.DataBind();
        ucRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ChkRankProm()
    {
        ucRank.Items.Clear();

        ucRank.DataSource = PhoenixRegistersRank.ListRankFilter(null, null, General.GetNullableInteger(ViewState["RANKID"].ToString()), 0);
        ucRank.DataTextField = "FLDRANKNAME";
        ucRank.DataValueField = "FLDRANKID";
        ucRank.DataBind();
        ucRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void CrewSignOn_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCrewPromottion())
                {
                    ucError.Visible = true;
                    return;

                }

                SavePromotionDemotion();
                SetEmployeePrimaryDetails();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SavePromotionDemotion()
    {

        try
        {
            string status = string.Empty;
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                PhoenixCrewManagement.CrewOffshorePromotionDemotionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    int.Parse(strEmployeeId),
                                                                    int.Parse(ucRank.SelectedValue),
                                                                    General.GetNullableInteger(string.Empty),
                                                                    DateTime.Parse(txtPromotionDate.Text), General.GetNullableInteger(ViewState["RANKID"].ToString()),
                                                                    txtRemarks.Text
                                                                    );
                status = "Successfully promoted/Demotion";
            }
            else
            {
                PhoenixCrewManagement.CrewPromotionDemotionRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    int.Parse(strEmployeeId),
                                                                    int.Parse(ucRank.SelectedValue),
                                                                    General.GetNullableInteger(string.Empty),
                                                                    DateTime.Parse(txtPromotionDate.Text), General.GetNullableInteger(ViewState["RANKID"].ToString()),
                                                                    txtRemarks.Text
                                                                    );
                status = "Request Created Successfully";
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelpactivity','ifMoreInfo',null);", true);
            BindData();
            gvPromDem.Rebind();
            ucStatus.Visible = true;
            ucStatus.Text = status;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCrewPromottion()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (ucRank.SelectedValue.Trim().Equals("Dummy") || ucRank.SelectedValue.Trim().Equals(""))
            ucError.ErrorMessage = "Please select a rank";

        if (!DateTime.TryParse(txtPromotionDate.Text, out resultDate))
            ucError.ErrorMessage = "Promotion/Demotion Date is required.";
        else if (DateTime.TryParse(txtPromotionDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Promotion/Demotion  Date should be earlier than current date";
        }
        else if (txtRemarks.Text.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Please Enter the Remarks";
        }
        return (!ucError.IsError);

    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPresentRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + (dt.Rows[0]["FLDMIDDLENAME"].ToString() == "" ? "" : " ") + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                txtSignedOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNONDATE"].ToString()));
                txtReliefDue.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELEFDUEDATE"].ToString()));
                txtPromotionGrade.Text = dt.Rows[0]["FLDPROMOTIONGRADE"].ToString();
                txtBtod.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDBTOD"].ToString()));
                txtEtod.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDETOD"].ToString()));
                txtActivity.Text = dt.Rows[0]["FLDACTIVITYNAME"].ToString();
                txtFromDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDACTIVITYDATE"].ToString()));
                txtToDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDACTIVITYTODATE"].ToString()));
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        string[] alColumns = { "FLDRANKNAME", "FLDDATE", "FLDSTATUSDESC", "FLDCREATEDDATE" };
        string[] alCaptions = { "Rank", "Date", "Status", "Created Date" };
        DataSet ds;
        ds = PhoenixCrewManagement.CrewPromotionDemotionList(General.GetNullableInteger(strEmployeeId));
        General.SetPrintOptions("gvPromDem", "Crew Promotion/Demotion", alCaptions, alColumns, ds);
        gvPromDem.DataSource = ds;
        gvPromDem.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void Rebind()
    {
        gvPromDem.SelectedIndexes.Clear();
        gvPromDem.EditIndexes.Clear();
        gvPromDem.DataSource = null;
        gvPromDem.Rebind();
    }
    protected void gvPromDem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel id = (RadLabel)e.Item.FindControl("lbldtkey");
                PhoenixCrewManagement.CrewPromotionDemotioncancel(new Guid(id.Text));

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvPromDem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvPromDem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drvType = (DataRowView)e.Item.DataItem;
                RadLabel lbldtkey = (RadLabel)e.Item.FindControl("lbldtkey");
                LinkButton smddelete = (LinkButton)e.Item.FindControl("smddelete");
                if (smddelete != null)
                    smddelete.Visible = SessionUtil.CanAccess(this.ViewState, smddelete.CommandName);
                LinkButton cmdComment = (LinkButton)e.Item.FindControl("cmdComment");
                if (cmdComment != null)
                {
                    cmdComment.Visible = SessionUtil.CanAccess(this.ViewState, cmdComment.CommandName);
                    RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
                    //cmdComment.Attributes.Add("OnClientClick", "javascript:parent.openNewWindow('CrewComment',''," + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lbldtkey.Text + "&empid=" + empid.Text + "'); return false;");
                    cmdComment.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lbldtkey.Text + "&empid=" + empid.Text + "'); return false;");
                }
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
              
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drvType["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lbldtkey.Text + "&mod=" + PhoenixModule.CREW + "','medium'); return false;");
                }              
            }
        }
    }

}
