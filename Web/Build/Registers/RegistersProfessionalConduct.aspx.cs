using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersProfessionalConduct : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersProfessionalconduct.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('GvProfessionalConduct')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");         
            MenuProfessionalConduct.AccessRights = this.ViewState;
            MenuProfessionalConduct.MenuList = toolbar1.Show();
            toolbar1 = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                GvProfessionalConduct.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDCATEGORYDESC", "FLDRANKID", "FLDCONDUCTQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Category", "Applied To", "Evaluation Item", "Order Sequence" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAppraisalConductQuestion.AppraisalConductQuestionSearch(General.GetNullableInteger(ucQuicks.SelectedQuick)
                                                                                          , General.GetNullableInteger(ddlprofessionalconduct.SelectedHard)
                                                                                          , sortexpression
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , GvProfessionalConduct.PageSize
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );

        General.SetPrintOptions("GvProfessionalConduct", "Professional Conduct", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GvProfessionalConduct.DataSource = ds;
            GvProfessionalConduct.VirtualItemCount = iRowCount;
        }
        else
        {
            GvProfessionalConduct.DataSource = "";
        }
    }
    protected void MenuProfessionalConduct_TabStripCommand(object sender, EventArgs e)
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
                GvProfessionalConduct.Rebind();
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

        string[] alColumns = { "FLDCATEGORYDESC", "FLDRANKID", "FLDCONDUCTQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Category", "Applied To", "Evaluation Item", "Order Sequence" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersAppraisalConductQuestion.AppraisalConductQuestionSearch(General.GetNullableInteger(ucQuicks.SelectedQuick)
                                                                                          , General.GetNullableInteger(ddlprofessionalconduct.SelectedHard)
                                                                                         , sortexpression
                                                                                         , sortdirection
                                                                                         , (int)ViewState["PAGENUMBER"]
                                                                                         , GvProfessionalConduct.PageSize
                                                                                         , ref iRowCount
                                                                                         , ref iTotalPageCount
                                                                                      );

        Response.AddHeader("Content-Disposition", "attachment; filename=Professional.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Professional Conduct </h3></td>");
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
    private bool IsValidProfessionalConduct(string category, string appliedto, string evaluationitem, string ordersequence, string maxscore)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(category) == null)
            ucError.ErrorMessage = "Category is required.";
        if (General.GetNullableInteger(appliedto) == null)
            ucError.ErrorMessage = "Applied TO is required.";

        if (evaluationitem.Trim().Equals(""))
            ucError.ErrorMessage = "Evaluation Item is required.";

        if (General.GetNullableInteger(ordersequence) == null)
            ucError.ErrorMessage = "Order Sequence is required.";

        if (General.GetNullableInteger(maxscore) == null)
            ucError.ErrorMessage = "Max score is required.";

        return (!ucError.IsError);
    }

    private void InsertProfessionalConduct(int? appliedTo, string evaluationitem, int? ordersequence, int? category, string shortcode, int? maxscore, int active)
    {
        PhoenixRegistersAppraisalConductQuestion.InsertAppraisalConductQuestion(
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , appliedTo
                                                                         , evaluationitem
                                                                         , ordersequence
                                                                         , category, shortcode
                                                                         , maxscore
                                                                         , active
                                                                  );
    }

    private void UpdateProfessionalConduct(int questionid, int? appliedTo, string evaluationitem, int? ordersequence, int? category, string shortcode, int? maxscore, int active)
    {
        PhoenixRegistersAppraisalConductQuestion.UpdateAppraisalConductQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                        , appliedTo
                                                                        , evaluationitem
                                                                        , ordersequence, category, shortcode
                                                                        , maxscore
                                                                        , active);
    }

    private void deleteprofessionalconduct(int questionid)
    {
        PhoenixRegistersAppraisalConductQuestion.DeleteAppraisalConductQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                 );
    }
    
    protected void GvProfessionalConduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidProfessionalConduct(((UserControlQuick)e.Item.FindControl("ucQuick")).SelectedQuick
                    , ((UserControlHard)e.Item.FindControl("ddlprofessionalconductinsert")).SelectedHard.ToUpper(),
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucmaxscoreInsert")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertProfessionalConduct(
                    int.Parse(((UserControlHard)e.Item.FindControl("ddlprofessionalconductinsert")).SelectedHard.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text)
                    , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucQuick")).SelectedQuick)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtAddShortCode")).Text)
                    , General.GetNullableInteger(((UserControlNumber)e.Item.FindControl("ucmaxscoreInsert")).Text),
                    ((RadCheckBox)e.Item.FindControl("chkactiveadd")).Checked == true ? 1 : 0);
                BindData();
                GvProfessionalConduct.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deleteprofessionalconduct(int.Parse(((RadLabel)e.Item.FindControl("lblquestionid")).Text));
                BindData();
                GvProfessionalConduct.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string evalutionitem = string.Empty;
                evalutionitem = ((RadTextBox)e.Item.FindControl("txtevaluationitemedit")).Text;
                if (!IsValidProfessionalConduct(((UserControlQuick)e.Item.FindControl("ucQuickEdit")).SelectedQuick,
                                           ((UserControlHard)e.Item.FindControl("ddlprofessionalconductedit")).SelectedHard.ToUpper()
                                           , evalutionitem
                                           , ((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text,
                                           ((UserControlNumber)e.Item.FindControl("ucmaxscoreedit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateProfessionalConduct(
                                 int.Parse(((RadLabel)e.Item.FindControl("lblquestionidedit")).Text)
                                 , int.Parse(((UserControlHard)e.Item.FindControl("ddlprofessionalconductedit")).SelectedHard.ToUpper())
                                 , evalutionitem
                                 , int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text)
                                 , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucQuickEdit")).SelectedQuick)
                                 , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtEditShortCode")).Text)
                                 , int.Parse(((UserControlNumber)e.Item.FindControl("ucmaxscoreedit")).Text),
                                 ((RadCheckBox)e.Item.FindControl("chkactive")).Checked == true ? 1 : 0);
                BindData();
                GvProfessionalConduct.Rebind();
            }
            if (e.CommandName == "Page")
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

    protected void GvProfessionalConduct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvProfessionalConduct.CurrentPageIndex + 1;
        BindData();
    }

    protected void GvProfessionalConduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlprofessionalconductedit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDRANK"].ToString();
            UserControlQuick ucquick = (UserControlQuick)e.Item.FindControl("ucQuickEdit");

            if (ucquick != null)
            {
                ucquick.SelectedQuick = drvHard["FLDCATEGORY"].ToString();
            }
            LinkButton OccasionMapping = (LinkButton)e.Item.FindControl("cmdoccasionmapping");
            if (OccasionMapping != null)
            {
                OccasionMapping.Visible = SessionUtil.CanAccess(this.ViewState, OccasionMapping.CommandName);
                OccasionMapping.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersOccasionQuestionMapping.aspx?categoryid=0&questionid=" + drvHard["FLDCONDUCTQUESTIONID"] + "&rankcategoryid=" + drvHard["FLDRANK"] + "'); return false;");
            }
            LinkButton cmdVMap = (LinkButton)e.Item.FindControl("cmdmap");
            if (cmdVMap != null)
                cmdVMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterProfessionalConductVesselType.aspx?ID=" + drvHard["FLDCONDUCTQUESTIONID"] + "');return false;");
        }
    }
}