using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonDiscussion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (Request.QueryString["travelinstruction"] != null)
        {
            toolbarmain.AddButton("Post Instruction", "POSTCOMMENT",ToolBarDirection.Right);
        }
        else
        {
            toolbarmain.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
        }
        MenuDiscussion.MenuList = toolbarmain.Show();
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;

            gvDiscussion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
  
    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Request.QueryString["SESSIONID"] != null)
        {
            Guid sessionid = new Guid(Request.QueryString["SESSIONID"].ToString());
            DataTable dtdtkey = PhoenixCommoneProcessing.GetDocumentNumber(sessionid);
            Guid dtkey = new Guid(dtdtkey.Rows[0]["DTKEY"].ToString());
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(dtkey
                                                                    , null
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDiscussion.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount,null);
        }
        else 
        {
            PhoenixCommonDiscussion objdiscussion = (PhoenixCommonDiscussion)Session["PHOENIXDISCUSSION"];
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(objdiscussion.dtkey
                                                                   , null, sortexpression, sortdirection
                                                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDiscussion.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount
                                                                   ,objdiscussion.type);           
        }

        gvDiscussion.DataSource = ds.Tables[0];
        gvDiscussion.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDiscussion_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
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

    /// <summary>
    ///1. If user come through the application at that time email will NOT go
    ///2. If user open this screen by clicking the email link email will MUST go to each people's involve in this discussion
    ///   by checking the "SESSIONID" in the queryString.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }

            if (Request.QueryString["SESSIONID"] != null)
            {
                DataTable dt = PhoenixCommoneProcessing.GetDocumentNumber(new Guid(Request.QueryString["SESSIONID"]));
                PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(dt.Rows[0]["DTKEY"].ToString()), 4, txtNotesDescription.Text.Trim(),null);
                DataTable dtTodoDetails = PhoenixCommonNotification.GetToDoDetails(new Guid(dt.Rows[0]["DTKEY"].ToString()));

                string userids = dtTodoDetails.Rows[0]["FLDDOERID"].ToString();
                string[] useridsvalues = userids.Split(',');
                for (int i = 0; i <= useridsvalues.Length - 1; i++)
                {
                    if (useridsvalues[i] != "")
                    {
                        PhoenixCommoneProcessing.PrepareEmailMessage(PhoenixCommonNotification.GetUserDetails(useridsvalues[i]).Rows[0]["FLDEMAIL"].ToString(), "DIS", (new Guid(dt.Rows[0]["DTKEY"].ToString())), "", "", "", "<subject line>", txtNotesDescription.Text, "<sendersignature>", "");
                    }
                }
            }
            else
            {
                PhoenixCommonDiscussion objdiscussion = (PhoenixCommonDiscussion)Session["PHOENIXDISCUSSION"];
                PhoenixCommonDiscussion.TransTypeDiscussionInsert(objdiscussion.dtkey, objdiscussion.userid, txtNotesDescription.Text.Trim(),objdiscussion.type);
            }

            txtNotesDescription.Text = "";

            BindData();
            gvDiscussion.Rebind();
        }
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);

    }




}
