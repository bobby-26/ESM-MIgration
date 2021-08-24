using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshoreProposal : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvAQ.Rows)
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
            if (!IsPostBack)
            {
                ViewState["VSLID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["PAGENUMBERT"] = 1;
                ViewState["SORTEXPRESSIONT"] = null;
                ViewState["SORTDIRECTIONT"] = null;

                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["trainingmatrixid"] = "";
                ViewState["offsignerid"] = "";
                ViewState["reliefdate"] = "";
                ViewState["rankid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";
                ViewState["crewplanid"] = "";
                ViewState["iseditable"] = "";

                ViewState["proposalrowindex"] = "";
                ViewState["proposalwaivedyn"] = "";
                ViewState["proposaledititem"] = "0";

                ViewState["trainingneedrowindex"] = "";
                ViewState["trainingneedwaivedyn"] = "";
                ViewState["trainingneededititem"] = "0";

                ViewState["Trainingcoursetype"] = "";
                ViewState["Trainingcoursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "7");

                if (Request.QueryString["vslid"] != null)
                    ViewState["VSLID"] = Request.QueryString["vslid"];

                if (Request.QueryString["empid"] != null)
                    ViewState["empid"] = Request.QueryString["empid"];

                if (Request.QueryString["rankid"] != null && Request.QueryString["rankid"].ToString() != "")
                {
                    ViewState["rankid"] = Request.QueryString["rankid"];
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                }

                if (Request.QueryString["reliefdate"] != null && Request.QueryString["reliefdate"].ToString() != "")
                {
                    ViewState["reliefdate"] = Request.QueryString["reliefdate"].ToString();
                }

                if (Request.QueryString["reliefdate"] != null && Request.QueryString["reliefdate"].ToString() != "")
                {

                    DataTable dt = PhoenixCrewOffshoreReliefRequest.FetchBudgetedCurrency(General.GetNullableDateTime(Request.QueryString["reliefdate"].ToString()),
                                                                                          General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                                                                                          General.GetNullableInteger(Request.QueryString["rankid"].ToString())
                                                                                          );
                    if (dt.Rows.Count > 0)
                    {
                        lblcurrencytype.Text = "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
                        lblcurrencytypedp.Text = "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
                        lblcurrencytypeid.Text = dt.Rows[0]["FLDCURRENCYID"].ToString();
                        txtSalagreed.Text = dt.Rows[0]["FLD1STYEARSCALE"].ToString();
                        txtDPAllowance.Text = dt.Rows[0]["FLDDPALLOWANCE"].ToString();
                    }
                }
                if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
                    ViewState["offsignerid"] = Request.QueryString["offsignerid"].ToString();

                if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
                    ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                if (Request.QueryString["approval"] != null && Request.QueryString["approval"].ToString() != "")
                    ViewState["approval"] = Request.QueryString["approval"].ToString();

                if (Request.QueryString["iseditable"] != null && Request.QueryString["iseditable"].ToString() != "")
                    ViewState["iseditable"] = Request.QueryString["iseditable"].ToString();

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (ViewState["crewplanid"] != null && ViewState["crewplanid"].ToString() != "")
                    BindPlan(ViewState["crewplanid"].ToString());

                BindFields();
                BindChecklist();
              
            //    SetPageNavigatorTN();
                BindTrainingMatrix();
                BindOffSigner();
                gvCrewRecommendedCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }



            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreProposal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreProposal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ExcelTN");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewRecommendedCourses')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINTTN");
            MenuCrewRecommendedCourse.AccessRights = this.ViewState;
            MenuCrewRecommendedCourse.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            if (ViewState["crewplanid"] != null && ViewState["crewplanid"].ToString() != "")
            {
                if (ViewState["iseditable"].ToString().Equals("1"))
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            else
                toolbar.AddButton("Confirm", "CONFIRMPROPOSE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);

            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbar.Show();

         
        

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                    ddlRank.SelectedValue = dr["FLDRANKID"].ToString();
                if (General.GetNullableInteger(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                {
                    //BindTrainingMatrix();
                    if (ddlTrainingMatrix.Items.FindItemByValue(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                        ddlTrainingMatrix.SelectedValue = dr["FLDTRAININGMATRIXID"].ToString();
                }
                if (General.GetNullableDateTime(dr["FLDEXPECTEDJOINDATE"].ToString()) != null)
                    ucDate.Text = dr["FLDEXPECTEDJOINDATE"].ToString();
                if (General.GetNullableInteger(dr["FLDOFFSIGNERID"].ToString()) != null)
                {
                    BindOffSigner();
                    if (ddlOffSigner.Items.FindItemByValue(dr["FLDOFFSIGNERID"].ToString()) != null)
                        ddlOffSigner.SelectedValue = dr["FLDOFFSIGNERID"].ToString();
                }
            }
            txtSalagreed.Text = dr["FLDSALAGREEDUSD"].ToString();
            txtDPAllowance.Text = dr["FLDDPALLOWANCE"].ToString();
            txtEPFcontribution.Text = dr["FLDEPFCONTRIBUTION"].ToString();
            txtEPFcontribution2.Text = dr["FLDEPFAMOUNT"].ToString();
            txtContractPeriod.Text = dr["FLDCONTRACTPERIOD"].ToString();
            rblTrainingNeeds.SelectedValue = dr["FLDTRAININGNEEDSYN"].ToString();
            rblPreviousAppraisal.SelectedValue = dr["FLDPREVIOUSAPPRAISALYN"].ToString();
            rblRefCheck.SelectedValue = dr["FLDREFERENCECHECKYN"].ToString();
            txtRemaks.Text = dr["FLDPROPOSALREMARKS"].ToString();
            txtPlusMinusPeriod.Text = dr["FLDPLUSORMINUSRANGE"].ToString();
        }
    }

    protected void BindFields()
    {

        BindRank();
        DataSet ds = new DataSet();



        ds = PhoenixCrewOffshoreReliefRequest.EditCrewProposal(General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                                , General.GetNullableInteger(ViewState["rankid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtContractPeriod.Text = ds.Tables[0].Rows[0]["FLDCONTRACTPERIODDAYS"].ToString();
        }
        if (ViewState["rankid"] != null && ViewState["rankid"].ToString() != "")
            ddlRank.SelectedValue = ViewState["rankid"].ToString();

        if (ViewState["vesselid"] != null && ViewState["vesselid"].ToString() != "")
        {
            ucVessel.SelectedVessel = ViewState["vesselid"].ToString();
            ucVessel.bind();
            SetVesselType(null, null);
        }

        if (ViewState["reliefdate"] != null && ViewState["reliefdate"].ToString() != "")
            ucDate.Text = ViewState["reliefdate"].ToString();

        if (ViewState["offsignerid"] != null && ViewState["offsignerid"].ToString() != "")
            ddlOffSigner.SelectedValue = ViewState["offsignerid"].ToString();

        if (ViewState["trainingmatrixid"] != null && ViewState["trainingmatrixid"].ToString() != "")
            ddlTrainingMatrix.SelectedValue = ViewState["trainingmatrixid"].ToString();


    }

    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
        BindOffSigner();
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
            //BindTrainingMatrix();
            BindOffSigner();
        }
    }

    public void BindRank()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

        if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
        {
            if (ddlTrainingMatrix.Items.FindItemByValue(Request.QueryString["trainingmatrixid"].ToString()) != null)
                ddlTrainingMatrix.SelectedValue = Request.QueryString["trainingmatrixid"].ToString();
        }
        else
        {
            if (ddlTrainingMatrix.Items.Count == 2)
                ddlTrainingMatrix.SelectedIndex = 1;

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
                cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId,
                    General.GetNullableInteger(ViewState["empid"].ToString()));
            }
        }
        else
            cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel),
                General.GetNullableInteger(rank.SelectedValue), General.GetNullableInteger(ViewState["empid"].ToString()));
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

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                ViewState["rankid"] = General.GetNullableInteger(ddlRank.SelectedValue);
                ViewState["vesselid"] = General.GetNullableInteger(ucVessel.SelectedVessel);
                ViewState["reliefdate"] = General.GetNullableDateTime(ucDate.Text);
                ViewState["offsignerid"] = General.GetNullableInteger(ddlOffSigner.SelectedValue);
                ViewState["trainingmatrixid"] = General.GetNullableInteger(ddlTrainingMatrix.SelectedValue);

                Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + ViewState["empid"]
                    + "&rankid=" + ViewState["rankid"]
                    + "&vesselid=" + ViewState["vesselid"]
                    + "&reliefdate=" + ViewState["reliefdate"]
                    + "&offsignerid=" + ViewState["offsignerid"]
                    + "&trainingmatrixid=" + ViewState["trainingmatrixid"]
                    + "&personalmaster=" + Request.QueryString["personalmaster"]
                    + "&newapplicant=" + Request.QueryString["newapplicant"]
                    + "&popup=" + Request.QueryString["popup"]
                    + "&crewplanid=" + ViewState["crewplanid"]
                    + "&approval=" + ViewState["approval"], false);

                //Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx" 
                //    + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
            else if (CommandName.ToUpper().Equals("CONFIRMPROPOSE"))
            {
                if (!IsValidProposal())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                getChecklistAnswer();
                string ostn = "";
                if (ViewState["pendingtraining"].ToString() == "1")
                    ostn = "0";
                else
                    ostn = "1";

                PhoenixCrewOffshoreReliefRequest.InsertPlan(int.Parse(ViewState["empid"].ToString()),
                    int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString()),
                    General.GetNullableInteger(ViewState["offsignerid"].ToString()),
                    DateTime.Parse(ViewState["reliefdate"].ToString()), int.Parse(ViewState["trainingmatrixid"].ToString()),
                    General.GetNullableInteger(txtSalagreed.Text),
                    General.GetNullableString(txtRemaks.Text),
                    General.GetNullableInteger(ostn), //General.GetNullableInteger(rblTrainingNeeds.SelectedValue),
                    General.GetNullableInteger(ViewState["previousappraisal"].ToString()), //General.GetNullableInteger(rblPreviousAppraisal.SelectedValue),
                    General.GetNullableInteger(ViewState["refcheck"].ToString()), //General.GetNullableInteger(rblRefCheck.SelectedValue),
                    General.GetNullableInteger(txtDPAllowance.Text),
                     General.GetNullableInteger(txtContractPeriod.Text),
                    General.GetNullableInteger(txtPlusMinusPeriod.Text),
                    General.GetNullableDecimal(txtEPFcontribution.Text),
                    General.GetNullableDecimal(txtEPFcontribution2.Text)

                    );

                ucStatus.Text = "This candidate is proposed for the vessel successfully.";
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidProposal())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                getChecklistAnswer();
                string ostn = "";
                if (ViewState["pendingtraining"].ToString() == "1")
                    ostn = "0";
                else
                    ostn = "1";

                PhoenixCrewOffshoreReliefRequest.UpdatePlan(General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableInteger(txtSalagreed.Text),
                    General.GetNullableString(txtRemaks.Text),
                    General.GetNullableInteger(ostn), //General.GetNullableInteger(rblTrainingNeeds.SelectedValue),
                    General.GetNullableInteger(ViewState["previousappraisal"].ToString()), //General.GetNullableInteger(rblPreviousAppraisal.SelectedValue),
                    General.GetNullableInteger(ViewState["refcheck"].ToString()), //General.GetNullableInteger(rblRefCheck.SelectedValue),
                    General.GetNullableInteger(txtDPAllowance.Text),
                    General.GetNullableInteger(txtContractPeriod.Text),
                    General.GetNullableInteger(txtPlusMinusPeriod.Text),
                    General.GetNullableDecimal(txtEPFcontribution.Text),
                    General.GetNullableDecimal(txtEPFcontribution2.Text)

                    );
                ucStatus.Text = "Proposal is updated.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidProposal()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        getChecklistAnswer();

        if (General.GetNullableInteger(ViewState["vesselid"].ToString()) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ViewState["rankid"].ToString()) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ViewState["reliefdate"].ToString()) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        if (General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        if (txtSalagreed.Text == null || txtSalagreed.Text == "")
            ucError.ErrorMessage = "Daily Rate is required.";

        if (txtContractPeriod.Text == null || txtContractPeriod.Text == "")
            ucError.ErrorMessage = "Tenure (Days) is required.";

        if (txtEPFcontribution.Text == null || txtEPFcontribution.Text == "")
            ucError.ErrorMessage = "EPF Contribution is required.";

        if (General.GetNullableInteger(txtPlusMinusPeriod.Text) == null)
            ucError.ErrorMessage = "+/- period (days) is required.";

        //if (General.GetNullableInteger(rblTrainingNeeds.SelectedValue) == null)
        //    ucError.ErrorMessage = "Has no outstanding training needs?";

        if (General.GetNullableInteger(ViewState["previousappraisal"].ToString()) == null)
            ucError.ErrorMessage = "Previous appraisals are satisfactory?";

        if (General.GetNullableInteger(ViewState["refcheck"].ToString()) == null)
            ucError.ErrorMessage = "Reference checks are satisfactory?";

        if (General.GetNullableString(txtRemaks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
       
    }

    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDTOTALSCORE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report", "Total Score", "Recommended Promotion Y/N", "Fit for Re-employment Y/N", "HOD Remarks", "Master Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(ViewState["empid"].ToString())
               , General.GetNullableInteger(Request.QueryString["vslid"])
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , iRowCount
               , ref iRowCount
               , ref iTotalPageCount
               );

        Response.AddHeader("Content-Disposition", "attachment; filename=Appraisal.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Appraisal</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

  

   
    protected void gvAQ_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
      
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = 1; //DEFAULT DESC SORT
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDTOTALSCORE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDHEADDEPTCOMMENT", "FLDMASTERCOMMENT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report", "Total Score", "Recommended Promotion Y/N", "Fit for Re-employment Y/N", "HOD Remarks", "Master Remarks" };
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(ViewState["empid"].ToString())
               , General.GetNullableInteger(Request.QueryString["vslid"])
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

            General.SetPrintOptions("gvAQ", "Appraisal", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAQ.DataSource = ds.Tables[0];
               
                            
            }
            else
            {
                gvAQ.DataSource = ds.Tables[0];
            }
            gvAQ.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    // Training Needs

    private void BindTrainingNeeds()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            ViewState["pendingtraining"] = 0;
            string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDTOBEDONEBYNAME" };
            string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "To be done by" };
            string sortexpression = (ViewState["SORTEXPRESSIONT"] == null) ? null : (ViewState["SORTEXPRESSIONT"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTIONT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONT"].ToString());

            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchPendingTrainingCourse(int.Parse(ViewState["empid"].ToString()),
                General.GetNullableInteger(ViewState["rankid"].ToString()),
                General.GetNullableInteger(ViewState["vesselid"].ToString()),
                General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()),
                sortexpression, sortdirection,
                gvCrewRecommendedCourses.CurrentPageIndex+1,
               gvCrewRecommendedCourses.PageSize,
                ref iRowCount,
                ref iTotalPageCount, 1);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewRecommendedCourses", "Training Needs", alCaptions, alColumns, ds);

            gvCrewRecommendedCourses.DataSource = ds;
            gvCrewRecommendedCourses.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTT"] = iRowCount;
            ViewState["TOTALPAGECOUNTT"] = iTotalPageCount;

            if (ViewState["iseditable"].ToString().Equals("0"))
            {
                gvCrewRecommendedCourses.Columns[17].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewRecommendedCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCELTN"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDTOBEDONEBYNAME" };
                string[] alCaptions = { "No", "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "To be done by" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSIONT"] == null) ? null : (ViewState["SORTEXPRESSIONT"].ToString());

                if (ViewState["SORTDIRECTIONT"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONT"].ToString());

                if (ViewState["ROWCOUNTT"] == null || Int32.Parse(ViewState["ROWCOUNTT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTT"].ToString());

                DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchPendingTrainingCourse(int.Parse(ViewState["empid"].ToString()),
                                        General.GetNullableInteger(ViewState["rankid"].ToString()),
                                        General.GetNullableInteger(ViewState["vesselid"].ToString()),
                                        General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()),
                                        sortexpression, sortdirection,
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        iRowCount,
                                        ref iRowCount,
                                        ref iTotalPageCount, 1);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Training Needs", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
  

  
    protected void gvCrewRecommendedCourses_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "As reported by the vessel";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvCrewRecommendedCourses.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    


    // PROPOSAL CHECKLIST

    private void BindChecklist()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreReliefRequest.SearchProposalChecklist(int.Parse(ViewState["vesselid"].ToString())
                                                    , int.Parse(ViewState["rankid"].ToString())
                                                    , int.Parse(ViewState["empid"].ToString())
                                                    , int.Parse(ViewState["trainingmatrixid"].ToString()));


            gvProposalCheckList.DataSource = ds.Tables[0];

            if (ViewState["iseditable"].ToString().Equals("0"))
            {
                gvProposalCheckList.Columns[7].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    

    private bool IsValidWaivedFilter(string waiveyn, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(waiveyn) != null && General.GetNullableInteger(waiveyn) == 1)
        {
            if (General.GetNullableString(reason) == null)
                ucError.ErrorMessage = "Reason is required.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidWaivedTN(string waivingtype, string waiveyn, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(waiveyn) != null && General.GetNullableInteger(waiveyn) == 1)
        {
            if (General.GetNullableInteger(waivingtype) == null)
                ucError.ErrorMessage = "Waiving type is required.";

            if (General.GetNullableString(reason) == null)
                ucError.ErrorMessage = "Reason is required.";
        }

        return (!ucError.IsError);
    }

    protected void chkWaivedYNProposal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadTextBox txtReason = (RadTextBox)gvRow.FindControl("txtReason");

            //gvProposalCheckList.EditIndex = gvRow.DataItemIndex;
            //gvProposalCheckList.SelectedIndex = gvRow.DataItemIndex;
            ViewState["proposalrowindex"] = gvRow.DataSetIndex;
            ViewState["proposalwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["proposaledititem"] = 1;

            //  BindChecklist();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindChecklist();
        }
    }

    protected void rblAnswer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            GridDataItem gvRow = (GridDataItem)rbl.Parent.Parent;

            CheckBox cb = (CheckBox)gvRow.FindControl("chkWaivedYN");
            //gvProposalCheckList.SelectedIndexes = gvRow.DataItemIndex;
            if (rbl.SelectedValue == "0")
                cb.Enabled = true;
            else
                cb.Enabled = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindChecklist();
        }
    }

    protected void getChecklistAnswer()
    {

        foreach (GridDataItem gvRow in gvProposalCheckList.MasterTableView.Items)
        {
            if (gvRow.DataSetIndex == 0)
                ViewState["previousappraisal"] = ((RadioButtonList)gvRow.FindControl("rblAnswer")).SelectedValue;
            if (gvRow.DataSetIndex == 1)
                ViewState["refcheck"] = ((RadioButtonList)gvRow.FindControl("rblAnswer")).SelectedValue;
        }
    }

    protected void rblWaivingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            GridDataItem gvRow = (GridDataItem)rbl.Parent.Parent;

            CheckBox cb = (CheckBox)gvRow.FindControl("chkWaivedYN");
            //gvCrewRecommendedCourses.SelectedIndex = gvRow.DataItemIndex;
            if (General.GetNullableInteger(rbl.SelectedValue) != null)
                cb.Enabled = true;
            else
                cb.Enabled = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindTrainingNeeds();
        }
    }

    protected void chkWaivedYNTN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

           
            RadTextBox txtReason = (RadTextBox)gvRow.FindControl("txtReason");
            RadioButtonList rbl = (RadioButtonList)gvRow.FindControl("rblWaivingType");

            //gvCrewRecommendedCourses.EditIndex = gvRow.DataItemIndex;
            //gvCrewRecommendedCourses.SelectedIndex = gvRow.DataItemIndex;
            ViewState["trainingneedrowindex"] = gvRow.DataSetIndex;
            ViewState["trainingneedwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["trainingneededititem"] = 1;

            if (General.GetNullableInteger(ViewState["trainingneedwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["trainingneedwaivedyn"].ToString()) == 1)
            {
                cb.Checked = true;
                if (rbl != null) rbl.Enabled = true;
                if (txtReason != null) txtReason.CssClass = "input_mandatory";
            }
            else
            {
                cb.Checked = false;
                if (rbl != null) rbl.Enabled = false;
                if (txtReason != null) txtReason.CssClass = "input";
            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindTrainingNeeds();
            gvCrewRecommendedCourses.Rebind();
        }
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        decimal epf = Convert.ToDecimal(txtEPFcontribution.Text);
        decimal salary = Convert.ToDecimal(txtSalagreed.Text);

        decimal percentage = salary * (epf / 100);

        txtEPFcontribution2.Text = percentage.ToString();
    }


    protected void gvProposalCheckList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindChecklist();
    }

    protected void gvProposalCheckList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)e.Item.FindControl("chkWaivedYN");
                RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
                RadLabel lblWaivedDocumentId = (RadLabel)e.Item.FindControl("lblWaivedDocumentId");

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

                BindChecklist();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProposalCheckList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblShortfall = (RadLabel)e.Item.FindControl("lblShortfall");
            RadLabel lblQuestion = (RadLabel)e.Item.FindControl("lblQuestion");
            RadLabel lblStage = (RadLabel)e.Item.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadLabel lblStageid = (RadLabel)e.Item.FindControl("lblStageid");
            RadLabel lblApprovalStageid = (RadLabel)e.Item.FindControl("lblApprovalStageid");
            RadioButtonList rblAnswer = (RadioButtonList)e.Item.FindControl("rblAnswer");

        

            if (rblAnswer != null)
            {
                rblAnswer.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDANSWER").ToString();
            }

            if (General.GetNullableInteger(ViewState["proposalrowindex"].ToString()) != null && General.GetNullableInteger(ViewState["proposalrowindex"].ToString()) == e.Item.DataSetIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["proposalwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["proposalwaivedyn"].ToString()) == 1)
                    {
                        rblAnswer.SelectedValue = "0";
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

            if (rblAnswer != null && rblAnswer.SelectedValue == "0")
            {
                chkWaivedYN.Enabled = true;
                if (txtReason != null) txtReason.Visible = true;
            }
            else
            {
                chkWaivedYN.Enabled = false;
                if (txtReason != null) txtReason.Visible = false;
            }

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindTrainingNeeds();
    }

    protected void gvCrewRecommendedCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

         
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)e.Item.FindControl("chkWaivedYN");
                RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
                RadLabel lblWaivedDocumentId = (RadLabel)e.Item.FindControl("lblWaivedDocumentId");
                RadLabel lblTrainingNeedID = (RadLabel)e.Item.FindControl("lblTrainingNeedID");
                RadioButtonList rblWaivingType = (RadioButtonList)e.Item.FindControl("rblWaivingType");

                if (!IsValidWaivedTN(General.GetNullableString(rblWaivingType.SelectedValue), (cb.Checked ? "1" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "2" : "0"),
                    General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    0,
                    General.GetNullableGuid(lblTrainingNeedID.Text),
                    General.GetNullableInteger(rblWaivingType.SelectedValue), 1);
              
                BindTrainingNeeds();
                gvCrewRecommendedCourses.Rebind();
            }
          
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewRecommendedCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadioButtonList rblWaivingType = (RadioButtonList)e.Item.FindControl("rblWaivingType");

           
                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.OFFSHORE + "&type=OFFSHORETRAINING&cmdname=OFFSHORETRAININGUPLOAD'); return true;");
                }

            

            if (rblWaivingType != null)
            {
                rblWaivingType.SelectedValue = drv["FLDWAIVINGTYPE"].ToString();
            }

            if (General.GetNullableInteger(ViewState["trainingneedrowindex"].ToString()) != null && General.GetNullableInteger(ViewState["trainingneedrowindex"].ToString()) == e.Item.DataSetIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["trainingneedwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["trainingneedwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (rblWaivingType != null) rblWaivingType.Enabled = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (rblWaivingType != null) rblWaivingType.Enabled = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["trainingneedrowindex"] = "";
                    ViewState["trainingneedwaivedyn"] = "";
                    ViewState["trainingneededititem"] = "0";
                }
            }

            if (chkWaivedYN != null && chkWaivedYN.Checked)
            {
                if (rblWaivingType != null) rblWaivingType.Enabled = true;
            }
            else
            {
                if (rblWaivingType != null) rblWaivingType.Enabled = false;
            }

            RadTextBox txtremarks = (RadTextBox)e.Item.FindControl("txtRemarks");
            if (txtremarks != null)
            {
                if (drv["FLDAPPROVED"].ToString() == "0")
                    txtremarks.CssClass = "input_mandatory";
            }
            DropDownList ddlapprove = (DropDownList)e.Item.FindControl("ddlApproveEdit");
            if (ddlapprove != null) ddlapprove.SelectedValue = drv["FLDAPPROVED"].ToString();

            if (drv["FLDCOMPLETEDYN"].ToString().Equals("0"))
            {
                e.Item.ForeColor = System.Drawing.Color.Red;
                ViewState["pendingtraining"] = 1;
            }
            if (ViewState["pendingtraining"].ToString().Equals("1"))
            {
                lblNote.Text = "Note : There are training needs pending for this employee. Please see below.";
                lblNote.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblNote.Text = "Note : There are NO training needs pending for this employee.";
                lblNote.ForeColor = System.Drawing.Color.Blue;
            }
        }
    }

    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

       
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            Filter.CurrentAppraisalSelection = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;
            //Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
           
                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                    att.ImageUrl = Session["images"] + "/no-attachment.png";

                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=NAPPRAISALUPLOAD'); return false;");
                }
                else
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=NAPPRAISALUPLOAD'); return false;");
                }

                LinkButton cmdAppraisal = (LinkButton)e.Item.FindControl("cmdAppraisal");
                RadLabel lblAppraisalId = (RadLabel)e.Item.FindControl("lblAppraisalId");
                if (cmdAppraisal != null)
                {
                    cmdAppraisal.Visible = SessionUtil.CanAccess(this.ViewState, cmdAppraisal.CommandName);
                    if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                    {
                        cmdAppraisal.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                    else
                    {
                        cmdAppraisal.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                }

                LinkButton cmdPromotion = (LinkButton)e.Item.FindControl("cmdPromotion");
                if (cmdPromotion != null)
                {
                    cmdPromotion.Visible = SessionUtil.CanAccess(this.ViewState, cmdPromotion.CommandName);
                    if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                    {
                        cmdPromotion.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                    else
                    {
                        cmdPromotion.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION"
                            + "&appraisalid=" + lblAppraisalId.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1'); return false;");
                    }
                }
            

            RadLabel lblHODRemarks = (RadLabel)e.Item.FindControl("lblHODRemarks");
            if (lblHODRemarks != null)
            {
                UserControlToolTip ucHODRemarks = (UserControlToolTip)e.Item.FindControl("ucHODRemarks");
                lblHODRemarks.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucHODRemarks.ToolTip + "', 'visible');");
                lblHODRemarks.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucHODRemarks.ToolTip + "', 'hidden');");
            }

            RadLabel lblMasterRemarks = (RadLabel)e.Item.FindControl("lblMasterRemarks");
            if (lblMasterRemarks != null)
            {
                UserControlToolTip ucMasterRemarks = (UserControlToolTip)e.Item.FindControl("ucMasterRemarks");
                lblMasterRemarks.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucMasterRemarks.ToolTip + "', 'visible');");
                lblMasterRemarks.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucMasterRemarks.ToolTip + "', 'hidden');");
            }
        }
      
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }
}
