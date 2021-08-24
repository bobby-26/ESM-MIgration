using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;

public partial class CrewOffshorePendingWaivers : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSuitability.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvSuitability.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                strEmployeeId = Filter.CurrentCrewSelection;
            else if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["crewplanid"] = "";
                ViewState["empid"] = strEmployeeId;
                ViewState["approval"] = "";
                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["vsltype"] = "";
                ViewState["rowindex"] = "";
                ViewState["waivedyn"] = "";
                ViewState["edititem"] = "0";

                ViewState["rankexprowindex"] = "";
                ViewState["rankexpwaivedyn"] = "";
                ViewState["rankexpedititem"] = "0";

                ViewState["vtexprowindex"] = "";
                ViewState["vtexpwaivedyn"] = "";
                ViewState["vtexpedititem"] = "0";

                ViewState["proposalrowindex"] = "";
                ViewState["proposalwaivedyn"] = "";
                ViewState["proposaledititem"] = "0";

                ucDate.Text = DateTime.Now.ToShortDateString();

                if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                    SetEmployeePrimaryDetails();

                if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                    SetNewApplicantPrimaryDetails();

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                {
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
                    BindPlan(ViewState["crewplanid"].ToString());
                }

                if (Request.QueryString["approval"] != null && Request.QueryString["approval"].ToString() != "")
                    ViewState["approval"] = Request.QueryString["approval"].ToString();

                BindData();
            }
            setToolBar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void setToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK");
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbar.Show();
    }
    
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + ViewState["empid"]
                    + "&crewplanid=" + ViewState["crewplanid"].ToString()
                    + "&approval=1"
                    + "&personalmaster=" + Request.QueryString["personalmaster"]
                    + "&newapplicant=" + Request.QueryString["newapplicant"]
                    + "&popup=" + Request.QueryString["popup"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindOffSigner()
    {
        UserControlVessel vsl = ucVessel;
        DropDownList rank = ddlRank;
        DropDownList cob = ddlOffSigner;

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
                //cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId);
                cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId, General.GetNullableInteger(strEmployeeId));
            }
        }
        else
            //cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedValue));
            cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel)
                                                                          , General.GetNullableInteger(rank.SelectedValue)
                                                                          , General.GetNullableInteger(strEmployeeId)
                                                                          );
        cob.DataTextField = "FLDEMPLOYEENAME";
        cob.DataValueField = "FLDEMPLOYEEID";
        cob.DataBind();
        cob.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindTrainingMatrix()
    {
        ddlTrainingMatrix.Items.Clear();
        DataTable dt = new DataTable();
        dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";

        if (dt.Rows.Count > 0)
        {
            ddlTrainingMatrix.DataSource = dt;
        }
        ddlTrainingMatrix.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ddlTrainingMatrix.DataBind();
    }

    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreReliefRequest.SearchPendingWaivers(General.GetNullableGuid(ViewState["crewplanid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSuitability.DataSource = ds.Tables[0];
                gvSuitability.DataBind();
                GridDecorator.MergeRows(gvSuitability);
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvSuitability);
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                gvRankExp.DataSource = ds.Tables[1];
                gvRankExp.DataBind();
                GridDecorator.MergeRowsExperience(gvRankExp);
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[1], gvRankExp);
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                gvVesselTypeExp.DataSource = ds.Tables[2];
                gvVesselTypeExp.DataBind();
                GridDecorator.MergeRowsExperience(gvVesselTypeExp);
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[2], gvVesselTypeExp);
            }
            //if (ds.Tables[3].Rows.Count > 0)
            //{
            //    gvProposalCheckList.DataSource = ds.Tables[3];
            //    gvProposalCheckList.DataBind();
            //}
            //else
            //{
            //    ShowNoRecordsFound(ds.Tables[3], gvProposalCheckList);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSuitability_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label lblMissingYN = (Label)e.Row.FindControl("lblMissingYN");
                Label lblExpiredYN = (Label)e.Row.FindControl("lblExpiredYN");
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                Label lblExpiryDate = (Label)e.Row.FindControl("lblExpiryDate");
                Label lblNationality = (Label)e.Row.FindControl("lblNationality");
                LinkButton lnkName = (LinkButton)e.Row.FindControl("lnkName");
                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                Label lblAttachmenttype = (Label)e.Row.FindControl("lblAttachmenttype");
                Label lblStage = (Label)e.Row.FindControl("lblStage");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                CheckBox chkWaivedYN = (CheckBox)e.Row.FindControl("chkWaivedYN");
                TextBox txtReason = (TextBox)e.Row.FindControl("txtReason");
                Label lblVerifiedYN = (Label)e.Row.FindControl("lblVerifiedYN");
                Label lblReqDocumentName = (Label)e.Row.FindControl("lblReqDocumentName");
                Label lblStageid = (Label)e.Row.FindControl("lblStageid");
                Label lblProposalStageid = (Label)e.Row.FindControl("lblProposalStageid");
                CheckBox chkCanbeWaivedYN = (CheckBox)e.Row.FindControl("chkCanbeWaivedYN");
                Label lblAuthenticatedYN = (Label)e.Row.FindControl("lblAuthenticatedYN");
                Label lblDocumentType = (Label)e.Row.FindControl("lblDocumentType");

                if (General.GetNullableInteger(ViewState["rowindex"].ToString()) != null && General.GetNullableInteger(ViewState["rowindex"].ToString()) == e.Row.DataItemIndex)
                {
                    if (chkWaivedYN != null)
                    {
                        if (General.GetNullableInteger(ViewState["waivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["waivedyn"].ToString()) == 1)
                        {
                            chkWaivedYN.Checked = true;
                            if (txtReason != null) txtReason.CssClass = "input_mandatory";
                        }
                        else
                        {
                            chkWaivedYN.Checked = false;
                            if (txtReason != null) txtReason.CssClass = "input";
                        }
                        ViewState["rowindex"] = "";
                        ViewState["waivedyn"] = "";
                        ViewState["edititem"] = "0";
                    }
                }

                if (lblExpiredYN.Text.Trim() == "1" || lblMissingYN.Text.Trim() == "1" || (lblVerifiedYN.Text.Trim() == "0" && lblDocumentType.Text.Trim() == "2")
                    || lblAuthenticatedYN.Text.Trim() == "0")
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
                if (lblDTKey != null && !string.IsNullOrEmpty(lblDTKey.Text) && lblMissingYN.Text.Trim() == "0")
                {
                    if (lblDocumentName != null) lblDocumentName.Visible = false;
                    if (lnkName != null) lnkName.Visible = true;
                    if (lblExpiredYN.Text.Trim() == "0" && lblMissingYN.Text.Trim() == "0" && (lblVerifiedYN.Text.Trim() == "1" && lblDocumentType.Text.Trim() == "2")
                       && lblAuthenticatedYN.Text.Trim() == "1")
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

                if (chkWaivedYN != null)
                    chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
            }
        }
    }

    protected void gvVesselTypeExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblShortfall = (Label)e.Row.FindControl("lblShortfall");
            Label lblVesselType = (Label)e.Row.FindControl("lblVesselType");
            Label lblVesselTypeExp = (Label)e.Row.FindControl("lblVesselTypeExp");
            Label lblStage = (Label)e.Row.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Row.FindControl("chkWaivedYN");
            TextBox txtReason = (TextBox)e.Row.FindControl("txtReason");
            Label lblStageid = (Label)e.Row.FindControl("lblStageid");
            Label lblProposalStageid = (Label)e.Row.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Row.FindControl("chkCanbeWaivedYN");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (lblShortfall.Text == "1")
            {
                lblVesselType.ForeColor = System.Drawing.Color.Red;
                lblVesselTypeExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
            if (chkWaivedYN != null)
                chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

            if (General.GetNullableInteger(ViewState["vtexprowindex"].ToString()) != null && General.GetNullableInteger(ViewState["vtexprowindex"].ToString()) == e.Row.DataItemIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["vtexpwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["vtexpwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["vtexprowindex"] = "";
                    ViewState["vtexpwaivedyn"] = "";
                    ViewState["vtexpedititem"] = "0";
                }
            }

            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvRankExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblShortfall = (Label)e.Row.FindControl("lblShortfall");
            Label lblRank = (Label)e.Row.FindControl("lblRank");
            Label lblRankExp = (Label)e.Row.FindControl("lblRankExp");
            Label lblStage = (Label)e.Row.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Row.FindControl("chkWaivedYN");
            TextBox txtReason = (TextBox)e.Row.FindControl("txtReason");
            Label lblStageid = (Label)e.Row.FindControl("lblStageid");
            Label lblProposalStageid = (Label)e.Row.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Row.FindControl("chkCanbeWaivedYN");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (lblShortfall.Text == "1")
            {
                lblRank.ForeColor = System.Drawing.Color.Red;
                lblRankExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
            if (chkWaivedYN != null)
                chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

            if (General.GetNullableInteger(ViewState["rankexprowindex"].ToString()) != null && General.GetNullableInteger(ViewState["rankexprowindex"].ToString()) == e.Row.DataItemIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["rankexpwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["rankexpwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["rankexprowindex"] = "";
                    ViewState["rankexpwaivedyn"] = "";
                    ViewState["rankexpedititem"] = "0";
                }
            }

            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvProposalCheckList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblShortfall = (Label)e.Row.FindControl("lblShortfall");
            Label lblQuestion = (Label)e.Row.FindControl("lblQuestion");            
            Label lblStage = (Label)e.Row.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Row.FindControl("chkWaivedYN");
            TextBox txtReason = (TextBox)e.Row.FindControl("txtReason");
            Label lblStageid = (Label)e.Row.FindControl("lblStageid");
            Label lblApprovalStageid = (Label)e.Row.FindControl("lblApprovalStageid");
            

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (lblShortfall.Text == "1")
            {
                lblQuestion.ForeColor = System.Drawing.Color.Red;
                lblQuestion.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
            //if (chkWaivedYN != null)
            //    chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

            if (General.GetNullableInteger(ViewState["proposalrowindex"].ToString()) != null && General.GetNullableInteger(ViewState["proposalrowindex"].ToString()) == e.Row.DataItemIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["proposalwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["proposalwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["proposalrowindex"] = "";
                    ViewState["proposalwaivedyn"] = "";
                    ViewState["proposaledititem"] = "0";
                }
            }

            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentCategoryName = ((Label)gridView.Rows[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblCategoryName")).Text;

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

                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                    previousRow.Cells[3].RowSpan + 1;
                previousRow.Cells[3].Visible = false;

                row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                        previousRow.Cells[4].RowSpan + 1;
                previousRow.Cells[4].Visible = false;

                row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                       previousRow.Cells[5].RowSpan + 1;
                previousRow.Cells[5].Visible = false;

                row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                       previousRow.Cells[6].RowSpan + 1;
                previousRow.Cells[6].Visible = false;

                row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
                                       previousRow.Cells[7].RowSpan + 1;
                previousRow.Cells[7].Visible = false;

                row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                       previousRow.Cells[8].RowSpan + 1;
                previousRow.Cells[8].Visible = false;

                row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                       previousRow.Cells[9].RowSpan + 1;
                previousRow.Cells[9].Visible = false;

                row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                       previousRow.Cells[10].RowSpan + 1;
                previousRow.Cells[10].Visible = false;
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
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANKPOSTED"].ToString();
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
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null || General.GetNullableInteger(Request.QueryString["vesselid"]) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? Request.QueryString["vesselid"] : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
            BindOffSigner();
            BindData();
        }
    }

    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
        BindOffSigner();
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
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANK"].ToString();
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
        //SetPageNavigator();
    }

    private bool IsValidCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        if (General.GetNullableInteger(ddlTrainingMatrix.SelectedValue) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        return (!ucError.IsError);
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

    private bool IsValidWaivedFilter(string waiveyn, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required to waive the documents.";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        if (General.GetNullableInteger(ddlTrainingMatrix.SelectedValue) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        if (General.GetNullableInteger(waiveyn) != null && General.GetNullableInteger(waiveyn) == 1)
        {
            if (General.GetNullableString(reason) == null)
                ucError.ErrorMessage = "Reason is required.";
        }

        return (!ucError.IsError);
    }

    protected void chkWaivedYN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");

            gvSuitability.EditIndex = gvRow.DataItemIndex;
            gvSuitability.SelectedIndex = gvRow.DataItemIndex;
            ViewState["rowindex"] = gvRow.DataItemIndex;
            ViewState["waivedyn"] = cb.Checked ? 1 : 0;
            ViewState["edititem"] = 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void chkCanbeWaivedYN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            Label lblDocumentType = (Label)gvRow.FindControl("lblDocumentType");
            Label lblDocumentId = (Label)gvRow.FindControl("lblDocumentId");
            Label lblWaivedDocumentId = (Label)gvRow.FindControl("lblWaivedDocumentId");

            string doctid;

            if (lblDocumentType.Text == "1" || lblDocumentType.Text == "2" || lblDocumentType.Text == "3" || lblDocumentType.Text == "5" || lblDocumentType.Text == "6")
                doctid = null;
            else
                doctid = lblDocumentId.Text;

            PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                  General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                  General.GetNullableInteger(lblDocumentType.Text), General.GetNullableInteger(doctid),
                  General.GetNullableInteger(ddlTrainingMatrix.SelectedValue), General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                  General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void chkCanbeWaivedYNRankExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            Label lblDocumentType = (Label)gvRow.FindControl("lblDocumentType");
            Label lblWaivedDocumentId = (Label)gvRow.FindControl("lblWaivedDocumentId");

            PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                  General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                  General.GetNullableInteger(lblDocumentType.Text), null,
                  General.GetNullableInteger(ddlTrainingMatrix.SelectedValue), General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                  General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void chkCanbeWaivedYNVTExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            Label lblDocumentType = (Label)gvRow.FindControl("lblDocumentType");
            Label lblWaivedDocumentId = (Label)gvRow.FindControl("lblWaivedDocumentId");

            PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                  General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                  General.GetNullableInteger(lblDocumentType.Text), null,
                  General.GetNullableInteger(ddlTrainingMatrix.SelectedValue), General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                  General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void chkWaivedYNRankExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");

            gvRankExp.EditIndex = gvRow.DataItemIndex;
            gvRankExp.SelectedIndex = gvRow.DataItemIndex;
            ViewState["rankexprowindex"] = gvRow.DataItemIndex;
            ViewState["rankexpwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["rankexpedititem"] = 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void chkWaivedYNVTExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");

            gvVesselTypeExp.EditIndex = gvRow.DataItemIndex;
            gvVesselTypeExp.SelectedIndex = gvRow.DataItemIndex;
            ViewState["vtexprowindex"] = gvRow.DataItemIndex;
            ViewState["vtexpwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["vtexpedititem"] = 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }
    protected void chkWaivedYNProposal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");

            gvProposalCheckList.EditIndex = gvRow.DataItemIndex;
            gvProposalCheckList.SelectedIndex = gvRow.DataItemIndex;
            ViewState["proposalrowindex"] = gvRow.DataItemIndex;
            ViewState["proposalwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["proposaledititem"] = 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }
    private void BindPlan(string crewplanid)
    {

        DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(crewplanid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (General.GetNullableInteger(dr["FLDVESSELID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(dr["FLDVESSELID"].ToString()));
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
                }
                if (General.GetNullableInteger(dr["FLDRANKID"].ToString()) != null)
                {
                    ddlRank.SelectedValue = dr["FLDRANKID"].ToString();
                    ddlRank.DataBind();
                }
                if (General.GetNullableInteger(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                {
                    BindTrainingMatrix();
                    if (ddlTrainingMatrix.Items.FindByValue(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                        ddlTrainingMatrix.SelectedValue = dr["FLDTRAININGMATRIXID"].ToString();
                }
                if (General.GetNullableDateTime(dr["FLDEXPECTEDJOINDATE"].ToString()) != null)
                    ucDate.Text = dr["FLDEXPECTEDJOINDATE"].ToString();
                if (General.GetNullableInteger(dr["FLDOFFSIGNERID"].ToString()) != null)
                {
                    BindOffSigner();
                    if (ddlOffSigner.Items.FindByValue(dr["FLDOFFSIGNERID"].ToString()) != null)
                        ddlOffSigner.SelectedValue = dr["FLDOFFSIGNERID"].ToString();
                }
            }
        }
    }

    protected void ddlTrainingMatrix_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    
    protected void gvSuitability_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            ViewState["rowindex"] = "";
            ViewState["waivedyn"] = "";
            ViewState["edititem"] = "0";
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSuitability_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSuitability_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Label lblDocumentType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType");
                Label lblDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId");
                CheckBox cb = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaivedYN");
                TextBox txtReason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReason");
                Label lblWaivedDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCanbeWaivedYN");
                string doctid;

                if (lblDocumentType.Text == "1" || lblDocumentType.Text == "2" || lblDocumentType.Text == "3" || lblDocumentType.Text == "5" || lblDocumentType.Text == "6")
                    doctid = null;
                else
                    doctid = lblDocumentId.Text;

                if (!IsValidWaivedFilter((cb.Checked ? "1" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), General.GetNullableInteger(doctid),
                    General.GetNullableInteger(cb.Checked ? "1" : "0"),
                    General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRankExp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVERANKEXP"))
            {
                Label lblDocumentType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaivedYN");
                TextBox txtReason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReason");
                Label lblWaivedDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCanbeWaivedYN");

                if (!IsValidWaivedFilter((cb.Checked ? "1" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "1" : "0"),
                    General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDITRANKEXP"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELRANKEXP"))
            {
                ViewState["rankexprowindex"] = "";
                ViewState["rankexpwaivedyn"] = "";
                ViewState["rankexpedititem"] = "0";
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselTypeExp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVEVTEXP"))
            {
                Label lblDocumentType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaivedYN");
                TextBox txtReason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReason");
                Label lblWaivedDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCanbeWaivedYN");

                if (!IsValidWaivedFilter((cb.Checked ? "1" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "1" : "0"),
                    General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDITVTEXP"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELVTEXP"))
            {
                ViewState["vtexprowindex"] = "";
                ViewState["vtexpwaivedyn"] = "";
                ViewState["vtexpedititem"] = "0";
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProposalCheckList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVEPROPOSAL"))
            {
                Label lblDocumentType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaivedYN");
                TextBox txtReason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReason");
                Label lblWaivedDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblWaivedDocumentId");                

                if (!IsValidWaivedFilter((cb.Checked ? "1" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "1" : "0"),
                    General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    0);
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDITPROPOSAL"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELPROPOSAL"))
            {
                ViewState["proposalrowindex"] = "";
                ViewState["proposalwaivedyn"] = "";
                ViewState["proposaledititem"] = "0";
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
