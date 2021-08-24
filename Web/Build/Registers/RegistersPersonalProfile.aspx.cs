using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPersonalProfile : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();           
            toolbar1.AddFontAwesomeButton("../Registers/RegistersPersonalProfile.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('GvPersonalProfile')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuPersonalProfile.AccessRights = this.ViewState;
            MenuPersonalProfile.MenuList = toolbar1.Show();
            toolbar1 = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                GvPersonalProfile.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPROFILECATEGORY", "FLDPROFILERANKCLASSIFICATION", "FLDPROFILEQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "PROFILECATEGORY", "APPLIED TO", "PROFILEQUESTION", "ORDER SEQUENCE" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAppraisalProfileQuestion.AppraisalProfileQuestionSearch(
                                                                                          General.GetNullableInteger(ddlpersonalprofile.SelectedHard)
                                                                                          , General.GetNullableInteger(ddlrank.SelectedHard)
                                                                                          , sortexpression
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , GvPersonalProfile.PageSize
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );

        General.SetPrintOptions("GvPersonalProfile", "Personal profile", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GvPersonalProfile.DataSource = ds;
            GvPersonalProfile.VirtualItemCount = iRowCount;
        }
        else
        {
            GvPersonalProfile.DataSource = "";
        }
    }


    protected void MenuPersonalProfile_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                GvPersonalProfile.Rebind();
            }
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

        string[] alColumns = { "FLDPROFILECATEGORY", "FLDPROFILERANKCLASSIFICATION", "FLDPROFILEQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "PROFILECATEGORY", "APPLIED TO", "PROFILEQUESTION", "ORDER SEQUENCE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersAppraisalProfileQuestion.AppraisalProfileQuestionSearch(
                                                                                          General.GetNullableInteger(ddlpersonalprofile.SelectedHard)
                                                                                          , General.GetNullableInteger(ddlrank.SelectedHard)
                                                                                          , sortexpression
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , GvPersonalProfile.PageSize
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );

        Response.AddHeader("Content-Disposition", "attachment; filename=Personal Profile.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Personal Profile </h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private bool IsValidPersonalProfile(string category, string evaluationitem, string rankclassification, string ordersequence, string maxscore)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (evaluationitem.Trim().Equals(""))
            ucError.ErrorMessage = "Evaluation Item is required.";

        if (General.GetNullableInteger(rankclassification) == null)
            ucError.ErrorMessage = "Applied To is required.";

        if (General.GetNullableInteger(ordersequence) == null)
            ucError.ErrorMessage = "Order Sequence is required.";

        if (General.GetNullableInteger(maxscore) == null)
            ucError.ErrorMessage = "Max Score is required.";


        return (!ucError.IsError);
    }

    private void InsertPersonalProfile(int? category, string evaluationitem, int? rankclassification, int? ordersequence, int? maxscore, int active, int? itemcategoryid)
    {
        PhoenixRegistersAppraisalProfileQuestion.InsertAppraisalProfileQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , category
                                                                        , evaluationitem
                                                                        , rankclassification
                                                                        , ordersequence
                                                                        , maxscore
                                                                        , active
                                                                        , itemcategoryid

                                                                 );
    }

    private void UpdatePersonalProfile(int questionid, int? category, string evaluationitem, int? rankclassification, int? ordersequence, int? maxscore, int active, int? itemcategoryid)
    {
        PhoenixRegistersAppraisalProfileQuestion.UpdateAppraisalProfileQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                        , category
                                                                        , evaluationitem
                                                                        , rankclassification
                                                                        , ordersequence
                                                                        , maxscore
                                                                        , active
                                                                         , itemcategoryid
                                                                 );
    }

    private void DeletePersonalProfile(int questionid)
    {
        PhoenixRegistersAppraisalProfileQuestion.DeleteAppraisalProfileQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                 );
    }
    protected void GvPersonalProfile_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPersonalProfile(((UserControlHard)e.Item.FindControl("ddlpersonalprofileinsert")).SelectedHard.ToUpper(),
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    ((UserControlHard)e.Item.FindControl("ddlrankinsert")).SelectedHard.ToUpper(),
                    ((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucmaxscoreInsert")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPersonalProfile(
                    int.Parse(((UserControlHard)e.Item.FindControl("ddlpersonalprofileinsert")).SelectedHard.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    int.Parse(((UserControlHard)e.Item.FindControl("ddlrankinsert")).SelectedHard.ToString()),
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text),
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucmaxscoreInsert")).Text),
                    ((RadCheckBox)e.Item.FindControl("chkactiveadd")).Checked == true ? 1 : 0,
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucQuick")).SelectedQuick));

                BindData();
                GvPersonalProfile.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePersonalProfile(int.Parse(((RadLabel)e.Item.FindControl("lblquestionid")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidPersonalProfile(((UserControlHard)e.Item.FindControl("ddlpersonalprofileedit")).SelectedHard.ToUpper(),
                   ((RadTextBox)e.Item.FindControl("txtevaluationitemedit")).Text,
                   ((UserControlHard)e.Item.FindControl("ddlrankedit")).SelectedHard.ToUpper(),
                   ((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text,
                   ((UserControlNumber)e.Item.FindControl("ucmaxscoreedit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePersonalProfile(
                                 int.Parse(((RadLabel)e.Item.FindControl("lblquestionidedit")).Text),
                                 int.Parse(((UserControlHard)e.Item.FindControl("ddlpersonalprofileedit")).SelectedHard.ToString()),
                                 ((RadTextBox)e.Item.FindControl("txtevaluationitemedit")).Text,
                                 int.Parse(((UserControlHard)e.Item.FindControl("ddlrankedit")).SelectedHard.ToString()),
                                 int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text),
                                 int.Parse(((UserControlNumber)e.Item.FindControl("ucmaxscoreEdit")).Text),
                                 (((RadCheckBox)e.Item.FindControl("chkactive")).Checked == true ? 1 : 0),
                                 General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucQuickEdit")).SelectedQuick));
                BindData();
                GvPersonalProfile.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GvPersonalProfile_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvPersonalProfile.CurrentPageIndex + 1;
        BindData();
    }

    protected void GvPersonalProfile_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            LinkButton Mapping = (LinkButton)e.Item.FindControl("cmdMapping");
            if (Mapping != null) Mapping.Visible = SessionUtil.CanAccess(this.ViewState, Mapping.CommandName);

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlpersonalprofileedit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDPROFILECATEGORYID"].ToString();

            UserControlHard ucHard1 = (UserControlHard)e.Item.FindControl("ddlrankedit");
            DataRowView drvHard1 = (DataRowView)e.Item.DataItem;
            if (ucHard1 != null) ucHard1.SelectedHard = drvHard1["FLDRANKID"].ToString();

            UserControlQuick ucquick = (UserControlQuick)e.Item.FindControl("ucQuickEdit");

            if (ucquick != null)
            {
                ucquick.SelectedQuick = drvHard["FLDCATEGORYID"].ToString();
            }

            if (Mapping != null)
            {
                if (drvHard1["FLDPROFILECATEGORY"].ToString().ToUpper() == "MID TENURE")
                {
                    Mapping.Visible = true;

                    Mapping.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersPersonalprofileRemarksMapping.aspx?profilecategoryid=" + drvHard["FLDPROFILECATEGORYID"].ToString() + "&profilerankgroup="
                              + drvHard1["FLDRANKID"].ToString() + "&profilequestionid=" + drvHard1["FLDPROFILEQUESTIONID"] + "'); return false;");
                }
                else
                {
                    Mapping.Visible = false;
                }
            }
            LinkButton OccasionMapping = (LinkButton)e.Item.FindControl("cmdoccasionmapping");
            if (OccasionMapping != null)
            {
                OccasionMapping.Visible = SessionUtil.CanAccess(this.ViewState, OccasionMapping.CommandName);
                OccasionMapping.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersOccasionQuestionMapping.aspx?categoryid=" + drvHard["FLDPROFILECATEGORYID"].ToString() + "&questionid=" + drvHard1["FLDPROFILEQUESTIONID"] + "&rankcategoryid=" + drvHard1["FLDRANKID"].ToString() + "'); return false;");
            }

            LinkButton cmdVMap = (LinkButton)e.Item.FindControl("cmdmap");
            if (cmdVMap != null)
                cmdVMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterPersonalProfileVesselType.aspx?ID=" + drvHard1["FLDPROFILEQUESTIONID"] + "');return false;");
            LinkButton cmdRankMap = (LinkButton)e.Item.FindControl("cmdRankMap");
            if (cmdRankMap != null)
                cmdRankMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterPersonalProfileRankMap.aspx?QuestionID=" + drvHard1["FLDPROFILEQUESTIONID"] + "&RankCategory=" + drvHard1["FLDRANKID"].ToString() + "');return false;");
        }
    }
}
        
