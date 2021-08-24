using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingMatrixRequirement : PhoenixBasePage
{

    private string strEmployeeId = string.Empty;
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvSuitability.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);           
                    

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingMatrixRequirement.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingMatrixRequirement.aspx", "Find", "search.png", "SEARCH");
            //MenuCrewSuitabilityList.AccessRights = this.ViewState;
            //MenuCrewSuitabilityList.MenuList = toolbar.Show();          

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["SIGNONOFFID"] = "";
                ViewState["trainingmatrixid"] = "";
                ViewState["offsignerid"] = "";
                ViewState["vsltype"] = "";
                ViewState["Charterer"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";

                      


                if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                    SetEmployeePrimaryDetails();

                if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                    SetNewApplicantPrimaryDetails();

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                {
                    strEmployeeId = Request.QueryString["empid"].ToString();
                    ViewState["empid"] = strEmployeeId;      
                }

                if (Request.QueryString["signonoffid"] != null && Request.QueryString["signonoffid"].ToString() != "")
                {
                    ViewState["SIGNONOFFID"] = Request.QueryString["signonoffid"].ToString();
                    SetPrimaryDetails();
                }
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreReliefRequest.EditSignOnOff(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString())
                , General.GetNullableInteger(ViewState["empid"].ToString()));


            tdempno.Visible = false;
            tdempnum.Visible = false;

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                lblVesselID.Text = dt.Rows[0]["FLDVESSELID"].ToString();
                ucVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
                //lblVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                lblRankID.Text = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                lblTraningMatrixID.Text = dt.Rows[0]["FLDTRANINGMATRIXID"].ToString();
                ucDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString();
            }
            //Bind Vessel Details
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(lblVesselID.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
            BindOffSigner();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindOffSigner()
    {
        UserControlVessel vsl = ucVessel;
        RadComboBox rank = ddlRank;
        RadComboBox cob = ddlOffSigner;

        int? VesselId = General.GetNullableInteger(vsl.SelectedVessel);
        int? RankId = General.GetNullableInteger(rank.SelectedValue);

        cob.Items.Clear();

        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
            {
                cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId);
            }
        }
        else
            cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedValue));
        cob.DataTextField = "FLDEMPLOYEENAME";
        cob.DataValueField = "FLDEMPLOYEEID";
        cob.DataBind();
        cob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
        {
            if (cob.Items.FindItemByValue(Request.QueryString["offsignerid"].ToString()) != null)
                cob.SelectedValue = Request.QueryString["offsignerid"].ToString();
        }
    }
    protected void BindTrainingMatrix()
    {
        ddlTrainingMatrix.Items.Clear();
        ddlTrainingMatrix.DataSource = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";
        ddlTrainingMatrix.DataBind();
        ddlTrainingMatrix.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (lblTraningMatrixID.Text != null && lblTraningMatrixID.Text != "")
        {
            if (ddlTrainingMatrix.Items.FindItemByValue(lblTraningMatrixID.Text) != null)
                ddlTrainingMatrix.SelectedValue = lblTraningMatrixID.Text;
        }
        else
        {
            if (ddlTrainingMatrix.Items.Count == 2)
                ddlTrainingMatrix.SelectedIndex = 1;

        }
    }

    private void BindData()
    {
        try
        {
            int issuitable = 1;

            DataSet ds = PhoenixCrewOffshoreReliefRequest.CrewSuitabilityOfVessel(
                General.GetNullableInteger(lblVesselID.Text) == null ? 0 : General.GetNullableInteger(lblVesselID.Text)
                , General.GetNullableInteger(ddlRank.SelectedValue) == null ? 0 : General.GetNullableInteger(ddlRank.SelectedValue)
                , int.Parse(ViewState["empid"].ToString())
                , General.GetNullableDateTime(ucDate.Text)
                , General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                , ref issuitable
                , General.GetNullableInteger("1")
                );

            gvSuitability.DataSource = ds.Tables[0];
            gvRankExp.DataSource = ds.Tables[1];
            gvVesselTypeExp.DataSource = ds.Tables[2];


          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewSuitabilityList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            BindData();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDCATEGORY", "FLDDOCUMENTNAME", "FLDDATEOFEXPIRY", "FLDNATIONALITY" };
        string[] alCaptions = { "Category", "Document", "Expiry Date", "Nationality" };

        int issuitable = 1;
        DataSet ds = PhoenixCrewOffshoreReliefRequest.CrewSuitabilityOfVessel(
            General.GetNullableInteger(lblVesselID.Text) == null ? 0 : General.GetNullableInteger(lblVesselID.Text)
                , General.GetNullableInteger(lblRankID.Text) == null ? 0 : General.GetNullableInteger(lblRankID.Text)
                , int.Parse(ViewState["empid"].ToString())
                , General.GetNullableDateTime(ucDate.Text)
                , General.GetNullableInteger(lblTraningMatrixID.Text)
                , ref issuitable
                , General.GetNullableInteger("1")
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewSuitabilityCheckList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Suitability Check List</h3></td>");
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
        Response.Write("<b>Rank:</b>" + txtRank.Text);
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
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
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
                LinkButton lnkName = (LinkButton)e.Row.FindControl("lnkName");
                RadLabel lblDTKey = (RadLabel)e.Row.FindControl("lblDTKey");
                RadLabel lblAttachmenttype = (RadLabel)e.Row.FindControl("lblAttachmenttype");
                RadLabel lblStage = (RadLabel)e.Row.FindControl("lblStage");
                RadLabel lblStatus = (RadLabel)e.Row.FindControl("lblStatus");
                RadLabel lblVerifiedYN = (RadLabel)e.Row.FindControl("lblVerifiedYN");
                RadLabel lblReqDocumentName = (RadLabel)e.Row.FindControl("lblReqDocumentName");

                //if (lblMissingYN.Text.Trim() == "1")
                //{
                //    lblDocumentName.ForeColor = System.Drawing.Color.Blue;
                //    lblExpiryDate.ForeColor = System.Drawing.Color.Blue;
                //    lblNationality.ForeColor = System.Drawing.Color.Blue;
                //    lnkName.ForeColor = System.Drawing.Color.Blue;
                //    if (lblDocumentName != null) lblDocumentName.Visible = true;
                //    if (lnkName != null) lnkName.Visible = false;
                //}
                if (lblExpiredYN.Text.Trim() == "1" || lblMissingYN.Text.Trim() == "1" || lblVerifiedYN.Text.Trim() == "0")
                {
                    lblDocumentName.ForeColor = System.Drawing.Color.Red;
                    lblExpiryDate.ForeColor = System.Drawing.Color.Red;
                    lblNationality.ForeColor = System.Drawing.Color.Red;
                    lnkName.ForeColor = System.Drawing.Color.Red;
                    lblStage.ForeColor = System.Drawing.Color.Red;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    if (lblMissingYN.Text.Trim() == "1")
                    {
                        if (lblReqDocumentName != null) lblReqDocumentName.ForeColor = System.Drawing.Color.Red;
                        if (lblDocumentName != null) lblDocumentName.Visible = false;
                    }
                }
                if (lblDTKey != null && !string.IsNullOrEmpty(lblDTKey.Text))
                {
                    if (lblDocumentName != null) lblDocumentName.Visible = false;
                    if (lnkName != null) lnkName.Visible = true;
                    if (lblExpiredYN.Text.Trim() == "0" && lblMissingYN.Text.Trim() == "0" && lblVerifiedYN.Text.Trim() == "1")
                        lnkName.ForeColor = System.Drawing.Color.Black;
                }
                if (lnkName != null)
                {
                    if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                    {
                        lnkName.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                    }
                    else
                    {
                        lnkName.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                    }
                }
            }
        }
    }

    protected void gvVesselTypeExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lblShortfall = (RadLabel)e.Row.FindControl("lblShortfall");
            RadLabel lblVesselType = (RadLabel)e.Row.FindControl("lblVesselType");
            RadLabel lblVesselTypeExp = (RadLabel)e.Row.FindControl("lblVesselTypeExp");
            RadLabel lblStage = (RadLabel)e.Row.FindControl("lblStage");
            if (lblShortfall.Text == "1")
            {
                lblVesselType.ForeColor = System.Drawing.Color.Red;
                lblVesselTypeExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void gvRankExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lblShortfall = (RadLabel)e.Row.FindControl("lblShortfall");
            RadLabel lblRank = (RadLabel)e.Row.FindControl("lblRank");
            RadLabel lblRankExp = (RadLabel)e.Row.FindControl("lblRankExp");
            RadLabel lblStage = (RadLabel)e.Row.FindControl("lblStage");
            if (lblShortfall.Text == "1")
            {
                lblRank.ForeColor = System.Drawing.Color.Red;
                lblRankExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void gvSuitability_PreRender(object sender, EventArgs e)
    {
       // GridDecorator.MergeRows(gvSuitability);
    }

    protected void gvRankExp_PreRender(object sender, EventArgs e)
    {
      //  GridDecorator.MergeRowsExperience(gvRankExp);
    }

    protected void gvVesselTypeExp_PreRender(object sender, EventArgs e)
    {
       // GridDecorator.MergeRowsExperience(gvVesselTypeExp);
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblCategoryName")).Text;

                if (currentCategoryName == previousCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                        previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }

            }

        }

        public static void MergeRowsExperience(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                bool currentWaivedyn = ((CheckBox)gridView.Rows[rowIndex].FindControl("chkWaivedYN")).Checked;
                bool previousWaivedyn = ((CheckBox)gridView.Rows[rowIndex + 1].FindControl("chkWaivedYN")).Checked;

                string currentStagename = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblStage")).Text;
                string PreviousStagename = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblStage")).Text;

                if (currentStagename == PreviousStagename)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                        previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                if (currentWaivedyn == previousWaivedyn)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                        previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }
            }
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            //txtEmployeeNumber.Visible = true;

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();

                ddlRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlRank.SelectedValue = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["empid"].ToString()));


            tdempno.Visible = false;
            tdempnum.Visible = false;

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                lblVesselID.Text = ViewState["VESSELID"].ToString();
                lblRankID.Text = dt.Rows[0]["FLDRANK"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
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
        ViewState["Charterer"] = "";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null || General.GetNullableInteger(lblVesselID.Text) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? lblVesselID.Text : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
            BindOffSigner();
        }
    }
    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
        BindOffSigner();
    }  

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        //SetPageNavigator();
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

        if (General.GetNullableInteger(ddlTrainingMatrix.SelectedValue) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        return (!ucError.IsError);
    }

    protected void gvSuitability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRankExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVesselTypeExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

