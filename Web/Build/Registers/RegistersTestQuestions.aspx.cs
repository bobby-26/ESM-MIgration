using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegistersTestQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersTestQuestions.aspx?courseid=" + Request.QueryString["courseid"], "Export to Excel","<i class=\"fas fa-file-excel\"></i>" , "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentTestQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuRegistersDocumentTestQuestions.AccessRights = this.ViewState;
        MenuRegistersDocumentTestQuestions.MenuList = toolbar.Show();
        
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "TESTSAVE",ToolBarDirection.Right);

        toolbar.AddButton("Upload Question", "TESTQUESTIONUPLOAD",ToolBarDirection.Right);
        MenuTest.AccessRights = this.ViewState;
        MenuTest.MenuList = toolbar.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["courseid"] = "";
            ViewState["examid"] = "";

            if (Request.QueryString["courseid"] != null && Request.QueryString["courseid"].ToString() != "")
                ViewState["courseid"] = Request.QueryString["courseid"].ToString();

            EditTestConfiguration();
            
                gvDocumentTestQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
        
    }

    protected void MenuTest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("TESTSAVE"))
            {
                if (!IsValidTest())
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(ViewState["examid"].ToString()) == null)
                {
                    PhoenixRegistersExamConfiguration.InsertConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableString(txtTestName.Text), int.Parse(ViewState["courseid"].ToString()), null,
                        General.GetNullableInteger(ucMaxQuestions.Text), General.GetNullableInteger(ucPassmark.Text));
                }
                else
                {
                    PhoenixRegistersExamConfiguration.UpdateConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["examid"].ToString()),
                        General.GetNullableString(txtTestName.Text), int.Parse(ViewState["courseid"].ToString()), null,
                        General.GetNullableInteger(ucMaxQuestions.Text), General.GetNullableInteger(ucPassmark.Text));
                }
                EditTestConfiguration();
                ucStatus.Text = "Updated Successfully";                
            }
            else if (CommandName.ToUpper().Equals("TESTQUESTIONUPLOAD"))
            {
                if (!IsValidTest())
                {
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:Openpopup('EXCEL', '', '../Registers/RegistersExport2XL.aspx?examid=" + ViewState["examid"].ToString() + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
               // PhoenixCrewOffshore2XL.Export2XLTrainingQuestion(General.GetNullableGuid(ViewState["examid"].ToString()));
                EditTestConfiguration();
                BindData();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidTest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtTestName.Text) == null)
            ucError.ErrorMessage = "Test Name is required.";
        if (General.GetNullableInteger(ucMaxQuestions.Text) == null)
            ucError.ErrorMessage = "Max No. of Questions is required.";
        if (General.GetNullableInteger(ucPassmark.Text) == null)
            ucError.ErrorMessage = "Pass Mark is required.";

        return (!ucError.IsError);
    }

    protected void EditTestConfiguration()
    {
        DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(int.Parse(ViewState["courseid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["examid"] = dr["FLDEXAMID"].ToString();
            txtCourse.Text = dr["FLDCOURSE"].ToString();            
        }

        DataSet ds1 = PhoenixRegistersExamConfiguration.EditConfiguration(General.GetNullableGuid(ViewState["examid"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            txtTestName.Text = dr["FLDEXAMNAME"].ToString();
            ucMaxQuestions.Text = dr["FLDNOOFQUESTIONS"].ToString();
            ucPassmark.Text = dr["FLDPASSMARK"].ToString();
            ucPassPercentage.Text = dr["FLDPASSPERCENTAGE"].ToString();
        }
    }

    protected void BindCourse(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
        ddl.DataTextField = "FLDCOURSE";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEXAMNAME", "FLDQREFERENCENO", "FLDQUESTION", "FLDACTIVE" };
        string[] alCaptions = { "Test Name", "Reference", "Question", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersTestQuestions.DocumentQuestionSearch(null,
            General.GetNullableInteger(ViewState["courseid"].ToString()), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentTestQuestions.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["examid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=TestQuestions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Test Questions</h3></td>");
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
    protected void RegistersDocumentTestQuestions_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
          
        
        if (CommandName.ToUpper().Equals("FIND"))
        {
            
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentTestQuestions.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEXAMNAME", "FLDQREFERENCENO", "FLDQUESTION",  "FLDACTIVE" };
        string[] alCaptions = { "Test Name", "Reference", "Question", "ActiveYN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersTestQuestions.DocumentQuestionSearch(null,
            General.GetNullableInteger(ViewState["courseid"].ToString()), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentTestQuestions.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["examid"].ToString()));

        General.SetPrintOptions("gvDocumentTestQuestions", "Test Questions", alCaptions, alColumns, ds);

        gvDocumentTestQuestions.DataSource = ds;
        gvDocumentTestQuestions.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocumentTestQuestions_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    //protected void gvDocumentTestQuestions_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //      gvDocumentTestQuestions.SelectedIndex = e.NewSelectedIndex;
    //}
   
    protected void gvDocumentTestQuestions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string isactive = string.Empty;
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentQuestion(((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersTestQuestions.InsertDocumentTestQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Text, null, null,
                    General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? "1" : "0"),
                    General.GetNullableInteger(ViewState["courseid"].ToString()),
                    General.GetNullableGuid(ViewState["examid"].ToString()),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceAdd")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucSortAdd")).Text));

                BindData();
                gvDocumentTestQuestions.Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;

            }


            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDocumentQuestion(((RadTextBox)e.Item.FindControl("txtQuestionEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
               
                int questionid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDQUESTIONID"].ToString());
               
                RadCheckBox ActiveYN = ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit"));
                if (ActiveYN != null)
                {
                    isactive = ActiveYN.Checked == true ? "1" : "0";
                }
                PhoenixRegistersTestQuestions.UpdateDocumentTestQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((RadLabel)e.Item.FindControl("lblQuestionidEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtQuestionEdit")).Text, null, null,
                    General.GetNullableInteger(isactive),
                    General.GetNullableInteger(ViewState["courseid"].ToString()),
                    General.GetNullableGuid(ViewState["examid"].ToString()),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceEdit")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucSortEdit")).Text));

                BindData();
                gvDocumentTestQuestions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentTestQuestion(Int32.Parse(((RadLabel)e.Item.FindControl("lblQuestionid")).Text));
                BindData();
                gvDocumentTestQuestions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ANSWER"))
            {

                string questionid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDQUESTIONID"].ToString();
                Response.Redirect("../Registers/RegistersTestAnswers.aspx?QuestionId=" + questionid + "&courseid=" + ViewState["courseid"].ToString());
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvDocumentTestQuestions_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            RadLabel lblQuestionid = (RadLabel)e.Item.FindControl("lblQuestionid");
            string sQuestionId="";
            if (lblQuestionid != null)
            {
                 sQuestionId = lblQuestionid.Text;
            }
            LinkButton cmdAnswer = (LinkButton)e.Item.FindControl("cmdAnswer");
            if (cmdAnswer != null)
            {
                string sQuestion = ((RadLabel)e.Item.FindControl("lblQuestion")).Text;
                cmdAnswer.Visible = SessionUtil.CanAccess(this.ViewState, cmdAnswer.CommandName);
            }
        }

        //  if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        //  {
        //      Label lblCourse = (Label)e.Row.FindControl("lblCourse");

        //      ImageButton ImgCourse = (ImageButton)e.Row.FindControl("ImgCourse");

        //      if (ImgCourse != null)
        //      {
        //          if (lblCourse != null)
        //          {
        //              if (lblCourse.Text != "")
        //              {
        //                  ImgCourse.Visible = true;
        //                  UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucCourse");
        //                  if (uct != null)
        //                  {
        //                      ImgCourse.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
        //                      ImgCourse.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        //                  }
        //              }
        //              else
        //                  ImgCourse.Visible = false;
        //          }
        //      }
        RadLabel lblQuestion = (RadLabel)e.Item.FindControl("lblQuestion");
            UserControlToolTip ucToolTipQuestion = (UserControlToolTip)e.Item.FindControl("ucToolTipQuestion");
            if (ucToolTipQuestion != null)
            {
             
                ucToolTipQuestion.Position = ToolTipPosition.TopCenter;
                ucToolTipQuestion.TargetControlId = lblQuestion.ClientID;
            }
    }
    private bool IsValidDocumentQuestion(string Question)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["examid"].ToString()) == null)
            ucError.ErrorMessage = "Please save test details before adding test questions.";

        if (Question.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
   
    protected void ApplicableCourse(object sender, EventArgs e)
    {
        //if (!String.IsNullOrEmpty(General.GetNullableString(cblRank.SelectedValue)))
        //{
        //    int setid = int.Parse(ViewState["SETID"].ToString());
        //    PhoenixRegistersWorkingGearSetDetails.UpdateWorkingGearSetRanks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, setid, ChkboxSelectedValue(cblRank));
        //    FillMappedRanks();
        //}
    }

    private void DeleteDocumentTestQuestion(int Questionid)
    {
        PhoenixRegistersTestQuestions.DeleteDocumentTestQuestion(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Questionid);
    }
    protected void gvDocumentTestQuestions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentTestQuestions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
