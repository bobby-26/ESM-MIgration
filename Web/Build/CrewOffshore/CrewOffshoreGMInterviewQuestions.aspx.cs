using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreGMInterviewQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreGMInterviewQuestions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreQuestions.AccessRights = this.ViewState;
            MenuOffshoreQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvOffshoreQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();

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

        string[] alColumns = { "FLDQUESTION" };
        string[] alCaptions = { "Question" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreGMInterviewQuestions.SearchInterviewQuestions(null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreQuestions.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreInterviewQuestions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Interview Questions</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void MenuOffshoreQuestions_TabStripCommand(object sender, EventArgs e)
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUESTION" };
        string[] alCaptions = { "Question" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreGMInterviewQuestions.SearchInterviewQuestions(null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreQuestions.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreQuestions", "Interview Questions", alCaptions, alColumns, ds);

        gvOffshoreQuestions.DataSource = ds;
        gvOffshoreQuestions.VirtualItemCount = iRowCount;

        //General.SetPrintOptions("gvProductType", "Product Type", alCaptions, alColumns, ds);

        //gvProductType.DataSource = ds;
        //gvProductType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvOffshoreQuestions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            CheckBoxList chkRankListEdit = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
            if (chkRankListEdit != null)
            {
                chkRankListEdit.DataSource = PhoenixCrewOffshoreGMInterviewQuestions.ListRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                chkRankListEdit.DataTextField = "FLDRANKNAME";
                chkRankListEdit.DataValueField = "FLDRANKID";
                chkRankListEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = drv["FLDRANKLIST"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            //if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            //{
                RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
                LinkButton ImgRankList = (LinkButton)e.Item.FindControl("ImgRankList");
                if (ImgRankList != null)
                {
                    if (lblRank != null)
                    {
                        if (lblRank.Text != "")
                        {
                            ImgRankList.Visible = true;
                            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRankList");
                            if (uct != null)
                            {
                                //ImgRankList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                                //ImgRankList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                                uct.Position = ToolTipPosition.TopCenter;
                                uct.TargetControlId = ImgRankList.ClientID;
                            
                            }
                        }
                        else
                            ImgRankList.Visible = false;
                    }
                }
            //}
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
            CheckBoxList chkRankListAdd = (CheckBoxList)e.Item.FindControl("chkRankListAdd");
            if (chkRankListAdd != null)
            {
                chkRankListAdd.DataSource = PhoenixCrewOffshoreGMInterviewQuestions.ListRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                chkRankListAdd.DataTextField = "FLDRANKNAME";
                chkRankListAdd.DataValueField = "FLDRANKID";
                chkRankListAdd.DataBind();
            }
        }
    }

    protected void gvOffshoreQuestions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (e.Item is GridFooterItem)
            //{

                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidData(((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    CheckBoxList chka = (CheckBoxList)e.Item.FindControl("chkRankListAdd");
                    string RList = "";
                    string RankList = "";
                    foreach (ListItem li in chka.Items)
                    {
                        if (li.Selected)
                        {
                            RList += li.Value + ",";
                        }
                    }

                    if (RList != "")
                    {
                        RankList = "," + RList;
                    }

                    PhoenixCrewOffshoreGMInterviewQuestions.InsertInterviewQuestion(((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Text
                                                                                    , General.GetNullableString(RankList));
                    BindData();
                    gvOffshoreQuestions.Rebind();
                    ((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Focus();
                }
            

            //else if(e.Item is GridDataItem)
            //{
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    //int questionid = int.Parse(ViewState[""].ToString());
                    //string a = ;
                  
                        GridDataItem item = e.Item as GridDataItem;

                        PhoenixCrewOffshoreGMInterviewQuestions.DeleteInterviewQuestion(Int32.Parse(item.GetDataKeyValue("FLDQUESTIONID").ToString()));
                        BindData();
                        gvOffshoreQuestions.Rebind();
                    
                    }
                
                else if(e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidData(((RadTextBox)e.Item.FindControl("txtQuestionEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    //int questionid = int.Parse(e.Item.Value.ToString());
                    CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
                    string RList = "";
                    string RankList = "";
                    foreach (ListItem li in chk.Items)
                    {
                        if (li.Selected)
                        {
                            RList += li.Value + ",";
                        }
                    }

                    if (RList != "")
                    {
                        RankList = "," + RList;
                    }
                    
                    GridDataItem item = e.Item as GridDataItem;
                    
                    //int questionid = int.Parse(ViewState["FLDQUESTIONID"].ToString());

                    PhoenixCrewOffshoreGMInterviewQuestions.UpdateInterviewQuestion((Int32.Parse(item.GetDataKeyValue("FLDQUESTIONID").ToString())), ((RadTextBox)e.Item.FindControl("txtQuestionEdit")).Text, General.GetNullableString(RankList));

                    BindData();
                    gvOffshoreQuestions.Rebind();
                }
                //else if(e.CommandName.ToUpper().Equals("EDIT"))
                //{
                //    ((RadLabel)e.Item.FindControl("lblQuestion")).Focus();
                //}
                
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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


    private bool IsValidData(string question)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (question.Equals(""))
            ucError.ErrorMessage = "Question is required.";

        return (!ucError.IsError);
    }

    protected void gvOffshoreQuestions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreQuestions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
