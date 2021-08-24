using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewSuitabilityCheck : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewSuitabilityCheck.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Crew/CrewSuitabilityCheck.aspx?empid=" + Request.QueryString["empid"], "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            MenuCrewSuitabilityList.MenuList = toolbar.Show();
            if (Request.QueryString["vslid"] != null)
            {
                ucVessel.SelectedVessel = Request.QueryString["vslid"];
            }
            if (Request.QueryString["rnkid"] != null)
            {
                ddlRank.SelectedValue = Request.QueryString["rnkid"];
            }
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["personalmaster"] != null)
                    SetEmployeePrimaryDetails();

                if (Request.QueryString["newapplicant"] != null)
                    SetNewApplicantPrimaryDetails();
                if (Request.QueryString["vesselid"] != null)
                {
                    ucVessel.SelectedVessel = Request.QueryString["vesselid"];
                    SetVesselType(null, null);
                }
                if (Request.QueryString["rankid"] != null)
                {
                    ddlRank.SelectedValue = Request.QueryString["rankid"];
                }
            }            SetIncompatibleVesselType();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSuitability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.CrewSuitabilityOfVessel(
                General.GetNullableInteger(ucVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ddlRank.SelectedValue) == null ? 0 : General.GetNullableInteger(ddlRank.SelectedValue)
                , int.Parse(Request.QueryString["empid"].ToString())
                , General.GetNullableDateTime(ucDate.Text)
                );
            gvSuitability.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewSuitabilityList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            Rebind();
        }
    }
    protected void Rebind()
    {
        gvSuitability.SelectedIndexes.Clear();
        gvSuitability.EditIndexes.Clear();
        gvSuitability.DataSource = null;
        gvSuitability.Rebind();
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCATEGORY", "FLDDOCUMENTNAME", "FLDDATEOFEXPIRY", "FLDNATIONALITY", "FLDALTERNATERANK", "FLDMEETINGREQYN" };
        string[] alCaptions = { "Category", "Document", "Expiry Date", "Nationality", "Alternate Rank", "Meeting Requirement" };


        DataTable dt = PhoenixCrewManagement.CrewSuitabilityOfVessel(
                   General.GetNullableInteger(ucVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ucVessel.SelectedVessel)
                   , General.GetNullableInteger(ddlRank.SelectedValue) == null ? 0 : General.GetNullableInteger(ddlRank.SelectedValue)
                   , int.Parse(Request.QueryString["empid"].ToString())
                   , General.GetNullableDateTime(ucDate.Text)
                   );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewSuitabilityCheckList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Suitability Check List List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td>");
        Response.Write("<b>Emp Name:</b>" + txtFirstName.Text + " " + txtMiddleName.Text + " " + txtLastName.Text);
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("<b>Emp No:</b>" + txtEmployeeNumber.Text);
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("<b>Rank:</b>" + ddlRank.SelectedValue);
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvSuitability_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                RadLabel lblMissingYN = (RadLabel)e.Row.FindControl("lblMissingYN");
                RadLabel lblExpiredYN = (RadLabel)e.Row.FindControl("lblExpiredYN");
                RadLabel lblDocumentName = (RadLabel)e.Row.FindControl("lblDocumentName");
                RadLabel lblExpiryDate = (RadLabel)e.Row.FindControl("lblExpiryDate");
                RadLabel lblNationality = (RadLabel)e.Row.FindControl("lblNationality");
                //Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                RadLabel lblGoExpired = (RadLabel)e.Row.FindControl("lblGoExpiredYN");
                RadLabel lblAlternatDocGoExp = (RadLabel)e.Row.FindControl("lblAlternatDocGoExp");
                RadLabel lblAlternateRank = (RadLabel)e.Row.FindControl("lblAlternateRank");
                if (lblMissingYN.Text.Trim() == "1")
                {
                    lblDocumentName.Attributes.Add("style", "color:blue !important");
                    lblExpiryDate.Attributes.Add("style", "color:blue !important");
                    lblNationality.Attributes.Add("style", "color:blue !important");
                }
                else if (lblExpiredYN.Text.Trim() == "1")
                {
                    lblDocumentName.ForeColor = System.Drawing.Color.Red;
                    lblExpiryDate.ForeColor = System.Drawing.Color.Red;
                    lblNationality.ForeColor = System.Drawing.Color.Red;
                }
                else if (lblGoExpired.Text.Trim() == "1")
                {
                    lblDocumentName.ForeColor = System.Drawing.Color.DarkOrange;
                    lblExpiryDate.ForeColor = System.Drawing.Color.DarkOrange;
                    lblNationality.ForeColor = System.Drawing.Color.DarkOrange;
                }
                if (lblAlternatDocGoExp.Text.Trim() == "1")
                {
                    lblAlternateRank.ForeColor = System.Drawing.Color.DarkOrange;
                }
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (att != null)
                {
                    if (drv["FLDDTKEY"].ToString() == "")
                        att.Visible = false;
                    else
                        att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                    if (drv["FLDISATTACHMENT"].ToString() == "")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    else if (lblMissingYN.Text.Trim() == "1")
                        att.Visible = false;
                    else
                        att.Attributes.Add("onclick", "javascript:Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                            + PhoenixModule.CREW + "&type=" + drv["FLDATTACHMENTTYPE"].ToString() + "&U=no'); return false;");
                }
            }
        }
    }
    protected void gvSuitability_PreRender(object sender, EventArgs e)
    {
        //for (int rowIndex = gvSuitability.Items.Count-2 ; rowIndex >= 0; rowIndex--)
        //{
        //    GridDataItem row = gvSuitability.Items[rowIndex];
        //    GridDataItem previousRow = gvSuitability.Items[rowIndex + 1];

        //    RadLabel currentCategoryName = ((RadLabel)gvSuitability.Items[rowIndex].FindControl("lblCategoryName"));
        //    RadLabel previousCategoryName = ((RadLabel)gvSuitability.Items[rowIndex + 1].FindControl("lblCategoryName"));

        //    if (currentCategoryName.Text == previousCategoryName.Text)
        //    {
        //        row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
        //                            previousRow.Cells[1].RowSpan + 1;
        //        previousCategoryName.Visible = false;
        //    }
        //}
    }
    protected void gvSuitability_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            RadLabel lblExpiryDate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            RadLabel lblNationality = (RadLabel)e.Item.FindControl("lblNationality");
            RadLabel lblGoExpired = (RadLabel)e.Item.FindControl("lblGoExpiredYN");
            RadLabel lblAlternatDocGoExp = (RadLabel)e.Item.FindControl("lblAlternatDocGoExp");
            RadLabel lblAlternateRank = (RadLabel)e.Item.FindControl("lblAlternateRank");
            if (lblMissingYN.Text.Trim() == "1")
            {
                lblDocumentName.Attributes.Add("style", "color:blue !important");
                lblExpiryDate.Attributes.Add("style", "color:blue !important");
                lblNationality.Attributes.Add("style", "color:blue !important");
            }

            else if (lblExpiredYN.Text.Trim() == "1")
            {
                lblDocumentName.Attributes.Add("style", "color:red !important");
                lblExpiryDate.Attributes.Add("style", "color:red !important");
                lblNationality.Attributes.Add("style", "color:red !important");
            }
            else if (lblGoExpired.Text.Trim() == "1")
            {
                lblDocumentName.Attributes.Add("style", "color:DarkOrange !important");
                lblExpiryDate.Attributes.Add("style", "color:DarkOrange !important");
                lblNationality.Attributes.Add("style", "color:DarkOrange !important");
            }
            if (lblAlternatDocGoExp.Text.Trim() == "1")
            {
                lblAlternateRank.Attributes.Add("style", "color:DarkOrange !important");
            }
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDDTKEY"].ToString() == "")
                    att.Visible = false;
                else
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else if (lblMissingYN.Text.Trim() == "1")
                    att.Visible = false;
                else
                    att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.CREW + "&type=" + drv["FLDATTACHMENTTYPE"].ToString() + "&U=no'); return false;");
            }
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Request.QueryString["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetIncompatibleVesselType()
    {
        try
        {
            DataSet ds = PhoenixCrewIncompatibility.ListEmployeeIncompatibility(int.Parse(Request.QueryString["empid"].ToString()), 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtInCompVslType.Text = ds.Tables[0].Rows[0]["FLDVESSELTYPENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetVesselType(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null || General.GetNullableInteger(Request.QueryString["vesselid"]) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? Request.QueryString["vesselid"] : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            ucVesselType.SelectedVesseltype = "";
            ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
        }
    }
    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Request.QueryString["empid"].ToString()));
            tdempno.Visible = false;
            tdempnum.Visible = false;
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        return (!ucError.IsError);
    }
}
